/****************************************************************************************
 * Term Project     : Solidworks Extension to implement automation of reading a 3D model,*
 *                    creating drawing and exporting the BOM with picture to an excel    *
 *                    sheet and UI to create parts and assembly of BUSH.                 *                                                    
 * Team Mates       : Dhanush, Prashanth, Nikhil                                         *                                                     
 * File Name        : ExportBom.cs                                                       *                                         
 ****************************************************************************************/

/// *************************************USINGS*******************************************
/// System namespace defines commonly-used values and reference data types
/// SolidWorks.Interop provides access to all solidworks attributes/properties/features through the API
/// Xarial.Xcad.Solidwroks enables the abstraction layers for the CAD API allowing CAD agnostic development
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swdocumentmgr;
using Xarial.XCad.SolidWorks;

/// <summary>
/// Namespace: SwApp 
/// </summary>
namespace SwApp
{
    /// <summary>
    /// Class: ExportBom
    /// Purpose: This class file contains methods that will read all BOM Table information such as the active model
    ///          view features and sub-Features, table annotations and boundaru conditions.
    /// </summary>
    public class ExportBom
    {
        /// This will allow access to solidworks assemblies documents and drawings.
        public ModelDoc2 ActiveAssemblyDoc = null;

        /// This will allow access to functions that perform drawing operations.
        public DrawingDoc ActiveDwg = null;

        /// This will provide access to interface exposed in Solidworks API.
        private readonly ISldWorks _sw = null;

        /// <summary>
        /// Constructor: ExportBom
        /// Purpose: This will be called when object of class ExportBom is created to initilize the data members
        /// </summary>
        /// <param name="application"></param>
        /// <param name="iActiveDoc"></param>
        public ExportBom(ISwApplication application, ModelDoc2 iActiveDoc)
        {
            ActiveAssemblyDoc = iActiveDoc;
            ActiveDwg = iActiveDoc as DrawingDoc;
            _sw = application.Sw;
        }

