/// <summary>
/// Main namespace of the project defined across different source files
/// </summary>
namespace SwApp
{
    /// <summary>
    /// Class : PropertyEditorForm
    /// Purpose : This form is prepared for UI. It will display the assembly tree and 
    ///           allow user to choose required parts for BOM.
    /// Note : This is partial class. The other part is in PropertyEditorForm.cs file
    /// </summary> 
    partial class PropertyEditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <return>Nil</return>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyEditorForm));
            this.BackPanelSpltCnt = new System.Windows.Forms.SplitContainer();
            this.AssemblyTreeView = new BrightIdeasSoftware.TreeListView();
            this.ComponentClmn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.IsBomClmn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.GuiImgLst = new System.Windows.Forms.ImageList(this.components);
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BackPanelSpltCnt)).BeginInit();
            this.BackPanelSpltCnt.Panel1.SuspendLayout();
            this.BackPanelSpltCnt.Panel2.SuspendLayout();
            this.BackPanelSpltCnt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AssemblyTreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // BackPanelSpltCnt
            // 
            this.BackPanelSpltCnt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackPanelSpltCnt.Location = new System.Drawing.Point(0, 0);
            this.BackPanelSpltCnt.Name = "BackPanelSpltCnt";
            this.BackPanelSpltCnt.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // BackPanelSpltCnt.Panel1
            // 
            this.BackPanelSpltCnt.Panel1.Controls.Add(this.AssemblyTreeView);
            // 
            // BackPanelSpltCnt.Panel2
            // 
            this.BackPanelSpltCnt.Panel2.Controls.Add(this.CancelBtn);
            this.BackPanelSpltCnt.Panel2.Controls.Add(this.OkBtn);
            this.BackPanelSpltCnt.Size = new System.Drawing.Size(784, 561);
            this.BackPanelSpltCnt.SplitterDistance = 505;
            this.BackPanelSpltCnt.TabIndex = 1;
            // 
            // AssemblyTreeView
            // 
            this.AssemblyTreeView.AllColumns.Add(this.ComponentClmn);
            this.AssemblyTreeView.AllColumns.Add(this.IsBomClmn);
            this.AssemblyTreeView.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AssemblyTreeView.BackColor = System.Drawing.Color.White;
            this.AssemblyTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AssemblyTreeView.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClickAlways;
            this.AssemblyTreeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ComponentClmn,
            this.IsBomClmn});
            this.AssemblyTreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.AssemblyTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssemblyTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.AssemblyTreeView.ForeColor = System.Drawing.Color.Black;
            this.AssemblyTreeView.GridLines = true;
            this.AssemblyTreeView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AssemblyTreeView.HideSelection = false;
            this.AssemblyTreeView.LabelWrap = false;
            this.AssemblyTreeView.Location = new System.Drawing.Point(0, 0);
            this.AssemblyTreeView.MultiSelect = false;
            this.AssemblyTreeView.Name = "AssemblyTreeView";
            this.AssemblyTreeView.RowHeight = 20;
            this.AssemblyTreeView.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(160)))), ((int)(((byte)(185)))));
            this.AssemblyTreeView.ShowGroups = false;
            this.AssemblyTreeView.Size = new System.Drawing.Size(784, 505);
            this.AssemblyTreeView.SmallImageList = this.GuiImgLst;
            this.AssemblyTreeView.TabIndex = 0;
            this.AssemblyTreeView.UseCompatibleStateImageBehavior = false;
            this.AssemblyTreeView.View = System.Windows.Forms.View.Details;
            this.AssemblyTreeView.VirtualMode = true;
            this.AssemblyTreeView.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.AssemblyTreeViewCellEditFinishing);
            this.AssemblyTreeView.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.AssemblyTreeViewCellEditStarting);
            // 
            // ComponentClmn
            // 
            this.ComponentClmn.AspectName = "ComponentName";
            this.ComponentClmn.Text = "Component";
            this.ComponentClmn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ComponentClmn.Width = 500;
            // 
            // IsBomClmn
            // 
            this.IsBomClmn.AspectName = "IsBom";
            this.IsBomClmn.Text = "IsBOM?";
            this.IsBomClmn.Width = 289;
            // 
            // GuiImgLst
            // 
            this.GuiImgLst.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("GuiImgLst.ImageStream")));
            this.GuiImgLst.TransparentColor = System.Drawing.Color.Transparent;
            this.GuiImgLst.Images.SetKeyName(0, "assm");
            this.GuiImgLst.Images.SetKeyName(1, "no");
            this.GuiImgLst.Images.SetKeyName(2, "part");
            this.GuiImgLst.Images.SetKeyName(3, "yes");            
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(685, 6);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 36);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtnClicked);
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(598, 6);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(80, 36);
            this.OkBtn.TabIndex = 0;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtnClicked);
            // 
            // PropertyEditorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.BackPanelSpltCnt);
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "PropertyEditorForm";
            this.BackPanelSpltCnt.Panel1.ResumeLayout(false);
            this.BackPanelSpltCnt.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BackPanelSpltCnt)).EndInit();
            this.BackPanelSpltCnt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AssemblyTreeView)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer BackPanelSpltCnt;
        public BrightIdeasSoftware.OLVColumn ComponentClmn;
        public BrightIdeasSoftware.OLVColumn IsBomClmn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        public BrightIdeasSoftware.TreeListView AssemblyTreeView;
        private System.Windows.Forms.ImageList GuiImgLst;
    }
}