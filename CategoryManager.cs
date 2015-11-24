using System;
using System.Collections;
using System.Windows.Forms;

namespace MemoryPlayer
{
	/// <summary>
	/// CategoryManager manages memory categories, choosing display colour, etc.
	/// </summary>
	public class CategoryManager
	{
        public class CategoryInfo
        {
            public CategoryInfo()
            {
                Size = 0;
				Highest = 0;
				Active = true;
            }

            public string Name;
            public int Id;            
			public long Highest;
			public long Size;
            public bool Active;

            private System.Drawing.Color mColour;
            public System.Drawing.Color Colour
            {
                get
                {
                    return mColour;
                }
                set
                {
                    mColour = value;
                }
            }
        };

		// public CategoryManager( MemoryViewControl viewControl )
		public CategoryManager()
		{
            mCategories = new Hashtable();
            // mMemoryViewControl = viewControl;

            CategoryInfo info = new CategoryInfo();
            info.Name = "";
            info.Id = 0;            
            info.Colour = System.Drawing.Color.LightGray;
            mCategories[ "" ] = info;

			// Replaced with GetDefaultCategory and assigned elsewhere
			// mMemoryViewControl.CategoryList.Items.Add( info, true );
            // mMemoryViewControl.CategoryList.ItemCheck += new ItemCheckEventHandler( CategoryList_itemCheck );            
		}

		public CategoryInfo GetDefaultCategory()
		{
			return (CategoryInfo)mCategories[""];
		}

        public CategoryInfo AssignCategory( MemoryLogItem item )
        {            
            CategoryInfo info = GetCategory( item.Name );

            if ( info == null )
            {
                info = new CategoryInfo();
                info.Name = item.Name.Split( ':' )[ 0 ];
                info.Id = mCategories.Keys.Count;
                info.Colour = ColourWheel.GetColour( info.Id - 1 );
                info.Size = 0;
				info.Highest = 0;

                // Console.WriteLine( "Creating Category " + info.Id + ": " + info.Name );

                // Find a new category, loop around if we've used all existing brushes
                
				mCategories[ info.Name ] = info;

				// VC: re-implemented at AddLogItem level
                // mMemoryViewControl.CategoryList.Items.Add( info, true );
            }
            
            return info;
        }

        public void ResetSizes()
        {
            foreach ( CategoryInfo info in mCategories.Values )
            {
                info.Size = 0;
				info.Highest = 0;
			}
        }

        public void AllocItem( MemoryLogItem item )
        {
            CategoryInfo info = GetCategory( item.Name );
            info.Size += item.Size;
			if( info.Size > info.Highest )
				info.Highest = info.Size;
        }

        public void FreeItem( MemoryLogItem item )
        {            
            CategoryInfo info = GetCategory( item.Name );
            info.Size -= item.Size;
        }


        public CategoryInfo GetCategory( string name )
        {
            CategoryInfo info = null;
            
            if ( mCategories.ContainsKey( name ) )
            {
                return (CategoryInfo)mCategories[ name ];
            }

            string[] strarray = name.Split(':');

            // "Global" allocations - no name
            if ( strarray.Length <= 1 )
            {
                info = (CategoryInfo)mCategories[ "" ];
            }
            else if ( mCategories.ContainsKey( strarray[0] ) )
            {
                info = (CategoryInfo)mCategories[ strarray[0] ];
            }

            return info;
        }

        private Hashtable mCategories;
        // private MemoryViewControl mMemoryViewControl;

		public IDictionaryEnumerator GetEnumerator()
		{
			return mCategories.GetEnumerator();
		}
	}
}
