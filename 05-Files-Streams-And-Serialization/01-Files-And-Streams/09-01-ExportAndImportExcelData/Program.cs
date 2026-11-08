using _09_01_ExportAndImportExcelData;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;

namespace WorkingWithExcel
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                #region Data
                List<Employee> empList = new List<Employee>();
                for (int i = 0; i < 100; i++)
                {
                    Employee emp = new Employee()
                    {
                        ID = 100 + i,
                        Name = "Працівник-" + i,
                        Address = "Україна, м. Київ",
                        Email = "employee" + i + "@example.com",
                        IsPermanent = true,
                        Mobile = "+380123456789",
                        RegdNo = 20000 + i,
                        Salary = 15000 + i,
                        ProfileURL = "profile" + i
                    };
                    empList.Add(emp);
                }
                #endregion

                #region Creating Excel Sheet
                HSSFWorkbook workbook = new HSSFWorkbook();
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Звіт компанії");

                HSSFFont font = (HSSFFont)workbook.CreateFont();
                #endregion

                #region Creating Cell Styles

                // Company title style
                var Company = workbook.CreateCellStyle();
                Company.Alignment = HorizontalAlignment.Left;
                var CompanyFont = workbook.CreateFont();
                CompanyFont.FontName = "Arial";
                CompanyFont.Color = HSSFColor.Blue.Index;
                CompanyFont.IsBold = true;
                CompanyFont.FontHeightInPoints = 16;
                Company.SetFont(CompanyFont);

                // Address style
                var Address = workbook.CreateCellStyle();
                Address.Alignment = HorizontalAlignment.Left;
                var AddressFont = workbook.CreateFont();
                AddressFont.FontName = "Arial";
                AddressFont.IsBold = true;
                AddressFont.FontHeightInPoints = 10;
                Address.SetFont(AddressFont);

                // Header style
                var Header = workbook.CreateCellStyle();
                Header.Alignment = HorizontalAlignment.Center;
                Header.FillForegroundColor = HSSFColor.LightBlue.Index;
                Header.FillPattern = FillPattern.SolidForeground;

                var HeaderFont = workbook.CreateFont();
                HeaderFont.FontName = "Arial";
                HeaderFont.IsBold = true;
                HeaderFont.Color = HSSFColor.White.Index;
                HeaderFont.FontHeightInPoints = 10;
                Header.SetFont(HeaderFont);

                Header.BorderLeft = BorderStyle.Thin;
                Header.BorderTop = BorderStyle.Thin;
                Header.BorderRight = BorderStyle.Thin;
                Header.BorderBottom = BorderStyle.Thin;

                // Numeric data style
                var NumData = workbook.CreateCellStyle();
                var newDataFormat = workbook.CreateDataFormat();
                NumData.DataFormat = newDataFormat.GetFormat("##0.00");

                // Text data style
                var Data = workbook.CreateCellStyle();
                Data.Alignment = HorizontalAlignment.Center;
                var DataFont = workbook.CreateFont();
                DataFont.FontName = "Arial";
                DataFont.FontHeightInPoints = 9;
                Data.SetFont(DataFont);

                Data.BorderLeft = BorderStyle.Thin;
                Data.BorderTop = BorderStyle.Thin;
                Data.BorderRight = BorderStyle.Thin;
                Data.BorderBottom = BorderStyle.Thin;

                // Link style
                var linkData = workbook.CreateCellStyle();
                linkData.Alignment = HorizontalAlignment.Center;
                var linkDataFont = workbook.CreateFont();
                linkDataFont.FontName = "Arial";
                linkDataFont.IsBold = true;
                linkDataFont.Color = HSSFColor.Blue.Index;
                linkDataFont.Underline = FontUnderlineType.Single;
                linkData.SetFont(linkDataFont);

                linkData.BorderLeft = BorderStyle.Thin;
                linkData.BorderTop = BorderStyle.Thin;
                linkData.BorderRight = BorderStyle.Thin;
                linkData.BorderBottom = BorderStyle.Thin;

                #endregion

                #region Company Header
                int rowIndex = 2;
                var row = sheet.CreateRow(rowIndex);
                var cell = row.CreateCell(2);

                cell.SetCellValue("Навчальний центр C# та .NET");
                cell.CellStyle = Company;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 14));

                rowIndex++;
                var row1 = sheet.CreateRow(rowIndex);
                var cell1 = row1.CreateCell(2);
                cell1.SetCellValue("Україна, Київ, вул. Програмістів, 10");
                cell1.CellStyle = Address;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 2, 14));
                #endregion

                #region Headers
                rowIndex = 7;
                var excelheaderrow = sheet.CreateRow(rowIndex);

                string[] headers = {
                    "№","ID","ПІБ","Адреса","Email","Постійний",
                    "Телефон","Реєстраційний №","Зарплата","Профіль"
                };

                int cellheaderindex = 0;

                foreach (var h in headers)
                {
                    var hcell = excelheaderrow.CreateCell(cellheaderindex);
                    hcell.SetCellValue(h);
                    hcell.CellStyle = Header;
                    cellheaderindex++;
                }
                #endregion

                #region Data Rows
                int SR_NO = 0;

                foreach (Employee data in empList)
                {
                    rowIndex++;
                    SR_NO++;
                    var gridrow = sheet.CreateRow(rowIndex);

                    int c = 0;
                    gridrow.CreateCell(c).SetCellValue(SR_NO); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.ID); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.Name); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.Address); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.Email); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.IsPermanent ? "Так" : "Ні"); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.Mobile); gridrow.GetCell(c++).CellStyle = Data;
                    gridrow.CreateCell(c).SetCellValue(data.RegdNo); gridrow.GetCell(c++).CellStyle = Data;

                    var salaryCell = gridrow.CreateCell(c);
                    salaryCell.SetCellValue(Convert.ToDouble(data.Salary));
                    salaryCell.CellStyle = NumData;
                    c++;

                    var linkCell = gridrow.CreateCell(c);
                    linkCell.SetCellValue(data.ProfileURL);

                    HSSFHyperlink link = new HSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = "https://example.com/" + data.ProfileURL
                    };

                    linkCell.Hyperlink = link;
                    linkCell.CellStyle = linkData;
                }
                #endregion

                #region Freeze + Column Width
                sheet.CreateFreezePane(0, 8, 0, 8);

                for (int i = 0; i <= cellheaderindex; i++)
                    sheet.SetColumnWidth(i, 5000);
                #endregion

                #region Total
                var startrow = 9;
                var lastdatarow = rowIndex + 1;

                rowIndex += 2;

                var totalRow = sheet.CreateRow(rowIndex);
                var totalCell = totalRow.CreateCell(0);
                totalCell.SetCellValue("ВСЬОГО");
                totalCell.CellStyle = Header;

                totalCell = totalRow.CreateCell(8);
                string strFormula = $"SUBTOTAL(9,I{startrow}:I{lastdatarow})";
                totalCell.SetCellFormula(strFormula);
                totalCell.CellStyle = Header;

                HSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                #endregion

                #region Logo
                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

                HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 255, 1, 2, 2, 5)
                {
                    AnchorType = (int)AnchorType.MoveAndResize
                };

                HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(
                    anchor,
                    LoadImage(@"C:\1\Logo.png", workbook)
                );

                picture.Resize();
                picture.LineStyle = (LineStyle)HSSFPicture.LINESTYLE_NONE;
                #endregion

                #region Save File
                string FileName = "Звіт_" + DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss") + ".xls";

                using (FileStream file = new FileStream(@"C:\1\" + FileName, FileMode.Create))
                {
                    workbook.Write(file);
                }

                Console.WriteLine("Файл успішно створено!");
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        public static int LoadImage(string path, HSSFWorkbook wb)
        {
            using FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);
        }
    }
}
