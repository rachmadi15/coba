using IDXWeb.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using SystemTask = System.Threading.Tasks.Task;

namespace IDXWeb.Support.DPS
{
    public class Uploader
    {
        private readonly string failedmessage = "Import Failed!!";
        private readonly string successMessage = "Import Completed!!";
        private readonly string NoModel = "No Model Found!!";
        private readonly HttpClient client;
        private string statusFile;
        private int PeriodeId;
        protected readonly IContentService ContentService;
        protected ILog log = LogManager.GetLogger("DPSUploader");
        protected int Year = 0;
        protected int Month = 0;

        public Uploader(IContentService ContentService)
        {
            this.ContentService = ContentService;
            statusFile = "";

            var handler = new HttpClientHandler
            {
                Proxy = null,
                UseProxy = false
            };

            client = new HttpClient(handler)
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["DPS_BASE_URL"]),
                Timeout = TimeSpan.FromMinutes(300)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Upload(IContent content, int month = 0, int year = 0, string rootPath = "~/")
        {
            string fileInput = content.GetValue<string>("statisticFile");
            var filePath = rootPath + fileInput;

            Thread uploadJob = new Thread(delegate () {
                if (!System.IO.File.Exists(filePath)) {
                    content.SetValue("status", "Failed to open the uploaded file");
                    ContentService.Save(content);
                    return;
                }
                FileStream fileStream = System.IO.File.OpenRead(filePath);
                try {
                    ProcessExcelData(fileStream, content, content.GetValue<string>("type"), month, year, content.GetValue<bool>("merge"))
                        .ContinueWith(param => {
                            fileStream.Close();
                        });
                } catch (Exception e) {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                    content.SetValue("status", e.Message);
                    ContentService.Save(content);
                    fileStream.Close();
                }
            });
            uploadJob.IsBackground = true;
            uploadJob.Start();
        }