        /// <summary>
        /// Method: ExportBomToExcel
        /// Purpose: Extract features from model and table annotations for use in drawing sheet
        /// </summary>
        public void ExportBomToExcel()
        {
            /// Declarations
            string[] TableNames = null;

            /// Activates the specified drawing view, IsoView from the active model document
            ActiveDwg.ActivateView("IsoView");
            ModelDoc2 Model = ActiveDwg as ModelDoc2;

            /// Obtain the details of current sheet
            Sheet DrwSheet = default(Sheet);
            DrwSheet = (Sheet)ActiveDwg.GetCurrentSheet();

            /// This will allows access to the model document.
            ModelDocExtension ModelDocExt = default(ModelDocExtension);

            /// Setting default bool status to false
            bool status = false;
            ModelDocExt = (ModelDocExtension)Model.Extension;

            /// This allows access to the model.
            status = ModelDocExt.SelectByID2("Sheet1", "SHEET", 0, 0, 0, false, 0, null, 0);

            /// Obtain the views from the drawing sheet
            object[] Views = null;
            Views = (object[])DrwSheet.GetViews();

            foreach (object vView in Views)
            {
                /// Obtaining the default view and features
                View DrwView = default(View);
                DrwView = (View)vView;
                Feature ViewFeature = default(Feature);
                ViewFeature = (Feature)ActiveDwg.FeatureByName(DrwView.Name);

                /// Traverse the features in the view
                Feature SubFeature = default(Feature);
                SubFeature = (Feature)ViewFeature.GetFirstSubFeature();

                while (!(SubFeature == null))
                {
                    string TableType = SubFeature.GetTypeName2();
                    if (TableType == "HoleTableFeat")
                    {
                        HoleTable SwHoleTable = default(HoleTable);

                        /// Hole table features are extracted here
                        SwHoleTable = (HoleTable)SubFeature.GetSpecificFeature2();
                        object[] HoleTables = null;
                        HoleTables = (object[])SwHoleTable.GetTableAnnotations();
                    }

                }
            }

            /// Initializing the active drawing object
            View ActiveDrawingView = ActiveDwg.ActiveDrawingView as View;
            if (ActiveDrawingView != null)
            {
                /// Active sheet in drawing is obtained
                Sheet CurrentSheet = (Sheet)ActiveDwg.GetViews();
                object[] TableAnnotations = (object[])ActiveDrawingView.GetTableAnnotations();
                foreach (object Annotation in TableAnnotations)
                {
                    /// Table annotations are traversed here
                    TableAnnotation TableAnnotation = Annotation as TableAnnotation;
                    if (TableAnnotation != null)
                    {
                        if (TableAnnotation.Type == (int)swTableAnnotationType_e.swTableAnnotation_BillOfMaterials)
                        {
                            /// Creating object for table annotations and boundary conditions
                            var BomTableAnnotation = TableAnnotation as IBomTableAnnotation;
                            TableBoundryCondition TableBoundryConditions = new TableBoundryCondition();
                            if (BomTableAnnotation != null)
                            {
                                /// Table positions for header is obtained
                                swTableHeaderPosition_e TableHeaderPosition = (swTableHeaderPosition_e)TableAnnotation.GetHeaderStyle();

                                /// Specifying the index positions for the boundary of the table which will be generated
                                TableBoundryConditions.RowHeaderIndex = 0;
                                TableBoundryConditions.StartIndex = 1;
                                TableBoundryConditions.EndIndex = TableAnnotation.RowCount - 1;
                                TableBoundryConditions.HeaderPosition = swTableHeaderPosition_e.swTableHeader_Top;

                                /// Checking the table header position for alignment
                                if (TableHeaderPosition == swTableHeaderPosition_e.swTableHeader_Bottom)
                                {
                                    TableBoundryConditions.RowHeaderIndex = TableAnnotation.RowCount - 1;
                                    TableBoundryConditions.StartIndex = 0;
                                    TableBoundryConditions.EndIndex = TableAnnotation.RowCount - 2;
                                    TableBoundryConditions.HeaderPosition = swTableHeaderPosition_e.swTableHeader_Bottom;
                                }
                                ProcessTable(BomTableAnnotation, TableAnnotation, TableBoundryConditions);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method: ExportBomToExcel1
        /// Purpose: BOM features are extracted from active SW model file and export the features
        /// </summary>
        /// <returns>Features are printed from swFeat</returns>
        public void ExportBomToExcel1()
        {
            ModelDoc2 SwModel = default(ModelDoc2);
            DrawingDoc SwDraw = default(DrawingDoc);
            Feature SwFeat = default(Feature);
            BomFeature SwBomFeat = default(BomFeature);

            /// Active assembly and document 
            SwModel = (ModelDoc2)_sw.ActiveDoc;
            SwDraw = (DrawingDoc)SwModel;
            SwFeat = (Feature)SwModel.FirstFeature();

            while ((SwFeat != null))
            {
                if ("BomFeat" == SwFeat.GetTypeName())
                {
                    /// Obtaining all the solidworks features from the BOM Table
                    Debug.Print("Feature Name: " + SwFeat.Name);
                    SwBomFeat = (BomFeature)SwFeat.GetSpecificFeature2();
                    ProcessBomFeature(_sw, SwModel, SwBomFeat);
                }
                SwFeat = (Feature)SwFeat.GetNextFeature();
            }
        }

        public void ProcessTableAnn(ISldWorks swApp, ModelDoc2 swModel, TableAnnotation swTableAnn, string ConfigName)
        {
            int nNumRow = 0;
            int J = 0;
            int I = 0;
            string ItemNumber = null;
            string PartNumber = null;
            bool RowLocked;
            double RowHeight;

            /// The table annotation/titles are created
            Debug.Print("   Table Title: " + swTableAnn.Title);
            nNumRow = swTableAnn.RowCount;

            /// BOM table annotation created
            BomTableAnnotation swBOMTableAnn = default(BomTableAnnotation);
            swBOMTableAnn = (BomTableAnnotation)swTableAnn;

            for (J = 0; J <= nNumRow - 1; J++)
            {
                /// Specifying the height and width for printing part number, item, component.
                RowLocked = swTableAnn.GetLockRowHeight(J);
                RowHeight = swTableAnn.GetRowHeight(J);
                Debug.Print("   Row Number " + J + " (height = " + RowHeight + "; height locked = " + RowLocked + ")");
                Debug.Print("     Component Count: " + swBOMTableAnn.GetComponentsCount2(J, ConfigName, out ItemNumber, out PartNumber));
                Debug.Print("       Item Number: " + ItemNumber);
                Debug.Print("       Part Number: " + PartNumber);

                object[] vPtArr = null;
                Component2 swComp = null;
                object pt = null;

                /// Obtaining all the configuration names
                vPtArr = (object[])swBOMTableAnn.GetComponents2(J, ConfigName);

                if (((vPtArr != null)))
                {
                    for (I = 0; I <= vPtArr.GetUpperBound(0); I++)
                    {
                        pt = vPtArr[I];
                        swComp = (Component2)pt;
                        if ((swComp != null))
                        {
                            /// Printing the component, configuration name and component path.
                            Debug.Print("           Component Name: " + swComp.Name2);
                            Debug.Print("           Configuration Name: " + swComp.ReferencedConfiguration);
                            Debug.Print("           Component Path: " + swComp.GetPathName());
                        }
                        else
                        {
                            Debug.Print("  Could not get component.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method: ProcessBomFeature
        /// Purpose: This is used to process some BOM features like annotations, header positions and boundary conditions. 
        /// </summary>
        /// <param name="swApp"></param>
        /// <param name="swModel"></param>
        /// <param name="swBomFeat"></param>
        /// <returns>BOM table annotations and boundary conditions</returns>
        public void ProcessBomFeature(ISldWorks swApp, ModelDoc2 swModel, BomFeature swBomFeat)
        {
            Feature SwFeat = default(Feature);
            object[] vTableArr = null;
            object vTable = null;
            string[] vConfigArray = null;
            object vConfig = null;
            string ConfigName = null;
            TableAnnotation swTable = default(TableAnnotation);
            object visibility = null;

            /// BOM features obtained
            SwFeat = swBomFeat.GetFeature();
            vTableArr = (object[])swBomFeat.GetTableAnnotations();

            /// Table annotation setting the bounding/boundary conditions
            foreach (TableAnnotation vTable_loopVariable in vTableArr)
            {
                vTable = vTable_loopVariable;
                swTable = (TableAnnotation)vTable;
                TableAnnotation TableAnnotation = swTable as TableAnnotation;
                if (TableAnnotation != null)
                {
                    if (TableAnnotation.Type == (int)swTableAnnotationType_e.swTableAnnotation_BillOfMaterials)
                    {
                        var BomTableAnnotation = TableAnnotation as IBomTableAnnotation;
                        TableBoundryCondition TableBoundryConditions = new TableBoundryCondition();
                        if (BomTableAnnotation != null)
                        {
                            swTableHeaderPosition_e tableHeaderPosition = (swTableHeaderPosition_e)TableAnnotation.GetHeaderStyle();
                            TableBoundryConditions.RowHeaderIndex = 0;
                            TableBoundryConditions.StartIndex = 1;
                            TableBoundryConditions.EndIndex = TableAnnotation.RowCount - 1;
                            TableBoundryConditions.HeaderPosition = swTableHeaderPosition_e.swTableHeader_Top;

                            if (tableHeaderPosition == swTableHeaderPosition_e.swTableHeader_Bottom)
                            {
                                TableBoundryConditions.RowHeaderIndex = TableAnnotation.RowCount - 1;
                                TableBoundryConditions.StartIndex = 0;
                                TableBoundryConditions.EndIndex = TableAnnotation.RowCount - 2;
                                TableBoundryConditions.HeaderPosition = swTableHeaderPosition_e.swTableHeader_Bottom;
                            }
                            ProcessTable(BomTableAnnotation, TableAnnotation, TableBoundryConditions);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method: ProcessTable
        /// Purpose: This will be used for obtaining the active config details and exporting the images to Excel Helper class file.
        /// </summary>
        /// <param name="bomTable"></param>
        /// <param name="table"></param>
        /// <param name="tableCondition"></param>
        /// <returns>need to be defined</returns>

        Tuple<bool, string> ProcessTable(IBomTableAnnotation BomTable, ITableAnnotation Table, TableBoundryCondition TableCondition)
        {
            try
            {
                ModelDoc2 activeDwg = ActiveDwg as ModelDoc2;
                string PathName = activeDwg.GetPathName();
                string DirectoryName = Path.GetDirectoryName(PathName);
                var BomFile = new FileInfo(DirectoryName + @"\bom.xls");
                if (BomFile.Exists)
                {
                    BomFile.Delete();
                }

                using (var p = new ExcelPackage(BomFile))
                {
                    int Height = 30;
                    int Width = 30;
                    double ColWidth = 0;

                    /// Get the Worksheet created in the previous codesample. 
                    var ws = p.Workbook.Worksheets.Add("BOM");

                    for (int i = TableCondition.StartIndex; i <= TableCondition.EndIndex; i++)
                    {
                        if (Table.RowHidden[i])
                            continue;

                        /// Declarations
                        string PartNumber = string.Empty;
                        string ItemNumber = string.Empty;

                        /// Obtaining components count
                        var count = BomTable.GetComponentsCount(i);

                        /// This will traverse thrugh all assembly configurations/ sub assemblies/ parts and obtain the image 
                        if (count > 0)
                        {
                            var Components = (object[])BomTable.GetComponents(i);
                            var SwComponent = Components.First() as Component2;
                            var ModelDoc = SwComponent.GetModelDoc2() as ModelDoc2;
                            if (ModelDoc != null)
                            {
                                var ModelDocTitle = Path.GetFileNameWithoutExtension(ModelDoc.GetTitle());
                                var ReferencedConfiguration = SwComponent.ReferencedConfiguration;
                                var Configuration = ModelDoc.GetActiveConfiguration() as Configuration;
                                if (Configuration != null)
                                {
                                    string ConfigurationName = Configuration.Name;
                                    if (ConfigurationName != ReferencedConfiguration)
                                        ModelDoc.ShowConfiguration2(ReferencedConfiguration);
                                }

                                object DispatchImg = null;
                                try
                                {
                                    {
                                        DispatchImg = _sw.GetPreviewBitmap(ModelDoc.GetPathName(), SwComponent.ReferencedConfiguration);
                                    }
                                }
                                catch (Exception e)
                                {
                                    Debug.Print(e.Message);
                                }

                                /// Dispatching the images to excel to specific rows and cells
                                if (DispatchImg != null)
                                {
                                    ws.Row(i + 1).Height = ExcelHelper.PixelHeightToExcel(Height);
                                    var Bitmap = PictureHelper.Convert(DispatchImg);
                                    var Image = Bitmap as Image;
                                    ExcelPicture pic = ws.Drawings.AddPicture(i.ToString(), Image);
                                    pic.SetPosition(i, 0, 0, 0);
                                    pic.SetSize(Height, (int)Width);
                                }
                                else
                                {
                                    ws.Row(i + 1).Height = ExcelHelper.PixelHeightToExcel(Height);
                                    ws.Cells[i + 1, 1].Value = "N/A";
                                }

                                for (int j = 0; j < Table.ColumnCount - 1; j++)
                                {
                                    if (Table.ColumnHidden[j])
                                        continue;

                                    ws.Cells[i + 1, j + 2].Value = Table.DisplayedText[i, j];
                                }
                                ModelDoc.Visible = false;
                            }
                            else
                            {
                                ws.Row(i + 1).Height = ExcelHelper.PixelHeightToExcel(Height);
                                ws.Cells[i + 1, 1].Value = "N/A";
                            }
                        }
                        else
                        {
                            ws.Row(i + 1).Height = ExcelHelper.PixelHeightToExcel(Height);
                            ws.Cells[i + 1, 1].Value = "N/A";
                            ws.Cells[i + 1, 2, i + 1, Table.ColumnCount].Merge = true;
                            ws.Cells[i + 1, 2, i + 1, Table.ColumnCount].Value = "Failed To get row values. API Error.";
                        }
                    }

                    for (int k = 0; k < Table.ColumnCount - 1; k++)
                    {
                        if (Table.ColumnHidden[k])
                            continue;
                        ws.Cells[TableCondition.RowHeaderIndex + 1, k + 2].Value = Table.DisplayedText[TableCondition.RowHeaderIndex, k];
                        ws.Cells[TableCondition.RowHeaderIndex + 1, k + 2].Style.Font.Bold = true;
                    }

                    ws.Cells[1, 2, Table.RowCount - 1, Table.ColumnCount - 1].AutoFitColumns();
                    ws.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    ws.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ColWidth = ExcelHelper.PixelWidthToExcel(30);
                    ws.Column(1).Width = ColWidth;

                    /// Adding row header
                    for (int k = 0; k < Table.ColumnCount; k++)
                    {
                        if (Table.ColumnHidden[k])
                            continue;
                        ws.Row(k + 2).Height = ExcelHelper.PixelHeightToExcel(Height);
                    }

                    /// Save and closing the package.
                    p.Save();
                }

            }
            catch (Exception e)
            {

                return new Tuple<bool, string>(false, $"Fatal error: {e.Message} / {e.StackTrace}");
            }

            return new Tuple<bool, string>(true, "No error.");
        }
    }
}
