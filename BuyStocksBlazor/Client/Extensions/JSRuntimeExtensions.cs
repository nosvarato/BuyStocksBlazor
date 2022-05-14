using Microsoft.JSInterop;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Extensions
{
    public static class JSRuntimeExtensions
    {
        public const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const TableStyles Style = TableStyles.Light11;

        public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
        {
            return jsRuntime.InvokeAsync<bool>("confirm", message);
        }
        public static ValueTask<bool> AddBodyClass(this IJSRuntime jsRuntime, string Classname)
        {
            return jsRuntime.InvokeAsync<bool>("SaphariNet.addBodyClass", Classname);
        }
        public static ValueTask<bool> RemoveBodyClass(this IJSRuntime jsRuntime, string Classname)
        {
            return jsRuntime.InvokeAsync<bool>("SaphariNet.removeBodyClass", Classname);
        }
        public static ValueTask<string> PrintDiv(this IJSRuntime jsRuntime, string PrintDiv)
        {
            return jsRuntime.InvokeAsync<string>("SaphariNet.printDiv", PrintDiv);
        }
        public static ValueTask<bool> ExportExcel<T>(this IJSRuntime jsRuntime, List<T> data, string reportname)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(reportname.Replace(".xlsx",""));
            worksheet.Cells["A1"].LoadFromDataTable(table, PrintHeaders: true, Style);
            for (var col = 1; col < table.Columns.Count + 1; col++)
            {
                worksheet.Column(col).AutoFit();
            }
            var now = DateTime.Now;
            var reportnamedatetime = reportname.Insert(reportname.LastIndexOf(".xlsx"), String.Format("_{0}{1}{2}_{3}{4}{5}", now.Year,now.Month,now.Day,now.Hour,now.Minute,now.Second));
            return jsRuntime.InvokeAsync<bool>("SaphariNet.saveAsFile", reportnamedatetime, package.GetAsByteArray());
            // return File(package.GetAsByteArray(), XlsxContentType, "Schools Report.xlsx");


        }
    }
}
