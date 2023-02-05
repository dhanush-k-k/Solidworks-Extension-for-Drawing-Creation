/*************************************************************************************
 * Term Project : Solidworks Extension to implement automation of reading a 3D model,*
 *                creating drawing and exporting the BOM with picture to an excel    *
 *                sheet and UI to create parts and assembly of BUSH.                 *
 * Purpose      : An UI along with functions to create parts and build bush assembly * 
 *                by taking the dimensions input from the user and handle events     *
 * File Name    : BushAssembly.cs                                                    *  
 * Team Mates   : Dhanush, Prashanth, Nikhil                                         *
**************************************************************************************/


/// *******************************USINGS******************************************************
/// System defines commonly used values and reference data types
/// System.Windows.Forms enables demonstrating UI,error messages, handling events and exceptions
/// SolidWorks.Interop provides access to all solidworks attributes/properties/features through the API
/// Xarial.Xcad.Solidwroks enables the abstraction layers for the CAD API allowing CAD agnostic development
using System;
using System.Diagnostics;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using Xarial.XCad.SolidWorks;

/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp.AssemblyCreation
{
    /// <summary>
    /// Class   : BushAssembly
    /// Purpose : This form is prepared for UI. It will display the Bush assembly part and 
    ///           allow user to enter the required dimensions for creation.
    /// Note : This is partial class. The other part is in BushAssembly.Designer.cs file
    /// </summary> 
    public partial class BushAssembly : Form
    {
        /// Instances for active document
        public ModelDoc2 ActiveDoc = null;
        private readonly ISldWorks SwApp = null;
        private ISwApplication SwApplication = null;

        /// <summary>
        /// Constructor : BushAssembly 
        /// Purpose : To assign the Solidworks application and initialize the component
        /// </summary>
        /// <param name="iswApplication"></param>
        /// <return> Nil </return>
        public BushAssembly(ISwApplication iswApplication)
        {
            SwApplication = iswApplication;
            SwApp = iswApplication.Sw;
            InitializeComponent();
        }

        /// <summary>
        /// Method  : LValueTextChanged
        /// Purpose : To log the change event when the length is entered. Also checks
        ///           forbidding anything else except digits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return> Nil </return>
        private void LValueTextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(LValue.Text, "[^0-9]"))
            {
                LValue.Text = LValue.Text.Remove(LValue.Text.Length - 1);
            }
        }

        /// <summary>
        /// Method  : WValueTextChanged
        /// Purpose : To log the change event when the width is entered. Also 
        ///           forbidding anything else except digits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return> Nil </return>
        private void WValueTextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(WValue.Text, "[^0-9]"))
            {
                WValue.Text = WValue.Text.Remove(WValue.Text.Length - 1);
            }
        }

        /// <summary>
        /// Method  : HValueTextChanged
        /// Purpose : To log the change event when the heigth is entered. Also 
        ///           forbidding anything else except digits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return> Nil </return>
        private void HValueTextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(HValue.Text, "[^0-9]"))
            {
                HValue.Text = HValue.Text.Remove(HValue.Text.Length - 1);
            }
        }

        /// Dimensions of Bush in Drawing in mm and Solidworks reference
        double CenterHoleDia = 18;
        double CylinderInnerDia = 32;
        double CylinderOuterDia = 42;
        double CylinderDepth = 18;
        double RibThickness = 10;
        double PlateThickness = 10;
        double MinimumHeight = 19;
        double WidthCheck = 5;
        double SwRefValue = 0.017453292519943334;

        /// X,Y,Z Axis variables and assigning datum as 0,0,0
        /// These variables also facilitates readability and understanding
        double XAxis = 0;
        double YAxis = 0;
        double ZAxis = 0;

        /// <summary>
        /// Method : ModelPartsClick
        /// Purpose : To log the event when the Model Parts button is Clicked. This
        ///           method prevents any empty fields, builds all three parts and 
        ///           display messages to user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return> Nil </return>
        private void ModelPartsClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LValue.Text) || string.IsNullOrWhiteSpace(WValue.Text)
                || string.IsNullOrWhiteSpace(HValue.Text))
            {
                MessageBox.Show("Values should not be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /// The dimensions are converted to double and stored in Length, Width, Height
            double Length = Convert.ToDouble(LValue.Text);
            double Width = Convert.ToDouble(WValue.Text);
            double Height = Convert.ToDouble(HValue.Text);

            /// Conditions to check detail size as per drawing and display message for incorrect inputs
            if (Width >= CylinderOuterDia - WidthCheck)
            {
                MessageBox.Show("Width of base can not exceed or equal the diameter of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Width < RibThickness)
            {
                MessageBox.Show("Width of base can not be less than the width of the rib",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Length <= CylinderOuterDia)
            {
                MessageBox.Show("Length of base can not be less than or equal " +
                    "to the diameter of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Height <= MinimumHeight)
            {
                MessageBox.Show("Height of detail can not be less than or equal " +
                    "to the length of the inner section of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /// Conversion from mm to m and from Diameter to Radius
            /// Negative sign assigned for axis values in sketch of parts
            double CenterHoleRad = -CenterHoleDia / 2000;
            double CylinderInnerRad = -CylinderInnerDia / 2000;
            double CylinderOuterRad = -CylinderOuterDia / 2000;
            RibThickness = RibThickness / 1000;
            PlateThickness = PlateThickness / 1000;
            CylinderDepth = CylinderDepth / 1000;
            Length = -Length / 1000 ;
            Width = -Width / 1000 ;
            Height = (Height - 10) / 1000;

            /// Making the part visible
            SwApp.Visible = true;

            /// Selection of detail modeling mode
            SwApp.NewPart();
            ModelDoc2 SwDoc = null;
            bool BoolStatus = false;
            int longstatus = 0;

            /// Detailed plate part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            SwDoc = ((ModelDoc2)(SwApp.ActiveDoc));
            SketchSegment SkhSegment = null;
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CylinderOuterRad, YAxis, ZAxis)));
            SwDoc.ShowNamedView2("*Trimetric", 8);
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Arc1", "SKETCHSEGMENT", XAxis, YAxis, ZAxis, false, 0, null, 0);
            Feature MyFeature = null;
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, PlateThickness, PlateThickness, false, false, 
            false, false, SwRefValue, SwRefValue, false, false, false, false, true, true, true, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            Array SkLines = null;
            SkLines = ((Array)(SwDoc.SketchManager.CreateCenterRectangle(XAxis, YAxis, ZAxis, Length / 2, Width / 2, 0)));
            SwDoc.ViewZoomtofit2();
            SwDoc.ShowNamedView2("*Top", 5);
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(Length / 2, Math.Abs(Width / 2), ZAxis, XAxis, -CylinderOuterRad, ZAxis)));
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(XAxis, -CylinderOuterRad, ZAxis, Math.Abs(Length / 2), Math.Abs(Width / 2), 
                         ZAxis)));
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(Length / 2, Width / 2, ZAxis, XAxis, CylinderOuterRad, ZAxis)));
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(XAxis, CylinderOuterRad, ZAxis, Math.Abs(Length / 2), Width / 2, ZAxis)));
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Line1", "SKETCHSEGMENT", -0.0044024507535198876, 0, Width / 2, false, 2, null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            BoolStatus = SwDoc.Extension.SelectByID2("Line6", "SKETCHSEGMENT", -0.012921376009237012, 0, -0.0054405793723103189, false, 2, 
                         null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            BoolStatus = SwDoc.Extension.SelectByID2("Line5", "SKETCHSEGMENT", -0.014135271591155435, 0, 0.005951693301539128, false, 2, 
                         null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            BoolStatus = SwDoc.Extension.SelectByID2("Line5", "SKETCHSEGMENT", 0.01393190144612956, 0, -0.0058660637667913938, false, 2, 
                         null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            BoolStatus = SwDoc.Extension.SelectByID2("Line6", "SKETCHSEGMENT", 0.011650517863344173, 0, 0.0049054812056186, false, 2, 
                         null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            BoolStatus = SwDoc.Extension.SelectByID2("Line3", "SKETCHSEGMENT", -0.016022866314760208, 0, Math.Abs(Width / 2), false, 2, 
                         null, 0);
            BoolStatus = SwDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            SwDoc.ClearSelection2(true);
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, PlateThickness, PlateThickness, false, 
                          false, false, false, 
                         SwRefValue, SwRefValue, false, false, false, false, true, true, true, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CenterHoleRad, YAxis, ZAxis)));
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Arc1", "SKETCHSEGMENT", XAxis, YAxis, ZAxis, false, 0, null, 0);
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureCut3(true, false, true, 0, 0, PlateThickness, PlateThickness, false, false, false, false,
                 SwRefValue, SwRefValue, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;

            /// Saving the detail of first build
            SwDoc.ClearSelection2(true);
            longstatus = SwDoc.SaveAs3(@"D:\SwApp\Plate.SLDPRT", 0, 2);

            /// Detailed center part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            SwApp.NewPart();
            SwDoc = ((ModelDoc2)(SwApp.ActiveDoc));
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkhSegment = null;
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CylinderOuterRad, YAxis , ZAxis)));
            SwDoc.ClearSelection2(true);
            SkhSegment= null;
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CenterHoleRad, YAxis, ZAxis)));
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Sketch1", "SKETCH", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.ShowNamedView2("*Trimetric", 8);
            SwDoc.ViewZoomtofit2();
            MyFeature = null;
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, Height, 0, false, false, false, false, 
                        SwRefValue, SwRefValue, false, false, false, false, true, true, true, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            BoolStatus = SwDoc.Extension.SelectByRay(7.68158929433679E-03, Height, -0.013874870662896, 0, -1, 0, 3.26467545009313E-04, 2, false, 
                         0, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkhSegment = null;
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CylinderInnerRad, YAxis, ZAxis)));
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Sketch1", "SKETCH", XAxis, YAxis, ZAxis, false, 0, null, 0);
            MyFeature = null;
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureCut3(true, false, false, 0, 0, CylinderDepth, PlateThickness, false, false, false, false, 
               SwRefValue, SwRefValue, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            SwDoc.ShowNamedView2("*Top", 5);
            SwDoc.ViewZoomtofit2();

            /// Saving the detail of second build
            SwDoc.ClearSelection2(true);
            longstatus = SwDoc.SaveAs3(@"D:\SwApp\Center.SLDPRT", 0, 2);

            /// Detailed rib part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            SwApp.NewPart();
            SwDoc = ((ModelDoc2)(SwApp.ActiveDoc));
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkLines = null;
            SkLines = ((Array)(SwDoc.SketchManager.CreateCenterRectangle(XAxis, YAxis, ZAxis, Length / 2, -RibThickness/2, ZAxis)));
            SwDoc.ShowNamedView2("*Trimetric", 8);
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Sketch1", "SKETCH", XAxis, YAxis, ZAxis, false, 0, null, 0);
            MyFeature = null;
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, Height, PlateThickness, false, false, false, 
                       false, SwRefValue, SwRefValue, false, false, false, false, true, true, true, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            SwDoc.ShowNamedView2("*Front", 1);
            BoolStatus = SwDoc.Extension.SelectByID2("Front Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ClearSelection2(true);
            SkhSegment = null;
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(Length / 2, Height, ZAxis, CylinderOuterRad, Height, ZAxis)));
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(CylinderOuterRad, Height, ZAxis, Length / 2, YAxis, ZAxis)));
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(Length / 2, YAxis, ZAxis, Length / 2, Height, ZAxis)));
            SwDoc.ClearSelection2(true);
            SkhSegment = null;
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(-Length / 2, Height, ZAxis, -CylinderOuterRad, Height, ZAxis)));
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(-CylinderOuterRad, Height, ZAxis, -Length / 2, YAxis, ZAxis)));
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateLine(-Length / 2, YAxis, ZAxis, -Length / 2, Height, ZAxis)));
            BoolStatus = SwDoc.Extension.SelectByID2("Sketch1", "SKETCH", XAxis, YAxis, ZAxis, false, 0, null, 0);
            MyFeature = null;
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureCut3(false, false, false, 1, 1, Height, Height, false, false, false, false, 
                  SwRefValue, SwRefValue, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;
            BoolStatus = SwDoc.Extension.SelectByID2("Top Plane", "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwDoc.SketchManager.InsertSketch(true);
            SwDoc.ShowNamedView2("*Top", 5);
            SwDoc.ClearSelection2(true);
            SkhSegment = ((SketchSegment)(SwDoc.SketchManager.CreateCircle(XAxis, YAxis, ZAxis, CylinderOuterRad, YAxis, ZAxis)));
            SwDoc.ClearSelection2(true);
            BoolStatus = SwDoc.Extension.SelectByID2("Sketch1", "SKETCH", XAxis, YAxis, ZAxis, false, 0, null, 0);
            MyFeature = ((Feature)(SwDoc.FeatureManager.FeatureCut3(true, false, true, 0, 0, Height, Height, false, false, false, false, 
                 SwRefValue, SwRefValue, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            SwDoc.ISelectionManager.EnableContourSelection = false;

            /// Saving the detail of third build
            SwDoc.ClearSelection2(true);
            longstatus = SwDoc.SaveAs3(@"D:\SwApp\Rib.SLDPRT", 0, 2);

            /// Close all documents and display message to user 
            SwApp.CloseAllDocuments(true);
            SwApplication.ShowMessageBox("Parts Generated. Proceed with assembly");
        }

        /// <summary>
        /// Method  : CoincidentPlaneMate
        /// Purpose : This method is defined to mate the planes of parts within a sepcified assembly doc.
        ///           The plane of first and second part to be mate, current part and assembly
        ///           are passed as arguments for completing the mate.
        /// </summary>
        /// <param name="RefPlane1"></param>
        /// <param name="RefPlane2"></param>
        /// <param name="RefPart"></param>
        /// <param name="RefAssembly"></param>
        /// <return> Nil </return>
        private void CoincidentPlaneMate(string RefPlane1, string RefPlane2, ModelDoc2 RefPart, AssemblyDoc RefAssembly )
        {
            /// Instance of new feature, Mate and Entities
            /// The Mate entities are assigned using Entity
            /// swMate used to store the mates
            /// swFeat to create the feature after mating the planes
            Feature SwFeat = default(Feature);
            bool BoolStatus = false;
            Mate2 SwMate = default(Mate2);
            int Errors = 0;
            Entity SwEnt1 = default(Entity);
            Entity SwEnt2 = default(Entity);

            /// Selection of top planes for mate of entities
            BoolStatus = RefPart.Extension.SelectByID2(RefPlane1, "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwEnt1 = (Entity)((SelectionMgr)(RefPart.SelectionManager)).GetSelectedObject6(1, -1);
            BoolStatus = RefPart.Extension.SelectByID2(RefPlane2, "PLANE", XAxis, YAxis, ZAxis, false, 0, null, 0);
            SwEnt2 = (Entity)((SelectionMgr)(RefPart.SelectionManager)).GetSelectedObject6(1, -1);
            SwEnt1.Select4(true, null);
            SwEnt2.Select4(true, null);

            /// Mating top planes and rebuild the parts
            SwMate = RefAssembly.AddDistanceMate(2, true, 0, 0, 0, 1, 1, out Errors);
            SwFeat = (Feature)SwMate;
            RefPart.EditRebuild3();
        }

        /// <summary>
        /// Method : BuildAsmClick
        /// Purpose : To log the event when the Build Assembly button is Clicked. This
        ///           method builds the assembly using generated three parts and also
        ///           display messages to user as per conditions. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return> Nil </return>
        private void BuildAsmClick(object sender, EventArgs e)
        {
            /// Condition to check parts are generated
            if (SwApp == null)
            {
                MessageBox.Show("You have to build all three details beforehand",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                /// Creation of new swDoc file for storing assembly
                /// Creation of new temporary part for opening and adding to assembly file 
                /// Creation of Feature object
                /// longstatus and longwarnings are initialized to store the operations performed
                AssemblyDoc SwAssembly = SwApp.INewAssembly();
                ModelDoc2 Part = default(ModelDoc2);
                int longwarnings = 0;
                int longstatus = 0;
                Part = (ModelDoc2)SwApp.ActiveDoc;
                Component2 SwInsertedComponent = default(Component2);
                double PlateCenter = -0.005;

                /// Insert Plate component, add to assembly and close the part file
                SwApp.OpenDoc6(@"D:\SwApp\Plate.SLDPRT", 1, 0, "", longstatus, longwarnings);
                SwInsertedComponent = SwAssembly.AddComponent4(@"D:\SwApp\Plate.SLDPRT", "Default", XAxis, PlateCenter, ZAxis);
                SwApp.CloseDoc(@"D:\SwApp\Plate.SLDPRT");

                /// Insert Center component, store to temporary object and close the part file
                SwApp.OpenDoc6(@"D:\SwApp\Center.SLDPRT", 1, 0, "", longstatus, longwarnings);
                SwInsertedComponent = SwAssembly.AddComponent4(@"D:\SwApp\Center.SLDPRT", "Default", XAxis, YAxis, ZAxis);
                SwApp.CloseDoc(@"D:\SwApp\Center.SLDPRT");

                /// Insert Rib component, store to temporary object and close the part file
                SwApp.OpenDoc6(@"D:\SwApp\Rib.SLDPRT", 1, 0, "", longstatus, longwarnings);
                SwInsertedComponent = SwAssembly.AddComponent4(@"D:\SwApp\Rib.SLDPRT", "Default", XAxis, YAxis, ZAxis);
                SwApp.CloseDoc(@"D:\SwApp\Rib.SLDPRT");

                /// Mating Top Plane
                CoincidentPlaneMate("Top Plane", "Top Plane@Center-1@Assem1", Part, SwAssembly);
                CoincidentPlaneMate("Top Plane", "Top Plane@Rib-1@Assem1", Part, SwAssembly);

                /// Mating Front Plane
                CoincidentPlaneMate("Front Plane@Plate-1@Assem1", "Front Plane@Center-1@Assem1", Part, SwAssembly);
                CoincidentPlaneMate("Front Plane@Plate-1@Assem1", "Front Plane@Rib-1@Assem1", Part, SwAssembly);

                /// Mating Right Plane
                CoincidentPlaneMate("Right Plane@Plate-1@Assem1", "Right Plane@Center-1@Assem1", Part, SwAssembly);
                CoincidentPlaneMate("Right Plane@Plate-1@Assem1", "Right Plane@Rib-1@Assem1", Part, SwAssembly);

                /// Assembly created and store in the folder.
                longstatus = Part.SaveAsSilent(@"D:\SwApp\BushAssembly.SLDASM",false);
                SwApplication.ShowMessageBox(@"Assembly Generated and Saved in D:\SwApp\BushAssembly.SLDASM");
                this.Close();
            }
        }
    }
}