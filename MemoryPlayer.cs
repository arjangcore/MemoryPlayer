using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;


namespace MemoryPlayer
{
    /// <summary>
    /// MainView.  Main form of the app.  Handles most of the GUI functionality and program logic
    /// </summary>
    public class MainView : System.Windows.Forms.Form
    {
		private MemoryPlayerLogic mLogic;
		private ArrayList mUndoBuffer;		

        private FindDialog mFindDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelMemoryView;
        private System.Windows.Forms.Splitter splitterTopBottom;

        private System.Windows.Forms.MenuItem menuItemAttachTo;

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemOpen;
        private System.Windows.Forms.MenuItem menuItemFileSplitter;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader size;
        private System.Windows.Forms.ColumnHeader memAddress;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.Splitter splitterTop;
        private MemoryPlayer.MemoryViewControl memoryViewControl;
        private System.Windows.Forms.ListView memoryLog;
        private System.Windows.Forms.MenuItem menuItemEdit;
        private System.Windows.Forms.MenuItem menuItemFind;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListToolBar;

        private System.Windows.Forms.MenuItem menuItemUndo;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private MemoryPlayer.MemoryPicture memoryPictureZoom;
        private System.Windows.Forms.HScrollBar scrollBarZoom;
        private System.Windows.Forms.Splitter splitterMemoryViews;
        private MemoryPlayer.MemoryPicture memoryPictureFull;
        private System.Windows.Forms.ToolBar toolBar;
        private System.Windows.Forms.ToolBarButton toolBarButtonPlayback;
        private System.Windows.Forms.ToolBarButton toolBarButtonPause;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel statusBarStart;
        private System.Windows.Forms.StatusBarPanel statusBarPos;
        private System.Windows.Forms.StatusBarPanel statusBarMemory;
        private System.Windows.Forms.StatusBarPanel statusBarEnd;
        private System.Windows.Forms.GroupBox memoryViewPanel;
        private System.Windows.Forms.StatusBarPanel statusBarMemoryFree;
        private System.Windows.Forms.StatusBarPanel statusBarMemoryAllocated;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItemRefresh;
        private int mUndoPos;
		
        public MainView()
        {
            //
            // Required for Windows Form Designer support
            //
			InitializeComponent();
			mLogic = new MemoryPlayerLogic(true, this, memoryViewControl);

			memoryViewControl.CategoryList.Items.Add(
				mLogic.MemoryLogItems.Categories.GetDefaultCategory(), true);
			memoryViewControl.CategoryList.ItemCheck += new ItemCheckEventHandler( CategoryList_itemCheck );            

			memoryViewControl.LabelLogChanged += new EventHandler( LabelLog_SelectedIndexChanged );
			memoryViewControl.CategoriesChanged += new EventHandler( Categories_SelectedIndexChanged );

			SetPlayButtonImage();
			ResetUndo();
        }

