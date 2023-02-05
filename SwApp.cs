/*************************************************************************************
 * Term Project : Solidworks Extension to implement automation of reading a 3D model,*
 *                creating drawing and exporting the BOM with picture to an excel    *
 *                sheet and UI to create parts and assembly of BUSH.                 *
 * Purpose      : Entry point of the program, includes driving class,methods to call *
 *                and traverse across all the files in the project                   * 
 * File Name    : SwApp.cs                                                           *  
 * Team Mates   : Dhanush, Prashanth, Nikhil                                         *
**************************************************************************************/


/// *******************************USINGS*******************************************************
/// System defines commonly used values and reference data types
/// Xarial.Xcad enables the abstraction layers for the CAD API allowing CAD agnostic development
/// SwApp.AssemblyCreation references the BushAssembly UI
/// System.Windows.Forms enables demonstrating UI,error messages, handling events and exceptions
using System;
using System.ComponentModel;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SwApp.AssemblyCreation;
using SwApp.Properties;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;

/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
    /// <summary>
    /// Class  :   SwApp
    /// Purpose: This class defines the command/ Tabs made avaialble to user in Solidworks CAD tool
    /// </summary>
    /// Explicitly setting COM visibility to true so that the class will be visible to Solidworks API
    [ComVisible(true)]
    public class SwApp : SwAddInEx
    {
        [Title("SwApp-Project")]
        public enum Commands_e
        {
            /// <summary>
            /// Plugin buttons/Tabs that will be created in Solidworks CAD tool
            /// </summary>
            [Icon(typeof(Resources), nameof(Resources.bracket))]
            [Title("Create Bush")]
            [Description("Creates the parts and Assembly of Bush")]
            [CommandItemInfo(WorkspaceTypes_e.Assembly | WorkspaceTypes_e.Part)]
            CreateBushAssembly,

            [Icon(typeof(Resources), nameof(Resources.assembly))]
            [Title("Assembly Properties")]
            [Description("Edit the assembly properties")]
            [CommandItemInfo(WorkspaceTypes_e.Assembly | WorkspaceTypes_e.Part | WorkspaceTypes_e.Drawing)]
            AssemblyProperties,

            [Icon(typeof(Resources), nameof(Resources.drawing))]
            [Title("Generate Drawing")]
            [Description("Iso view drawings and balloons are generated")]
            [CommandItemInfo(WorkspaceTypes_e.Assembly | WorkspaceTypes_e.Part | WorkspaceTypes_e.Drawing)]
            GenerateDrawing,

            [Icon(typeof(Resources), nameof(Resources.bom))]
            [Title("Export BOM")]
            [Description("BOM is generated along with the screenshot of components")]
            [CommandItemInfo(WorkspaceTypes_e.Assembly | WorkspaceTypes_e.Part | WorkspaceTypes_e.Drawing)]
            ExportBom
        }

        /// <summary>
        /// Main Function: OnConnect
        /// Purpose:       Entry point of the program.Creating addins in Solidworks when starting the execution of the program
        /// </summary>
        public override void OnConnect()
        {
            CommandManager.AddCommandGroup<Commands_e>().CommandClick += OnButtonClick;
        }

        /// <summary>
        ///  Initializing the PropertyEditorForm object and assigning null value
        /// </summary>
        public static PropertyEditorForm IPropertyEditorGuiObj = null;

        /// <summary>
        /// Function: OnButtonClick 
        /// Purpose:  Executes different commands based on the user selection
        /// </summary>
        /// <param name="cmd"> command clicked/selected by the user </param>
        [STAThread]
        private void OnButtonClick(Commands_e cmd)
        {
            AssemblyReading assemblyRead = new AssemblyReading(Application);

            /// Switch cases depending on the user selection
            switch (cmd)
            {
                /// On Create Bush assembly button, The function calls the Bush UI to perform all operations
                case Commands_e.CreateBushAssembly:
                {

                    try
                    {
                        /// Initialising the Bush assembly UI application
                        BushAssembly bushAssembly = new BushAssembly(Application);
                        DialogResult dialogResult = bushAssembly.ShowDialog();
                        Application.ShowMessageBox("Successfully created Bush assembly");
                    }
                    catch
                    {
                        Application.ShowMessageBox("Error in creating Bush assembly");
                    }

                    break;
                }

                /// Commands to be executed when user selects Assembly Properties
                /// BOM structure of the active document will be read and data populated in the GUI window
                case Commands_e.AssemblyProperties:
                {
                    try
                    {
                        /// Instance of PropertyEditorForm assemPropEditor
                        /// Assigned to initialised IPropertyEditorGuiObj for adding data to GUI window
                        PropertyEditorForm assemPropEditor = new PropertyEditorForm();
                        IPropertyEditorGuiObj = assemPropEditor;

                        /// GUI Column Icon Settings
                        assemblyRead.PropertyEditorAssemblySettings(assemPropEditor);

                        /// Callig ReadAssembly method to assign BOM structure data to allComps
                        ComponentsData allComps = assemblyRead.ReadAssembly(Application);

                        /// Adding BOM structure data to the GUI window
                        assemPropEditor.AssemblyTreeView.AddObject(allComps);

                        /// Displaying the expanded tree view in the GUI window
                        assemPropEditor.AssemblyTreeView.ExpandAll();
                        assemPropEditor.Show();
                    }
                    catch
                    {
                        Application.ShowMessageBox("Error in reading the assemby files/components");
                    }

                    break;
                }

                /// Command to generate the drawing from the assembly
                case Commands_e.GenerateDrawing:
                {
                    try
                    {
                        /// Active assembly opened in solidworks.
                        ModelDoc2 activeAssmDoc = (ModelDoc2)Application.Sw.ActiveDoc;

                        /// Creating object for Drawing Generation.
                        DrawingGeneration dwgGen = new DrawingGeneration(Application, activeAssmDoc);

                        /// New drawing sheet is created.
                        dwgGen.CreateNewSheet();

                        /// The various views are added to the drawing sheet. BOM is inserted. Ballos are also inserted.
                        dwgGen.InsertView();

                        /// Confirmation message to user for successful drawing creation.
                        /// Some version of solidworks may require the user to click an icon to return to main sheet.
                        Application.ShowMessageBox("Successfully generated Drawing with four views and inserted BOM table\n\n" +
                            "If you dont see the views, please click the semi-transparent Blue Arrow visible on top right " +
                            "of the solidworks drawing window" + "to see the completed drawing sheet.");
                    }
                    catch
                    {
                        Application.ShowMessageBox("Error in creating the drawing views with BOM table");
                    }

                    break;
                }

                /// Command to export the BOM as per user selections into the Excel file.
                case Commands_e.ExportBom:
                {
                    try
                    {
                        ModelDoc2 activeDwgDoc = (ModelDoc2)Application.Sw.ActiveDoc;
                        activeDwgDoc.Save();

                        /// This will call the exportBOM method and outputs an Excel File
                        ExportBom exportBom = new ExportBom(Application, activeDwgDoc);
                        exportBom.ExportBomToExcel1();

                        /// Confirmation message to user.
                        Application.ShowMessageBox("Successfully exported BOM to Excel");

                    }
                    catch (Exception e)
                    {
                        Application.ShowMessageBox("Error in exporting BOM to Excel");
                    }

                    break;
                }
            }
        }
    }
}
