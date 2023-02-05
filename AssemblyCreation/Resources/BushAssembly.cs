using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Xarial.XCad.SolidWorks;
using static System.Windows.Forms.AxHost;

/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp.AssemblyCreation
{
    /// <summary>
    /// Class : BushAssembly
    /// Purpose : This form is prepared for UI. It will display the Bush assembly part and 
    ///           allow user to enter the required dimensions for creation.
    /// Note : This is partial class. The other part is in BushAssembly.Designer.cs file
    /// </summary> 
    public partial class BushAssembly : Form
    {
        /// Instances for active document
        public ModelDoc2 ActiveDoc = null;
        private readonly ISldWorks swApp = null;
        private ISwApplication swApplication = null;

        /// <summary>
        /// Constructor : Bush Assembly 
        /// Purpose : To assign the Solidworks application and initialize the component
        /// </summary>
        /// <param name="iswApplication"></param>
        /// <return> Nil </return>
        public BushAssembly(ISwApplication iswApplication)
        {
            swApplication = iswApplication;
            swApp = iswApplication.Sw;
            InitializeComponent();
        }

        /// <summary>
        /// Method : LValueTextChanged
        /// Purpose : To log the change event when the length is entered. Also checks
        ///           forbidding anything else except digits.
        /// </summary>
        /// <param name="iswApplication"></param>
        /// <return> Nil </return>
        private void LValueTextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(LValue.Text, "[^0-9]"))
            {
                LValue.Text = LValue.Text.Remove(LValue.Text.Length - 1);
            }
        }

        /// <summary>
        /// Method : WValueTextChanged
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
        /// Method : HValueTextChanged
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

            /// The dimensions are converted to double and stored in x1, x2, x3
            double x1 = Convert.ToDouble(LValue.Text);
            double x2 = Convert.ToDouble(WValue.Text);
            double x3 = Convert.ToDouble(HValue.Text);

            /// Conditions to check detail size and display message for incorrect inputs
            if (x2 >= 42 - 5)
            {
                MessageBox.Show("Width of base can not exceed or equal the diameter of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (x2 < 10)
            {
                MessageBox.Show("Width of base can not be less than the width of the rib",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (x1 <= 42)
            {
                MessageBox.Show("Length of base can not be less than or equal " +
                    "to the diameter of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (x3 <= 19)
            {
                MessageBox.Show("Height of detail can not be less than or equal " +
                    "to the length of the inner section of the cylinder",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            /// Conversion from mm to m
            x1 = -x1 / 1000 ;
            x2 = -x2 / 1000 ;
            x3 = (x3 - 10) / 1000;

            /// Making the part visible
            swApp.Visible = true;

            /// Selection of detail modeling mode
            swApp.NewPart();
            ModelDoc2 swDoc = null;
            bool boolstatus = false;
            int longstatus = 0;

            /// Detailed plate part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            SketchSegment skSegment = null;
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0, 0, 0, -0.021, 0, 0)));
            swDoc.ShowNamedView2("*Trimetric", 8);
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            Feature myFeature = null;
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.01, 0.01, false, false, 
            false, false, 0.017453292519943334, 0.017453292519943334, false, false, false, false, true, true, true, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SetUserPreferenceToggle(((int)(swUserPreferenceToggle_e.swSketchAddConstToRectEntity)), 
                                                    ((int)(swUserPreferenceOption_e.swDetailingNoOptionSpecified)), true);
            boolstatus = swDoc.Extension.SetUserPreferenceToggle(((int)(swUserPreferenceToggle_e.swSketchAddConstLineDiagonalType)), 
                                                   ((int)(swUserPreferenceOption_e.swDetailingNoOptionSpecified)), true);
            Array vSkLines = null;
            vSkLines = ((Array)(swDoc.SketchManager.CreateCenterRectangle(0, 0, 0, x1 / 2, x2 / 2, 0)));
            swDoc.ViewZoomtofit2();
            swDoc.ShowNamedView2("*Top", 5);
            swDoc.ClearSelection2(true);
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(x1 / 2, Math.Abs(x2 / 2), 0.000000, 0.000000, 0.021000, 0.000000)));
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(0.000000, 0.021000, 0.000000, Math.Abs(x1 / 2), Math.Abs(x2 / 2), 
                         0.000000)));
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(x1 / 2, x2 / 2, 0.000000, 0.000000, -0.021000, 0.000000)));
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(0.000000, -0.021000, 0.000000, Math.Abs(x1 / 2), x2 / 2, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Line1", "SKETCHSEGMENT", -0.0044024507535198876, 0, x2 / 2, false, 2, null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            boolstatus = swDoc.Extension.SelectByID2("Line6", "SKETCHSEGMENT", -0.012921376009237012, 0, -0.0054405793723103189, false, 2, 
                         null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            boolstatus = swDoc.Extension.SelectByID2("Line5", "SKETCHSEGMENT", -0.014135271591155435, 0, 0.005951693301539128, false, 2, 
                         null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            boolstatus = swDoc.Extension.SelectByID2("Line5", "SKETCHSEGMENT", 0.01393190144612956, 0, -0.0058660637667913938, false, 2, 
                         null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            boolstatus = swDoc.Extension.SelectByID2("Line6", "SKETCHSEGMENT", 0.011650517863344173, 0, 0.0049054812056186, false, 2, 
                         null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            boolstatus = swDoc.Extension.SelectByID2("Line3", "SKETCHSEGMENT", -0.016022866314760208, 0, Math.Abs(x2 / 2), false, 2, 
                         null, 0);
            boolstatus = swDoc.SketchManager.SketchTrim(4, 0, 0, 0);
            swDoc.ClearSelection2(true);
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.01, 0.01, false, false, false, false, 
                         0.017453292519943334, 0.017453292519943334, false, false, false, false, true, true, true, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0.000000, 0.000000, 0.000000, -0.009, 0, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Arc1", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut3(true, false, true, 0, 0, 0.01, 0.01, false, false, false, false, 
                 0.017453292519943334, 0.017453292519943334, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;

            /// Saving the detail of first build
            swDoc.ClearSelection2(true);
            longstatus = swDoc.SaveAs3(@"D:\SwApp\Plate.SLDPRT", 0, 2);

            /// Detailed center part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            swApp.NewPart();
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = null;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0.000000, 0.000000, 0.000000, -0.021, 0, 0.000000)));
            swDoc.ClearSelection2(true);
            skSegment= null;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0.000000, 0.000000, 0.000000, -0.009, 0, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            swDoc.ShowNamedView2("*Trimetric", 8);
            swDoc.ViewZoomtofit2();
            myFeature = null;
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, x3, 0, false, false, false, false, 
                        0.017453292519943334, 0.017453292519943334, false, false, false, false, true, true, true, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            boolstatus = swDoc.Extension.SelectByRay(7.68158929433679E-03, x3, -0.013874870662896, 0, -1, 0, 3.26467545009313E-04, 2, false, 
                         0, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = null;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0.000000, 0.000000, 0.000000, -0.016, 0, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            myFeature = null;
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut3(true, false, false, 0, 0, 0.018, 0.01, false, false, false, false, 
               0.017453292519943334, 0.017453292519943334, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            swDoc.ShowNamedView2("*Top", 5);
            swDoc.ViewZoomtofit2();

            /// Saving the detail of second build
            swDoc.ClearSelection2(true);
            longstatus = swDoc.SaveAs3(@"D:\SwApp\Center.SLDPRT", 0, 2);

            /// Detailed rib part building
            /// Selection of plane, sketching, extrusion, feature cut, finally build part
            swApp.NewPart();
            swDoc = ((ModelDoc2)(swApp.ActiveDoc));
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SetUserPreferenceToggle(((int)(swUserPreferenceToggle_e.swSketchAddConstToRectEntity)), 
                         ((int)(swUserPreferenceOption_e.swDetailingNoOptionSpecified)), true);
            boolstatus = swDoc.Extension.SetUserPreferenceToggle(((int)(swUserPreferenceToggle_e.swSketchAddConstLineDiagonalType)), 
                         ((int)(swUserPreferenceOption_e.swDetailingNoOptionSpecified)), true);
            vSkLines = null;
            vSkLines = ((Array)(swDoc.SketchManager.CreateCenterRectangle(0, 0, 0, x1 / 2, -0.005, 0)));
            swDoc.ShowNamedView2("*Trimetric", 8);
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            myFeature = null;
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureExtrusion2(true, false, false, 0, 0, x3, 0.01, false, false, false, 
                       false, 0.017453292519943334, 0.017453292519943334, false, false, false, false, true, true, true, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            swDoc.ShowNamedView2("*Front", 1);
            boolstatus = swDoc.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = null;
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(x1 / 2, 0.000000, 0.000000, x1 / 2, 0.010000, 0.000000)));
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(x1 / 2, 0.010000, 0.000000, -0.020908, x3, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Line3", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut3(false, false, false, 1, 1, x3, x3, false, false, false, false, 
                  0.017453292519943334, 0.017453292519943334, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            boolstatus = swDoc.Extension.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ClearSelection2(true);
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(Math.Abs(x1 / 2), 0.000000, 0.000000, Math.Abs(x1 / 2), 0.010000, 
                        0.000000)));
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateLine(Math.Abs(x1 / 2), 0.010000, 0.000000, 0.020908, x3, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Line3", "SKETCHSEGMENT", 0, 0, 0, false, 0, null, 0);
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut3(false, true, false, 1, 1, x3, x3, false, false, false, false, 
                0.017453292519943334, 0.017453292519943334, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;
            boolstatus = swDoc.Extension.SelectByID2("Top Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
            swDoc.SketchManager.InsertSketch(true);
            swDoc.ShowNamedView2("*Top", 5);
            swDoc.ClearSelection2(true);
            skSegment = ((SketchSegment)(swDoc.SketchManager.CreateCircle(0.000000, 0.000000, 0.000000, -0.021, 0, 0.000000)));
            swDoc.ClearSelection2(true);
            boolstatus = swDoc.Extension.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
            myFeature = ((Feature)(swDoc.FeatureManager.FeatureCut3(true, false, true, 0, 0, x3, x3, false, false, false, false, 
                 0.017453292519943334, 0.017453292519943334, false, false, false, false, false, true, true, true, true, false, 0, 0, false)));
            swDoc.ISelectionManager.EnableContourSelection = false;

            /// Saving the detail of third build
            swDoc.ClearSelection2(true);
            longstatus = swDoc.SaveAs3(@"D:\SwApp\Rib.SLDPRT", 0, 2);

            /// Close all documents and display message to user 
            swApp.CloseAllDocuments(true);
            swApplication.ShowMessageBox("Parts Generated. Proceed with assembly");
        }

        /// <summary>
        /// Method : CoincidentPlaneMate
        /// Purpose : This method is defined to mate the planes of parts within a sepcified assembly doc.
        /// </summary>
        /// <param name="RefPlane1"></param>
        /// <param name="RefPlane2"></param>
        /// <param name="RefPart"></param>
        /// <param name="RefAssembly"></param>
        /// <return> Nil </return>
        private void CoincidentPlaneMate(string RefPlane1, string RefPlane2, ModelDoc2 RefPart, AssemblyDoc RefAssembly )
        {
            Feature swFeat = default(Feature);
            bool boolstatus = false;
            Mate2 swMate = default(Mate2);
            int errors = 0;
            Entity swEnt1 = default(Entity);
            Entity swEnt2 = default(Entity);

            /// Selection of top planes for mate of entities
            boolstatus = RefPart.Extension.SelectByID2(RefPlane1, "PLANE", 0, 0, 0, false, 0, null, 0);
            swEnt1 = (Entity)((SelectionMgr)(RefPart.SelectionManager)).GetSelectedObject6(1, -1);
            boolstatus = RefPart.Extension.SelectByID2(RefPlane2, "PLANE", 0, 0, 0, false, 0, null, 0);
            swEnt2 = (Entity)((SelectionMgr)(RefPart.SelectionManager)).GetSelectedObject6(1, -1);
            swEnt1.Select4(true, null);
            swEnt2.Select4(true, null);

            /// Mating top planes and rebuild the parts
            swMate = RefAssembly.AddDistanceMate(2, true, 0, 0, 0, 1, 1, out errors);
            Debug.Print("First arc condition as defined in swDistanceMateArcConditions_e: " + swMate.DistanceFirstArcCondition);
            Debug.Print("Second arc condition as defined in swDistanceMateArcConditions_e: " + swMate.DistanceSecondArcCondition);
            swFeat = (Feature)swMate;
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
            if (swApp == null)
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
                AssemblyDoc swAssembly = swApp.INewAssembly();
                ModelDoc2 Part = default(ModelDoc2);
                int longwarnings = 0;
                int longstatus = 0;

                Part = (ModelDoc2)swApp.ActiveDoc;
                Component2 swInsertedComponent = default(Component2);

                /// Insert Plate component, add to assembly and close the part file
                swApp.OpenDoc6(@"D:\SwApp\Plate.SLDPRT", 1, 0, "", longstatus, longwarnings);
                swInsertedComponent = swAssembly.AddComponent4(@"D:\SwApp\Plate.SLDPRT", "Default", 0, -0.005, 0);
                swApp.CloseDoc(@"D:\SwApp\Plate.SLDPRT");

                /// Insert Center component, store to temporary object and close the part file
                swApp.OpenDoc6(@"D:\SwApp\Center.SLDPRT", 1, 0, "", longstatus, longwarnings);
                swInsertedComponent = swAssembly.AddComponent4(@"D:\SwApp\Center.SLDPRT", "Default", 0, 0, 0);
                swApp.CloseDoc(@"D:\SwApp\Center.SLDPRT");

                /// Insert Rib component, store to temporary object and close the part file
                swApp.OpenDoc6(@"D:\SwApp\Rib.SLDPRT", 1, 0, "", longstatus, longwarnings);
                swInsertedComponent = swAssembly.AddComponent4(@"D:\SwApp\Rib.SLDPRT", "Default", 0, 0, 0);
                swApp.CloseDoc(@"D:\SwApp\Rib.SLDPRT");

                /// Mating Top Plane
                CoincidentPlaneMate("Top Plane", "Top Plane@Center-1@Assem1", Part, swAssembly);
                CoincidentPlaneMate("Top Plane", "Top Plane@Rib-1@Assem1", Part, swAssembly);

                /// Mating Front Plane
                CoincidentPlaneMate("Front Plane@Plate-1@Assem1", "Front Plane@Center-1@Assem1", Part, swAssembly);
                CoincidentPlaneMate("Front Plane@Plate-1@Assem1", "Front Plane@Rib-1@Assem1", Part, swAssembly);

                /// Mating Right Plane
                CoincidentPlaneMate("Right Plane@Plate-1@Assem1", "Right Plane@Center-1@Assem1", Part, swAssembly);
                CoincidentPlaneMate("Right Plane@Plate-1@Assem1", "Right Plane@Rib-1@Assem1", Part, swAssembly);

                /// Assembly created and store in the folder.
                longstatus = Part.SaveAsSilent(@"D:\SwApp\Assem1.SLDASM",false);
                swApplication.ShowMessageBox(@"Assembly Generated and Saved in D:\SwApp\Assem1.SLDASM");
                this.Close();
            }
        }
    }
}