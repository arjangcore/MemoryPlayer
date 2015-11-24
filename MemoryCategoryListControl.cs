using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryPlayer
{
    /// <summary>
    /// MemoryCategoryListControl
    /// Controls the memory category selection
    /// TODO: this should probably have been a ListView not a ListBox
    /// </summary>
	public class MemoryCategoryListControl : System.Windows.Forms.CheckedListBox
	{        
		private System.ComponentModel.IContainer components = null;

		public MemoryCategoryListControl()
		{
			// This call is required by the Windows Form Designer.
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ( Items.Count == 0 ) return;

            CategoryManager.CategoryInfo info = (CategoryManager.CategoryInfo)Items[ e.Index ];

            e.DrawBackground();

            Rectangle bounds = e.Bounds;

            // Draw the check box
            ButtonState bs = ButtonState.Normal;
            if ( this.GetItemChecked( e.Index ) ) bs = ButtonState.Checked;
            ControlPaint.DrawCheckBox( e.Graphics, bounds.X, bounds.Y, bounds.Height, bounds.Height, bs );

            bounds.X += e.Bounds.Height + 4;

            e.Graphics.FillRectangle( new SolidBrush( info.Colour ), bounds.X, bounds.Y, bounds.Height * 2, bounds.Height );
            e.Graphics.DrawRectangle( Pens.Black, bounds.X, bounds.Y, bounds.Height * 2, bounds.Height );

            bounds.X += e.Bounds.Height * 2 + 4;

            // Draw the current item text based on the current Font and the custom brush settings.
            string name = info.Name;
            if ( info.Name == "" )
            {
                name = "No Category";
            }
            
            e.Graphics.DrawString( name, e.Font, new SolidBrush( e.ForeColor ), bounds, StringFormat.GenericDefault);

            // Get the max string len, and put the size after it.
            int maxWidth = 0;
            foreach (CategoryManager.CategoryInfo inf in Items )
            {
                string nm = inf.Name;
                if ( inf.Name == "" )
                {
                    nm = "No Category";
                }

                int width = (int)e.Graphics.MeasureString( nm, e.Font ).Width;
                if ( width > maxWidth )
                {
                    maxWidth = width;
                }
            }

            bounds.X += maxWidth + 12;            
            e.Graphics.DrawString( "Size: " + info.Size, e.Font, new SolidBrush( e.ForeColor ), bounds, StringFormat.GenericDefault);

			maxWidth = 0;
			foreach (CategoryManager.CategoryInfo inf in Items ) {
				string nm = "Size: " + inf.Size;
				int width = (int)e.Graphics.MeasureString( nm, e.Font ).Width;
				if ( width > maxWidth ) {
					maxWidth = width;
				}
			}

			bounds.X += maxWidth + 12;            
			e.Graphics.DrawString( "Peak: " + info.Highest, e.Font, new SolidBrush( e.ForeColor ), bounds, StringFormat.GenericDefault);

			// If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
        }
		#endregion
	}
}

