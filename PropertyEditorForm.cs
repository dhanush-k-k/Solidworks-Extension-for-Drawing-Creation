/*************************************************************************************
 * Term Project : Solidworks Extension to implement automation of reading a 3D model,*
 *                creating drawing and exporting the BOM with picture to an excel    *
 *                sheet and UI to create parts and assembly of BUSH.                 *
 * Purpose      : An UI to display the tree view of assemblies, along with functions * 
 *                to expand the view to see child components, drop down menu to      *
 *                select whether part should be included in BOM using event handlers *
 * File Name    : PropertyEditorForm.cs                                              *  
 * Team Mates   : Dhanush, Prashanth, Nikhil                                         *
**************************************************************************************/


/// *******************************USINGS*******************************************************
/// System defines commonly used values and reference data types
/// Xarial.Xcad enables the abstraction layers for the CAD API allowing CAD agnostic development
/// System.Windows.Forms enables demonstrating UI,error messages, handling events and exceptions
/// BrightIdeasSoftware enables the Tree view in the UI
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Microsoft.Office.Interop.Excel;

/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
    /// <summary>
    /// Class   : PropertyEditorForm
    /// Purpose : This form is prepared for UI. It will display the assembly tree and 
    ///           allow user to choose required parts for BOM.
    /// Note : This is partial class. The other part is in PropertyEditorForm.Designer.cs file
    /// </summary> 
    public partial class PropertyEditorForm : Form
    {
        /// <summary>
        /// Constructor : PropertyEditorForm 
        /// Purpose : To initialize the component of the UI form
        /// </summary>
        /// <param>No Arguments are passed</param>
        /// <return> Nil </return>
        public PropertyEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method  : AssemblyTreeViewCellEditStarting
        /// Purpose : To populate the tree view and allow user select whether the component 
        ///           should be taken into BOM by drop menu of Yes or No
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return>Nil</return>
        private void AssemblyTreeViewCellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            /// Object of ComponentsData to populate the tree in component column
            /// This is also coupled with backend methods in Assembly Reading
            ComponentsData eRowObject = (ComponentsData)e.RowObject;

            /// Checking if the column is component column
            /// If true proceed to Bom column check
            if (e.Column == SwApp.IPropertyEditorGuiObj.ComponentClmn)
            {
                e.Cancel = true;
                return;
            }

            /// Checking the if column is Bom column 
            /// If its a Bom column selection option and drop down menu is provided
            if (e.Column == SwApp.IPropertyEditorGuiObj.IsBomClmn)
            {
                /// Checking whether the row object is assembly
                if (eRowObject.IsAssembly)
                {
                    e.Cancel = true;
                    return;
                }

                /// ComboBox object cb for styling
                /// Drop down style and Yes or No options provided
                /// Initial value is Yes for all the components of tree
                ComboBox cb = new ComboBox();
                cb.Bounds = e.CellBounds;
                cb.Font = ((ObjectListView)sender).Font;
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Items.Clear();
                cb.Items.Add("Yes");
                cb.Items.Add("No");
                cb.SelectedIndex = 0; //Initial Value

                /// Allowing user to change from Yes or No 
                string isDetail = eRowObject.IsBom;
                switch (isDetail)
                {
                    case "Yes":
                        cb.SelectedIndex = 0;
                        break;

                    case "No":
                        cb.SelectedIndex = 1;
                        break;
                }

                /// As per the selection from user the event logged
                cb.SelectedIndexChanged += delegate (object o, EventArgs args)
                {
                    ((ComponentsData)e.RowObject).IsBom = cb.SelectedText;
                };

                e.Control = cb;
            }
        }

        /// <summary>
        /// Method  : AssemblyTreeViewCellEditFinishing
        /// Purpose : After the user has selected the required option, the list is redrawn and 
        ///          updated the model, finally cell editing is finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return>Nil</return>
        private void AssemblyTreeViewCellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            /// Initializing the object of ComponentsData
            ComponentsData eRowObject = (ComponentsData)e.RowObject;

            /// Checking if the options are updated in the Bom Column 
            if (e.Column == SwApp.IPropertyEditorGuiObj.IsBomClmn)
            {
                if (e.Control is ComboBox)
                {
                    ComboBox eControl = (ComboBox)e.Control;
                    int selectedIndex = eControl.SelectedIndex;
                    var eControlSelectedItem = (string)eControl.SelectedItem;
                    eRowObject.IsBom = eControlSelectedItem;
                }
            }

            /// Any updating will have been down in the SelectedIndexChanged event handler
            /// Making the list redraw the involved ListViewItem
            ((ObjectListView)sender).RefreshItem(e.ListViewItem);

            /// Updated the model object, thus canceling the auto update
            e.Cancel = true;
        }

        /// <summary>
        /// Method : OkBtnClicked
        /// Purpose : Send the event details to backend and close the UI dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtnClicked(object sender, EventArgs e)
        {
            SwApp.IPropertyEditorGuiObj.Close();
        }

        /// <summary>
        /// Method : CancelBtnClicked
        /// Purpose : To cancel the operation and close the UI dialog box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <return>Nil</return>
        private void CancelBtnClicked(object sender, EventArgs e)
        {
            SwApp.IPropertyEditorGuiObj.Close();
        }      
    }
}

