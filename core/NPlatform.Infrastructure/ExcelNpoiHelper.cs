/***********************************************************
**项目名称:	                                                                  				   
**功能描述:Npoi Excel操作类
**作    者: 	易栋梁                                         			   
**版 本 号:	1.0                                             			   
**创建日期： 2015/12/2 16:16:34
**修改历史：
************************************************************/

namespace NPlatform.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using NPOI.HPSF;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Excel 操作类，基于NPOI
    /// </summary>
    public class ExcelNpoiHelper : IOfficeHelper
    {
        private string _saveAsFile = string.Empty;

        HSSFWorkbook hssfworkbook = null;

        private FileStream readfile = null;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <param name="saveAsFile">保存路径</param>
        public ExcelNpoiHelper(string filePath, string saveAsFile)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            _saveAsFile = string.IsNullOrEmpty(saveAsFile) ? filePath : saveAsFile;
            readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
        }

        /// <summary>
        ///     DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        /// <param name="strSheetName">工作表名称</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName, string strSheetName)
        {
            if (strSheetName == string.Empty)
            {
                strSheetName = "Sheet";
            }

            using (var ms = Export(dtSource, strHeaderText, strSheetName))
            {
                using (var fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    var data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        ///     DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strSheetName">工作表名称</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText, string strSheetName)
        {
            var workbook = new HSSFWorkbook();

            // HSSFSheet sheet = workbook.CreateSheet();// workbook.CreateSheet();   
            var sheet = workbook.CreateSheet(strSheetName);
            {
                var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "长沙中交经纬";
                workbook.DocumentSummaryInformation = dsi;

                var si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "长沙中交经纬";

                // 填加xls文件作者信息      
                si.ApplicationName = "NPOI"; // 填加xls文件创建程序信息      
                si.LastAuthor = "ExcelHelper"; // 填加xls文件最后保存者信息      
                si.Comments = "系统自动创建文件"; // 填加xls文件作者信息      
                si.Title = strHeaderText; // 填加xls文件标题信息      
                si.Subject = strHeaderText; // 填加文件主题信息      
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");



            var arrColWidth = new int[dtSource.Columns.Count];

            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.Default.GetBytes(item.ColumnName).Length;
            }

            for (var i = 0; i < dtSource.Rows.Count; i++)
            {
                for (var j = 0; j < dtSource.Columns.Count; j++)
                {
                    var intTemp = Encoding.Default.GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }



            var rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet(strSheetName + rowIndex / 65535);
                    }
                    {
                        #region 表头及样式

                        var headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));

                        #endregion
                    }
                    {
                        #region 列头及样式

                        var headerRow = sheet.CreateRow(1);

                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                            var colWidth = (arrColWidth[column.Ordinal] + 1) * 256;
                            if (colWidth < 255 * 256)
                            {
                                sheet.SetColumnWidth(column.Ordinal, colWidth < 3000 ? 3000 : colWidth);
                            }
                            else
                            {
                                sheet.SetColumnWidth(column.Ordinal, 6000);
                            }
                        }

                        #endregion
                    }

                    rowIndex = 2;
                }

                #endregion

                #region 填充内容

                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal);

                    var drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": // 字符串类型      
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime": // 日期类型      
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; // 格式化显示      
                            break;
                        case "System.Boolean": // 布尔型      
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16": // 整型      
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal": // 浮点型      
                        case "System.Double":
                            double doubV;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull": // 空值处理      
                            newCell.SetCellValue(string.Empty);
                            break;
                        default:
                            newCell.SetCellValue(string.Empty);
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }
            var ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            workbook = null;
            return ms;
        }

        /// <summary>
        /// 获取所有的sheet名称
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> GetSheetName(string fileName)
        {
            var arrayList = new List<string>();
            try
            {
                var readfile = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                var hssfworkbook = new HSSFWorkbook(readfile);
                for (var i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    arrayList.Add(hssfworkbook.GetSheetName(i));
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());
            }

            return arrayList;
        }

        /// <summary>
        ///     获取Sheet个数
        /// </summary>
        public static int GetSheetNumber(string filePath)
        {
            var number = 0;
            try
            {
                var readfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                var hssfworkbook = new HSSFWorkbook(readfile);
                number = hssfworkbook.NumberOfSheets;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());
            }

            return number;
        }

        /// <summary>
        ///     读取excel
        ///     默认第一行为表头，导入第一个工作表
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <param name="removeEmpty">移除空白行</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName, bool removeEmpty = false)
        {
            var dt = new DataTable();

            IWorkbook hssfworkbook;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                // hssfworkbook = new HSSFWorkbook(file);
                hssfworkbook = WorkbookFactory.Create(file);
            }

            var sheet = hssfworkbook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var headerRow = sheet.GetRow(0);
            int cellCount = headerRow.Cells.Count; // 表格的列数固定为列头个数

            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                var cellHeard = cell == null ? j.ToString() : cell.ToString().Trim();
                if (cellHeard == string.Empty || dt.Columns.Contains(cellHeard))
                    cellHeard = string.Format("{0}{1}{2}", cellHeard, j, DateTime.Now.ToString("ssfff"));
                dt.Columns.Add(cellHeard);
            }

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dataRow = dt.NewRow();
                bool isAdd = false;
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null)
                    {
                        dataRow[j >= 0 ? j : 0] = null;
                    }
                    else
                    {
                        // dataRow[j] = cell.ToString();   
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                dataRow[j] = null;
                                break;
                            case CellType.Boolean:
                                dataRow[j] = cell.BooleanCellValue;
                                break;
                            case CellType.Numeric:
                                var str = cell.CellStyle.GetDataFormatString();
                                if (!string.IsNullOrWhiteSpace(str))
                                {
                                    if (str.Contains("yy") || str.Contains("mm") || str.Contains("dd")
                                        || str.Contains("hh"))
                                    {
                                        dataRow[j] = cell.DateCellValue;
                                    }
                                    else
                                    {
                                        dataRow[j] = cell.ToString();
                                    }
                                }
                                else
                                {
                                    dataRow[j] = cell.NumericCellValue;
                                }
                                break;
                            case CellType.String:
                                dataRow[j] = cell.StringCellValue;
                                break;
                            case CellType.Error:
                                dataRow[j] = cell.ErrorCellValue;
                                break;
                            case CellType.Formula:
                                {
                                    switch (cell.CachedFormulaResultType)
                                    {
                                        case CellType.Blank:
                                            dataRow[j] = null;
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = cell.BooleanCellValue;
                                            break;
                                        case CellType.Numeric:
                                            var str1 = cell.CellStyle.GetDataFormatString();
                                            if (str1.Contains("yyyy") || str1.Contains("mm") || str1.Contains("dd")
                                                || str1.Contains("hh"))
                                            {
                                                dataRow[j] = cell.DateCellValue;
                                            }
                                            else
                                                dataRow[j] = cell.NumericCellValue;

                                            break;
                                        case CellType.String:
                                            dataRow[j] = cell.StringCellValue;
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = cell.ErrorCellValue;
                                            break;
                                        default:
                                            dataRow[j] = cell.ToString();
                                            break;
                                    }

                                    break;
                                }

                            default:
                                dataRow[j] = cell.ToString();
                                break;
                        }

                        if (dataRow[j] != null)
                            isAdd = true;
                    }
                }

                if (!isAdd && removeEmpty)
                    continue;
                dt.Rows.Add(dataRow);
            }

            for (var i = dt.Rows.Count - 1; i > 0; i--)
            {
                var needRemove = true;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j] != null)
                    {
                        if (dt.Rows[i][j].ToString() != string.Empty)
                        {
                            needRemove = false;
                            goto LoadEnd;
                        }
                    }
                }

                if (needRemove)
                {
                    dt.Rows[i].Delete();
                }
            }

            LoadEnd:

            return dt;
        }

        /// <summary>
        /// 把某个DataTable 导入到某个Excel
        /// </summary>
        /// <param name="outputFile">目标文件</param>
        /// <param name="sheetName">Sheet名</param>
        /// <param name="dt">需要到处的table</param>
        public static void InsertSheet(string outputFile, string sheetName, DataTable dt)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            var hssfworkbook = WorkbookFactory.Create(readfile);

            // HSSFWorkbook hssfworkbook = new HSSFWorkbook(readfile);  
            var num = hssfworkbook.GetSheetIndex(sheetName);
            ISheet sheet1;
            if (num >= 0)
                sheet1 = hssfworkbook.GetSheet(sheetName);
            else
            {
                sheet1 = hssfworkbook.CreateSheet(sheetName);
            }

            try
            {
                if (sheet1.GetRow(0) == null)
                {
                    sheet1.CreateRow(0);
                }

                for (var coluid = 0; coluid < dt.Columns.Count; coluid++)
                {
                    if (sheet1.GetRow(0).GetCell(coluid) == null)
                    {
                        sheet1.GetRow(0).CreateCell(coluid);
                    }

                    sheet1.GetRow(0).GetCell(coluid).SetCellValue(dt.Columns[coluid].ColumnName);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                throw;
            }

            for (var i = 1; i <= dt.Rows.Count; i++)
            {
                try
                {
                    if (sheet1.GetRow(i) == null)
                    {
                        sheet1.CreateRow(i);
                    }

                    for (var coluid = 0; coluid < dt.Columns.Count; coluid++)
                    {
                        if (sheet1.GetRow(i).GetCell(coluid) == null)
                        {
                            sheet1.GetRow(i).CreateCell(coluid);
                        }

                        sheet1.GetRow(i).GetCell(coluid).SetCellValue(dt.Rows[i - 1][coluid].ToString());
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());

                    // throw;  
                }
            }

            try
            {
                readfile.Close();
                var writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 加载数据到dataset
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static DataSet ReadDataToDs(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            DataSet ds = new DataSet(fi.Name);
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                // IWorkbook workbook = new HSSFWorkbook(file);
                IWorkbook workbook = WorkbookFactory.Create(file);
                var sheetCount = workbook.NumberOfSheets;
                for (int i = 0; i < sheetCount; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);
                    var tab = RenderDataTableFromsheet(sheet, 0);
                    ds.Tables.Add(tab);
                }

                return ds;
            }
        }

        /// <summary>
        /// 加载数据到dataset
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <returns></returns>
        public static DataSet ReadDataToDs(Stream fileStream)
        {
            DataSet ds = new DataSet();

            // IWorkbook workbook = new HSSFWorkbook(file);
            IWorkbook workbook = WorkbookFactory.Create(fileStream);
            var sheetCount = workbook.NumberOfSheets;
            for (int i = 0; i < sheetCount; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                var tab = RenderDataTableFromsheet(sheet, 0);
                ds.Tables.Add(tab);
            }

            return ds;
        }

        /// <summary>
        ///     从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, string sheetName, int headerRowIndex)
        {
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                // IWorkbook workbook = new HSSFWorkbook(file);
                IWorkbook workbook = WorkbookFactory.Create(file);
                return RenderDataTableFromExcel(workbook, sheetName, headerRowIndex);
            }
        }

        /// <summary>
        ///     从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="strFileName">Excel文件全路径(服务器路径)</param>
        /// <param name="sheetIndex">要获取数据的工作表序号(从0开始)</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        public static DataTable RenderDataTableFromExcel(string strFileName, int sheetIndex, int headerRowIndex)
        {
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                // IWorkbook workbook = new HSSFWorkbook(file);
                IWorkbook workbook = WorkbookFactory.Create(file);
                var sheetName = workbook.GetSheetName(sheetIndex);
                return RenderDataTableFromExcel(workbook, sheetName, headerRowIndex);
            }
        }

        /// <summary>
        ///     更新Excel表格限定于Excel2003
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colId">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetName, string[] updateData, int colId, int rowId)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);
            var hssfworkbook = new HSSFWorkbook(readfile);
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowId) == null)
                    {
                        sheet1.CreateRow(i + rowId);
                    }

                    if (sheet1.GetRow(i + rowId).GetCell(colId) == null)
                    {
                        sheet1.GetRow(i + rowId).CreateCell(colId);
                    }

                    sheet1.GetRow(i + rowId).GetCell(colId).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    throw;
                }
            }

            readfile.Close();

            var writefile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            hssfworkbook.Write(writefile);
            writefile.Flush();
            writefile.Close();
            writefile.Dispose();
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colIds">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public static void UpdateExcel(
            string outputFile,
            string sheetName,
            string[][] updateData,
            int[] colIds,
            int rowId)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var j = 0; j < colIds.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowId) == null)
                        {
                            sheet1.CreateRow(i + rowId);
                        }

                        if (sheet1.GetRow(i + rowId).GetCell(colIds[j]) == null)
                        {
                            sheet1.GetRow(i + rowId).CreateCell(colIds[j]);
                        }

                        sheet1.GetRow(i + rowId).GetCell(colIds[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }

            try
            {
                var writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colId">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public static void UpdateExcel(string outputFile, string sheetName, double[] updateData, int colId, int rowId)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowId) == null)
                    {
                        sheet1.CreateRow(i + rowId);
                    }

                    if (sheet1.GetRow(i + rowId).GetCell(colId) == null)
                    {
                        sheet1.GetRow(i + rowId).CreateCell(colId);
                    }

                    sheet1.GetRow(i + rowId).GetCell(colId).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    throw;
                }
            }

            try
            {
                readfile.Close();
                var writefile = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="outputFile">需更新的excel表格路径</param>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colIds">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public static void UpdateExcel(
            string outputFile,
            string sheetName,
            double[][] updateData,
            int[] colIds,
            int rowId)
        {
            var readfile = new FileStream(outputFile, FileMode.Open, FileAccess.Read);

            var hssfworkbook = new HSSFWorkbook(readfile);
            readfile.Close();
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var j = 0; j < colIds.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowId) == null)
                        {
                            sheet1.CreateRow(i + rowId);
                        }

                        if (sheet1.GetRow(i + rowId).GetCell(colIds[j]) == null)
                        {
                            sheet1.GetRow(i + rowId).CreateCell(colIds[j]);
                        }

                        sheet1.GetRow(i + rowId).GetCell(colIds[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }

            try
            {
                var writefile = new FileStream(outputFile, FileMode.Create);
                hssfworkbook.Write(writefile);
                writefile.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            var writeFile = new FileStream(_saveAsFile, FileMode.Create);
            hssfworkbook.Write(writeFile);
            writeFile.Close();
            writeFile.Dispose();
            return true;
        }

        /// <summary>
        /// 获取sheet名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetSheetName()
        {
            var arrayList = new List<string>();
            try
            {
                for (var i = 0; i < hssfworkbook.NumberOfSheets; i++)
                {
                    arrayList.Add(hssfworkbook.GetSheetName(i));
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.ToString());
            }

            return arrayList;
        }

        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="imgPath">图片路径</param>
        /// <param name="colId">列</param>
        /// <param name="rowId">行</param>
        /// <param name="endCol">结束列</param>
        /// <param name="endRow">结束行</param>
        public void InsertPng(string sheetName, string imgPath, int colId, int rowId, int endCol, int endRow)
        {
            byte[] content = File.ReadAllBytes(imgPath);
            int pictureIdx = hssfworkbook.AddPicture(content, PictureType.PNG);

            var sheet1 = hssfworkbook.GetSheet(sheetName);
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet1.CreateDrawingPatriarch();

            // ##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, colId, rowId, endCol, endRow);
            HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
            pict.Resize();
        }

        /// <summary>
        ///     更新Excel表格限定于Excel2003
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colId">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public void UpdateExcel(string sheetName, string[] updateData, int colId, int rowId)
        {
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowId) == null)
                    {
                        sheet1.CreateRow(i + rowId);
                    }

                    if (sheet1.GetRow(i + rowId).GetCell(colId) == null)
                    {
                        sheet1.GetRow(i + rowId).CreateCell(colId);
                    }

                    sheet1.GetRow(i + rowId).GetCell(colId).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    throw;
                }
            }

            hssfworkbook.ForceFormulaRecalculation = true;
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colIds">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public void UpdateExcel(string sheetName, string[][] updateData, int[] colIds, int rowId)
        {
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var j = 0; j < colIds.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowId) == null)
                        {
                            sheet1.CreateRow(i + rowId);
                        }

                        if (sheet1.GetRow(i + rowId).GetCell(colIds[j]) == null)
                        {
                            sheet1.GetRow(i + rowId).CreateCell(colIds[j]);
                        }

                        sheet1.GetRow(i + rowId).GetCell(colIds[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colId">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public void UpdateExcel(string sheetName, double[] updateData, int colId, int rowId)
        {
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var i = 0; i < updateData.Length; i++)
            {
                try
                {
                    if (sheet1.GetRow(i + rowId) == null)
                    {
                        sheet1.CreateRow(i + rowId);
                    }

                    if (sheet1.GetRow(i + rowId).GetCell(colId) == null)
                    {
                        sheet1.GetRow(i + rowId).CreateCell(colId);
                    }

                    sheet1.GetRow(i + rowId).GetCell(colId).SetCellValue(updateData[i]);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        /// <summary>
        ///     更新Excel表格Excel2003
        /// </summary>
        /// <param name="sheetName">sheet名</param>
        /// <param name="updateData">需更新的数据</param>
        /// <param name="colIds">需更新的列号</param>
        /// <param name="rowId">需更新的开始行号</param>
        public void UpdateExcel(string sheetName, double[][] updateData, int[] colIds, int rowId)
        {
            var sheet1 = hssfworkbook.GetSheet(sheetName);
            for (var j = 0; j < colIds.Length; j++)
            {
                for (var i = 0; i < updateData[j].Length; i++)
                {
                    try
                    {
                        if (sheet1.GetRow(i + rowId) == null)
                        {
                            sheet1.CreateRow(i + rowId);
                        }

                        if (sheet1.GetRow(i + rowId).GetCell(colIds[j]) == null)
                        {
                            sheet1.GetRow(i + rowId).CreateCell(colIds[j]);
                        }

                        sheet1.GetRow(i + rowId).GetCell(colIds[j]).SetCellValue(updateData[j][i]);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                }
            }
        }

        /// <summary>  
        /// 由DataSet导出Excel  
        /// </summary>  
        /// <param name="sourceDs">要导出数据的DataSet</param>  
        /// <param name="sheetName">工作表名称</param>  
        /// <returns>Excel工作表</returns>  
        private static MemoryStream ExportDataSetToExcel(DataSet sourceDs, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            string[] sheetNames = sheetName.Split(',');
            for (int i = 0; i < sheetNames.Length; i++)
            {
                ISheet sheet = workbook.CreateSheet(sheetNames[i]);

                IRow headerRow = sheet.CreateRow(0);
                HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                HSSFFont font = workbook.CreateFont() as HSSFFont;
                font.FontHeightInPoints = 10;
                font.Boldweight = 700;
                headStyle.SetFont(font);

                // 取得列宽  
                int[] arrColWidth = new int[sourceDs.Tables[i].Columns.Count];
                foreach (DataColumn item in sourceDs.Tables[i].Columns)
                {
                    arrColWidth[item.Ordinal] = Encoding.Default.GetBytes(item.ColumnName.ToString()).Length;
                }

                // 处理列头  
                foreach (DataColumn column in sourceDs.Tables[i].Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                    // 设置列宽  
                    sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                }



                int rowIndex = 1;
                foreach (DataRow row in sourceDs.Tables[i].Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in sourceDs.Tables[i].Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }


            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            workbook = null;
            return ms;
        }

        /// <summary>
        ///     从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="workbook">要处理的工作薄</param>
        /// <param name="sheetName">要获取数据的工作表名称</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <returns></returns>
        private static DataTable RenderDataTableFromExcel(IWorkbook workbook, string sheetName, int headerRowIndex)
        {
            var sheet = workbook.GetSheet(sheetName);

            DataTable dt = RenderDataTableFromsheet(sheet, headerRowIndex);
            workbook = null;
            return dt;
        }

        /// <summary>
        ///     从Excel中获取数据到DataTable
        /// </summary>
        /// <param name="sheet">要获取数据的工作表</param>
        /// <param name="headerRowIndex">工作表标题行所在行号(从0开始)</param>
        /// <param name="removeEmpty">是否移除空行</param>
        /// <returns></returns>
        private static DataTable RenderDataTableFromsheet(ISheet sheet, int headerRowIndex, bool removeEmpty = false)
        {
            // var sheet = workbook.GetSheet(sheetName);
            var table = new DataTable(sheet.SheetName);
            try
            {
                var headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.Cells.Count;

                for (int i = 0; i < cellCount; i++)
                {
                    var cell = headerRow.Cells[i];
                    var cellHeard = cell == null ? i.ToString() : cell.ToString().Trim();
                    if (cellHeard == string.Empty || table.Columns.Contains(cellHeard))
                        cellHeard = string.Format("{0}{1}{2}", cellHeard, i, DateTime.Now.ToString("ssfff"));
                    table.Columns.Add(cellHeard);
                }

                for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    var dataRow = table.NewRow();
                    bool isAdd = false;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        var cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dataRow[j >= 0 ? j : 0] = null;
                        }
                        else
                        {
                            try
                            {


                                // dataRow[j] = cell.ToString();   
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        dataRow[j] = null;
                                        break;
                                    case CellType.Boolean:
                                        dataRow[j] = cell.BooleanCellValue;
                                        break;
                                    case CellType.Numeric:
                                        var str1 = cell.CellStyle.GetDataFormatString();
                                        if (str1.Contains("yy") || str1.Contains("mm") || str1.Contains("dd")
                                            || str1.Contains("hh"))
                                        {
                                            dataRow[j] = cell.DateCellValue;
                                        }
                                        else
                                            dataRow[j] = cell.NumericCellValue;

                                        break;
                                    case CellType.String:
                                        dataRow[j] = cell.StringCellValue;
                                        break;
                                    case CellType.Error:
                                        dataRow[j] = cell.ErrorCellValue;
                                        break;
                                    case CellType.Formula:
                                        {
                                            switch (cell.CachedFormulaResultType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = null;
                                                    break;
                                                case CellType.Boolean:
                                                    dataRow[j] = cell.BooleanCellValue;
                                                    break;
                                                case CellType.Numeric:
                                                    var str = cell.CellStyle.GetDataFormatString();
                                                    if (str.Contains("yy") || str.Contains("mm") || str.Contains("dd")
                                                        || str.Contains("hh"))
                                                    {
                                                        dataRow[j] = cell.DateCellValue;
                                                    }
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;

                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                                case CellType.Error:
                                                    dataRow[j] = cell.ErrorCellValue;
                                                    break;
                                                default:
                                                    dataRow[j] = cell.ToString();
                                                    break;
                                            }

                                            break;
                                        }

                                    default:
                                        dataRow[j] = cell.ToString();
                                        break;
                                }

                                if (dataRow[j] != null) isAdd = true;


                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(
                                    string.Format("{0},row {1}, col {2},{3}", table.TableName, i, j, ex.Message));
                                dataRow[j] = cell.ToString();
                            }
                        }
                    }

                    if (!isAdd && removeEmpty)
                        continue;
                    table.Rows.Add(dataRow);

                    // dataRow[j] = row.GetCell(j).ToString();   
                }
            }
            catch (Exception ex)
            {
                table.Clear();
                table.Columns.Clear();
                table.Columns.Add("出错了");
                var dr = table.NewRow();
                dr[0] = ex.Message;
                table.Rows.Add(dr);
                return table;
            }
            finally
            {
                // sheet.Dispose();   
                sheet = null;
            }

            for (var i = table.Rows.Count - 1; i > 0; i--)
            {
                var needRemove = true;
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    if (table.Rows[i][j] != null)
                    {
                        if (table.Rows[i][j].ToString() != string.Empty)
                        {
                            needRemove = false;
                            goto LoadEnd;
                        }
                    }
                }

                if (needRemove)
                {
                    table.Rows[i].Delete();
                }
            }

            LoadEnd:

            return table;
        }

        /// <summary>
        /// 插入空白行
        /// </summary>
        private void InsertRow(int start, int rowCount)
        {
            var sheet = hssfworkbook.GetSheetAt(0);
            sheet.ShiftRows(start, rowCount, 2);
        }

        /// <summary>
        /// List转datatable
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="items">值</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                string colName = prop.Name;

                Type t = GetCoreType(prop.PropertyType);
                var targetAttribute = prop.GetCustomAttribute(typeof(DescriptionAttribute));
                if (targetAttribute != null)
                {
                    DescriptionAttribute descAttribute = targetAttribute as DescriptionAttribute;
                    if (descAttribute != null)
                    {
                        colName = descAttribute.Description;
                    }
                }

                tb.Columns.Add(colName, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="t">类型</param>
        /// <returns>Type</returns>
        private static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        /// <summary>
        /// 类型是否为空
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>bool</returns>
        private static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }

}