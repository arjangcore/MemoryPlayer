using System;
using System.Drawing;
using System.Collections;
using System.Threading;

namespace MemoryPlayer
{
	/// <summary>
	/// MemoryGrapher
	/// Draws the memory view.
	/// </summary>
	public class MemoryGrapher
	{
		public MemoryGrapher( System.Windows.Forms.PictureBox box, Simulation sim )
		{
            mSimulation = sim;
            mDestRegion = box;

            mMemoryStart = sim.MemoryStart;
            mMemoryEnd = sim.MemoryEnd;

            mVisibleStart = mMemoryStart;
            mVisibleEnd = mMemoryEnd;

            mBoundingBoxStart = mMemoryStart;
            mBoundingBoxEnd = mMemoryEnd;

            mSimulation = sim;
		}


		public int Check(Point pt)
		{
			foreach (MemoryLogItem memLogItem in mSimulation.Allocations)
			{
				Rectangle rect = CreateRectangle(memLogItem);
				if (!rect.IsEmpty)
				{
					if (rect.Contains(pt))
					{
						return memLogItem.Id;
					}
				}
			}
			return -1;
		}

        private int mLastStart;
        private int mLastEnd;
        private bool ScaleChanged()
        {
            bool changed = 
                ( mLastStart != MemoryToVisible( MemoryStart ) )
             || ( mLastEnd != MemoryToVisible( MemoryEnd ) );

            if ( changed )
            {
                mLastStart = MemoryToVisible( MemoryStart );
                mLastEnd = MemoryToVisible( MemoryEnd );
                return true;
            }
            return false;
        }

		public void Draw(Graphics g)
        {    
            g.Clear( System.Drawing.Color.White );
            
            // Draw all rectangles
            foreach (MemoryLogItem memLogItem in mSimulation.Allocations)
			{
                Rectangle rect = CreateRectangle( memLogItem );                

                if ( !rect.IsEmpty )
                {
                    Brush brush = System.Drawing.Brushes.Black;
                    if ( memLogItem.Category != null && memLogItem.Category.Active )
                    {
                        brush = new SolidBrush( memLogItem.Category.Colour );
                    }

                    g.FillRectangle( brush, rect );                    
                }
			}            

            // Draw a tick at each megabyte boundary
            if ( MegBoundaryEnabled )
            {
                const int ONE_MEG = (1024 * 1024);
                int totalMegs = (int)((mSimulation.MemorySize) / ONE_MEG);
                for ( int megBoundary = 1; megBoundary <  totalMegs + 1; megBoundary++ )
                {
                    int x = MemoryToVisible( MemoryStart + megBoundary * ONE_MEG );
                    g.DrawLine( System.Drawing.Pens.Black, 
                        new Point( x, 0 ), 
                        new Point( x, mDestRegion.Size.Height/5 ) );
                    g.DrawLine( System.Drawing.Pens.Black, 
                        new Point( x, (mDestRegion.Size.Height/5) * 4 ), 
                        new Point( x, mDestRegion.Size.Height ) );
                }
            }

            if ( BoundingBoxEnabled )
            {
                int startX = MemoryToVisible( BoundingBoxStart );
                int endX = MemoryToVisible( BoundingBoxEnd );
                
                Pen pen = (Pen)System.Drawing.Pens.Black.Clone();
                pen.Width = 4;
                g.DrawRectangle( pen, startX,     1, endX - startX,     mDestRegion.Size.Height - 1 );                
            }
        }

        private Rectangle CreateRectangle(MemoryLogItem memLogItem)
        {
            // Map this item onto the visible space.
            int startX = MemoryToVisible( memLogItem.Location );
            int endX = MemoryToVisible( memLogItem.EndPoint );

            if ( endX - startX < 1 ) endX = startX + 1;

            if ( startX >= mDestRegion.Size.Width || endX <= 0 || endX - startX < 1 )
            {
                // Item completely invisible.  Don't draw
                return new Rectangle( 0, 0, 0, 0 );
            }            

            return new Rectangle(startX, 0, endX - startX, mDestRegion.Size.Height);
        }


        
        public int MemoryToVisible( long addr )
        {
            double memoryStart = MemoryStart;
            double visibleStart = (double)VisibleStart;
            double visibleEnd = (double)VisibleEnd;

            double visible = (double)(visibleEnd - visibleStart);

            return (int)(((double)(mDestRegion.Size.Width) * ((double)addr - (visibleStart))) / visible);
        }

        public long VisibleToMemory( int pixel )
        {
            double memoryStart = MemoryStart;
            double visibleStart = (double)VisibleStart;
            double visibleEnd = (double)VisibleEnd;

            double visible = (double)(VisibleEnd - VisibleStart);            

            return (long)(((double)pixel / ((double)mDestRegion.Size.Width)) * visible) + VisibleStart;

        }


        public long MemoryStart 
		{ 
			get 
			{ 
				return mMemoryStart; 
			} 
			set 
			{ 
				mMemoryStart = value; 
			} 
		} 

        public long MemoryEnd 
        { 
            get 
            { 
                return mMemoryEnd; 
            } 
            
            set 
            { 
                mMemoryEnd = value; 
            } 
        }

        public long VisibleStart
        { 
            get 
            { 
                return mVisibleStart; 
            } 
            set
            {
                // if ( value < mVisibleEnd )
                {
                    mVisibleStart = value;
                    mDestRegion.Invalidate();
                }
            }
        }

        public long VisibleEnd 
        { 
            get 
            { 
                return mVisibleEnd;
            }
            set
            {
                // if ( value > mVisibleStart )
                {
                    mVisibleEnd = value;                
                    mDestRegion.Invalidate();
                }
            }
        }

        public bool BoundingBoxEnabled
        {
            get 
            {
                return mBoundingBoxEnabled;
            }
            set
            {
                mBoundingBoxEnabled = value;
                mDestRegion.Invalidate();
            }
        }
        
        public long BoundingBoxStart
        {
            get
            {
                return mBoundingBoxStart;
            }
            set
            {
                // if ( value < mBoundingBoxEnd )
                {
                    mBoundingBoxStart = value;
                    mDestRegion.Invalidate();
                }
            }
        }

        public long BoundingBoxEnd
        {
            get
            {
                return mBoundingBoxEnd;
            }
            set
            {
                // if ( value > mBoundingBoxStart )
                {
                    mBoundingBoxEnd = value;
                    mDestRegion.Invalidate();
                }
            }
        }

        public bool MegBoundaryEnabled
        {
            get
            {
                return true;
            }
        }

        private Simulation mSimulation;
        private System.Windows.Forms.PictureBox mDestRegion;
        private long mMemoryStart;
        private long mMemoryEnd;
        private long mVisibleStart;
        private long mVisibleEnd;
        private bool mBoundingBoxEnabled;
        private long mBoundingBoxStart;
        private long mBoundingBoxEnd;
	}
}
