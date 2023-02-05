/*************************************************************************************
 * Term Project     : Solidworks Extension to implement automation of reading a 3D   *
 *                    model, creating individual parts and assembly file from them,  *
 *                    creating drawing and exporting the BOM with picture to an      *
 *                    excel sheet.                                                   *
 * Team Mates       : Dhanush, Prashanth, Nikhil                                     *
 * File Name        : AssemblyReading.cs                                             *                            
 ************************************************************************************/

/// *******************************USINGS*********************************************
using System.Collections.Generic;
using System.Windows.Documents;
using SolidWorks.Interop.sldworks;
using System.Linq;

/// *******************************NAMESPACE*******************************************
/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
/// **********************************CLASS*********************************************
    /// <summary>
    /// Class  : ComponentsData
    /// Purpose: Storage of data of all components in the CAD assembly file including the sub-assemblies and parts inside them
    /// </summary>
    public class ComponentsData
    {
        public Component2 CadComponent { get; set; }
        public string ComponentName { get; set; }
        public List<ComponentsData> ChildComponents { get; set; }
        public string IsBom { get; set; }

        /// <summary>
        /// Property: IsAssembly
        /// Purpose:  Checking if the component has any BOM structure(children) under it        
        /// </summary>
        /// <returns> Bool value True/False - bool type </returns>
        public bool IsAssembly
        {
            get
            {
                if (ChildComponents.Any())
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Property: CanExpand
        /// Purpose:  Checking if the component can be expanded to obtain BOM tree        
        /// </summary>
        /// <returns> Bool value True/False - bool type </returns>
        public bool CanExpand
        {
            get
            {
                if (ChildComponents.Any())
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///  Constructor: Initialising the object created with the class variables.
        /// </summary>
        public ComponentsData()
        {
            ComponentName = string.Empty;
            ChildComponents = new List<ComponentsData>(0);
            IsBom = "Yes";
            CadComponent = null;
        }
    }
}