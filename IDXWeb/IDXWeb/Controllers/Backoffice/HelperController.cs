using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Umbraco.Web;

namespace IDXWeb.Controllers.Backoffice
{
    public class HelperController : BaseApiController
    {
        [HttpGet]
        [System.Web.Http.AcceptVerbs("GET")]
        public string SyncStatistic(int nid)
        {
            var statRoot = Umbraco.TypedContent(nid);
            if (statRoot == null) return "Statistic node not found";
            
            var children = statRoot.Descendants("statisticUploaderItem");
            var prop = Umbraco.DataTypeService.GetAllDataTypeDefinitions().FirstOrDefault(x => 
                x.Name.ToLower().Contains("statistic uploader") && 
                x.Name.ToLower().Contains("statistic type") &&
                x.Name.ToLower().Contains("dropdown")
            );
            if (prop == null) return "Dropdown property not found";
            IEnumerable<Umbraco.Core.Models.PreValue> prevalues = Umbraco.DataTypeService.GetPreValuesCollectionByDataTypeId(prop.Id).PreValuesAsDictionary.Select(x => x.Value);
            
            //var prop = Umbraco.DataTypeService.GetDataTypeDefinitionByPropertyEditorAlias(props.PropertyTypeAlias);
            
            // var prevalues = props.
            //return "Done";
            foreach (var child in children.OrderByDescending(x => x.UpdateDate)) {
                //var code = child.GetPropertyValue<string>("statisticNumber");
                var code = child.Name;
                var currType = child.GetPropertyValue<string>("statisticType");
                var currYear = child.GetPropertyValue<string>("statisticYear");
                if (string.IsNullOrEmpty(code) || !string.IsNullOrEmpty(currType) || !string.IsNullOrEmpty(currType))
                    continue;
                var yearCode = "";
                try { 
                    yearCode = code.Substring(2, 2);
                    var yInt = 0;
                    if (!int.TryParse(yearCode, out yInt)) {
                        yearCode = child.Name.Substring(2, 2);
                    } 
                } catch {
                    continue;
                }
                Umbraco.Core.Models.PreValue type = null;
                var typeStr = "";
                code = code.ToUpper();
                if (code.StartsWith("W")) {
                    typeStr = "weekly";
                } else if (code.StartsWith("M")) {
                    typeStr = "monthly";
                } else if (code.StartsWith("Q")) {
                    typeStr = "quarterly";
                } else if (code.StartsWith("Y")) {
                    typeStr = "yearly";
                } else {
                    continue;
                }
                type = prevalues.FirstOrDefault(x => x.Value.ToLower() == typeStr);
                if (type == null) return "Option " + typeStr + " not found.";

                var content = Services.ContentService.GetById(child.Id);
                content.SetValue("statisticType", type.Id);
                content.SetValue("statisticYear", "20" + yearCode);
                Services.ContentService.SaveAndPublishWithStatus(content);
            }
            return "Done";
        }
    }
}