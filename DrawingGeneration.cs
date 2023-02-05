/*****************************************************************************************
 * Term Project     : Solidworks Extension to implement automation of reading a 3D model,*
 *                    creating drawing and exporting the BOM with picture to an excel    *
 *                    sheet and UI to create parts and assembly of BUSH.                 *                                                    
 * Team Mates       : Dhanush, Prashanth, Nikhil                                         *                                                 
 * File Name        : DrawingGeneration.cs                                               *                                         
 ****************************************************************************************/

/// *******************************USINGS*******************************************
/// System namespace defines commonly-used values and reference data types
/// Xarial.Xcad enables the abstraction layers for the CAD API allowing CAD agnostic development
/// SolidWorks.Interop provides access to all solidworks attributes/properties/features through the API
/// System.Windows.Forms is used for the showing error messages and handling exceptions.
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xarial.XCad;
using Xarial.XCad.Documents;
using Xarial.XCad.Data.Enums;
using Xarial.XCad.Documents.Services;
using Xarial.XCad.SolidWorks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows.Forms;

/// <summary>
/// Namespce : SwApp
/// </summary>
namespace SwApp
{
    /// <summary>
    /// Class: DrawingGeneration
    /// Purpose: Generating new drawing sheet, inserting the various views, inserting the BOM table, inserting the 
    ///          balloons and then saving the generated drawing file in the directory from where assembly/model
    ///          was opened. 
    /// </summary>
    public class DrawingGeneration
    {
        /// ModelDoc2 will allow access to solidworks documents:parts, assemblies and drawings and assign to null
        public ModelDoc2 ActiveAssemblyDoc = null;

        /// DrawingDoc will allow access to functions that perform drawing operations
        public DrawingDoc ActiveDwg = null;

        /// IsldWorks will provide access to interface exposed in Solidworks API either directly/in-directly
        private readonly ISldWorks _sw = null;

        /// This will be used to display any messages inside solidworks 
        private ISwApplication SwApplication = null;

        /// <summary>
        /// Constructor: DrawingGeneration
        /// Purpose: ISwApplication is an implementation of IXApplication, ISwDocument 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="iActiveDoc"></param>
        public DrawingGeneration(ISwApplication application, ModelDoc2 iActiveDoc)
        {
            ActiveAssemblyDoc = iActiveDoc;
            _sw = application.Sw;

            /// SwApplication is used for displaying message box within solidworks
            SwApplication = application;
        }

