using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExporter
{
    using Microsoft.Win32;
    using SoundDomain.Model.Entities;
    using System.Runtime.InteropServices;
    using System.Windows;
    using Excel = Microsoft.Office.Interop.Excel;

    public class Exporter
    {
        public void ExportList(IList<SoundHeard> Sounds)
        {
            var SoundsByFrequency = (from p in Sounds
                           group p by p.Sound.Frequency into g
                           select new { Frequency = g.Key, Sounds = g.Select(m => new { Volume = m.Sound.Volume, Answear = m.Answer }).ToList() }).ToList().ToDictionary(key => key.Frequency, val => val.Sounds); ;



            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet=null;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkBook.Sheets.Add(Count: SoundsByFrequency.Count - 1);

            int workSheet = 1;
            foreach(var soundByFrequency in SoundsByFrequency)
            {
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(workSheet);
                xlWorkSheet.Name = soundByFrequency.Key.ToString();

                xlWorkSheet.Cells[1, 1] = ColumnNames.FREQUENCY;
                xlWorkSheet.Cells[1, 2] = ColumnNames.VOLUME;
                xlWorkSheet.Cells[1, 3] = ColumnNames.ANSWEAR;

                int row = 2;
                foreach (var sound in soundByFrequency.Value)
                {
                    xlWorkSheet.Cells[row, 1] = soundByFrequency.Key.ToString();
                    xlWorkSheet.Cells[row, 2] = sound.Volume;
                    xlWorkSheet.Cells[row, 3] = sound.Answear;

                    row++;
                }

                workSheet++;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel file|*.xls";
            saveFileDialog1.Title = "Save an Excel File";

            bool? resultDialog = saveFileDialog1.ShowDialog();
            if (resultDialog.HasValue && resultDialog.Value == true)
            {
                xlWorkBook.SaveAs(saveFileDialog1.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
            }
            else
            {
                xlWorkBook.Close(false, misValue, misValue);
            }
            
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            
        }
    }
}