		public MainView(String budgetfile, String infile, String outfile)
		{
			mLogic = new MemoryPlayerLogic(false, null, null);
			mLogic.ReadHeapBudget(budgetfile);
			bool success = mLogic.ReadFile(infile);
			if(success)
			{
				mLogic.MemorySimulation.MoveTo(mLogic.MemoryLogItems.Count - 1);
				mLogic.WriteFile(infile, outfile);
			}
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainView));
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemOpen = new System.Windows.Forms.MenuItem();
			this.menuItemAttachTo = new System.Windows.Forms.MenuItem();
			this.menuItemFileSplitter = new System.Windows.Forms.MenuItem();
			this.menuItemExit = new System.Windows.Forms.MenuItem();
			this.menuItemEdit = new System.Windows.Forms.MenuItem();
			this.menuItemUndo = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItemFind = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItemRefresh = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemAbout = new System.Windows.Forms.MenuItem();
			this.imageListToolBar = new System.Windows.Forms.ImageList(this.components);
			this.panelTop = new System.Windows.Forms.Panel();
			this.memoryViewControl = new MemoryPlayer.MemoryViewControl();
			this.splitterTop = new System.Windows.Forms.Splitter();
			this.memoryLog = new System.Windows.Forms.ListView();
			this.type = new System.Windows.Forms.ColumnHeader();
			this.name = new System.Windows.Forms.ColumnHeader();
			this.size = new System.Windows.Forms.ColumnHeader();
			this.memAddress = new System.Windows.Forms.ColumnHeader();
			this.time = new System.Windows.Forms.ColumnHeader();
			this.splitterTopBottom = new System.Windows.Forms.Splitter();
			this.panelMemoryView = new System.Windows.Forms.Panel();
			this.memoryViewPanel = new System.Windows.Forms.GroupBox();
			this.splitterMemoryViews = new System.Windows.Forms.Splitter();
			this.memoryPictureZoom = new MemoryPlayer.MemoryPicture();
			this.memoryPictureFull = new MemoryPlayer.MemoryPicture();
			this.scrollBarZoom = new System.Windows.Forms.HScrollBar();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonPlayback = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonPause = new System.Windows.Forms.ToolBarButton();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarStart = new System.Windows.Forms.StatusBarPanel();
			this.statusBarPos = new System.Windows.Forms.StatusBarPanel();
			this.statusBarMemory = new System.Windows.Forms.StatusBarPanel();
			this.statusBarMemoryAllocated = new System.Windows.Forms.StatusBarPanel();
			this.statusBarMemoryFree = new System.Windows.Forms.StatusBarPanel();
			this.statusBarEnd = new System.Windows.Forms.StatusBarPanel();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.panelTop.SuspendLayout();
			this.panelMemoryView.SuspendLayout();
			this.memoryViewPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPos)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemoryAllocated)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemoryFree)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarEnd)).BeginInit();
			this.SuspendLayout();
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Memory Dump (*.csv)|*.csv";
			this.openFileDialog.InitialDirectory = ".";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItemFile,
																					 this.menuItemEdit,
																					 this.menuItem1});
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 0;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemOpen,
																						 this.menuItemAttachTo,
																						 this.menuItemFileSplitter,
																						 this.menuItemExit});
			this.menuItemFile.Text = "File";
			// 
			// menuItemOpen
			// 
			this.menuItemOpen.Index = 0;
			this.menuItemOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.menuItemOpen.Text = "Open";
			this.menuItemOpen.Click += new System.EventHandler(this.menuItemFile_Click);
			// 
			// menuItemAttachTo
			// 
			this.menuItemAttachTo.Index = 1;
			this.menuItemAttachTo.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.menuItemAttachTo.Text = "Attach";
			this.menuItemAttachTo.Click += new System.EventHandler(this.menuItemAttachTo_Click);
			// 
			// menuItemFileSplitter
			// 
			this.menuItemFileSplitter.Index = 2;
			this.menuItemFileSplitter.Text = "-";
			// 
			// menuItemExit
			// 
			this.menuItemExit.Index = 3;
			this.menuItemExit.Text = "Exit";
			this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItemEdit
			// 
			this.menuItemEdit.Index = 1;
			this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItemUndo,
																						 this.menuItem3,
																						 this.menuItemFind,
																						 this.menuItem4,
																						 this.menuItemRefresh});
			this.menuItemEdit.Text = "Edit";
			// 
			// menuItemUndo
			// 
			this.menuItemUndo.Index = 0;
			this.menuItemUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuItemUndo.Text = "Undo";
			this.menuItemUndo.Click += new System.EventHandler(this.menuItemUndo_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "-";
			// 
			// menuItemFind
			// 
			this.menuItemFind.Index = 2;
			this.menuItemFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.menuItemFind.Text = "Find";
			this.menuItemFind.Click += new System.EventHandler(this.menuItemFind_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// menuItemRefresh
			// 
			this.menuItemRefresh.Index = 4;
			this.menuItemRefresh.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.menuItemRefresh.Text = "Refresh";
			this.menuItemRefresh.Click += new System.EventHandler(this.menuItemRefresh_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemAbout});
			this.menuItem1.Text = "Help";
			// 
			// menuItemAbout
			// 
			this.menuItemAbout.Index = 0;
			this.menuItemAbout.Text = "About";
			this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// imageListToolBar
			// 
			this.imageListToolBar.ColorDepth = System.Windows.Forms.ColorDepth.Depth4Bit;
			this.imageListToolBar.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolBar.ImageStream")));
			this.imageListToolBar.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.memoryViewControl);
			this.panelTop.Controls.Add(this.splitterTop);
			this.panelTop.Controls.Add(this.memoryLog);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(768, 185);
			this.panelTop.TabIndex = 30;
			// 
			// memoryViewControl
			// 
			this.memoryViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoryViewControl.Location = new System.Drawing.Point(411, 0);
			this.memoryViewControl.Name = "memoryViewControl";
			this.memoryViewControl.Size = new System.Drawing.Size(357, 185);
			this.memoryViewControl.TabIndex = 36;
			// 
			// splitterTop
			// 
			this.splitterTop.Location = new System.Drawing.Point(408, 0);
			this.splitterTop.Name = "splitterTop";
			this.splitterTop.Size = new System.Drawing.Size(3, 185);
			this.splitterTop.TabIndex = 35;
			this.splitterTop.TabStop = false;
			// 
			// memoryLog
			// 
			this.memoryLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.type,
																						this.name,
																						this.size,
																						this.memAddress,
																						this.time});
			this.memoryLog.Dock = System.Windows.Forms.DockStyle.Left;
			this.memoryLog.FullRowSelect = true;
			this.memoryLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.memoryLog.HideSelection = false;
			this.memoryLog.Location = new System.Drawing.Point(0, 0);
			this.memoryLog.MultiSelect = false;
			this.memoryLog.Name = "memoryLog";
			this.memoryLog.Size = new System.Drawing.Size(408, 185);
			this.memoryLog.TabIndex = 34;
			this.memoryLog.View = System.Windows.Forms.View.Details;
			this.memoryLog.SelectedIndexChanged += new System.EventHandler(this.MemoryLog_SelectedIndexChanged);
			// 
			// type
			// 
			this.type.Text = "Type";
			this.type.Width = 36;
			// 
			// name
			// 
			this.name.Text = "Name";
			this.name.Width = 40;
			// 
			// size
			// 
			this.size.Text = "Size";
			this.size.Width = 32;
			// 
			// memAddress
			// 
			this.memAddress.Text = "At";
			this.memAddress.Width = 31;
			// 
			// time
			// 
			this.time.Text = "Time";
			this.time.Width = 40;
			// 
			// splitterTopBottom
			// 
			this.splitterTopBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitterTopBottom.Location = new System.Drawing.Point(0, 182);
			this.splitterTopBottom.Name = "splitterTopBottom";
			this.splitterTopBottom.Size = new System.Drawing.Size(768, 3);
			this.splitterTopBottom.TabIndex = 31;
			this.splitterTopBottom.TabStop = false;
			// 
			// panelMemoryView
			// 
			this.panelMemoryView.Controls.Add(this.memoryViewPanel);
			this.panelMemoryView.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelMemoryView.Location = new System.Drawing.Point(0, 185);
			this.panelMemoryView.Name = "panelMemoryView";
			this.panelMemoryView.Size = new System.Drawing.Size(768, 248);
			this.panelMemoryView.TabIndex = 5;
			// 
			// memoryViewPanel
			// 
			this.memoryViewPanel.Controls.Add(this.splitterMemoryViews);
			this.memoryViewPanel.Controls.Add(this.memoryPictureZoom);
			this.memoryViewPanel.Controls.Add(this.memoryPictureFull);
			this.memoryViewPanel.Controls.Add(this.scrollBarZoom);
			this.memoryViewPanel.Controls.Add(this.toolBar);
			this.memoryViewPanel.Controls.Add(this.statusBar);
			this.memoryViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoryViewPanel.Location = new System.Drawing.Point(0, 0);
			this.memoryViewPanel.Name = "memoryViewPanel";
			this.memoryViewPanel.Size = new System.Drawing.Size(768, 248);
			this.memoryViewPanel.TabIndex = 28;
			this.memoryViewPanel.TabStop = false;
			this.memoryViewPanel.Text = "Memory View";
			// 
			// splitterMemoryViews
			// 
			this.splitterMemoryViews.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitterMemoryViews.Location = new System.Drawing.Point(3, 111);
			this.splitterMemoryViews.Name = "splitterMemoryViews";
			this.splitterMemoryViews.Size = new System.Drawing.Size(762, 3);
			this.splitterMemoryViews.TabIndex = 12;
			this.splitterMemoryViews.TabStop = false;
			// 
			// memoryPictureZoom
			// 
			this.memoryPictureZoom.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoryPictureZoom.Location = new System.Drawing.Point(3, 16);
			this.memoryPictureZoom.Name = "memoryPictureZoom";
			this.memoryPictureZoom.Size = new System.Drawing.Size(762, 98);
			this.memoryPictureZoom.TabIndex = 9;
			this.toolTip.SetToolTip(this.memoryPictureZoom, "Select Allocation");
			this.memoryPictureZoom.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MemoryViewZoom_MouseMove);
			this.memoryPictureZoom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.memoryPictureZoom_MouseDown);
			// 
			// memoryPictureFull
			// 
			this.memoryPictureFull.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.memoryPictureFull.Location = new System.Drawing.Point(3, 114);
			this.memoryPictureFull.Name = "memoryPictureFull";
			this.memoryPictureFull.Size = new System.Drawing.Size(762, 64);
			this.memoryPictureFull.TabIndex = 11;
			this.toolTip.SetToolTip(this.memoryPictureFull, "Center View");
			this.memoryPictureFull.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MemoryViewZoom_MouseMove);
			this.memoryPictureFull.MouseDown += new System.Windows.Forms.MouseEventHandler(this.memoryPictureFull_MouseDown);
			// 
			// scrollBarZoom
			// 
			this.scrollBarZoom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.scrollBarZoom.Location = new System.Drawing.Point(3, 178);
			this.scrollBarZoom.Maximum = 1024;
			this.scrollBarZoom.Name = "scrollBarZoom";
			this.scrollBarZoom.Size = new System.Drawing.Size(762, 17);
			this.scrollBarZoom.TabIndex = 13;
			this.toolTip.SetToolTip(this.scrollBarZoom, "Set View Range");
			this.scrollBarZoom.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarZoom_Scroll);
			// 
			// toolBar
			// 
			this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.toolBarButtonPlayback,
																					   this.toolBarButtonPause});
			this.toolBar.ButtonSize = new System.Drawing.Size(23, 22);
			this.toolBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageListToolBar;
			this.toolBar.Location = new System.Drawing.Point(3, 195);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(762, 28);
			this.toolBar.TabIndex = 14;
			this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// toolBarButtonPlayback
			// 
			this.toolBarButtonPlayback.ImageIndex = 1;
			this.toolBarButtonPlayback.ToolTipText = "Resume Simulation";
			// 
			// toolBarButtonPause
			// 
			this.toolBarButtonPause.ImageIndex = 0;
			this.toolBarButtonPause.ToolTipText = "Pause Simulation";
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(3, 223);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarStart,
																						 this.statusBarPos,
																						 this.statusBarMemory,
																						 this.statusBarMemoryAllocated,
																						 this.statusBarMemoryFree,
																						 this.statusBarEnd});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(762, 22);
			this.statusBar.TabIndex = 10;
			this.statusBar.Text = "Load Memory File (*.csv)";
			// 
			// statusBarStart
			// 
			this.statusBarStart.Text = "Start";
			// 
			// statusBarPos
			// 
			this.statusBarPos.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPos.Text = "Load Memory File";
			this.statusBarPos.Width = 136;
			// 
			// statusBarMemory
			// 
			this.statusBarMemory.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarMemory.Width = 136;
			// 
			// statusBarMemoryAllocated
			// 
			this.statusBarMemoryAllocated.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarMemoryAllocated.Width = 136;
			// 
			// statusBarMemoryFree
			// 
			this.statusBarMemoryFree.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarMemoryFree.Width = 136;
			// 
			// statusBarEnd
			// 
			this.statusBarEnd.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.statusBarEnd.Text = "End";
			// 
			// MainView
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(768, 433);
			this.Controls.Add(this.splitterTopBottom);
			this.Controls.Add(this.panelTop);
			this.Controls.Add(this.panelMemoryView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Memory Player";
			this.panelTop.ResumeLayout(false);
			this.panelMemoryView.ResumeLayout(false);
			this.memoryViewPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPos)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemoryAllocated)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarMemoryFree)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarEnd)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args) 
        {   
			String budgetfile, infile, outfile;
			MainView mv;

			if(args.Length > 0)
			{
				if( args[0] == "-?" || args[0] == "-h" ||
					args[0] == "--help" || args[0] == "-help" ||
					args.Length != 3)
				{
					String help = "Interactive Mode (default): MemoryPlayer.exe\n";
					help += "Non-Interactive Mode: MemoryPlayer.exe <budget xml file> <infile> <outfile>";
					MessageBox.Show(help, "MemoryPlayer", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				budgetfile = args[0];
				infile = args[1];
				outfile = args[2];
				mv = new MainView(budgetfile, infile, outfile);
				return;
			}
			else
			{
				mv = new MainView();
			}

            // try
            // {
                Application.Run(mv);
            // }
//            catch(NullReferenceException ex)
//            {
//                Console.WriteLine(ex);
//                Console.WriteLine(ex.StackTrace);
//            }
//            catch(Exception ex)
//            {
//                Console.WriteLine(ex);
//            }            
        }

        #region MemoryViewControl Interface
        private void MemoryLog_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Synchronize the simulation with the currently selected index.
            if ((memoryLog.SelectedItems.Count > 0) && mLogic.MemorySimulation != null) // (mSimulation != null) )
            {
                int pos = memoryLog.SelectedItems[0].Index;
                // mSimulation.MoveTo( pos );
				mLogic.MemorySimulation.MoveTo(pos);
                AddToUndo( pos );

                UpdateStatusBar();
                memoryPictureZoom.Invalidate( true );
                memoryPictureFull.Invalidate( true );
                memoryViewControl.Invalidate( true );
            }
        }

        private void LabelLog_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (memoryViewControl.LabelLog.SelectedItems.Count > 0) 
            {
                ListView.SelectedListViewItemCollection mListViewCollection = memoryViewControl.LabelLog.SelectedItems;
                ListViewItem mListViewItem = mListViewCollection[0];

                int pos = Convert.ToInt32(mListViewItem.SubItems[3].Text);
                // mSimulation.MoveTo( pos );
				mLogic.MemorySimulation.MoveTo(pos);
                AddToUndo( pos );
                
                UpdateStatusBar();
                memoryPictureZoom.Invalidate( true );
                memoryPictureFull.Invalidate( true );
                memoryViewControl.Invalidate( true );
            }            
        }

        private void Categories_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            memoryPictureZoom.Invalidate( true );
            memoryPictureFull.Invalidate( true );              
        }
        #endregion



        #region GUI/Data Interface
        private void UpdateStatusBar()
        {
            if ( memoryPictureZoom.Grapher != null )
            {
                this.statusBarStart.Text = "0x" + memoryPictureZoom.Grapher.VisibleStart.ToString("X8");
                this.statusBarEnd.Text = "0x" + memoryPictureZoom.Grapher.VisibleEnd.ToString("X8");
            }
            this.statusBarMemory.Text = "Memory: " + (mLogic.MemoryLogItems.MemorySize/1024) + "k";
            this.statusBarMemoryAllocated.Text = "Allocated: " + (mLogic.MemorySimulation.MemoryAllocated/1024) /* (mSimulation.MemoryAllocated/1024) */+ "k";
            this.statusBarMemoryFree.Text = "Free: " + (mLogic.MemorySimulation.MemoryFree/1024) + "k";
        }

        private void SynchronizeGUIToSimulation()
        {
            // Deselect selected
            foreach(Object item in memoryLog.SelectedItems)
            {
                ((ListViewItem)item).Selected = false;
            }

            if ((mLogic.MemorySimulation != null) && mLogic.MemorySimulation.Started)
            {
                ListViewItem lvi = memoryLog.Items[mLogic.MemorySimulation.CurrentAllocation];
                lvi.Selected = true;

                memoryLog.EnsureVisible(mLogic.MemorySimulation.CurrentAllocation);
            }

            memoryViewControl.Invalidate( true );
        }

        internal void StartGUIUpdate()
        {
            memoryLog.BeginUpdate();
            memoryViewControl.LabelLog.BeginUpdate();
        }

        internal void EndGUIUpdate()
        {
            memoryViewControl.LabelLog.EndUpdate();
            memoryLog.EndUpdate();

            bool itemsChanged = memoryLog.Items.Count != mLogic.MemoryLogItems.Count; // mMemoryLogItems.Count;

            if ( mLogic.MemorySimulation != null )
            {
                // mSimulation = new Simulation(mMemoryLogItems);
                memoryPictureZoom.Initialize( mLogic.MemorySimulation);
                memoryPictureFull.Initialize( mLogic.MemorySimulation);
                memoryPictureFull.Grapher.BoundingBoxEnabled = true;
            }
            
            
            // for ( int i = memoryLog.Items.Count; i < mMemoryLogItems.Count; i++ )
			for ( int i = memoryLog.Items.Count; i < mLogic.MemoryLogItems.Count; i++ )
            {            
                MemoryLogItem logItem = mLogic.MemoryLogItems.GetItem( i );

                // fill up the listView columns
                ListViewItem lvi = new ListViewItem(logItem.TypeName);
                lvi.SubItems.Add(logItem.Name);                
                lvi.SubItems.Add(Convert.ToString(logItem.Size));
                lvi.SubItems.Add( "0x" + logItem.Location.ToString("X8") );
                lvi.SubItems.Add(Convert.ToString(logItem.Time));
                lvi.SubItems.Add(" "); 

                memoryLog.Items.Add(lvi);

                if (logItem.TypeName == "LABEL")
                {
                    ListViewItem lvi2 = new ListViewItem(logItem.TypeName);
                    lvi2.SubItems.Add(logItem.Name);
                    lvi2.SubItems.Add(Convert.ToString(logItem.Time));
                    lvi2.SubItems.Add(Convert.ToString(logItem.Id));
                    lvi2.SubItems.Add(" ");

                    memoryViewControl.LabelLog.Items.Add(lvi2);
                }
            }

			// VC: not my comment out of mSimulation!
            scrollBarZoom.Minimum = (int)0; // (mSimulation.MemoryStart / (16 * 1024));
            scrollBarZoom.Maximum = (int)(mLogic.MemorySimulation.MemorySize /*mSimulation.MemorySize*//(16 * 1024) - 1);

            System.Console.WriteLine( "MIN {0}, MAX {1}", scrollBarZoom.Minimum, scrollBarZoom.Maximum );

            if ( itemsChanged )
            {
				// mSimulation.MoveTo( mMemoryLogItems.Count - 1 );
				mLogic.MemorySimulation.MoveTo( mLogic.MemoryLogItems.Count - 1 );
                SynchronizeGUIToSimulation();

                Invalidate( true );
                UpdateStatusBar();
            }
        }
        #endregion

        #region Main Menu Region
        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void menuItemAttachTo_Click(object sender, System.EventArgs e)
        {
            // mIsSimming = true;
			mLogic.IsSimming = true;
            OpenFile();
            // mTimer.Enabled = true;
			mLogic.LogicTimer.Enabled = true;
            SetPlayButtonImage();
        }

        private void menuItemFile_Click(object sender, System.EventArgs e)
        {
            // mIsSimming = false;
			mLogic.IsSimming = false;
            OpenFile();
            // mTimer.Enabled = false;
			mLogic.LogicTimer.Enabled = false;
            SetPlayButtonImage();
        }

        private void menuItemRefresh_Click(object sender, System.EventArgs e)
        {
            this.Invalidate(true);
        }

        private void menuItemUndo_Click(object sender, System.EventArgs e)
        {            
            Undo();
        }

        private void menuItemRedo_Click(object sender, System.EventArgs e)
        {
            Redo();
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog( this );
        }

        void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
				memoryLog.Items.Clear();
				memoryViewControl.LabelLog.Items.Clear();

				mLogic.ReadFile(openFileDialog.FileName);
				this.Text = "Memory Player - " + System.IO.Path.GetFileName( openFileDialog.FileName );

				// VC: update UI widgets with new memory control items

				// Resize columns, this could get annoying, only do on open
				foreach ( ColumnHeader ch in memoryLog.Columns )
				{
					ch.Width = -2;
				}
            
				foreach ( ColumnHeader ch in memoryViewControl.LabelLog.Columns )
				{
					ch.Width = -2;
				}
