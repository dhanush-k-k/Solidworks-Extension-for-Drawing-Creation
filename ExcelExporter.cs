/*************************************************************************************
 * Term Project     : Solidworks Extension to implement automation of reading a 3D   *
 *                    model, creating individual parts and assembly file from them,  *
 *                    creating drawing and exporting the BOM with picture to an      *
 *                    excel sheet.                                                   *
 * Team Mates       : Dhanush, Prashanth, Nikhil                                     *
 * File Name        : AssemblyReading.cs                                             *                            
 ************************************************************************************/

/// *******************************USINGS*********************************************
using System;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

/// *******************************NAMESPACE*******************************************
/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
/// **********************************CLASS********************************************
    /// <summary>
    /// Class:   ExcelExporter
    /// Purpose: Define suitable fiels, properties and methods required to export BOM information to Excel sheet
    /// </summary>
    public class ExcelExporter : IDisposable
    {
        #region Private Fields

        /// <summary>
        /// All private variables listed below are started with (_) as they share similar names with Excel objects
        /// </summary>
        private string _templatePath = string.Empty;
        private Excel.Application _application;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _worksheet;
        private Excel.Range _range;
        private Excel.Font _font;
        private Excel.Borders _borders;
        private Excel.Border _leftBorder;
        private Excel.Border _topBorder;
        private Excel.Border _rightBorder;
        private Excel.Border _bottomBorder;
        private int _sheetIndex = 1;
        private bool _isDisposed = false;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property: Get or Set sheet index
        /// </summary>
        public int SheetIndex
        {
            get
            {
                return this._sheetIndex;
            }
            set
            {
                if(value < 1 || value > this._workbook.Sheets.Count)
                {
                    value = 1;
                }
                this._sheetIndex = value;
                this._worksheet = this._workbook.Sheets[value];
                this._range = null;
                this._font = null;
            }
        }

        /// <summary>
        /// Property: Get or Set Exporting File name
        /// </summary>
        public string ExportFileName
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor to create new Excel sheet
        /// </summary>
        public ExcelExporter()
        {
            this._application = new Excel.Application();
            this._workbook = this._application.Workbooks.Add();
            this._sheetIndex = 1;
            this._worksheet = this._workbook.Sheets[this._sheetIndex];
        }

        /// <summary>
        /// Constructor to create Excel sheet using an existing template
        /// </summary>
        /// <param name="templatePath"> path of the template - type string </param>
        public ExcelExporter(string templatePath)
        {
            this._templatePath = templatePath;
            this._application = new Excel.Application();
            this._workbook = this._application.Workbooks.Open(this._templatePath);
            this._sheetIndex = 1;
            this._worksheet = this._workbook.Sheets[this._sheetIndex];
        }

        /// <summary>
        /// Destructor. For memory cleanup and releasing unmanaged resources
        /// </summary>
        ~ExcelExporter()
        {
            this.Dispose();
        }

        #endregion

        #region Methods
        /// Multiple overridden methods are defined to account for all possible cases.
        /// For instance, setting font values for an individual cell or a row of cells or column of cells or from starting row/column to end row/column.
        /// This is done keeping in mind the scalability of the project

        #region

        /// <summary>
        /// Method:  SetRange
        /// Purpose: Setting range of cells in Excel sheet with top, bottom, left and right borders
        /// </summary>
        /// <param name="row"> Number of rows - int type </param>
        /// <param name="col"> Number of columns - int type </param>
        /// <return> Nil </return>
        public void SetRange(int row, int col)
        {
            this._range = this._worksheet.Cells[row, col];
            this._font = this._range.Font;
            this._borders = this._range.Borders;
            this._leftBorder = this._borders[Excel.XlBordersIndex.xlEdgeLeft];
            this._topBorder = this._borders[Excel.XlBordersIndex.xlEdgeTop];
            this._rightBorder = this._borders[Excel.XlBordersIndex.xlEdgeRight];
            this._bottomBorder = this._borders[Excel.XlBordersIndex.xlEdgeBottom];
        }

        /// <summary>
        /// Method:  SetRange
        /// Purpose: Setting range of cells in Excel sheet with start and end row/coloum along with the 4 borders
        /// </summary>
        /// <param name="startRow"> Starting row number - int type </param>
        /// <param name="startCol"> Starting column number - int type </param>
        /// <param name="endRow"> End row number - int type </param>
        /// <param name="endCol"> End column number - int type </param>
        /// <return> Nil </return>
        public void SetRange(int startRow, int startCol, int endRow, int endCol)
        {
            this._range = this._worksheet.Range[this._worksheet.Cells[startRow, startCol], this._worksheet.Cells[endRow, endCol]];
            this._font = this._range.Font;
            this._borders = this._range.Borders;
            this._leftBorder = this._borders[Excel.XlBordersIndex.xlEdgeLeft];
            this._topBorder = this._borders[Excel.XlBordersIndex.xlEdgeTop];
            this._rightBorder = this._borders[Excel.XlBordersIndex.xlEdgeRight];
            this._bottomBorder = this._borders[Excel.XlBordersIndex.xlEdgeBottom];
        }

        #endregion

        #region

        /// <summary>
        /// Method:  CellValue
        /// Purpose:Adding the required data in Excel
        /// </summary>
        /// <param name="row"> Row number - int Type </param>
        /// <param name="col"> Coloumn number - int type </param>
        /// <param name="value"> value to assigned to cell in Excel - object type </param>
        /// <return> Nil </return>
        public void CellValue(int row, int col, object value)
        {
            this.SetRange(row, col);
            this._range.Value2 = value;
        }

        /// <summary>
        /// Method:  CellValue
        /// Purpose: Merging the necessary cells
        /// </summary>
        /// <param name="startRow"> Starting row number - int type </param>
        /// <param name="startCol"> Starting coloumn number - int type </param>
        /// <param name="endRow"> Ending row number - int type </param>
        /// <param name="endCol"> Ending coloumn number - int type </param>
        /// <param name="value"> value to assigned to cell in Excel - object type </param>
        /// <return> Nil </return>
        public void SetCellValue(int startRow, int startCol, int endRow, int endCol, object value)
        {
            this.MergeCells(startRow, startCol, endRow, endCol);
            this._range.Value2 = value;
            this._range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            this._range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        #endregion

        #region

        /// <summary>
        /// Method:  SetFont
        /// Purpose: Setting Font features - type, size for a cell
        /// </summary>
        /// <param name="fontName"> Name of the font to be set - string type </param>
        /// <param name="fontSize">  Size of the font to be set - int type </param>
        /// <param name="isBold"> To make font bold - bool type </param>
        /// <return> Nil </return>
        public void SetFont(string fontName, int fontSize, bool isBold)
        {
            if (!string.IsNullOrWhiteSpace(fontName))
            {
                this._font.Name = fontName;
            }
            if (fontSize > -1)
            {
                this._font.Size = fontSize;
            }
            if (isBold)
            {
                this._font.Bold = isBold;
            }

        }

        /// <summary>
        /// Method:  SetFont
        /// Purpose: Setting Font features - type, size
        /// </summary>
        /// <param name="row"> Row number of the cell - int type </param>
        /// <param name="col"> Coloumn number of the cell - int type </param>
        /// <param name="fontName"> Name of the font to be set - string type </param>
        /// <param name="fontSize">  Size of the font to be set - int type </param>
        /// <param name="isBold"> To make font bold - bool type </param>
        /// <return> Nil </return>
        public void SetFont(int row, int col, string fontName, int fontSize, bool isBold)
        {
            this.SetRange(row, col);
            this.SetFont(fontName, fontSize, isBold);
        }

        /// <summary>
        /// Method:  SetFont
        /// Purpose: Setting Font features - type, size and bold with range from starting to end coloum/row
        /// </summary>
        /// <param name="startRow"> Starting row number - int type </param>
        /// <param name="startCol"> Starting coloumn number - int type </param>
        /// <param name="endRow"> Ending row number - int type </param>
        /// <param name="endCol"> Ending coloumn number - int type </param>
        /// <param name="fontName"> Name of the font to be set - string type </param>
        /// <param name="fontSize">  Size of the font to be set - int type </param>
        /// <param name="isBold"> To make font bold - bool type </param>
        /// <return> Nil </return>
        public void SetFont(int startRow, int startCol, int endRow, int endCol, string fontName, int fontSize, bool isBold)
        {
            this.SetRange(startRow, startCol, endRow, endCol);
            this.SetFont(fontName, fontSize, isBold);
        }

        #endregion

        #region

        /// <summary>
        /// Method:  RowHeight
        /// Purpose: Setting row height for a particular cell
        /// </summary>
        /// <param name="height"> height of the cell in Excel sheet - double type </param>
        /// <return> Nil </return>
        public void SetRowHeight(double height)
        {
            if (height > 0)
            {
                this._range.RowHeight = height;
            }
        }

        /// <summary>
        /// Method:  RowHeight
        /// Purpose: Setting row height for cells in particular row of cells
        /// </summary>
        /// <param name="row"> Row number of the cells - int type </param>
        /// <param name="height"> height of the cell in Excel sheet - double type </param>
        /// <return> Nil </return>
        public void SetRowHeight(int row, double height)
        {
            this.SetRange(row, 1);
            this.SetRowHeight(height);
        }

        #endregion

        #region

        /// <summary>
        /// Method:  SetColumnWidth
        /// Purpose: Setting column width for cells in particular range of cells
        /// </summary>
        /// <param name="width"></param>
        public void SetColumnWidth(double width)
        {
            this._range.ColumnWidth = width;
        }

        /// <summary>
        /// Method:  SetColumnWidth
        /// Purpose: Setting column width for cells in particular column of cells
        /// </summary>
        /// <param name="col"> Column number of the cells - int type </param>
        /// <param name="width"> Width of the cell in Excel sheet - double type </param>
        public void SetColumnWidth(int col, double width)
        {
            this.SetRange(1, col);
            this.SetColumnWidth(width);
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  RowAutoFit
        /// Purpose: Auto fitting of copied data of a row
        /// </summary>
        /// <param> nil </param>
        public void RowAutoFit()
        {
            this._range.EntireRow.AutoFit();
        }

        /// <summary>
        /// Method:  RowAutoFit
        /// Purpose: Auto fitting of copied data of a particular row
        /// </summary>
        /// <param name="row"> Row number of the cells - Type int </param>
        public void RowAutoFit(int row)
        {
            this.SetRange(row, 1);
            this.RowAutoFit();
        }

        /// <summary>
        /// Method:  RowAutoFit
        /// Purpose: Auto fitting of copied data of a range of rows
        /// </summary>
        /// <param name="startRow"> Starting row number of the cells - Type int </param>
        /// <param name="endrow"> Starting row number of the cells - Type int </param>
        public void RowAutoFit(int startRow, int endRow)
        {
            this.SetRange(startRow, 1, endRow, 1);
            this.RowAutoFit();
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  ColumnAutoFit
        /// Purpose: Auto fitting of copied data of a column
        /// </summary>
        /// <param> nil </param>
        public void ColumnAutoFit()
        {
            this._range.EntireColumn.AutoFit();
        }

        /// <summary>
        /// Method:  ColumnAutoFit
        /// Purpose: Auto fitting of copied data of a particular column
        /// </summary>
        /// <param name="col"> Column number of the cells - Type int </param>
        public void ColumnAutoFit(int col)
        {
            this.SetRange(1, col);
            this.ColumnAutoFit();
        }

        /// <summary>
        /// Method:  ColumnAutoFit
        /// Purpose: Auto fitting of copied data of a range of columns
        /// </summary>
        /// <param name="startCol"> Starting column number of the cells - Type int </param>
        /// <param name="endCol"> Starting column number of the cells - Type int </param>
        public void ColumnAutoFit(int startCol, int endCol)
        {
            this.SetRange(1, startCol, 1, endCol);
            this.ColumnAutoFit();
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  SetAliment
        /// Purpose: Horizontal and vertical alignment of data in a cell
        /// </summary>
        /// <param name="horizontalAlignment"> </param>
        /// <param name="verticalAlignment"> </param>
        public void SetAliment(Excel.XlHAlign horizontalAlignment, Excel.XlVAlign verticalAlignment)
        {
            this._range.HorizontalAlignment = horizontalAlignment;
            this._range.VerticalAlignment = verticalAlignment;
        }

        /// <summary>
        /// Method:  SetAliment
        /// Purpose: Horizontal and vertical alignment of data in a range of row and column
        /// </summary>
        /// <param name="row"> Row number of the cells - Type int </param>
        /// <param name="col"> Column number of the cells - Type int </param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        public void SetAliment(int row, int col, Excel.XlHAlign horizontalAlignment, Excel.XlVAlign verticalAlignment)
        {
            this.SetRange(row, col);
            this.SetAliment(horizontalAlignment, verticalAlignment);
        }

        /// <summary>
        /// Method:  SetDefaultAliment
        /// Purpose: Horizontal and vertical alignment of data set to default
        /// </summary>
        /// <param name="row"> Row number of the cells - Type int </param>
        /// <param name="col"> Column number of the cells - Type int </param>
        public void SetDefaultAliment(int row, int col)
        {
            Excel.XlHAlign horizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            Excel.XlVAlign verticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            this.SetRange(row, col);
            this.SetAliment(horizontalAlignment, verticalAlignment);
        }

        /// <summary>
        /// Method:  SetAliment
        /// Purpose: Horizontal and vertical alignment of data of cells from particular row/column to another
        /// </summary>
        /// <param name="startRow"> Starting row of the cells - Type int </param>
        /// <param name="startCol"> Starting column of the cells - Type int </param>
        /// <param name="endRow"> Ending row of the cells - Type int </param>
        /// <param name="endCol"> Ending column of the cells - Type int </param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        public void SetAliment(int startRow, int startCol, int endRow, int endCol, Excel.XlHAlign horizontalAlignment, Excel.XlVAlign verticalAlignment)
        {
            this.SetRange(startRow, startCol, endRow, endCol);
            this.SetAliment(horizontalAlignment, verticalAlignment);
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  SetBorders
        /// Purpose: Setting different borders - left, right top & bottom with default or different styles
        /// </summary>
        /// <param name="style"> </param>

        public void SetBorders(Excel.XlLineStyle style)
        {
            this._range.Borders.LineStyle = style;
        }

        public void SetBorders(int row, int col, Excel.XlLineStyle style)
        {
            this.SetRange(row, col);
            this.SetBorders(style);
        }
        public void SetBordersDefaultStyle(int row, int col)
        {
            Excel.XlLineStyle style = Excel.XlLineStyle.xlContinuous;
            this.SetRange(row, col);
            this.SetBorders(style);
        }

        public void SetBorders(int startRow, int startCol, int endRow, int endCol, Excel.XlLineStyle style)
        {
            this.SetRange(startRow, startCol, endRow, endCol);
            this.SetBorders(style);
        }
        public void SetBordersDefaultStyle(int startRow, int startCol, int endRow, int endCol)
        {
            Excel.XlLineStyle style = Excel.XlLineStyle.xlContinuous;
            this.SetRange(startRow, startCol, endRow, endCol);
            this.SetBorders(style);
        }

        public void SetLeftBorder(Excel.XlLineStyle style)
        {
            this._leftBorder.LineStyle = style;
        }

        public void SetLeftBorder(int row, int col, Excel.XlLineStyle style)
        {
            this.SetRange(row, col);
            this.SetLeftBorder(style);
        }

        public void SetTopBorder(Excel.XlLineStyle style)
        {
            this._topBorder.LineStyle = style;
        }

        public void SetTopBorder(int row, int col, Excel.XlLineStyle style)
        {
            this.SetRange(row, col);
            this.SetTopBorder(style);
        }

        public void SetRightBorder(Excel.XlLineStyle style)
        {
            this._rightBorder.LineStyle = style;
        }

        public void SetRightBorder(int row, int col, Excel.XlLineStyle style)
        {
            this.SetRange(row, col);
            this.SetRightBorder(style);
        }

        public void SetBottomBorder(Excel.XlLineStyle style)
        {
            this._bottomBorder.LineStyle = style;
        }

        public void SetBottomBorder(int row, int col, Excel.XlLineStyle style)
        {
            this.SetRange(row, col);
            this.SetBottomBorder(style);
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  MergeCells
        /// Purpode: To merge cells from selected cells 
        /// </summary>
        /// <param name="startRow"> Starting row number - int type </param>
        /// <param name="startCol"> Starting coloumn number - int type </param>
        /// <param name="endRow"> Ending row number - int type </param>
        /// <param name="endCol"> Ending coloumn number - int type </param>
        /// <return> Nil </return>
        public void MergeCells(int startRow, int startCol, int endRow, int endCol)
        {
            try
            {
                this.SetRange(startRow, startCol, endRow, endCol);
                this._range.Merge();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region 

        /// <summary>
        /// Method:  SaveAs
        /// Purpose: Function for saving the file
        /// </summary>
        /// <param name="fileName"> file name - string type </param>
        /// <param name="isPrint"> Printing the file option - bool type </param>
        /// <param name="iPdfFileName"> pdf file name - string type </param>
        /// <return> Nil </return>
        public void SaveAs(string fileName, bool isPrint, string iPdfFileName = null)
        {
            try
            {

                this._application.DisplayAlerts = false;
                this._workbook.SaveCopyAs(fileName);
                if (isPrint)
                {
                    if (iPdfFileName != null)
                    {
                        this._workbook.ExportAsFixedFormat(
                        Excel.XlFixedFormatType.xlTypePDF,
                        iPdfFileName, OpenAfterPublish: true);
                    }
                }
                this._workbook.Close();
                this._application.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #endregion

        #region Disposable

        /// <summary>
        /// Method:  Dispose
        /// Purpose: Memory cleanup and releasing unmanaged resources.
        /// </summary>
        /// <param> Nil </param>
        /// <return> Nil </return>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method:  Dispose
        /// Purpose: Memory cleanup and releasing unmanaged resources.
        /// </summary>
        /// <param name="dispose"> checking if the object is already disposed - bool type </param>
        /// <return> Nil </return>
        protected virtual void Dispose(bool dispose)
        {
            if (this._isDisposed)
            {
                return;
            }
            if (dispose)
            {

            }

            Marshal.ReleaseComObject(this._leftBorder);
            Marshal.ReleaseComObject(this._topBorder);
            Marshal.ReleaseComObject(this._rightBorder);
            Marshal.ReleaseComObject(this._bottomBorder);
            Marshal.ReleaseComObject(this._borders);
            Marshal.ReleaseComObject(this._font);
            Marshal.ReleaseComObject(this._range);
            Marshal.ReleaseComObject(this._worksheet);
            Marshal.ReleaseComObject(this._workbook);
            Marshal.ReleaseComObject(this._application);

            this._isDisposed = true;
        }

        #endregion

    }
}
