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
using System.Collections;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using Xarial.XCad.Base.Enums;
using Xarial.XCad.SolidWorks;

/// *******************************NAMESPACE*******************************************
/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
/// **********************************CLASS*********************************************
    /// <summary>
    /// Class:   AssemblyReading
    /// Purpose: This class consists of objects and methods to read the required data from 3D assembly file
    /// </summary>
    public class AssemblyReading
    {
        public ModelDoc2 ActiveDoc = null;
        private readonly ISldWorks _swApplicationSw = null;

        /// <summary>
        /// Constructor to create new object of the class
        /// Assigning the Solidworks application and the active document in it to the object
        /// </summary>
        public AssemblyReading(ISwApplication application)
        {
            _swApplicationSw = application.Sw;
            ActiveDoc = (ModelDoc2)_swApplicationSw.ActiveDoc;
        }

        /// <summary>
        /// Method : ReadAssembly
        /// Purpose: This will read the 3D assembly file by accessing the methods from IComponent2 Interface in Solidworks API
        /// </summary>
        /// <param name="swApplication"> Solidworks application opened </param>
        /// <returns name="allComps"> Data of all the components including sub-assemblies and their child parts - ComponentsData type </returns> 
        public ComponentsData ReadAssembly(ISwApplication swApplication)
        {
            /// Initialising object of type ComponentsData in order to store the BOM structure information
            ComponentsData rootCompData = new ComponentsData();

            /// Assigning the currently opened and active CAD file in Solidworks CAD tool
            ActiveDoc = (ModelDoc2)_swApplicationSw.ActiveDoc;

            /// Checking if a CAD file is opened in Solidworks CAD tool
            if (ActiveDoc != null)
            {
                /// Assigning root or the main assembly information to 'rootComponent'
                Component2 rootComponent = (Component2)ActiveDoc.ConfigurationManager.ActiveConfiguration.GetRootComponent();

                /// Initialising 'rootCompData' which will be used to get information on the child components(BOM structure) inside 'rootcomponent'
                rootCompData = new ComponentsData();

                /// Checking if the CAD file consists of child components(BOM)
                rootCompData.IsBom = "Yes";

                /// Obtaining main CAD file name
                rootCompData.ComponentName = rootComponent.Name2;

                /// Obtaining the clild components(BOM) of the main CAD file
                rootCompData.ChildComponents = new List<ComponentsData>(0);

                /// Obtaining CAD information
                rootCompData.CadComponent = rootComponent;

                /// Calling TraverseAssemblyTree method to update 'allComps' with the complete child components(BOM) information
                TraverseAssemblyTree(rootComponent, rootCompData);
            }

            /// Displaying error message if there is no active CAD file opened in Solidworks
            else
            {
                swApplication.ShowMessageBox("Please open the assembly file", MessageBoxIcon_e.Error, MessageBoxButtons_e.Ok);
            }
   
            return rootCompData;
        }

        /// <summary>
        /// Function : TraverseAssemblyTree
        /// Purpose:   This function reads the assembly tree (Child components/BOM) of CAD file recursively.
        ///            It updates the object with information of every child components(BOM) contained in it.
        ///            Solidworks API types are used
        /// </summary>
        /// <param name="iComponent"> Solidworks component/assembly file - Component2 type </param>
        /// <param name="iComponentsData"> Data of the components - ComponentsData type </param>
        /// <returns> Nil </returns>
        public void TraverseAssemblyTree(Component2 iComponent, ComponentsData iComponentsData)
        {
            try
            {

                /// Initialising an object array with the child component/BOM.
                object[] children = (object[])iComponent.GetChildren();

                /// Checking if the main CAD file consists of child components/BOM - children
                if (children.Length > 0)
                {
                    /// Get children for each Child in this subassembly.
                    foreach (Component2 child in children)
                    {
                        ComponentsData childCompData = new ComponentsData();

                        /// Checking if each child consists of more children within them(sub-assembly)
                        childCompData.IsBom = "Yes";
                        childCompData.ComponentName = child.Name2;
                        childCompData.CadComponent = child;
                        iComponentsData.ChildComponents.Add(childCompData);

                        /// Recursively calling itself in order to run through all the child components and obtaining information on children contained inside each child.
                        /// Thus all the files in the complete BOM structure will be traversed obtaining the necessary information
                        TraverseAssemblyTree(child, childCompData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to read assembly file: {e}");
            }
        }

        /// <summary>
        /// Method : PropertyEditorAssemblySettings
        /// Purpose: Calling different methods from the SOlidworks API to fill out User interface created.
        ///          Providing options for user in UI to select which part/components to be exported to Excel BOM.
        ///          The different schemes/images to be shown based on selection- yes/No is also included.
        /// </summary>
        /// <param name="iPropertyEditorForm"> User interface form - PropertyEditorForm type </param>
        /// <returns> Different type of returns depending upon methods called inside </returns>
        public void PropertyEditorAssemblySettings(PropertyEditorForm iPropertyEditorForm)
        {
            /// Checking if the file consists of BOM structure(tree)
            iPropertyEditorForm.AssemblyTreeView.CanExpandGetter = delegate (object x)
            {
                bool b = true;
                b = x is ComponentsData;

                return b;
            };

            /// Getting the Child components for BOM tree
            iPropertyEditorForm.AssemblyTreeView.ChildrenGetter = delegate (object x)
            {
                object b = null;
                b = ((ComponentsData)x).ChildComponents;

                return (IEnumerable)b;
            };

            /// Expanding the BOM structure in UI
            iPropertyEditorForm.AssemblyTreeView.CanExpandGetter = delegate (object row)
            {
                bool b = ((ComponentsData)row).CanExpand;

                return b;
            };

            /// Checking if the file is assembly or part and setting the image accordingly
            iPropertyEditorForm.ComponentClmn.ImageGetter = delegate (object row)
            {
                bool isAssembly = ((ComponentsData)row).IsAssembly;
                if (isAssembly)
                {
                    return "assm";
                }

                else
                {
                    return "part";
                }
            };

            /// Changing the image in UI depending on whether the user selected Yes or No for BOM export option 
            iPropertyEditorForm.IsBomClmn.ImageGetter = delegate (object row)
            {
                string isBom = ((ComponentsData)row).IsBom;
                if (isBom == "Yes")
                {
                    return "yes";
                }

                else
                {
                    return "no";
                }
            };
        }
    }
}