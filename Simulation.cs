using System;
using System.Collections;

namespace MemoryPlayer
{
	/// <summary>
	/// Simulation.  Main class used to track current state of memory.
	/// </summary>
    public class Simulation
    {
        public Simulation(MemoryLogItemCollection logItems)
        {
            mLogItemCollection = logItems;
            mActiveAllocations = new ArrayList();
            mCurrentAllocation = -1;
        }


        public void Advance()
        {            
            // Check if there is room to advance
            if (mCurrentAllocation >= mLogItemCollection.Count - 1)
            {
                mCurrentAllocation = mLogItemCollection.Count - 1;
                return;
            }

            mCurrentAllocation++;

            // Update Active state of items            
            MemoryLogItem curItem  = mLogItemCollection.GetItem( mCurrentAllocation );            
            switch ( curItem.Type )
            {
                case MemoryLogItem.ItemType.Alloc:
                    mActiveAllocations.Add( curItem );
                    mLogItemCollection.Categories.AllocItem( curItem );
                    break;

                case MemoryLogItem.ItemType.Free:
                    // Find the alloc are deactiveate it!
                    // Do a linear search backwards
                    if ( curItem.Pair != null )
                    {                        
                        mActiveAllocations.Remove( curItem.Pair );
                        mLogItemCollection.Categories.FreeItem( curItem.Pair );
                    }
                    else
                    {
                        // What is this freeing???
                        // WARNING!
                    }
                    break;
            }

        }

        /// <summary>
        /// Runs the simulation to the given count.  Uses the advance and reverse
        /// </summary>
        /// <param name="allocationCount"></param>
        public void MoveTo(int targetAllocation)
        {
            if (targetAllocation < 0 || targetAllocation >= mLogItemCollection.Count)
            {
                throw new ArgumentException("Simulation::moveTo: Invalid argument", "targetAllocation");
            }

            if (mCurrentAllocation > targetAllocation)
            {
                // Can't go backwards a step at a time.  Reset to 0, and advance to this point.
                Reset();
                MoveTo( targetAllocation );
                /*
                while (mCurrentAllocation > targetAllocation)
                {
                    Reverse();
                }
                */
            }

            if (mCurrentAllocation < targetAllocation)
            {
                // go forward
                while (mCurrentAllocation < targetAllocation)
                {
                    Advance();
                }
            }
        }


        /// <summary>
        /// Set the simulation to the beginning
        /// </summary>
        public void Reset()
        {
            mCurrentAllocation = -1;
            mActiveAllocations.Clear();
            mLogItemCollection.Categories.ResetSizes();
        }


        public void Reverse()
        {
            mCurrentAllocation--;

            if ( mCurrentAllocation < 0 )
            {
                Reset();
                mCurrentAllocation = -1;
                return;
            }


            // Update Active state of items            
            MemoryLogItem curItem  = mLogItemCollection.GetItem( mCurrentAllocation );            
            switch ( curItem.Type )
            {
                case MemoryLogItem.ItemType.Alloc:
                    mActiveAllocations.Remove( curItem );
                    mLogItemCollection.Categories.FreeItem( curItem );
                    break;

                case MemoryLogItem.ItemType.Free:
                    // Find the alloc are deactiveate it!
                    // Do a linear search backwards
                    if ( curItem.Pair != null )
                    {                        
                        mActiveAllocations.Add( curItem.Pair );
                        mLogItemCollection.Categories.AllocItem( curItem.Pair );
                    }
                    else
                    {
                        // What is this freeing???
                    }
                    break;
            }
        }


        /// <summary>
        /// Value of -1 indicates an empty state.
        /// </summary>
        public int CurrentAllocation 
        { 
            get 
            { 
                return mCurrentAllocation; 
            } 
        }

        public ArrayList Allocations
        {
            get 
            {
                return mActiveAllocations;
            }
        }

        public MemoryLogItemCollection MemoryLog
        {
            get
            {
                return mLogItemCollection;
            }
        }

        /// <summary>
        /// Returns true if the simulation has started and there is a valid current allocation.
        /// </summary>
        public bool Started 
        { 
            get 
            { 
                return mCurrentAllocation > -1;
            } 
        }

        public long MemoryStart
        {
            get
            {
                return mLogItemCollection.MemoryStart;
            }
        }

        public long MemoryEnd
        {
            get
            {
                return mLogItemCollection.MemoryEnd;
            }
        }

        public long MemorySize
        {
            get
            {
                return MemoryEnd - MemoryStart;
            }
        }

        public long MemoryFree
        {
            get
            {
                long free = MemoryEnd - MemoryStart;
                return free - MemoryAllocated;
            }
        }

        public long MemoryAllocated
        {
            get
            {
                long alloc = 0;
                foreach ( MemoryLogItem curItem in Allocations )
                {
                    if ( curItem.Type == MemoryLogItem.ItemType.Alloc )
                    {
                        alloc += curItem.Size;
                    }
                }
                return alloc;
            }
        }

        private MemoryLogItemCollection mLogItemCollection;
        private int mCurrentAllocation;
        private ArrayList mActiveAllocations;
	}
}
