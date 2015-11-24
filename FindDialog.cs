using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MemoryPlayer
{
	/// <summary>
	/// FindDialog
	/// Dialog for searching the memory log.
	/// Currently only searches the "name" column
	/// </summary>
	public class FindDialog : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Button buttonFindNext;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonFindPrev;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private Simulation mSimulation;
        private MainView mMainView;
        private System.Windows.Forms.CheckBox checkBoxRegEx;
        private System.Windows.Forms.CheckBox checkBoxCaseSensitive;
        private System.Windows.Forms.Label label2;

		public FindDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        public void Initialize( MainView mainView, Simulation sim )
        {
            mMainView = mainView;
            mSimulation = sim;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonFindNext = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonFindPrev = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxRegEx = new System.Windows.Forms.CheckBox();
            this.checkBoxCaseSensitive = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(80, 8);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(224, 20);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.Text = "";
            // 
            // buttonFindNext
            // 
            this.buttonFindNext.Location = new System.Drawing.Point(312, 8);
            this.buttonFindNext.Name = "buttonFindNext";
            this.buttonFindNext.Size = new System.Drawing.Size(72, 24);
            this.buttonFindNext.TabIndex = 2;
            this.buttonFindNext.Text = "Find Next";
            this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(312, 72);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(72, 24);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonFindPrev
            // 
            this.buttonFindPrev.Location = new System.Drawing.Point(312, 40);
            this.buttonFindPrev.Name = "buttonFindPrev";
            this.buttonFindPrev.Size = new System.Drawing.Size(72, 24);
            this.buttonFindPrev.TabIndex = 4;
            this.buttonFindPrev.Text = "Find Prev";
            this.buttonFindPrev.Click += new System.EventHandler(this.buttonFindPrev_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Using:";
            // 
            // checkBoxRegEx
            // 
            this.checkBoxRegEx.Location = new System.Drawing.Point(80, 56);
            this.checkBoxRegEx.Name = "checkBoxRegEx";
            this.checkBoxRegEx.Size = new System.Drawing.Size(184, 24);
            this.checkBoxRegEx.TabIndex = 8;
            this.checkBoxRegEx.Text = "Regular Expression";
            // 
            // checkBoxCaseSensitive
            // 
            this.checkBoxCaseSensitive.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.checkBoxCaseSensitive.Location = new System.Drawing.Point(80, 40);
            this.checkBoxCaseSensitive.Name = "checkBoxCaseSensitive";
            this.checkBoxCaseSensitive.Size = new System.Drawing.Size(184, 16);
            this.checkBoxCaseSensitive.TabIndex = 7;
            this.checkBoxCaseSensitive.Text = "Match Case";
            // 
            // FindDialog
            // 
            this.AcceptButton = this.buttonFindNext;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(392, 102);
            this.Controls.Add(this.checkBoxRegEx);
            this.Controls.Add(this.checkBoxCaseSensitive);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonFindPrev);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonFindNext);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindDialog";
            this.ShowInTaskbar = false;
            this.Text = "Find";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
		#endregion
   
        private bool Match( string str, string pattern )
        {
            RegexOptions options = RegexOptions.None;

            if ( !checkBoxCaseSensitive.Checked )
            {
                str = str.ToLower();
                pattern = pattern.ToLower();
            }
            else
            {
                options = RegexOptions.IgnoreCase;
            }
            
            if ( checkBoxRegEx.Checked )
            {
                // Make a regex!
                return Regex.IsMatch( str, pattern, options );
            }

            // Default search
            return str.IndexOf( pattern ) >= 0;
        }


        private void buttonFindNext_Click(object sender, System.EventArgs e)
        {
            for ( int i = mSimulation.CurrentAllocation + 1; i < mSimulation.MemoryLog.Count; i++ )
            {                
                MemoryLogItem item = mSimulation.MemoryLog.GetItem( i );                

                if ( Match( item.Name, textBoxSearch.Text ) )
                {
                    mMainView.Found( i );
                    break;
                }
            }

            /* not found! */
        }

        private void buttonFindPrev_Click(object sender, System.EventArgs e)
        {
            for ( int i = mSimulation.CurrentAllocation - 1; i >= 0; i-- )
            {
                MemoryLogItem item = mSimulation.MemoryLog.GetItem( i );

                if ( Match( item.Name, textBoxSearch.Text ) )
                {
                    mMainView.Found( i );
                    break;
                }
            }

            /* not found! */        
        }

        private void buttonClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
	}
}
