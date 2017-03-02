using System;
using System.IO;
using System.Collections;
using System.Windows.Forms;
// a change for remote git test
namespace MemoryPlayer
{
	/// <summary>
	/// FileReader reads the memory log
	/// ReadLoop is meant to be called periodically to read any new entries.
	/// </summary>
    public class FileReader
    {
        private MainView mMainView;
        private StreamReader mFileReader;
        private int mReadFrequency;
        private long mLastMaxOffset;

		public FileReader( int readFreq )
		{
			mMainView = null;
			mReadFrequency = readFreq;
		}

		// deprecated
        public FileReader( MainView player, int readFreq )
        {
            mMainView = player;
            mReadFrequency = readFreq;
        }

        public int ReadFrequency
        {
            get
            {
                return mReadFrequency;
            }

            set
            {
                mReadFrequency = value;
            }
        }

        public void Start( string filename )
        {
            mLastMaxOffset = 0;

			try
			{
				mFileReader = new StreamReader(new FileStream(filename, 
					FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			}
			catch(FileNotFoundException fe)
			{
				throw fe;
			}
        }

        public ArrayList ReadLoop()
        {
            //if the file size has not changed, return
            if ( mFileReader.BaseStream.Length > mLastMaxOffset )
            {
                //seek to the last max offset
                mFileReader.BaseStream.Seek(mLastMaxOffset, SeekOrigin.Begin);

                //read out of the file until the EOF
                string line = "";
                long curPosition = mLastMaxOffset;
                ArrayList stringList = new ArrayList();

                    
                while ( (line = mFileReader.ReadLine()) != null )
                {
                    // quick sanity check of string
                    // sometimes while reading we will get bad lines as the log files is being written to
                    // in this case, stop reading and try to get a full line next time
                    if ( MemoryLogItem.IsValid( line ) )
                    {
                        stringList.Add( line );
                    }
                    else
                    {
                        Console.WriteLine( "BAD LINE: " + line );
                        // break;
                    }
                    mLastMaxOffset = mFileReader.BaseStream.Position;

                }

                return stringList;
            }

			return null;
        }

	}
}
