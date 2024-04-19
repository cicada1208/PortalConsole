using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Params;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;
using VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment;

namespace Lib.Wpf
{
    public class ExcelUtil
    {
        /// <summary>
        /// 匯出 Excel
        /// </summary>
        /// <remarks>需後續觀察其他型別是否也有Text屬性</remarks>
        public void ExportExcel(DataGrid dataGrid, string fileName = MsgParam.TitleExportRpt, string title = MsgParam.TitleExportRpt)
        {
            XSSFWorkbook workbook = null;
            ISheet sheet;
            MemoryStream excelData = null;
            CtrlUtil ctrlUtil = new CtrlUtil();

            try
            {
                if (!dataGrid.HasItems)
                {
                    MessageBox.Show(MsgParam.ExportNoData, MsgParam.TitlePrompt);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files(*.xlsx)|*.xlsx";
                saveFileDialog.AddExtension = true;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveFileDialog.FileName = fileName;
                saveFileDialog.Title = title;
                if (saveFileDialog.ShowDialog() != true) return;

                workbook = new XSSFWorkbook(); // 建立活頁簿
                sheet = workbook.CreateSheet("sheet"); // 建立sheet

                // 設定樣式
                IFont headerFont = workbook.CreateFont();
                headerFont.FontName = "微軟正黑體";
                headerFont.FontHeightInPoints = 10;
                headerFont.IsBold = true;
                IFont contentFont = workbook.CreateFont();
                contentFont.FontName = "微軟正黑體";
                contentFont.FontHeightInPoints = 10;
                ICellStyle headerStyle = workbook.CreateCellStyle();
                headerStyle.Alignment = HorizontalAlignment.Center; // 水平置中
                headerStyle.VerticalAlignment = VerticalAlignment.Center; // 垂直置中
                headerStyle.SetFont(headerFont);
                ICellStyle contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Left;
                contentStyle.VerticalAlignment = VerticalAlignment.Center;
                contentStyle.SetFont(contentFont);

                // 標題列
                //sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 2)); // 合併1-2列及A-C欄儲存格
                //sheet.CreateRow(idxExcelRow).CreateCell(idxCol).SetCellValue("cell value");
                int idxExcelRow = 0;
                sheet.CreateRow(idxExcelRow); // CreateRow 建立後, 再 GetRow 取得列
                for (int idxCol = 0; idxCol < dataGrid.Columns.Count; idxCol++)
                {
                    sheet.GetRow(idxExcelRow).CreateCell(idxCol).SetCellValue(dataGrid.Columns[idxCol].Header?.ToString());
                    sheet.GetRow(idxExcelRow).GetCell(idxCol).CellStyle = headerStyle;
                }
                idxExcelRow++;

                // 資料列
                DataGridRow dataGridRow;
                FrameworkElement cellContent;
                for (int idxRow = 0; idxRow < dataGrid.Items.Count; idxRow++)
                {
                    dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(idxRow) as DataGridRow;
                    if (dataGridRow == null)
                    {
                        // The DataGrid is virtualizing the items, the respective rows (i.e. containers) are only created when the row is in view.
                        dataGrid.ScrollIntoView(dataGrid.Items[idxRow]);
                        dataGrid.UpdateLayout();
                        dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(idxRow) as DataGridRow;
                        if (dataGridRow == null) continue;
                    }
                    sheet.CreateRow(idxExcelRow);
                    for (int idxCol = 0; idxCol < dataGrid.Columns.Count; idxCol++)
                    {
                        cellContent = dataGrid.Columns[idxCol].GetCellContent(dataGridRow);
                        sheet.GetRow(idxExcelRow).CreateCell(idxCol).SetCellValue(ctrlUtil.GetContentText<TextBlock>(cellContent));
                        sheet.GetRow(idxExcelRow).GetCell(idxCol).CellStyle = contentStyle;
                    }
                    idxExcelRow++;
                }

                // 寫檔
                excelData = new MemoryStream();
                workbook.Write(excelData);
                File.WriteAllBytes(saveFileDialog.FileName, excelData.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), MsgParam.TitlePrompt);
            }
            finally
            {
                excelData?.Dispose();
                excelData?.Close();
                workbook?.Close();
            }
        }

    }
}
