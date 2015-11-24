// --------------------------------------------------------------------------------------------------------
// Copyright: (c) 2005 by Electronic Arts Canada. All rights reserved.
// --------------------------------------------------------------------------------------------------------

// This file separates most of the user interface from the logic.

using System;
using System.Collections; // ArrayList
using System.IO; // TextWriter
using System.Data; // DataSet, DataTable

namespace MemoryPlayer
{
	public class MemoryPlayerLogic
	{
		protected FileReader mFileReader;
		private MemoryLogItemCollection mMemoryLogItems;
		private Simulation mSimulation;
		private System.Windows.Forms.Timer mTimer;
		private bool mIsSimming;
		
		DataSet mBudget;

		// GUI-related stuff that should eventually get moved out
		private bool mInteractiveMode;
		private MainView mMainView;
		private MemoryViewControl mMemoryViewControl;

		public MemoryPlayerLogic(bool interactiveMode,
			MainView mainView, MemoryViewControl memoryViewControl)
		{
			mMemoryLogItems = new MemoryLogItemCollection(); // MemoryLogItemCollection(memoryViewControl);
			mSimulation = null;
			mFileReader = null;
			mIsSimming = false;
			mInteractiveMode = interactiveMode;

			mBudget = new DataSet();
			mBudget.DataSetName = "Heap Budget";

			if(mInteractiveMode)
			{
				// HACK: Victor is sick of refactoring other people's code
				mMainView = mainView;
				mMemoryViewControl = memoryViewControl;
				mTimer = new System.Windows.Forms.Timer();
				mTimer.Tick += new EventHandler(TimerHandler);
			}
			else
			{
				mMainView = null;
				mMemoryViewControl = null;
				mTimer = null;
			}
		}

		public bool IsInteractiveMode
		{
			get
			{
				return mInteractiveMode;
			}
		}

		private void TimerHandler(Object myObject, EventArgs myEventArgs)
		{
			if (mIsSimming)
			{
				ArrayList stringList = mFileReader.ReadLoop();
				
				if(mSimulation == null)
				{
					mSimulation = new Simulation(mMemoryLogItems);
				}

				if(mInteractiveMode)
				{
					UpdateGUI(stringList);
				}
			}
		}

		public Simulation MemorySimulation
		{
			get
			{
				return mSimulation;
			}
		}

		public MemoryLogItemCollection MemoryLogItems
		{
			get
			{
				return mMemoryLogItems;
			}
		}

		public bool IsSimming
		{
			get
			{
				return mIsSimming;
			}
			set
			{
				mIsSimming = value;
			}
		}

		public System.Windows.Forms.Timer LogicTimer
		{
			get
			{
				return mTimer;
			}
		}

		/// <summary>
		/// Reads a memory log file into memory (har har).
		/// </summary>
		/// <param name="filename">The filename to read in.</param>
		public bool ReadFile(String filename)
		{
			mMemoryLogItems.Clear();
			mSimulation = null;

			mFileReader = new FileReader(500); // FileReader(this, 500);
			
			try 
			{
				mFileReader.Start( filename );                
			}
			catch(FileNotFoundException fe)
			{
				if(mInteractiveMode)
				{
					System.Windows.Forms.MessageBox.Show(
						"File Not Found: " + fe.FileName,
						"Open Memory Log File",
						System.Windows.Forms.MessageBoxButtons.OK,
						System.Windows.Forms.MessageBoxIcon.Stop);
					return false;
				}
				else
				{
					Console.WriteLine("File not found: " + fe.FileName);
					return false;
				}
			}

			ArrayList s = mFileReader.ReadLoop();
			mSimulation = new Simulation(mMemoryLogItems);

			if(mInteractiveMode)
			{
				UpdateGUI(s);

				mTimer.Interval = 500;
				mTimer.Start();
			}
			else
			{
				UpdateLog(s);
			}

			return true;
		}

		private void UpdateLog(ArrayList stringList)
		{
			if(stringList != null)
			{
				foreach ( string s in stringList )
				{
					AddLogItem(s);
				}
			}
		}

		/// <summary>
		/// Dumps a summary of the memory log file similar to the
		/// Category View Control contents.
		/// </summary>
		/// <param name="infile">The input filename.</param>
		/// <param name="outfile">The desired output filename.</param>
		public void WriteFile(String infile, String outfile)
		{
			StreamWriter sw = new StreamWriter(outfile, false);

			WriteCsvHeader(sw, infile, outfile);
			sw.WriteLine("Category Breakdown");
			WriteCategorySummaryCsv(sw);
			sw.WriteLine("");
			sw.WriteLine("Budget Comparison");
			WriteHeapBudgetComparisonCsv(sw);

			sw.Flush();
			sw.Close();
		}

