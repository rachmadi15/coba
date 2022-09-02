using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Web.Mvc;
using Umbraco.Core.Services;
using System.Linq;
using System.IO;
using ExcelDataReader;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Umbraco.Core.Models;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Core.Logging;
using System.Configuration;
using System.Data.SqlClient;
using IDXWeb.Models;
using System.Globalization;

namespace IDXWeb.Controllers
{
    public class StockNotationController : SurfaceController
    {

        private int enKey = 0;
        private int idKey = 0;
        private int notasiKey = 0;

        public StockNotationController()
        {
            try
            {
                var dataTypeService = ApplicationContext.Current.Services.DataTypeService;
                var allTypes = dataTypeService.GetAllDataTypeDefinitions();

                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var allCTypes = umbracoHelper.TypedContentAtXPath("//stockNotation");

                enKey = allCTypes.First(e => "Special Notation".InvariantEquals(e.Name)).Id;
                idKey = allCTypes.First(e => "Notasi Khusus".InvariantEquals(e.Name)).Id;
                notasiKey = allTypes.First(x => "Stock Notation Item - Notasi - Dropdown".InvariantEquals(x.Name)).Id;
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
            }

        }

        public ActionResult GetStockNotation(int nodeId)
        {
            try
            {
                var data = Umbraco.Content(nodeId).Children.Where("Visible");
                List<object> list = new List<object>();
                DateTime latestUpdate = DateTime.MinValue;

                if (data != null)
                {
                    foreach (var child in data)
                    {
                        try
                        {
                            list.Add(new
                            {
                                emitenCode = child.GetPropertyValue("emitenCode"),
                                notasi = child.GetPropertyValue("notasi"),
                                remarks = child.GetPropertyValue("remarks2")
                            });

                            if (DateTime.Compare(child.UpdateDate, latestUpdate) > 0)
                            {
                                latestUpdate = child.UpdateDate;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, $@"Error in retrieving data
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
id: {child.Id}
Name: {child.Name}
Message: {ex.Message}
xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
", ex);
                        }
                    }
                }

                var result = new
                {
                    data = list,
                    latestUpdate
                };

                var serializeObject = JsonConvert.SerializeObject(result);
                return Content(serializeObject, "application/json");
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        public HttpStatusCodeResult ImportExcel(string saveMode = "save", int userId = 0)
        {
            try
            {
                //request check
                var request = System.Web.HttpContext.Current.Request;
                var bad = new HttpResponseMessage(HttpStatusCode.BadRequest);
                if (request.Files.Count < 1)
                {
                    throw new HttpResponseException(bad);
                }
                IExcelDataReader reader;
                if (true)
                {
                    //process file
                    var language = "ID";
                    var file = request.Files[0];
                    var ext = Path.GetExtension(file.FileName);
                    if (file.FileName.Contains(" EN ") || file.FileName.Contains("-en-"))
                    {
                        language = "EN";
                    }
                    var dateStr = file.FileName.Substring(file.FileName.Length - 8 - System.IO.Path.GetExtension(file.FileName).Length, 8);
                    DateTime date;
                    DateTime.TryParseExact(dateStr, "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
                    var targetHeaders = new string[] { "Name", "Emiten Code" };
                    var headers = new List<string>();
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true,
                            ReadHeaderRow = (rowReader) =>
                            {
                                for (int i = 0; i < rowReader.FieldCount; i++)
                                {
                                    headers.Add(Convert.ToString(rowReader.GetValue(i)));
                                }
                            },
                            FilterColumn = (columnReader, columnIndex) =>
                            {
                                return targetHeaders.Contains(headers[columnIndex]);
                            }
                        }
                    };
                    if (ext.ToLower() == ".xls")
                    {
                        using (reader = ExcelReaderFactory.CreateBinaryReader(file.InputStream))
                        {
                            ReadData(reader, saveMode, userId, language, date);
                        }
                    }
                    else if (ext.ToLower() == ".xlsx")
                    {
                        using (reader = ExcelReaderFactory.CreateOpenXmlReader(file.InputStream))
                        {
                            ReadData(reader, saveMode, userId, language, date);
                        }
                    }
                    else if (ext.ToLower() == ".csv")
                    {
                        using (reader = ExcelReaderFactory.CreateCsvReader(file.InputStream))
                        {
                            ReadData(reader, saveMode, userId, language, date);
                        }
                    }
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
                return null;
            }
        }

        private void ReadData(IExcelDataReader reader, string saveMode, int userId, string language, DateTime date)
        {
            var result = reader.AsDataSet();
            UpdateDataToUmbraco(result, saveMode, userId, language, date);
        }
        private string RestructureNotation(string remarks)
        {
            var list = remarks.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(val => val.Trim()).ToArray();
            return String.Join(",", list);
        }

        private int BulkInsert(List<SpecialNotation> list)
        {
            var sql = GenerateSql(list);
            var conStr = ConfigurationManager.ConnectionStrings["idxweb"].ToString();
            var delSql = GenerateDeleteSql(list.First().Language);
            using (var conn = new SqlConnection(conStr))
            {
                conn.Open();
                SqlTransaction trs = conn.BeginTransaction();
                var cmd = new SqlCommand(sql, conn, trs);
                var delCmd = new SqlCommand(delSql, conn, trs);
                try
                {
                    delCmd.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    trs.Commit();
                }
                catch (Exception e) {
                    trs.Rollback();
                }
                conn.Close();
            }

            return 1;
        }
        private string GenerateDeleteSql(string language) {
            return "delete [dbo].[SpecialNotation] where Language = '" + language + "'";
        }

        private string GenerateSql(List<SpecialNotation> list)
        {
            var table = "[dbo].[SpecialNotation]";
            return $"insert into {table} " + BuildSqlHeader(typeof(SpecialNotation)) + " values " + BuildSqlValues(list);
        }
        private string BuildSqlHeader(Type type)
        {
            var sqlHeader = "";
            var props = typeof(SpecialNotation).GetProperties();
            for (var i = 0; i < props.Length; i++)
            {
                if (i > 0)
                {
                    sqlHeader += ",";
                }
                else if (i == 0)
                {
                    sqlHeader += "(";
                }

                if (props[i].PropertyType == typeof(string))
                {
                    sqlHeader += $"[{props[i].Name}]";
                }
                if (props[i].PropertyType == typeof(int))
                {
                    sqlHeader += $"[{props[i].Name}]";
                }
                if (props[i].PropertyType == typeof(DateTime?) || props[i].PropertyType == typeof(DateTime))
                {
                    sqlHeader += $"[{props[i].Name}]";
                }

                if (i == props.Length - 1)
                {
                    sqlHeader += ")";
                }
            }
            return sqlHeader;
        }
        private string BuildSqlValues(List<SpecialNotation> list)
        {
            var sqlDat = "";
            var listSql = new List<string>();
            var props = typeof(SpecialNotation).GetProperties();
            foreach (var dat in list)
            {
                for (var i = 0; i < props.Length; i++)
                {
                    if (i > 0)
                    {
                        sqlDat += ",";
                    }
                    else if (i == 0)
                    {
                        sqlDat += "(";
                    }
                    if (props[i].PropertyType == typeof(string))
                    {
                        sqlDat += $"'{props[i].GetValue(dat).ToString()}'";
                    }
                    if (props[i].PropertyType == typeof(int))
                    {
                        sqlDat += $"'{props[i].GetValue(dat).ToString()}'";
                    }
                    if (props[i].PropertyType == typeof(DateTime?) || props[i].PropertyType == typeof(DateTime))
                    {
                        sqlDat += $"'{ ((DateTime) props[i].GetValue(dat)).ToString("yyyy-MM-dd HH:mm:ss")}'";
                    }
                    if (i == props.Length - 1)
                    {
                        sqlDat += ")";
                    }
                }
                listSql.Add(sqlDat);
                sqlDat = "";
            }
            return string.Join(",", listSql);
        }

        private void UpdateDataToDatabase(string seccode, string remarks, string description, string language)
        {
            var conStr = ConfigurationManager.ConnectionStrings["idxweb"].ToString();
            var cmd = new SqlCommand();
            var table = "dbo.SpecialNotation";
            var sql = $@"insert into {table} ";
            string sqlHeader = "", sqlDat = "";
            SpecialNotation dat = new SpecialNotation
            {
                EmitenCode = seccode,
                Notation = RestructureNotation(remarks),
                Remarks = remarks,
                Description = description,
                Language = language == "EN" ? "en-us" : "id-id"
            };
            var props = typeof(SpecialNotation).GetProperties();
            for (var i = 0; i < props.Length; i++)
            {
                if (i > 0)
                {
                    sqlHeader += ",";
                    sqlDat += ",";
                }
                else if (i == 0)
                {
                    sqlHeader += "(";
                    sqlDat += "(";
                }
                try
                {

                    if (props[i].PropertyType == typeof(string))
                    {
                        sqlHeader += $"[{props[i].Name}]";
                        sqlDat += $"'{props[i].GetValue(dat).ToString()}'";
                    }
                    if (props[i].PropertyType == typeof(int))
                    {
                        sqlHeader += $"[{props[i].Name}]";
                        sqlDat += $"'{props[i].GetValue(dat).ToString()}'";
                    }
                }
                catch (Exception e)
                {

                }
                if (i == props.Length - 1)
                {
                    sqlHeader += ")";
                    sqlDat += ")";
                }
            }
            sql = sql + sqlHeader + "values" + sqlDat;
            cmd.CommandText = sql;
            using (var conn = new SqlConnection(conStr))
            {
                try
                {

                    conn.Open();
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception e)
                {
                }
            }
        }

        private void UpdateDataToUmbraco(DataSet result, string saveMode, int userId, string language, DateTime date)
        {
            IContentService ctSvc = ApplicationContext.Services.ContentService;
            var table = result.Tables[0];
            var header = new Dictionary<string, int>();
            var newCt = new List<string>();
            List<SpecialNotation> list = new List<SpecialNotation>();
            if (table.Rows.Count > 0 && table.Columns.Count > 0)
            {
                //var colEn = GetNotationItem(enKey.ToString());
                //var colId = GetNotationItem(idKey.ToString());

                for (int x = 0; x < table.Rows.Count; x++)
                {
                    //get header
                    if (x == 0)
                    {
                        for (int y = 0; y < table.Columns.Count; y++)
                        {
                            header.Add(table.Rows[x][y].ToString(), y);
                        }
                    }
                    else
                    {
                        int emitenCodeKey, remark2Key;
                        var successEmitenCode = header.TryGetValue("seccode", out emitenCodeKey);
                        var successRemarks2 = header.TryGetValue("remarks2", out remark2Key);

                        //20220729
                        int descriptionKey, languageKey;
                        var successDescription = header.TryGetValue("description", out descriptionKey);
                        //var successLanguage = header.TryGetValue("language", out languageKey);

                        if (successEmitenCode && successRemarks2)
                        {
                            var seccode = table.Rows[x][emitenCodeKey].ToString();
                            var remark2 = table.Rows[x][remark2Key].ToString();
                            string description = "";
                            if (successDescription)
                            {
                                description = table.Rows[x][descriptionKey].ToString();
                            }
                            list.Add(new SpecialNotation
                            {
                                EmitenCode = seccode,
                                Remarks = remark2,
                                Description = description,
                                Notation = RestructureNotation(remark2),
                                Language = language == "EN" ? "en-us" : "id-id",
                                CreatedDate = DateTime.Now,
                                Date = date
                            });

                            //update database
                            //UpdateDataToDatabase(seccode, remark2, description, language);

                            //update umbraco
                            //var ctEn = ChangeData(colEn, enKey, seccode, remark2);
                            //var ctId = ChangeData(colId, idKey, seccode, remark2);
                            //newCt.Add(table.Rows[x][emitenCodeKey].ToString());
                            //ctEn.ChangePublishedState(saveMode == "save" ? PublishedState.Saved : saveMode == "publish" ? PublishedState.Published : PublishedState.Unpublished);
                            //ctId.ChangePublishedState(saveMode == "save" ? PublishedState.Saved : saveMode == "publish" ? PublishedState.Published : PublishedState.Unpublished);
                            //if (saveMode == "save")
                            //{
                            //    ctSvc.Save(ctEn, userId);
                            //    ctSvc.Save(ctId, userId);
                            //}
                            //else if (saveMode == "publish")
                            //{
                            //    ctSvc.SaveAndPublishWithStatus(ctEn, userId);
                            //    ctSvc.SaveAndPublishWithStatus(ctId, userId);
                            //}
                        }
                    }
                }
                BulkInsert(list);
                //DeleteNotAccountedContent(newCt, enKey, idKey);
                //ctSvc.RebuildXmlStructures();
            }
        }

        private IContent ChangeData(IEnumerable<IContent> col, int parentId, string seccode, string remarks2)
        {
            IContentService ctSvc = ApplicationContext.Services.ContentService;
            IContent ct;
            var updateTrig = col.Where(e => e.Name == seccode).Count() > 0;
            if (!updateTrig)
            {
                //parentId = 2259 || 2262
                //seccode = table.Rows[x][emitenCodeKey].ToString()
                ct = ctSvc.CreateContent(seccode, parentId, "stockNotationItem");
            }
            else
            {
                ct = col.First(e => e.Name == seccode);
            }
            var multiKey = remarks2.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(val => val.Trim()).ToArray();
            List<int> fk = new List<int>();
            if (multiKey.Count() > 1)
            {
                fk.AddRange(GetMultipleNotasiId(multiKey.ToList()).ToList());
            }
            else
            {
                fk.AddRange(GetNotasiId(multiKey[0]).ToList());
            }
            var key = "";
            //multiple value handling
            if (fk.Count > 1)
            {
                key = string.Join(",", fk.ToArray());
            }
            else
            {
                key = fk.FirstOrDefault().ToString();
            }
            ct.SetValue("emitenCode", seccode);
            ct.SetValue("notasi", key);
            ct.SetValue("remarks2", remarks2);
            return ct;
        }

        private IEnumerable<int> GetNotasiId(string value)
        {
            List<int> id = new List<int>();
            IDataTypeService dtSvc = ApplicationContext.Services.DataTypeService;
            var allDef = dtSvc.GetAllDataTypeDefinitions();
            var col = dtSvc.GetPreValuesCollectionByDataTypeId(notasiKey);
            PreValue preValue = null;
            var count = 0;
            foreach (var ch in value)
            {
                if (count < 5)
                {
                    if (col.IsDictionaryBased)
                    {
                        preValue = col.PreValuesAsDictionary.FirstOrDefault(e => e.Value.Value == ch.ToString()).Value;
                    }
                    else
                    {
                        preValue = col.PreValuesAsArray.First(e => e.Value == ch.ToString());
                    }
                    if (preValue != null)
                    {
                        id.Add(preValue.Id);
                    }
                    else
                    {
                        //Add new value
                        id.Add(AddNewNotationItem(ch.ToString()));
                    }
                }
                count++;
            }
            return id;
        }

        private List<int> GetMultipleNotasiId(List<string> vals)
        {
            List<int> id = new List<int>();
            foreach (var val in vals)
            {
                IDataTypeService dtSvc = ApplicationContext.Services.DataTypeService;
                var allDef = dtSvc.GetAllDataTypeDefinitions();
                var col = dtSvc.GetPreValuesCollectionByDataTypeId(notasiKey);
                PreValue preValue = null;
                var count = 0;
                foreach (var ch in val)
                {
                    if (count < 5)
                    {
                        if (col.IsDictionaryBased)
                        {
                            preValue = col.PreValuesAsDictionary.FirstOrDefault(e => e.Value.Value == ch.ToString()).Value;
                        }
                        else
                        {
                            var arr = col.PreValuesAsArray;
                            preValue = arr.FirstOrDefault(e => e.Value == ch.ToString());
                        }
                        if (preValue != null)
                        {
                            id.Add(preValue.Id);
                        }
                        else
                        {
                            //Add new value
                            id.Add(AddNewNotationItem(ch.ToString()));
                        }
                    }
                    count++;
                }
            }
            return id.ToList();
        }

        private IEnumerable<IContent> GetNotationItem(string parentId = "2259")
        {
            var ctSvc = ApplicationContext.Services.ContentService;
            var cTypeSvc = ApplicationContext.Services.ContentTypeService;
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var col = helper.TypedContentAtXPath("//" + "stockNotationItem").ToArray().Where(e => e.DocumentTypeAlias == "stockNotationItem").ToArray();
            var typeId = cTypeSvc.GetContentType("stockNotationItem");
            var colNew = ctSvc.GetContentOfContentType(typeId.Id);
            var dats = new List<IContent>();
            dats.AddRange(ctSvc.GetByIds(colNew.Select(e => e.Id).ToList()));

            //**********parentId = 2259 || 2262
            dats = dats.Where(e => e.Trashed == false && e.ParentId.ToString() == parentId).ToList();
            return dats;
        }

        /****
         * Delete Old Data that no longer is used
         * **********/

        private void DeleteNotAccountedContent(List<string> names, int enKey, int idKey)
        {
            DeleteNotAccountedContentByCtId(enKey, names);
            DeleteNotAccountedContentByCtId(idKey, names);
        }

        private void DeleteNotAccountedContentByCtId(int parentId, List<string> names)
        {
            var oldCt = GetNotationItem(parentId.ToString());
            var ctSvc = ApplicationContext.Services.ContentService;
            foreach (var ct in oldCt)
            {
                if (!names.Contains(ct.Name))
                {
                    ctSvc.Delete(ct);
                }
            }
        }

        private int AddNewNotationItem(string param)
        {
            var dtSvc = ApplicationContext.Services.DataTypeService;
            var df = dtSvc.GetDataTypeDefinitionById(notasiKey);
            var curVal = dtSvc.GetPreValuesCollectionByDataTypeId(notasiKey);
            IDictionary<string, PreValue> preVals = new Dictionary<string, PreValue>();
            int count = 0;
            PreValue specialVal = null;
            if (curVal.IsDictionaryBased)
            {
                preVals = curVal.PreValuesAsDictionary;
                specialVal = preVals.FirstOrDefault(e => e.Key == "multiple").Value;
                preVals = preVals.Where(e => e.Key != "multiple").ToDictionary(e => e.Key, e => e.Value);
                count = preVals.Count;
            }
            else
            {
                var arr = curVal.PreValuesAsArray;
                if (arr.Count() > 0)
                {
                    foreach (var val in arr)
                    {
                        if (val.Value == "1" || val.Value == "0")
                        {
                            specialVal = new PreValue(val.Value);
                        }
                        else
                        {
                            preVals.Add(count.ToString(), val);
                            count++;
                        }
                    }
                }
            }
            preVals.Add(count.ToString(), new PreValue(param));
            //add special value
            if (specialVal != null)
            {
                preVals.Add("multiple", new PreValue(specialVal.Value));
            }
            else
            {
                preVals.Add("multiple", new PreValue("1"));
            }
            dtSvc.SaveDataTypeAndPreValues(df, preVals);
            var newCol = dtSvc.GetPreValuesCollectionByDataTypeId(notasiKey);
            PreValue newVal = null;
            if (newCol.IsDictionaryBased)
            {
                newVal = newCol.PreValuesAsDictionary.FirstOrDefault(e => e.Value.Value == param).Value;
            }
            else
            {
                newVal = newCol.PreValuesAsArray.FirstOrDefault(e => e.Value == param);
            }
            if (newVal != null)
            {
                return newVal.Id;
            }
            else
            {
                return -1;
            }
        }

        private void ResetNotationItem()
        {
            var dtSvc = ApplicationContext.Services.DataTypeService;
            var curVal = dtSvc.GetPreValuesCollectionByDataTypeId(notasiKey);
            IDictionary<string, PreValue> preVals = new Dictionary<string, PreValue>();
            dtSvc.SavePreValues(notasiKey, preVals);
        }

        public void ImportXlsx(string json, string saveMode, int userId, string language, DateTime date)
        {
            try
            {
                var jsonSet = JsonConvert.DeserializeObject<ObjectTemplate>(json);
                if (jsonSet.Object.Count() > 0)
                {
                    var dSet = new DataSet();
                    var dTable = dSet.Tables.Add();
                    dTable.Columns.Add("seccode");
                    dTable.Columns.Add("remarks2");
                    dTable.LoadDataRow(new object[] {
                    "seccode", "remarks2"
                }, LoadOption.PreserveChanges);
                    foreach (var jsonData in jsonSet.Object)
                    {
                        if (jsonData.seccode != null && jsonData.remarks2 != null)
                        {
                            dTable.LoadDataRow(new object[] {
                        jsonData.seccode,
                        jsonData.remarks2
                    }, LoadOption.PreserveChanges);
                        }
                    }
                    if (dTable.Rows.Count > 1)
                    {
                        UpdateDataToUmbraco(dSet, saveMode, userId, language, date);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, e.Message, e);
            }
        }

        private class ObjectTemplate
        {
            public IEnumerable<IImportTemplate> Object;
        }

        private class IImportTemplate
        {
            public string seccode;
            public string remarks2;
        }
    }
}