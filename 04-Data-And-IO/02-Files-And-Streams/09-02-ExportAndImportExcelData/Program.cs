using _09_02_ExportAndImportExcelData;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
                List<Employee> employees = new List<Employee>();

                string FilePath = @"C:\1\1.xlsx";   // ⟵ обовʼязково .xlsx (бо XSSFWorkbook)
                XSSFWorkbook workbook;

                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fs);
                }

                ISheet sheet = workbook.GetSheetAt(0);

                // ПРИСКАЧУЄМО ДО РЯДКА №8 (індекс 7)
                // бо над таблицею логотип, назва, адреса
                for (int rowIndex = 8; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row == null) continue;  // пропуск пустих рядків
                    if (row.Cells.TrueForAll(c => c == null || c.CellType == CellType.Blank))
                        continue;

                    Employee emp = new Employee()
                    {
                        ID = GetInt(row.GetCell(1)),
                        Name = GetString(row.GetCell(2)),
                        Address = GetString(row.GetCell(3)),
                        Email = GetString(row.GetCell(4)),

                        IsPermanent = GetString(row.GetCell(5)) == "Так",

                        Mobile = GetString(row.GetCell(6)),
                        RegdNo = GetInt(row.GetCell(7)),
                        Salary = GetDecimal(row.GetCell(8)),

                        ProfileURL = GetString(row.GetCell(9))
                    };

                    employees.Add(emp);
                }

                // Виводимо результат
                foreach (var e in employees)
                {
                    Console.WriteLine(
                        $"ID: {e.ID}, ПІБ: {e.Name}, Зарплата: {e.Salary}, Профіль: {e.ProfileURL}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ПОМИЛКА: " + ex.Message);
            }

            Console.ReadKey();
        }

        // ===================
        // ДОПОМІЖНІ МЕТОДИ
        // ===================

        static string GetString(ICell cell)
        {
            if (cell == null) return "";

            return cell.CellType switch
            {
                CellType.String => cell.StringCellValue.Trim(),
                CellType.Numeric => cell.NumericCellValue.ToString().Trim(),
                CellType.Boolean => cell.BooleanCellValue ? "Так" : "Ні",
                CellType.Formula =>
                    cell.CachedFormulaResultType == CellType.String
                        ? cell.StringCellValue
                        : cell.NumericCellValue.ToString(),
                CellType.Blank => "",
                _ => cell.ToString().Trim()
            };
        }

        static int GetInt(ICell cell)
        {
            if (cell == null) return 0;

            return cell.CellType switch
            {
                CellType.Numeric => (int)cell.NumericCellValue,
                CellType.String =>
                    int.TryParse(cell.StringCellValue, out int r) ? r : 0,
                CellType.Formula =>
                    int.TryParse(cell.ToString(), out int r2) ? r2 : 0,
                _ => 0
            };
        }

        static decimal GetDecimal(ICell cell)
        {
            if (cell == null) return 0;

            return cell.CellType switch
            {
                CellType.Numeric => (decimal)cell.NumericCellValue,
                CellType.String =>
                    decimal.TryParse(cell.StringCellValue.Replace(",", "."),
                                     System.Globalization.NumberStyles.Any,
                                     System.Globalization.CultureInfo.InvariantCulture,
                                     out decimal r)
                                     ? r : 0,
                _ => 0
            };
        }
    }
}
