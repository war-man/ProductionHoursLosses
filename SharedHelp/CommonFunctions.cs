using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductionHoursLosses.Models;
using Microsoft.Office.Interop;
using System.Data;

namespace ProductionHoursLosses.SharedHelp
{
    public class CommonFunctions
    {
        private static PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();
        public static void CreateLog(string action, string element, int element_id, string old_val, string new_val, string user)
        {
            LOG CreateLog = new LOG();

            CreateLog.ACTION = action;
            CreateLog.DATE = System.DateTime.Now;
            CreateLog.ELEMENT = element;
            CreateLog.ELEMENT_ID = element_id;
            CreateLog.OLD_VALUE = old_val;
            CreateLog.NEW_VALUE = new_val;
            CreateLog.USER = user;
            db.LOG.Add(CreateLog);
            db.SaveChanges();
        }

        public static void Write2Excel(System.Data.DataTable dataTable)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange1, excelCellrange2;
            var misValue = System.Reflection.Missing.Value;
            string PathFileName = "C:\\Temp\\PRD_HRS_LOSSES_" + DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + ".xls";

            // Start Excel and get Application object.  
            excel = new Microsoft.Office.Interop.Excel.Application();
            // for making Excel visible  
            excel.Visible = false;
            excel.DisplayAlerts = false;
            // Creation a new Workbook  
            excelworkBook = excel.Workbooks.Add(Type.Missing);
            // Workk sheet  
            excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
            excelSheet.Name = "ProductionHoursLosses";

            excelSheet.Cells[1, 1] = "Production Hours Losses";
            excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();


            int colIndex = 0;

            foreach (DataColumn col in dataTable.Columns)
            {
                colIndex++;
                excelSheet.Cells[3, colIndex] = col.ColumnName;
            }

            object[,] arr = new object[dataTable.Rows.Count, dataTable.Columns.Count];
            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                DataRow dr = dataTable.Rows[r];
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    arr[r, c] = dr[c];
                }
            }

            //int rowcount = dataTable.Rows.Count+2;
            //// now we resize the columns  
            //excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
            //excelCellrange.EntireColumn.AutoFit();
            //Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
            //border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            //border.Weight = 2d;

            excelCellrange1 = (Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[4, 1];
            excelCellrange2 = (Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[4 + dataTable.Rows.Count - 1, dataTable.Columns.Count];
            Microsoft.Office.Interop.Excel.Range range = excelSheet.get_Range(excelCellrange1, excelCellrange2);
            range.Value = arr;

            excelworkBook.SaveAs(PathFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            excelworkBook.Close(true);
            excel.Quit();


        }

        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
            if (IsFontbool == true)
            {
                range.Font.Bold = IsFontbool;
            }
        }
    }
}