        /// <summary>
        /// Method: CreateNewSheet
        /// Purpose: Drawing templates designed by user/company specifications is provided as a base for drawing page creation.
        ///          The new drawing sheet will be created, file renamed as per directory name and file saved in directory 
        ///          from where assembly file was opened.
        /// </summary>
        /// <returns> Drawing Sheet with .slddrw extension created </returns>
        public void CreateNewSheet()
        {
            /// The location for the template to be used for drawing creation is done here.
            ModelDoc2 SwDrawingDoc = _sw.INewDocument2(@"D:\SwApp\Templates\DrawingCustomA2.SLDDRW", (int)swDwgPaperSizes_e.swDwgPaperA2size, 10, 50);

            /// Assigning the swDrawingDoc to ActiveDwg which is active drawing
            if (SwDrawingDoc is DrawingDoc)
            {
                ActiveDwg = SwDrawingDoc as DrawingDoc;
            }
            else
            {
                MessageBox.Show("Unable to load Drawing document. Please check the path location!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /// The directory locations is extracted to be used for specifing location of newly generated drawing file.
            string PathName = ActiveAssemblyDoc.GetPathName();
            string Title = ActiveAssemblyDoc.GetTitle();
            string DirectoryName = Path.GetDirectoryName(PathName);
            var DrawingFile = new FileInfo(DirectoryName + @"\" + Title + ".slddrw");

            /// Drawing generated in solidworks opens in new window, if user unknowingly presses the generate drawing command/button
            /// again the old file will be deleted. This is to prevent duplication. 
            if (DrawingFile.Exists)
            {
                DrawingFile.Delete();
                SwApplication.ShowMessageBox("Drawing file which was auto-generated for the same assembly/part has " +
                    "been deleted to be replaced by new/updated Drawing file.");
            }
            else
            {
                MessageBox.Show("Drawing file does not exist!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            /// Saving the document generated.
            SwDrawingDoc.SaveAs(DirectoryName + @"\" + Title + ".slddrw");
        }

        /// <summary>
        /// Method: InsertView
        /// Purpose: Currently 4 types of views are implemented here, the drawing palatte is obtained from active assembly document.
        ///          BOM table insertion is also called inside InsertView
        /// </summary>
        /// <returns> Drawing sheet with ISO view, Front, Top, Right views and BOM Table</returns>
        public SolidWorks.Interop.sldworks.View InsertView()
        {
            /// Specifying sheet and assembly directory here
            Sheet CurrentSheet = (Sheet)ActiveDwg.GetCurrentSheet();
            string AsmPath = ActiveAssemblyDoc.GetPathName();
            ActiveDwg.GenerateViewPaletteViews(AsmPath);
            object[] DwgVwnames = (object[])ActiveDwg.GetDrawingPaletteViewNames();

            /// String array for View Types. This will be used for assigning to standard views in solidworks.
            string[] ViewName = new string[] { "*Front", "*Top", "*Right", "*Isometric" };

            /// Declaring variables 
            double X = 0.0, Y = 0.0, Z = 0.0;

            try
            {
                ///Conditional loop statement
                foreach (string DwgVwName in DwgVwnames)
                {
                    /// Front view added to the drawing sheet
                    if (DwgVwName.Equals(ViewName[0]))
                    {
                        SolidWorks.Interop.sldworks.View FrontView = ActiveDwg.DropDrawingViewFromPalette2(ViewName[0], X + 0.7052, Y + 0.504, Z);
                        BomTable BomTable = FrontView.GetBomTable() as BomTable;
                        FrontView.SetName2("Front View");
                        FrontView.SetDisplayMode3(true, (int)swDisplayMode_e.swSHADED_EDGES, false, true);
                    }

                    /// Top view added to the drawing sheet
                    else if (DwgVwName.Equals(ViewName[1]))
                    {
                        SolidWorks.Interop.sldworks.View TopView = ActiveDwg.DropDrawingViewFromPalette2(ViewName[1], X + 0.7052, Y + 0.3603, Z);
                        TopView.SetName2("Top View");
                        TopView.SetDisplayMode3(true, (int)swDisplayMode_e.swSHADED_EDGES, false, true);
                    }

                    /// Right/side view added to the drawing sheet
                    else if (DwgVwName.Equals(ViewName[2]))
                    {
                        SolidWorks.Interop.sldworks.View RightView = ActiveDwg.DropDrawingViewFromPalette2(ViewName[2], X + 0.502, Y + 0.504, Z);
                        RightView.SetName2("Right View");
                        RightView.SetDisplayMode3(true, (int)swDisplayMode_e.swSHADED_EDGES, false, true);
                    }

                    /// Isometric view added to the drawing sheet
                    else if (DwgVwName.Equals(ViewName[3]))
                    {
                        SolidWorks.Interop.sldworks.View IsometricView = ActiveDwg.DropDrawingViewFromPalette2(ViewName[3], X + 0.4041, Y + 0.2714, Z);
                        IsometricView.SetName2("Isometric View");
                        IsometricView.SetDisplayMode3(true, (int)swDisplayMode_e.swSHADED_EDGES, false, true);

                        /// The BOM is inserted to the drawing sheet once the Isometric view is generated.
                        /// iview represents drawing view fund in document, any one type of view can be passed for BOM.
                        this.InsertBom(IsometricView);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to insert Drawing view. Please try again!", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Fatal error: { e.Message} / { e.StackTrace}");
            }

            return null;
        }

        /// <summary>
        /// Method: InsertBom
        /// Purpose: Bill of materials is inserted to drawing document as per the template set by the user
        /// </summary>
        /// <param name="iView"></param>
        /// <returns> ActiveDwg </returns>
        public DrawingDoc InsertBom(SolidWorks.Interop.sldworks.View iView)
        {
            try
            {
                /// Anchoring the BOM configuration table
                int AnchorType = (int)swBOMConfigurationAnchorType_e.swBOMConfigurationAnchor_TopLeft;

                /// Specifying the BOM Table type. 
                /// Other types of BOM like TopLevel components, Indented components can be easily added if needed.
                int BomType = (int)swBomType_e.swBomType_PartsOnly;

                /// User can create custom BOM Table by specifying what parameters/information is needed inside BOM.
                /// User can do this by simply modifying the template in the location below.
                string TableTemplate = @"D:\SwApp\Templates\bom-all.sldbomtbt";
                string Configuration = "";
                BomTableAnnotation SwBOMAnnotation = iView.InsertBomTable3(false, 0.01, 0.5810, AnchorType, BomType, Configuration, TableTemplate, false);
                BomFeature SwBOMFeature = SwBOMAnnotation.BomFeature;

                /// This will call Create Auto Balloon method.
                /// CreateAutoBalloons(iView);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to insert BOM. Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error: { e.Message} / { e.StackTrace}");
            }

            return ActiveDwg;
        }

        /// <summary>
        /// Method: CreateAutoBallon
        /// Purpose: Ballon is created and added inisde the doc for the active drawing
        /// </summary>
        /// <param name="iView"></param>
        /// <returns> Balloons are added </returns>
        public void CreateAutoBallons(SolidWorks.Interop.sldworks.View iView)
        {
            try
            {
                ActiveDwg.ActivateView("IsoView");
                ModelDoc2 SwModel = ActiveDwg as ModelDoc2;
                ModelDocExtension SwModelDocExt = SwModel.Extension;

                /// Parameters specifying the styling and other information regarding the annotations.
                bool status = SwModelDocExt.SelectByID2("", "EDGE", 0.1205506330468, 0.261655309417, -0.0004000000000133,
                    false, 0, null, 0);
                BalloonOptions BomBalloonParams = SwModel.Extension.CreateBalloonOptions();
                BomBalloonParams.Style = (int)swBalloonStyle_e.swBS_Circular;
                BomBalloonParams.Size = (int)swBalloonFit_e.swBF_2Chars;
                BomBalloonParams.UpperTextContent = (int)swBalloonTextContent_e.swBalloonTextItemNumber;
                BomBalloonParams.UpperText = "";
                BomBalloonParams.ShowQuantity = true;
                BomBalloonParams.QuantityPlacement = (int)swBalloonQuantityPlacement_e.swBalloonQuantityPlacement_Right;
                BomBalloonParams.QuantityDenotationText = "PLACES";
                BomBalloonParams.QuantityOverride = false;
                BomBalloonParams.QuantityOverrideValue = "";
                BomBalloonParams.ItemNumberStart = 1;
                BomBalloonParams.ItemNumberIncrement = 1;
                BomBalloonParams.ItemOrder = (int)swBalloonItemNumbersOrder_e.swBalloonItemNumbers_DoNotChangeItemNumbers;

                /// The Balloons are applied to the ISO View 
                Note SwNote = SwModelDocExt.InsertBOMBalloon2(BomBalloonParams);

                /// Get whether balloon is a BOM balloon and print the name of the BOM balloon
                if (SwNote.IsBomBalloon())
                {
                    Debug.Print("Name of BOM balloon: " + SwNote.GetName());
                }
                else
                {
                    MessageBox.Show("Unable to load Auto Balloons. Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: { e.Message} / { e.StackTrace}");
            }

            /// Rebuilding the active drawing document
            ActiveDwg.ForceRebuild();
        }
    }
}