        public async Task<bool> ProcessExcelData(FileStream file, IContent node, string uploadType, int month, int year, bool merge = false)
        {
            Month = month;
            Year = year;
            if (!file.CanRead || file == null || file.Length <= 0 || string.IsNullOrEmpty(file.Name)) {
                throw new FileLoadException("Unable to open file");
            }

            var status = failedmessage;
            //List<List<object>> BulkData = new List<List<object>>();
            var BulkData = new Dictionary<string, List<object>>();

            var content = node;
            using (var package = new ExcelPackage(file))
            {
                var worksheets = package.Workbook.Worksheets;
                foreach (var sheet in worksheets)
                {
                    var header = new List<string>();
                    var workSheet = sheet;
                    var typeName = "IDXWeb.Models." + RemoveSpecialCharacters(sheet.Name);
                    Type type = Type.GetType(typeName);

                    if (type == null)
                    {
                        content.SetValue("status", "Data '" + sheet.Name + "' not found! Please check your sheet name.");
                        ContentService.Save(content);
                        //statusFile = NoModel;
                        return false;
                    }

                    //Type listType = typeof(List<>).MakeGenericType(new[] { type });

                    List<object> list = new List<object>();

                    var colLength = GetLastUsedColumn(workSheet);
                    var rowLength = GetLastUsedRow(workSheet);
                    object currCell = null;
                    object currValue = null;

                    // Iterate through rows
                    for (int rowIterator = 1; rowIterator <= rowLength; rowIterator++) {
                        try {
                            var item = Activator.CreateInstance(type);
                            // Iterate through columns
                            for (int colIterator = 1; colIterator <= colLength; colIterator++) {
                                // Get Headers
                                if (rowIterator == 1) {
                                    var colval = workSheet.Cells[1, colIterator].Value;
                                    currValue = colval;
                                    currCell = colval;
                                    if (colval != null) { // Getting all headers
                                        header.Add(colval.ToString());
                                    } else { // No more header, continuing to the next row
                                        colLength = colIterator - 1;
                                        break;
                                    }
                                } else {
                                    var listDataoftheRow = new List<string>();
                                    var head = header[colIterator - 1];
                                    var property = item.GetType().GetProperty(RemoveSpecialCharacters(head));
                                    var value = new object();
                                    var colval = workSheet.Cells[rowIterator, colIterator];
                                    currCell = colval;
                                    currValue = colval.Value;
                                    if (property != null) {
                                        //Parse Data TO Adjust current Property
                                        value = ParseToDataType(colval, property.PropertyType);

                                        if (value != null) {
                                            property.SetValue(item, value, null);
                                        }
                                    }
                                }
                            }
                            if (rowIterator > 1) {
                                list.Add(item);
                            }
                        } catch (Exception e) {
                            status = "Import Failed: at " + currCell.ToString() + ", value=" + currValue.ToString() + " (" + e.Message + ")";
                            content.SetValue("status", status);
                            statusFile = status;
                            ContentService.Save(content);
                        }
                    }
                    //Add Sheet data to bulk object
                    BulkData.Add(sheet.Name, list);
                }

                //Add ObjectName Array to Last of List
                //var tableNames = currentSheet.Select(x => RemoveSpecialCharacters(x.Name)).ToList();
                //BulkData.Add("models", tableNames.ToList<object>());

                var uppp = umbraco.library.GetPreValueAsString(Convert.ToInt32(uploadType));
                if (uppp == "Transactional")
                {
                    var periodMonth = content.GetValue<int>("month");
                    var periodYear = content.GetValue<int>("year");
                    var periode = new Periode()
                    {
                        PeriodYear = periodYear,
                        PeriodMonth = periodMonth,
                        EditedDate = DateTime.Now,
                        UploadedDate = DateTime.Now
                    };
                    await SaveDataAsync(periode, "Periode");
                }

                int maxData = 2000;
                int CountSplitted = 0;
                string CurrModel = "";
                foreach (var model in BulkData)
                {
                    int totalData = model.Value.Count;
                    CountSplitted = 0;

                    // Chunk data to 2000 items each request
                    while (totalData > CountSplitted)
                    {
                        if (totalData - CountSplitted < maxData) {
                            maxData = totalData - CountSplitted;
                        }

                        // JSON Format: { "data" : ..., "model" : ... }
                        var splitData = new {
                            model = model.Key,
                            data = model.Value.GetRange(CountSplitted, maxData)
                        };

                        status = await SaveBulkDataAsync(splitData, merge);

                        if (!merge && !status.Contains("true")) {
                            break;
                        }

                        if (merge) {
                            status = status.Replace("true", "").Replace("\"", "").Replace("\\n", "\n");
                            status += !string.IsNullOrEmpty(status) ? "\n" : "";
                            var currStatus = content.GetValue<string>("status");
                            content.SetValue("status", (string.IsNullOrEmpty(currStatus) ? "" : currStatus + "\n") + status + "Processed: " + (CountSplitted + 1) + " to " + (CountSplitted + maxData) + " of " + totalData);
                            ContentService.Save(content);
                        }

                        CountSplitted += maxData;
                    }
                }
                //Save Extracted Data
                if (merge) {
                    var currStatus = content.GetValue<string>("status");
                    content.SetValue("status", string.IsNullOrEmpty(currStatus) ? "" : currStatus + "\n\nDone");
                    ContentService.Save(content);
                    return true;
                }

                //reset periodeId After importing
                PeriodeId = 0;
                status = status.Replace("\"", "");

                //Add Status To Items
                if (!status.Contains("true"))
                {
                    if (status.Contains("Import Failed: "))
                    {
                        var errorRow = int.Parse(status.Split(new[] { "Import Failed: " }, StringSplitOptions.None)[1]);
                        errorRow += CountSplitted;
                        status = "Import Failed: " + CurrModel + "|" + errorRow;
                    }
                    var currStatus = content.GetValue<string>("status");
                    content.SetValue("status", status);
                    statusFile = status;
                    ContentService.Save(content);
                    return false;
                }
                else
                {
                    status = status.Replace("true\n", "").Replace("true", "");
                    content.SetValue("status", status + "\n" + successMessage);
                    statusFile = successMessage;
                    ContentService.Save(content);
                }

            }

            return true;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        protected int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            try {
                while (row >= 1)
                {
                    var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                    if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                    {
                        log.Info(range.Select(x => x.Value));
                        break;
                    }
                    row--;
                }
            } catch (Exception e) {
                log.Error(e.Message);
                log.Error(e.StackTrace);
            }
            return row;
        }

