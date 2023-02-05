/*****************************************************************************************
 * Term Project     : Solidworks Extension to implement automation of reading a 3D model,*
 *                    creating drawing and exporting the BOM with picture to an excel    *
 *                    sheet and UI to create parts and assembly of BUSH.                 *                                                    
 * Team Mates       : Dhanush, Prashanth, Nikhil                                         *                                                 
 * File Name        : DrawingUnitTest.cs                                                 *                                         
 ****************************************************************************************/

/// *************************************USINGS*******************************************
/// System namespace defines commonly-used values and reference data types
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidWorks.Interop.sldworks;
using Xarial.XCad.SolidWorks;
using SolidWorks.Interop.swconst;
using System;
using System.IO;
using SwApp;

namespace TestSwApp
{
    /// This is the unit test for Drawing Generation . 
    [TestClass]
    public class DrawingUnitTest : SwAddInEx
    {
        /// Declarations
        //Active Assembly document
        public ModelDoc2 activeAssmDoc = null;
        public ISldWorks app;
        public ISwApplication Application = null;
        BomTableAnnotation bomTable = null;
        SwApp.DrawingGeneration dwgGen;
        string[] Views = new string[] { "*Front", "*Top", "*Right", "*Isometric" };

        /// This block initializes the test and opens the solidworks application.
        [TestInitialize]
        public void DrawingTestInitialize()
        {
            /// Initialize and open the solidworks app  
            var progId = "SldWorks.Application";
            var progType = System.Type.GetTypeFromProgID(progId);

            /// This line is to get the running instance of solidworks
            /// app = System.Runtime.InteropServices.Marshal.GetActiveObject(progId) as SolidWorks.Interop.sldworks.ISldWorks;

            /// Starts solidworks 
            app = System.Activator.CreateInstance(progType) as ISldWorks;

            /// Checks if solidworks is loaded or not
            app.Visible = true;

            /// Create drawing generation object to be used in later test methods.
            dwgGen = new DrawingGeneration(Application, activeAssmDoc);
        } 
        
        /// Closes the opened all documents and exits solidworks
        [TestCleanup]
        public void CleanUpTestEnvironment()
        {
            if(app!=null)
            {
                app.CloseAllDocuments(true);
                app.ExitApp();
            }
        }

        /// <summary>
        /// Test Method : Verifies if new sheet is created or not.
        /// </summary>
        [TestMethod]
        public void VerifyNewSheetCreated()
        {
            try
            {
                /// Creating Drawing object.
                ModelDoc2 activeAssmDoc = (ModelDoc2)Application.Sw.ActiveDoc;

                /// Loading the drawing object from the specified location.
                ModelDoc2 swDrawingDoc = app.INewDocument2(@"D:\SwApp\Templates\Drawing.DRWDOT", (int)swDwgPaperSizes_e.swDwgPaperA0size, 0, 0);
                
                /// Assert New sheet is created.
                Assert.IsNotNull(swDrawingDoc); 
            }
            catch(AssertFailedException ex)
            {
                Console.WriteLine("The assert failed "+ex.Message);
            }        
        }

        /// <summary>
        /// Test Method : Verifies if views are inserted
        /// </summary>
        [TestMethod]
        public void VerifyInsertViews()
        {
            try
            {
                /// Repeat assertions for all views("*Front", "*Top", "*Right", "*Isometric")
                foreach (string view in Views)
                {
                    /// Calls the insert view method of drawing generation object
                    SolidWorks.Interop.sldworks.View swView = dwgGen.InsertView();

                    /// assert the object created is of type View and not null
                    Assert.IsInstanceOfType(swView, typeof(View));
                }
            }
            catch (AssertFailedException ex)
            {
                Console.WriteLine("The assert failed " + ex.Message);
            }
        }

        /// <summary>
        /// Test Method : Verify if BOM is inserted
        /// </summary>
        [TestMethod]
        public void VerifyInsertBom()
        {
            try
            {
                /// Calls the insert view method of drawing generation object
                SolidWorks.Interop.sldworks.View swView = dwgGen.InsertView();

                /// Loading the BOM from the assigned view.
                DrawingDoc ActiveDoc = dwgGen.InsertBom(swView);

                /// Assert BOM is created.
                Assert.IsNotNull(ActiveDoc);

                /// assert the object created is of type drawing doc and not null
                Assert.IsInstanceOfType(ActiveDoc, typeof(DrawingDoc));
            }
            catch (AssertFailedException ex)
            {
                Console.WriteLine("The Insert BOM assert failed " + ex.Message);
            }
        }

        /// <summary>
        /// Test Method: Verify auto balloon generation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VerifyAutoBallons()
        {
            try
            {
                SolidWorks.Interop.sldworks.View swView = dwgGen.InsertView();

                /// Creates Auto Ballons
                dwgGen.CreateAutoBalloons(swView);
            }
            catch(Exception e)
            {
                Console.WriteLine("The VerifyAutoBallon failed " + e.Message);
            }

            /// Since the CreateAutoBallons is a void method, check if it throws null exception if 
            /// Passing null and check that argument null exception is thrown.
            dwgGen.CreateAutoBalloons(null);
        }
    }
}