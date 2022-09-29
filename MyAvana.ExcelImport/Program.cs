using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MyAvana.DAL.Auth;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using OfficeOpenXml;
using RestSharp;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MyAvana.ExcelImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ReadAirtableData();

        }

        private static void ReadAirtableData()
        {
            var client = new RestClient("https://api.airtable.com/v0/appAb9o8W11GpTHv9/Products?maxRecords=100&view=Grid%20view");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Cookie", "brw=brwWubYIADpSOtcad");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "api.airtable.com");
            request.AddHeader("Postman-Token", "9449f0d4-296e-47d1-b1ea-ee149e6ca0c0,41d90f8f-2afb-4e4c-9189-3583ca96ff5b");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.18.0");
            request.AddHeader("Authorization", "Bearer keyHwXh4q26JP4gaN");
            IRestResponse response = client.Execute(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var model = JsonConvert.DeserializeObject<AirTableObject>(response.Content);
                bool IsSave = SaveDataInDB(model);
            }
        }

        private static bool SaveDataInDB(AirTableObject model)
        {
            try
            {
                using (var context = new EFCoreDemoContext())
                {
                    foreach (var record in model.records)
                    {
                        context.ProductEntities.Add(new MyAvanaApi.Models.Entities.ProductEntity() {
                            ActualName = record.fields.ActualName,
                            BrandName = record.fields.BrandName,
                            guid = Guid.NewGuid(),
                            ImageName = record.fields.ImageName,
                            Ingredients=record.fields.Ingredients,
                            ProductDetails=record.fields.ProductDetails,
                            ProductLink=record.fields.ProductLink,
                            TypeFor=record.fields.TypeFor,
                            ProductName=record.fields.ProductName,
                        });
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return false;
            }
        }

        static void ReadExcelFile()
        {
            try
            {
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(@"D:\Projects\MyAvana\MyAvana.ExcelImport\ExcelFiles\myavana.csv", false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    StringBuilder excelResult = new StringBuilder();

                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                        excelResult.AppendLine("----------------------------------------------- ");
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                        SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                        foreach (Row thecurrentrow in thesheetdata)
                        {
                            foreach (Cell thecurrentcell in thecurrentrow)
                            {
                                //statement to take the integer value  
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                //code to take the string value  
                                                excelResult.Append(item.Text.Text + " ");
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    excelResult.Append(Convert.ToInt16(thecurrentcell.InnerText) + " ");
                                }
                            }
                            excelResult.AppendLine();
                        }
                        excelResult.Append("");
                        Console.WriteLine(excelResult.ToString());
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception Ex)
            {

            }
        }
        private static T ReadFromExcel<T>(string path, bool hasHeader = true)
        {
            using (var excelPack = new ExcelPackage())
            {
                //Load excel stream
                using (var stream = File.OpenRead(path))
                {
                    excelPack.Load(stream);
                }

                //Lets Deal with first worksheet.(You may iterate here if dealing with multiple sheets)
                var ws = excelPack.Workbook.Worksheets[0];

                //Get all details as DataTable -because Datatable make life easy :)
                DataTable excelasTable = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    //Get colummn details
                    if (!string.IsNullOrEmpty(firstRowCell.Text))
                    {
                        string firstColumn = string.Format("Column {0}", firstRowCell.Start.Column);
                        excelasTable.Columns.Add(hasHeader ? firstRowCell.Text : firstColumn);
                    }
                }
                var startRow = hasHeader ? 2 : 1;
                //Get row details
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, excelasTable.Columns.Count];
                    DataRow row = excelasTable.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                //Get everything as generics and let end user decides on casting to required type
                var generatedType = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(excelasTable));
                return (T)Convert.ChangeType(generatedType, typeof(T));
            }
        }
    }
}
