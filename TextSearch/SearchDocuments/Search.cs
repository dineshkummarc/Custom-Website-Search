using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;


namespace AgileVisions.SearchDocuments
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbxQuery;
		private System.Windows.Forms.Button btnFind;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ListView lvResults;
		private System.Windows.Forms.ColumnHeader FileName;
		private System.Windows.Forms.ColumnHeader Location;
		private System.Windows.Forms.ColumnHeader Similarity;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblSearchIn;
		
		AgileVisions.IndexManager.Analyzer.Indexer indexer = new AgileVisions.IndexManager.Analyzer.Indexer();
		private System.Windows.Forms.StatusBar statusBar1;
		private string [] patterns = {"*.htm","*.html","*.doc","*.txt","*.xls"};

		public delegate void DoIndexing(string directoryname); 

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.tbxQuery = new System.Windows.Forms.TextBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lvResults = new System.Windows.Forms.ListView();
			this.FileName = new System.Windows.Forms.ColumnHeader();
			this.Location = new System.Windows.Forms.ColumnHeader();
			this.Similarity = new System.Windows.Forms.ColumnHeader();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.lblSearchIn = new System.Windows.Forms.Label();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.label3 = new System.Windows.Forms.Label();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tbxQuery
			// 
			this.tbxQuery.Location = new System.Drawing.Point(88, 32);
			this.tbxQuery.Name = "tbxQuery";
			this.tbxQuery.Size = new System.Drawing.Size(432, 20);
			this.tbxQuery.TabIndex = 2;
			this.tbxQuery.Text = "";
			// 
			// btnFind
			// 
			this.btnFind.Location = new System.Drawing.Point(528, 32);
			this.btnFind.Name = "btnFind";
			this.btnFind.Size = new System.Drawing.Size(32, 23);
			this.btnFind.TabIndex = 3;
			this.btnFind.Text = "&Go";
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.statusBar1);
			this.panel1.Controls.Add(this.lvResults);
			this.panel1.Location = new System.Drawing.Point(0, 128);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(664, 416);
			this.panel1.TabIndex = 5;
			// 
			// lvResults
			// 
			this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.FileName,
																						this.Location,
																						this.Similarity});
			this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResults.Location = new System.Drawing.Point(0, 0);
			this.lvResults.Name = "lvResults";
			this.lvResults.Size = new System.Drawing.Size(664, 416);
			this.lvResults.TabIndex = 5;
			this.lvResults.View = System.Windows.Forms.View.Details;
			// 
			// FileName
			// 
			this.FileName.Text = "Name";
			// 
			// Location
			// 
			this.Location.Text = "Location";
			// 
			// Similarity
			// 
			this.Similarity.Text = "Similarity";
			this.Similarity.Width = 114;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem4});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3});
			this.menuItem1.Text = "&Main";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "&Search in";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "&Exit";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Search in...";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSearchIn
			// 
			this.lblSearchIn.Location = new System.Drawing.Point(96, 0);
			this.lblSearchIn.Name = "lblSearchIn";
			this.lblSearchIn.Size = new System.Drawing.Size(568, 23);
			this.lblSearchIn.TabIndex = 7;
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem5});
			this.menuItem4.Text = "&Tools";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "&Index";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(0, 32);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Find";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 394);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(664, 22);
			this.statusBar1.TabIndex = 6;
			this.statusBar1.Text = "Ready";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(664, 550);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblSearchIn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.tbxQuery);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Search";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void btnIndex_Click(object sender, System.EventArgs e)
		{
			FileDialog dialog = new OpenFileDialog();
			System.Collections.Specialized.NameValueCollection nvcshares = (System.Collections.Specialized.NameValueCollection)
				System.Configuration.ConfigurationSettings.GetConfig("shares");
			System.Collections.Specialized.NameValueCollection nvcvirtuals = (System.Collections.Specialized.NameValueCollection)
				System.Configuration.ConfigurationSettings.GetConfig("virtuals");

			indexer.stemlevel = IndexManager.Analyzer.StemmerLevel.PruralsandPastParticiples;
			
			foreach(string sharename in nvcshares.Keys)
				CollectDocuments(nvcshares[sharename].ToString(), nvcvirtuals[sharename].ToString());

			foreach(string sharename in nvcshares.Keys)
				IndexDocuments(nvcshares[sharename].ToString(), nvcvirtuals[sharename].ToString());

			indexer.WriteIndices();

		}
		private void CollectDocuments(string directorypath, string virtualpath)
		{
			try
			{
				DirectoryInfo dirinfo = new DirectoryInfo(directorypath);
			
				foreach(string strPattern in patterns)
				{
					FileInfo [] fileInfo = dirinfo.GetFiles(strPattern);
					int i =0;
					float perc = i / (float)((fileInfo.Length > 0) ? fileInfo.Length:1);
					foreach(FileInfo fInfo in fileInfo)
					{
						if(fInfo.FullName.StartsWith("~")) continue;
						IndexManager.DocumentManagement.Document doc = new IndexManager.DocumentManagement.Document();
						doc.Name = fInfo.FullName;
						doc.ApplicationDirectory = fInfo.DirectoryName;
						doc.FileName = Path.GetFileName(fInfo.FullName);
						doc.VirtualDirectory = virtualpath;
						indexer.AddDocument(doc, External.SearchDocuments.Parsing.Parser.Parse(fInfo.FullName));
						perc = (i++/(float)fileInfo.Length) * 100;
						statusBar1.Text = perc.ToString() + " % completed";
					}
				}
			}
			catch(Exception ex)
			{
			}
			finally
			{
				statusBar1.Text = "Ready";
			}
		}

		private void IndexDocuments(string directorypath, string virtualpath)
		{
			try
			{
				DirectoryInfo dirinfo = new DirectoryInfo(directorypath);
				indexer.DocumentCollection.Clear();
				foreach(string strPattern in patterns)
				{
					foreach(FileInfo fInfo in dirinfo.GetFiles(strPattern))
					{
						if(fInfo.FullName.StartsWith("~")) continue;
						IndexManager.DocumentManagement.Document doc = new IndexManager.DocumentManagement.Document();
						doc.Name = fInfo.FullName;
						doc.ApplicationDirectory = fInfo.DirectoryName;
						doc.FileName = Path.GetFileName(fInfo.FullName);
						doc.VirtualDirectory = virtualpath;
						indexer.CreateDocumentIndices(doc, External.SearchDocuments.Parsing.Parser.Parse(fInfo.FullName));
					}
				}
			}
			catch(Exception ex)
			{
			}
			finally
			{
				statusBar1.Text = "Ready";
			}
		}

		private void btnFind_Click(object sender, System.EventArgs e)
		{
			lvResults.Items.Clear();
			IndexManager.Analyzer.Indexer localindexer = new IndexManager.Analyzer.Indexer();
			localindexer.IndexPath = indexer.IndexPath;
			localindexer.stemlevel = IndexManager.Analyzer.StemmerLevel.PruralsandPastParticiples;
			localindexer.ReadIndices();
			IndexManager.Results results = localindexer.Search(tbxQuery.Text);
			results = results;
			foreach(IndexManager.Result result in results.arrlist)
			{
				if(result.Score > 0.0)
				{
					string [] strItems = new string[3];
					strItems[0] = result.Doc.FileName;
					strItems[1] = result.Doc.ApplicationDirectory;
					strItems[2] = result.Score.ToString();
					lvResults.Items.Add(new ListViewItem(strItems));
				}
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			string localindexpath  = ConfigurationSettings.AppSettings["indexpath"].ToString();
			indexer.IndexPath = System.IO.Path.Combine(Directory.GetCurrentDirectory() , localindexpath);
			menuItem5.Enabled = false;
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nvcshares = (System.Collections.Specialized.NameValueCollection)
				System.Configuration.ConfigurationSettings.GetConfig("shares");
			System.Collections.Specialized.NameValueCollection nvcvirtuals = (System.Collections.Specialized.NameValueCollection)
				System.Configuration.ConfigurationSettings.GetConfig("virtuals");

			indexer.stemlevel = IndexManager.Analyzer.StemmerLevel.PruralsandPastParticiples;
			if(nvcshares.Count >0)
			{
				foreach(string sharename in nvcshares.Keys)
					CollectDocuments(nvcshares[sharename].ToString(), nvcvirtuals[sharename].ToString());
				foreach(string sharename in nvcshares.Keys)
					IndexDocuments(nvcshares[sharename].ToString(), nvcvirtuals[sharename].ToString());
			}
			else
			{
				if(Directory.Exists(lblSearchIn.Text))
				{
					menuItem5.Enabled = false;
					DoIndexing doIndexingPhase1 = new DoIndexing(CallAsyncIndexing);
					doIndexingPhase1.BeginInvoke(lblSearchIn.Text,new AsyncCallback(IndexingComplete),null);
				}
			}
		}
		private void IndexingComplete(IAsyncResult aresult)
		{
			DoIndexing doIndexingPhase2 = new DoIndexing(CallAsyncIndexingWrite);
			doIndexingPhase2.BeginInvoke(lblSearchIn.Text,new AsyncCallback(DocumentCollectionrReady),null);
		}
		private void DocumentCollectionrReady(IAsyncResult aresult)
		{
			indexer.WriteIndices();
			menuItem5.Enabled = true;
		}
		private void CallAsyncIndexing(string directoryname)
		{
			CollectDocuments(directoryname,"");
		}
		private void CallAsyncIndexingWrite(string directoryname)
		{
			IndexDocuments(directoryname,"");
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			SelectFolder selFolder = new SelectFolder();
			if(selFolder.ShowDialog() == DialogResult.OK)
			{
				if(Directory.Exists(selFolder.selecteddirectory.FullName))
				{
					lblSearchIn.Text = selFolder.selecteddirectory.FullName;
					menuItem5.Enabled = true;
				}
				else
					menuItem5.Enabled = false;
			}
		}
	}
}
