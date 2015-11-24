using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace MemoryPlayer
{
	/// <summary>
	/// MemoryPicture is a custom control for the MemoryGrapher
	/// </summary>
	public class MemoryPicture : System.Windows.Forms.UserControl
	{
        private System.Windows.Forms.PictureBox mainView;
        private MemoryGrapher mGrapher;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MemoryPicture()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            mGrapher = null;
		}

        public void Initialize( Simulation sim )
        {
            mGrapher = new MemoryGrapher( mainView, sim );
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainView = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// mainView
			// 
			this.mainView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainView.Location = new System.Drawing.Point(0, 0);
			this.mainView.Name = "mainView";
			this.mainView.Size = new System.Drawing.Size(712, 150);
			this.mainView.TabIndex = 0;
			this.mainView.TabStop = false;
			this.mainView.Paint += new System.Windows.Forms.PaintEventHandler(this.mainView_Paint);
			this.mainView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mainView_MouseMove);
			this.mainView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mainView_MouseDown);
			// 
			// MemoryPicture
			// 
			this.Controls.Add(this.mainView);
			this.Name = "MemoryPicture";
			this.Size = new System.Drawing.Size(712, 150);
			this.ResumeLayout(false);

		}
		#endregion

        private void mainView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if ( mGrapher != null )
            {
                mGrapher.Draw( e.Graphics );
            }
            else
            {
                e.Graphics.Clear( System.Drawing.SystemColors.Control );
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown (e);
        }
        
        private void mainView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnMouseDown( e );            
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove (e);
        }

        private void mainView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnMouseMove( e );
        }

        public MemoryGrapher Grapher
        {
            get
            {
                return mGrapher;
            }
        }
	}
}
