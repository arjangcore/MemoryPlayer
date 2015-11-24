using System;
using System.Collections;

namespace MemoryPlayer
{
    /// <summary>
    /// Collection of all memory log items
    /// </summary>
    public class MemoryLogItemCollection : IEnumerable
    {
        // public MemoryLogItemCollection( MemoryViewControl viewControl )
		public MemoryLogItemCollection()
        {
            mMemoryLogItems = new ArrayList();
            mCategories = new CategoryManager(); // new CategoryManager( viewControl );
            Clear();
        }

        public void Add(MemoryLogItem newItem)
        {
            mMemoryLogItems.Add(newItem);

			newItem.Category = mCategories.AssignCategory( newItem );

            // Attempt to match a Free with it's Alloc
            if ( newItem.Type == MemoryLogItem.ItemType.Free )
            {
                NameFreeBlock();
            }

            if (mEndTick < newItem.Time)
            {
                mEndTick = newItem.Time;
            }
            
            mNumOperations++;
            if (newItem.Type != MemoryLogItem.ItemType.Label && 
                newItem.Type != MemoryLogItem.ItemType.Size )
            {
                if (mMemoryAddressStart > newItem.Location)
                {
                    mMemoryAddressStart = newItem.Location;
                }

                if (mMemoryAddressEnd < (newItem.Location + newItem.Size))
                {
                    mMemoryAddressEnd = Convert.ToUInt32(newItem.Location + newItem.Size);
                }

                mMemorySize = mMemoryAddressEnd - mMemoryAddressStart;
            }
        }


        public int FindMatchingFree( int index )
        {
            MemoryLogItem allocItem = (MemoryLogItem)mMemoryLogItems[ index ];            

            for ( int i = index + 1; i < mMemoryLogItems.Count; i++ )
            {
                MemoryLogItem item = (MemoryLogItem)mMemoryLogItems[ i ];
                if ( item.Location == allocItem.Location )
                {
                    return i;
                }
            }

            // didn't find it, return -1
            return -1;
        }

        public void NameFreeBlock()
        {
            int count = mMemoryLogItems.Count;
            MemoryLogItem Item = (MemoryLogItem)mMemoryLogItems[count-1];
            if ((count >= 2) && (Item.Type == MemoryLogItem.ItemType.Free))
            {
                for (int index = count-2; index>=0; index--)
                {
                    MemoryLogItem PrevItem = (MemoryLogItem)mMemoryLogItems[index];
                    if ( (PrevItem.Type == MemoryLogItem.ItemType.Alloc) && (PrevItem.Location == Item.Location))
                    {
                        Item.SetName(PrevItem.Name);
                        Item.Pair = PrevItem;
                        PrevItem.Pair = Item;
                        break;
                    }
                }
            }

        }

        public void Clear()
        {
            mMemoryLogItems.Clear();
            mEndTick = 0;
            mNumOperations = 0;
            mMemoryAddressStart = uint.MaxValue;
            mMemoryAddressEnd = 0;
            mMemorySize = 0;
        }


        public int Count 
        { 
            get 
            { 
                return mMemoryLogItems.Count; 
            } 
        }


        public MemoryLogItem GetItem(int index)
        {
            return (MemoryLogItem)mMemoryLogItems[index];
        }


        public int NumOperations 
        { 
            get 
            { 
                return mNumOperations; 
            } 
        }

        public long MemoryStart 
        { 
            get 
            { 
                return mMemoryAddressStart; 
            } 
        }


        public long MemoryEnd 
        { 
            get 
            { 
                return mMemoryAddressEnd; 
            } 
        } 

        public long MemorySize 
        { 
            get 
            { 
                return mMemorySize; 
            } 
        } 

        public CategoryManager Categories
        {
            get
            {
                return mCategories;
            }
        }

        private CategoryManager mCategories;
        private ArrayList mMemoryLogItems;

        /// <summary>
        /// Last Tick for an allocation.  This defines the time span for the run.
        /// </summary>
        private int mEndTick;   
        /// <summary>
        /// Total number of allocations and frees.  Does not include labels
        /// </summary>
        private int mNumOperations;

        /// <summary>
        /// The lowest address of the allocations
        /// </summary>
        private uint mMemoryAddressStart;        

        /// <summary>
        /// The highest (address + size).  The end of the visible memory.
        /// </summary>
        private uint mMemoryAddressEnd;

        /// <summary>
        /// The total size of the logged memory space
        /// </summary>
        private uint mMemorySize;


        public virtual IEnumerator GetEnumerator()
        {
            return new MemoryLogItemCollection.Enumerator(this);
        }

        public class Enumerator : IEnumerator 
        {
            MemoryLogItemCollection outer;
            int currentIndex = -1;
            internal Enumerator(MemoryLogItemCollection outer)
            {
                this.outer = outer;
            }

            public object Current 
            { 
                get 
                {
                    if (currentIndex >= outer.mMemoryLogItems.Count) currentIndex = outer.mMemoryLogItems.Count - 1;
                    if (currentIndex < 0) currentIndex = 0;
                    
                    return outer.mMemoryLogItems[currentIndex];
                }
            }

            public bool MoveNext()
            {
                if (currentIndex >= outer.mMemoryLogItems.Count) currentIndex = outer.mMemoryLogItems.Count - 1;

                return ++currentIndex < outer.mMemoryLogItems.Count;
            }

            public bool MovePrevious()
            {
                if (currentIndex < 0) currentIndex = 0;

                return --currentIndex >= 0;
            }

            public bool IsEnd()
            {
                return currentIndex >= outer.mMemoryLogItems.Count - 1;
            }

            public bool IsStart()
            {
                return currentIndex <= 0;
            }

            public void Reset() 
            {
                currentIndex = -1;
            }
        }
    }
}
