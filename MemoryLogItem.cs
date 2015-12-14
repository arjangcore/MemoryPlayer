using System;
using System.Diagnostics;

namespace MemoryPlayer 
{
	/// <summary>
	/// A single memory log entry, like allocate, free or label.
	/// </summary>
    public class MemoryLogItem
	{ // comment...
        public enum ItemType { Alloc, Free, Label, Size }
        private static String[] ItemTypeName = {"ALLOC", "FREE", "LABEL", "SIZE"};


        public static bool IsValid( string text )
        {
            String[] arr = text.Split(',');            

            if (arr[0] == ItemTypeName[(int)ItemType.Label]
                && arr.Length >= 3 )
            {
                return true;
            }
            else if ((arr[0] == ItemTypeName[(int)ItemType.Alloc] || 
                      arr[0] == ItemTypeName[(int)ItemType.Free] )
                && arr.Length >= 6 )
            {
                return true;
            }
            else if ( arr[0] == ItemTypeName[(int)ItemType.Size] 
                && arr.Length >= 6)
            {
                return true;
            }

            return false;
        }

        public MemoryLogItem(String unparsedText, int id)
        {
            mPair = null;            
            String[]arr = unparsedText.Split(',');
            int size;
			int request;
			uint location;
            int time;
            String name;
			mId = id;            

            if (arr[0] == ItemTypeName[(int)ItemType.Label] )
            {
                name = arr[2];
				location = 0;
				size = 0;
				request = 0;
                time = Convert.ToInt32(arr[1]);
            }
			else if (arr[0] == ItemTypeName[(int)ItemType.Alloc] ) {
				name = arr[1];
				location = Convert.ToUInt32(arr[2], 16);
				size = Convert.ToInt32(arr[3]);
                request = 1;// Convert.ToInt32(arr[4]);
				time = Convert.ToInt32(arr[6]);
			}
			else if ( arr[0] == ItemTypeName[(int)ItemType.Free] )
            {
                name = arr[1];
				location = Convert.ToUInt32(arr[2], 16);
				size = Convert.ToInt32(arr[3]);
                request = 2;// Convert.ToInt32(arr[4]);
                time = Convert.ToInt32(arr[6]);
            }
            else if ( arr[0] == ItemTypeName[(int)ItemType.Size])
            {
                name = arr[1];
				location = Convert.ToUInt32(arr[2], 16);
				size = Convert.ToInt32(arr[3]);
				request = 0;
				time = Convert.ToInt32(arr[5]);
            }
            else
            {
                // bad data
                return ;
            }

            if ( name == null )
            {
                Console.WriteLine( "null: " + location );
            }

            Initialize(name, arr[0], size, request, location, time);

        }

		public MemoryLogItem(String name, String type, int size, uint location, int time, int id)
		{
            mId = id;
			Initialize(name, type, size, 0, location, time);
		}

		public void SetName(String newname)
		{
			mName = newname;
		}

        private void Initialize(String name, String type, int size, int request, uint location, int time)
        {
            mName = name;
            mSize = size;
			mRequest = request;
			mLocation = location;
            mEndPoint = Convert.ToUInt32(location + size);
            mTime = time;
 
            // Map type name onto type
            if (ItemTypeName[(int) ItemType.Alloc] == type)
            {
                mType = ItemType.Alloc;
            }
            else if (ItemTypeName[(int) ItemType.Free] == type)
            {
                mType = ItemType.Free;
            }
            else if (ItemTypeName[(int) ItemType.Label] == type)
            {
                mType = ItemType.Label;
            }
            else if (ItemTypeName[(int) ItemType.Size] == type)
            {
                mType = ItemType.Size;
            }
            else
            {
                Debug.Assert(false, "Invalid Item type");
            }

        }

        private String mName;
        private ItemType mType;
        private int mSize;
		private int mRequest;
		private uint mLocation;
        private int mTime;
        private uint mEndPoint;
		private int mId;        
        private MemoryLogItem mPair;
        private CategoryManager.CategoryInfo mCategory;

        
        // Category the memory is in (e.g. GUI::, SGSM::)
        public CategoryManager.CategoryInfo Category
        {
            get
            {
                return mCategory;
            } 
            set
            {
                mCategory = value;
            } 
        }

        // column B from Excel file *pg*
		public String Name 
		{
			get
			{
				return mName;
			} 
		}
        

		public int Id 
		{
			get
			{
				return mId;
			} 
		}

		public ItemType Type { get { return mType; } }
        
		// "ALLOC", "FREE" or "LABEL", or "SIZE"
		public String TypeName 
		{ 
			get 
			{ 
				return ItemTypeName[(int)mType];
			} 
		}

        public int Size 
        { 
            get 
            { 
                return mSize; 
            } 
        }

        public uint EndPoint 
        { 
            get 
            { 
                return mEndPoint; 
            } 
        }

        public uint Location 
        { 
            get 
            { 
                return mLocation; 
            } 
        }
        
        public int Time 
        { 
            get 
            { 
                return mTime; 
            } 
        }


        public MemoryLogItem Pair
        {
            get
            {
                return mPair;
            }

            set 
            {
                mPair = value;
            }
        }
	}
}