        private int GetLastUsedColumn(ExcelWorksheet sheet)
        {
            var Column = sheet.Dimension.End.Column;
            try {
                while (Column >= 1)
                {
                    var range = sheet.Cells[1, Column, 1, sheet.Dimension.End.Column];
                    if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                    {
                        log.Info(range.Select(x => x.Value));
                        break;
                    }
                    Column--;
                }
            } catch (Exception e) {
                log.Error(e.Message);
                log.Error(e.StackTrace);
            }
            return Column;
        }

        protected object ParseToDataType(ExcelRange cell, Type type)
        {
            var value = new Object();
            var a = cell.GetValue<string>();
            if (cell.Value == null || cell.GetValue<string>().Trim() == "(null)")
            {
                value = null;
                return value;
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type == typeof(bool))
            {
                value = cell.GetValue<bool>();
                return value;
            }

            if (type == typeof(int))
            {
                value = cell.GetValue<int>();
                return value;
            }

            if (type == typeof(decimal))
            {
                value = cell.GetValue<decimal>();
                return value;
            }

            if (type == typeof(DateTime))
            {
                try
                {
                    value = cell.GetValue<DateTime>();
                }
                catch (Exception e)
                {
                    var temp = new DateTime();
                    var str = cell.GetValue<string>();
                    string[] formats = {
                         "M/d/yyyy",
                         "MM/dd/yyyy",
                         "MM/d/yyyy",
                         "M/dd/yyyy"
                    };

                    if (DateTime.TryParseExact(str, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                    {
                        value = temp;
                    }
                    else
                    {
                        value = null;
                    }
                }
                return value;
            }

            return cell.GetValue<string>().Trim();
        }

        protected async Task<bool> SaveDataAsync(object product, string tableName)
        {
            try
            {
                var apiprefix = "api/importdata/";
                var json = new object();

                if (tableName == "Periode") {
                    apiprefix = "api/import";
                    json = product;
                } else {
                    var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ" };
                    json = JsonConvert.SerializeObject(product, settings);
                }

                var queryTableName = tableName;
                if (PeriodeId != 0)
                {
                    queryTableName += "?periodeId=" + PeriodeId;
                }

                HttpResponseMessage response = await client.PostAsJsonAsync(
                    apiprefix + queryTableName, json);
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                if (tableName == "Periode")
                {
                    int intId = 0;
                    try
                    {
                        var text = response.Content.ReadAsAsync(typeof(string));
                        if (text != null)
                        {
                            intId = Convert.ToInt32(text.Result);
                        }
                    }
                    catch
                    {
                        return false;
                    }

                    if (intId != 0)
                    {
                        PeriodeId = intId;
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        protected async Task<string> SaveBulkDataAsync(object product, bool merge = false)
        {
            try
            {
                var apiprefix = "api/importbulkdata?x=1";

                if (PeriodeId != 0) {
                    apiprefix += "&periodeId=" + PeriodeId;
                }
                if (Month > 0 && Year > 0) {
                    apiprefix += "&month=" + Month + "&year=" + Year;
                }

                if (merge) {
                    apiprefix += "&merge=true";
                }

                var settings = new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddTH:mm:ss.fffZ" };
                var json = JsonConvert.SerializeObject(product, settings);

                HttpResponseMessage response = await client.PostAsJsonAsync(apiprefix, json);
                var res = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode) {
                    log.Error("API request failed!");
                    log.Error(res);
                    if (res.Length > 500) {
                        log.Error("URL: " + response.Headers.Location);
                        return "Import Failed! Response too long to display.";
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return "Import Failed (" + e.Message + ")";
            }
        }
    }
}