		private void WriteCsvHeader(StreamWriter sw, String infile, String outfile)
		{
			sw.WriteLine("Source File,Date,Time");

			try
			{
				DateTime infiletime = File.GetLastWriteTime(infile);
				sw.WriteLine("{0},{1},{2}", Path.GetFileName(infile),
					infiletime.ToShortDateString(),
					infiletime.ToShortTimeString());
			}
			catch
			{
				sw.WriteLine("{0},{1},{2}", infile, "Unknown Date", "Unknown Time");
			}
			
			DateTime now = DateTime.Now;
			sw.WriteLine("");
			sw.WriteLine("Destination File,Date,Time");
			sw.WriteLine("{0},{1},{2}", Path.GetFileName(outfile),
				now.ToShortDateString(), now.ToShortTimeString());
			sw.WriteLine("");
		}

		private void WriteCategorySummaryCsv(StreamWriter sw)
		{
			// header information
			sw.WriteLine("Category,Size,Peak");
			long totalsize = 0;

			// dump the categories
			IDictionaryEnumerator e = mMemoryLogItems.Categories.GetEnumerator();
			while(e.MoveNext())
			{
				CategoryManager.CategoryInfo c =
					(CategoryManager.CategoryInfo)e.Value;
				String name;
				if(c.Name != String.Empty)
				{
					name = c.Name;
				}
				else
				{
					name = "No Category";
				}

				// Convert.ToString(c.Colour.ToArgb(), 16);
				sw.WriteLine("{0},{1},{2}", name, c.Size, c.Highest);
				totalsize += c.Size;
			}

			// total size
			sw.WriteLine("Total,{0}", totalsize);
		}

		public void ReadHeapBudget(String xmlfilename)
		{
			try
			{
				mBudget.ReadXml(xmlfilename);
			}
			catch
			{
				Console.WriteLine("Error reading budget XML file");
			}
		}

		private void WriteHeapBudgetComparisonCsv(StreamWriter sw)
		{
			int totalactual = 0;
			int totalbudgetmb = 0;
			// if(mBudget.Tables.Contains("heap_budget"))
			if(mBudget.Tables.Count == 1)
			{
				sw.WriteLine("Name,Actual (KB),Budget (KB),Delta (KB),Actual (MB),Budget (MB),Delta (MB)");
				// foreach(DataRow dr in mBudget.Tables["heap_budget"].Rows)
				foreach(DataRow dr in mBudget.Tables[0].Rows)
				{
					// dump the categories
					CategoryManager.CategoryInfo c_primary, c_secondary = null;

					c_primary =
						(CategoryManager.CategoryInfo)mMemoryLogItems.Categories.GetCategory((String)dr["name"]);
					
					if((String)dr["category_add"] != "")
					{
						c_secondary = 
							(CategoryManager.CategoryInfo)mMemoryLogItems.Categories.GetCategory((String)dr["category_add"]);
					}

					if(c_primary == null)
					{
						c_primary = c_secondary;
						c_secondary = null;
					}

					if(c_primary == null)
					{
						// not good - no categories mapped to budget
					}

					dr["actual"] = c_primary.Size;
					if(c_secondary != null)
					{
						dr["actual"] = Convert.ToInt32(dr["actual"].ToString(), 10) + c_secondary.Size;
					}
					
					int actual = Convert.ToInt32(dr["actual"].ToString(), 10);
					int budget = Convert.ToInt32(dr["budget"].ToString(), 10);
					dr["delta"] = actual - (budget * 1048576);

					// Convert.ToString(c.Colour.ToArgb(), 16);
					sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", dr["name"],
						Math.Round(Convert.ToDouble(dr["actual"].ToString()) / 1024.0f, 2),
						Math.Round(Convert.ToDouble(dr["budget"].ToString()) * 1024.0f, 2),
						Math.Round(Convert.ToDouble(dr["delta"].ToString()) / 1024.0f, 2),
						Math.Round(Convert.ToDouble(dr["actual"].ToString()) / 1048576.0f, 2),
						Math.Round(Convert.ToDouble(dr["budget"].ToString()), 2),
						Math.Round(Convert.ToDouble(dr["delta"].ToString()) / 1048576.0f), 2);

					totalactual += actual;
					totalbudgetmb += budget;
				}

				sw.WriteLine("Total,{0},{1},{2},{3},{4},{5}",
					Math.Round(totalactual / 1024.0f, 2),
					Math.Round(totalbudgetmb * 1024.0f, 2),
					Math.Round((totalactual - totalbudgetmb * 1048576) / 1024.0f, 2),
					Math.Round(totalactual / 1048576.0f, 2),
					Math.Round((float)totalbudgetmb, 2),
					Math.Round((totalactual / 1048576.0f - totalbudgetmb), 2));
			}

		}

		// add a log item to the GUI list
		public void AddLogItem(String text)
		{
			MemoryLogItem item = new MemoryLogItem( text, mMemoryLogItems.Count );

			if ( item.Name != null )
			{
				mMemoryLogItems.Add( item );
				if(mInteractiveMode && !mMemoryViewControl.CategoryList.Items.Contains(item.Category))
				{
					mMemoryViewControl.CategoryList.Items.Add( item.Category, true );
				}
			}
		}

		// HACK: Victor is sick of refactoring other people's code
		private void UpdateGUI(ArrayList stringList)
		{
			mMainView.StartGUIUpdate();
			UpdateLog(stringList);
			mMainView.EndGUIUpdate();
		}
	}
}