//                memoryLog.Items.Clear();
//                memoryViewControl.LabelLog.Items.Clear();
//                mMemoryLogItems.Clear();
//                mSimulation = null;
//
//                this.Text = "Memory Player - " + System.IO.Path.GetFileName( openFileDialog.FileName );
//
//                mFileReader = new FileReader( this, 500 );
//                mFileReader.Start( openFileDialog.FileName );                
//                
//                mFileReader.ReadLoop();
//
//
//                // Resize columns, this could get annoying, only do on open
//                foreach ( ColumnHeader ch in memoryLog.Columns )
//                {
//                    ch.Width = -2;
//                }
//            
//                foreach ( ColumnHeader ch in memoryViewControl.LabelLog.Columns )
//                {
//                    ch.Width = -2;
//                }
//
//
//                mTimer.Interval = 500;
//                mTimer.Start();
            }
        }
        #endregion

        #region Memory View Region
        private void MemoryViewZoom_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ( memoryPictureZoom.Grapher != null )
            {
                this.statusBarPos.Text = "Position 0x" + memoryPictureZoom.Grapher.VisibleToMemory( e.X ).ToString("X8");
            }
        }

        private void memoryPictureZoom_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ( memoryPictureZoom.Grapher == null ) return;

            Point mPoint = new Point( e.X,  e.Y );

            int entryNumber = memoryPictureZoom.Grapher.Check( mPoint );            
            if (entryNumber == -1) 
            {
                Console.WriteLine("Error. Not such entry is found");	
                return;
            }
            // if (mSimulation != null )
			if(mLogic.MemorySimulation != null)
            {                
                // Go to the allocation
                if ( (e.Button & MouseButtons.Left) == MouseButtons.Left )
                {
                    // mSimulation.MoveTo(entryNumber);
					mLogic.MemorySimulation.MoveTo(entryNumber);
                    AddToUndo( entryNumber );
                }
                    // Go to the free
                else if ( (e.Button & MouseButtons.Right) == MouseButtons.Right )
                {
                    int index = mLogic.MemoryLogItems.FindMatchingFree( entryNumber );

                    if ( index >= 0 )
                    {
                        // mSimulation.MoveTo( index );
						mLogic.MemorySimulation.MoveTo(index);
                        AddToUndo( index );
                    }
                }
				
                SynchronizeGUIToSimulation();
                UpdateStatusBar();
            }
        
        }


        private void memoryPictureFull_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ( memoryPictureFull.Grapher == null ) return;

            // Focus on the click
            long focus = memoryPictureFull.Grapher.VisibleToMemory( e.X );
            long visibleSize = memoryPictureZoom.Grapher.VisibleEnd - memoryPictureZoom.Grapher.VisibleStart;
            long newStart = focus - visibleSize / 2;

            newStart = Math.Max( memoryPictureZoom.Grapher.MemoryStart, newStart );
            newStart = Math.Min( memoryPictureZoom.Grapher.MemoryEnd - visibleSize, newStart );


            memoryPictureZoom.Grapher.VisibleStart = newStart;
            memoryPictureZoom.Grapher.VisibleEnd = newStart + visibleSize;

            long newVisibleSize = memoryPictureZoom.Grapher.VisibleEnd - memoryPictureZoom.Grapher.VisibleStart;


            // Zoom centered on the visible area
            memoryPictureFull.Grapher.BoundingBoxStart = memoryPictureZoom.Grapher.VisibleStart;
            memoryPictureFull.Grapher.BoundingBoxEnd = memoryPictureZoom.Grapher.VisibleEnd;

            UpdateStatusBar();
        }


        private void scrollBarZoom_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if ( memoryPictureFull.Grapher == null ) return;

            // Adjust the visibility of the Zoom window
            long visibleSize = memoryPictureZoom.Grapher.VisibleEnd - memoryPictureZoom.Grapher.VisibleStart;
            long focus = memoryPictureZoom.Grapher.VisibleStart + (visibleSize/2);

            long newVisibleSize = mLogic.MemorySimulation.MemorySize /*mSimulation.MemorySize*/ - (scrollBarZoom.Value * (16 * 1024));
                       
            memoryPictureZoom.Grapher.VisibleStart = focus - newVisibleSize / 2;
            memoryPictureZoom.Grapher.VisibleEnd = focus + newVisibleSize / 2;

            // Readjust the bounding box
            memoryPictureFull.Grapher.BoundingBoxStart = memoryPictureZoom.Grapher.VisibleStart;
            memoryPictureFull.Grapher.BoundingBoxEnd = memoryPictureZoom.Grapher.VisibleEnd;
        }
        #endregion

        #region Toolbar Region
        private void PlaybackButtonClick()
        {
            // if ( mIsSimming )
			if(mLogic.IsSimming)
            {
                // switch modes, and display
                // mTimer.Enabled = !mTimer.Enabled;
				mLogic.LogicTimer.Enabled = !mLogic.LogicTimer.Enabled;
                SetPlayButtonImage();
            }
        }

        private void SetPlayButtonImage()
        {            
            // if ( !mIsSimming )
			if(!mLogic.IsSimming)
            {
                toolBarButtonPlayback.Enabled = false;
                toolBarButtonPause.Enabled = false;
            }
            else
            {
                // if ( !mTimer.Enabled )
				if(!mLogic.LogicTimer.Enabled)
                {
                    toolBarButtonPlayback.Enabled = true;
                    toolBarButtonPause.Enabled = false;
                }
                else
                {
                    toolBarButtonPlayback.Enabled = false;
                    toolBarButtonPause.Enabled = true;
                }
            }
        }

        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {            
            if ( e.Button == toolBarButtonPlayback )
            {
                // mTimer.Enabled = true;
				mLogic.LogicTimer.Enabled = true;
                toolBarButtonPlayback.Enabled = false;
                toolBarButtonPause.Enabled = true;
            }
            else if ( e.Button == toolBarButtonPause )
            {
                // mTimer.Enabled = false;
				mLogic.LogicTimer.Enabled = false;
                toolBarButtonPlayback.Enabled = true;
                toolBarButtonPause.Enabled = false;
            }
        }
        #endregion

        #region Find Region
        public void Found( int i )
        {
            // mSimulation.MoveTo( i );
			mLogic.MemorySimulation.MoveTo(i);
            SynchronizeGUIToSimulation();
        }

        public void FindDialog_Closed(object sender, System.EventArgs e)
        {
            mFindDialog = null;
        }

        private void menuItemFind_Click(object sender, System.EventArgs e)
        {
            // Only one at a time!
            if ( mFindDialog != null ) return;
            if ( mLogic.MemorySimulation == null ) return;

            mFindDialog = new FindDialog();
            mFindDialog.Initialize( this, mLogic.MemorySimulation );
            mFindDialog.Closed += new EventHandler(FindDialog_Closed);
            mFindDialog.Show();
        }
        #endregion

        #region Undo/Redo Region
        public void Undo()
        {
            if ( mUndoPos > 0 )
            {
                mUndoPos--;
                int pos = (int)mUndoBuffer[ mUndoPos ];                
                // mSimulation.MoveTo( pos );
				mLogic.MemorySimulation.MoveTo(pos);

                // Console.WriteLine( "UNDO: " + mUndoPos + ": " + pos);
                SynchronizeGUIToSimulation();
            }

            if ( mUndoPos > 0 )
            {                
                // menuItemRedo.Enabled = true;
            }
            else
            {
                menuItemUndo.Enabled = false;
            }
        }

        public void Redo()
        {
            return;
            /*
            if ( mUndoPos < mUndoBuffer.Count )
            {                
                int pos = (int)mUndoBuffer[ mUndoPos ];
                mSimulation.MoveTo( pos );

                Console.WriteLine( "REDO: " + mUndoPos + ": " + pos);
                mUndoPos++;   
                SynchronizeGUIToSimulation();
            }

            if ( mUndoPos < mUndoBuffer.Count )
            {
                menuItemRedo.Enabled = true;
            }
            */
        }

        public void AddToUndo( int pos )
        {
            if ( mUndoPos < mUndoBuffer.Count )
            {
                if ( mUndoPos > 0 )
                {
                    int curPos = (int)mUndoBuffer[ mUndoPos ];
                    if ( pos == curPos )
                    {
                        return;
                    }
                }

                // Remove all "redo" steps
                if ( mUndoPos < mUndoBuffer.Count - 1)
                {
                    mUndoBuffer.RemoveRange( mUndoPos + 1, mUndoBuffer.Count - (mUndoPos + 1) );                
                    // menuItemRedo.Enabled = false;
                }
            }
            menuItemUndo.Enabled = true;
            mUndoBuffer.Add( pos );
            mUndoPos = mUndoBuffer.Count - 1;

            // Console.WriteLine( "ADD: " + mUndoPos + ": " + pos);
        }

        public int UndoBufferSize
        {
            get
            {
                return mUndoPos;
            }
        }

        public int RedoBufferSize
        {
            get
            {
                return mUndoBuffer.Count - mUndoPos;
            }
        }

        public void ResetUndo()
        {
//            menuItemRedo.Enabled = false;
            menuItemUndo.Enabled = false;

            mUndoBuffer = new ArrayList();
            mUndoPos = -1;
        }
        #endregion 

		private void CategoryList_itemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			CategoryManager.CategoryInfo info = (CategoryManager.CategoryInfo)memoryViewControl.CategoryList.Items[ e.Index ];            

			if ( e.NewValue == CheckState.Checked )
			{                
				info.Active = true;
			}
			else
			{
				info.Active = false;
			}
		}
    }
}
