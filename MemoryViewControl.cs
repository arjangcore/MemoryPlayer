using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MemoryPlayer
{
	/// <summary>
	/// MemoryViewControl is a custom control.  It manages all the Tab things in the main form.
	/// </summary>
    
	public class MemoryViewControl : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageLabels;
        private System.Windows.Forms.TabPage tabPageClasses;
        private System.Windows.Forms.TabPage tabPageCategories;
        private System.Windows.Forms.ListView labelLog;
        private System.Windows.Forms.ColumnHeader Label_Type;
        private System.Windows.Forms.ColumnHeader Label_Name;
        private System.Windows.Forms.ColumnHeader Label_Time;
        private System.Windows.Forms.ColumnHeader Label_Operation;
        private MemoryCategoryListControl categoryList;
        private System.Windows.Forms.Label label1;

        // Events
        public event EventHandler LabelLogChanged;
        public event EventHandler CategoriesChanged;
        // public event EventHandler ClassesChanged;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MemoryViewControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        public ListView LabelLog
        {
            get
            {
                return labelLog;
            }
        }

        public CheckedListBox CategoryList
        {
            get
            {                
                return categoryList;
            }
        }


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCategories = new System.Windows.Forms.TabPage();
            this.categoryList = new MemoryPlayer.MemoryCategoryListControl();
            this.tabPageLabels = new System.Windows.Forms.TabPage();
            this.labelLog = new System.Windows.Forms.ListView();
            this.Label_Type = new System.Windows.Forms.ColumnHeader();
            this.Label_Name = new System.Windows.Forms.ColumnHeader();
            this.Label_Time = new System.Windows.Forms.ColumnHeader();
            this.Label_Operation = new System.Windows.Forms.ColumnHeader();
            this.tabPageClasses = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPageCategories.SuspendLayout();
            this.tabPageLabels.SuspendLayout();
            this.tabPageClasses.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCategories);
            this.tabControl.Controls.Add(this.tabPageLabels);
            this.tabControl.Controls.Add(this.tabPageClasses);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(528, 416);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageCategories
            // 
            this.tabPageCategories.Controls.Add(this.categoryList);
            this.tabPageCategories.Location = new System.Drawing.Point(4, 22);
            this.tabPageCategories.Name = "tabPageCategories";
            this.tabPageCategories.Size = new System.Drawing.Size(520, 390);
            this.tabPageCategories.TabIndex = 2;
            this.tabPageCategories.Text = "Categories";
            // 
            // categoryList
            // 
            this.categoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categoryList.Location = new System.Drawing.Point(0, 0);
            this.categoryList.Name = "categoryList";
            this.categoryList.Size = new System.Drawing.Size(520, 379);
            this.categoryList.TabIndex = 0;
            this.categoryList.SelectedIndexChanged += new System.EventHandler(this.categoryList_SelectedIndexChanged);
            this.categoryList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.categoryList_ItemCheck);
            // 
            // tabPageLabels
            // 
            this.tabPageLabels.Controls.Add(this.labelLog);
            this.tabPageLabels.Location = new System.Drawing.Point(4, 22);
            this.tabPageLabels.Name = "tabPageLabels";
            this.tabPageLabels.Size = new System.Drawing.Size(520, 390);
            this.tabPageLabels.TabIndex = 0;
            this.tabPageLabels.Text = "Labels";
            // 
            // labelLog
            // 
            this.labelLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                                                                                       this.Label_Type,
                                                                                       this.Label_Name,
                                                                                       this.Label_Time,
                                                                                       this.Label_Operation});
            this.labelLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLog.FullRowSelect = true;
            this.labelLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.labelLog.HideSelection = false;
            this.labelLog.Location = new System.Drawing.Point(0, 0);
            this.labelLog.MultiSelect = false;
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(520, 390);
            this.labelLog.TabIndex = 16;
            this.labelLog.View = System.Windows.Forms.View.Details;
            this.labelLog.SelectedIndexChanged += new System.EventHandler(this.labelLog_SelectedIndexChanged);
            // 
            // Label_Type
            // 
            this.Label_Type.Text = "Type";
            this.Label_Type.Width = 40;
            // 
            // Label_Name
            // 
            this.Label_Name.Text = "Name";
            this.Label_Name.Width = 40;
            // 
            // Label_Time
            // 
            this.Label_Time.Text = "Time";
            this.Label_Time.Width = 40;
            // 
            // Label_Operation
            // 
            this.Label_Operation.Text = "Operation#";
            this.Label_Operation.Width = 40;
            // 
            // tabPageClasses
            // 
            this.tabPageClasses.Controls.Add(this.label1);
            this.tabPageClasses.Location = new System.Drawing.Point(4, 22);
            this.tabPageClasses.Name = "tabPageClasses";
            this.tabPageClasses.Size = new System.Drawing.Size(520, 390);
            this.tabPageClasses.TabIndex = 1;
            this.tabPageClasses.Text = "Classes";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(520, 390);
            this.label1.TabIndex = 0;
            this.label1.Text = "Realmem does not provide hooks to determine memclass creation/destruction";
            // 
            // MemoryViewControl
            // 
            this.Controls.Add(this.tabControl);
            this.Name = "MemoryViewControl";
            this.Size = new System.Drawing.Size(528, 416);
            this.tabControl.ResumeLayout(false);
            this.tabPageCategories.ResumeLayout(false);
            this.tabPageLabels.ResumeLayout(false);
            this.tabPageClasses.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void labelLog_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ( LabelLogChanged != null )
            {
                LabelLogChanged( sender, e );
            }
        }

        private void categoryList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ( CategoriesChanged != null )
            {
                CategoriesChanged( sender, e );
            }
        }

        private void categoryList_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if ( CategoriesChanged != null )
            {
                CategoriesChanged( sender, e );
            }                   
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint (e);
        }

	}
}
