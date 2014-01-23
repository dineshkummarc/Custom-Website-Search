using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CatalogQueryManager;
using System.Diagnostics;

namespace IndexingServerSample
{
	/// <summary>
	/// main form for the sample app
	/// </summary>
	public class IndexingServerManager : Form
	{
		const string HelpUrl = "http://msdn.microsoft.com/library/default.asp?url=/library/en-us/indexsrv/html/ixrefqls_3wj7.asp";
		const string ProvideQueryAndCatalog = "You must provide a catalog name and query string!";
		const string RemoteProvideQueryAndCatalog = "You must provide a remote machine name, catalog name and query string!";

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.Windows.Forms.Label CatalogLabel;
		private System.Windows.Forms.Label QueryLabale;
		private System.Windows.Forms.Label QueryResultLabel;
		private System.Windows.Forms.TextBox CatalogName;
		private System.Windows.Forms.ComboBox QueryToRun;
		private System.Windows.Forms.Button ExecuteLocalButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.ListView QueryResult;
		private System.Windows.Forms.Button ExecuteRemoteButton;
		private System.Windows.Forms.Label RemoteMachineNameLabel;
		private System.Windows.Forms.TextBox RemoteMachineName;
		private System.Windows.Forms.Label RecordCountLabel;
		private System.Windows.Forms.Label RecordCount;
		private System.Windows.Forms.Label InfoLabel;
		private System.Windows.Forms.Button MsdnHelpButton;
		private System.Windows.Forms.Button HelpButton;
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.CatalogLabel = new System.Windows.Forms.Label();
			this.QueryLabale = new System.Windows.Forms.Label();
			this.QueryResultLabel = new System.Windows.Forms.Label();
			this.CatalogName = new System.Windows.Forms.TextBox();
			this.QueryToRun = new System.Windows.Forms.ComboBox();
			this.ExecuteLocalButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.QueryResult = new System.Windows.Forms.ListView();
			this.ExecuteRemoteButton = new System.Windows.Forms.Button();
			this.RemoteMachineNameLabel = new System.Windows.Forms.Label();
			this.RemoteMachineName = new System.Windows.Forms.TextBox();
			this.RecordCountLabel = new System.Windows.Forms.Label();
			this.RecordCount = new System.Windows.Forms.Label();
			this.InfoLabel = new System.Windows.Forms.Label();
			this.HelpButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// CatalogLabel
			// 
			this.CatalogLabel.AutoSize = true;
			this.CatalogLabel.Location = new System.Drawing.Point(13, 40);
			this.CatalogLabel.Name = "CatalogLabel";
			this.CatalogLabel.Size = new System.Drawing.Size(43, 16);
			this.CatalogLabel.TabIndex = 2;
			this.CatalogLabel.Text = "Catalog";
			// 
			// QueryLabale
			// 
			this.QueryLabale.AutoSize = true;
			this.QueryLabale.Location = new System.Drawing.Point(13, 66);
			this.QueryLabale.Name = "QueryLabale";
			this.QueryLabale.Size = new System.Drawing.Size(35, 16);
			this.QueryLabale.TabIndex = 4;
			this.QueryLabale.Text = "Query";
			// 
			// QueryResultLabel
			// 
			this.QueryResultLabel.AutoSize = true;
			this.QueryResultLabel.Location = new System.Drawing.Point(13, 93);
			this.QueryResultLabel.Name = "QueryResultLabel";
			this.QueryResultLabel.Size = new System.Drawing.Size(80, 16);
			this.QueryResultLabel.TabIndex = 2;
			this.QueryResultLabel.Text = "Result of query";
			// 
			// CatalogName
			// 
			this.CatalogName.Location = new System.Drawing.Point(93, 37);
			this.CatalogName.Name = "CatalogName";
			this.CatalogName.Size = new System.Drawing.Size(363, 20);
			this.CatalogName.TabIndex = 3;
			this.CatalogName.Text = "System";
			// 
			// QueryToRun
			// 
			this.QueryToRun.Items.AddRange(new object[] {
															"SELECT * FROM FILEINFO",
															"SELECT * FROM FILEINFO WHERE CONTAINS(FILENAME, \'default\')",
															"SELECT * FROM FILEINFO WHERE CONTAINS(FILENAME, \'\"def*\"\')",
															"SELECT * FROM FILEINFO WHERE FREETEXT(FILENAME,\'default\')",
															"SELECT * FROM FILEINFO_ABSTRACT",
															"SELECT * FROM EXTENDED_FILEINFO",
															"SELECT * FROM WEBINFO",
															"SELECT * FROM EXTENDED_WEBINFO",
															"SELECT ACCESS, WRITE, CREATE, ALLOCSIZE, SIZE, RANK FROM SCOPE() ORDER BY RANK",
															"SELECT FILENAME, PATH, VPATH, SHORTFILENAME, HITCOUNT FROM SCOPE() ORDER BY FILEN" +
															"AME",
															"SELECT FILENAME, CHARACTERIZATION, DOCAUTHOR, DOCTITLE FROM SCOPE() ORDER BY FILE" +
															"NAME",
															"SELECT FILENAME, CLASSID, WORKID, USN FROM SCOPE() ORDER BY FILENAME",
															"SELECT FILENAME, PATH, VPATH, SHORTFILENAME, HITCOUNT FROM SCOPE(\'DEEP TRAVERSAL " +
															"OF \"/\"\') ORDER BY FILENAME",
															"SELECT FILENAME, PATH, VPATH, SHORTFILENAME, HITCOUNT FROM SCOPE(\'SHALLOW TRAVERS" +
															"AL OF \"/\"\') ORDER BY FILENAME"});
			this.QueryToRun.Location = new System.Drawing.Point(93, 63);
			this.QueryToRun.Name = "QueryToRun";
			this.QueryToRun.Size = new System.Drawing.Size(363, 21);
			this.QueryToRun.TabIndex = 5;
			// 
			// ExecuteLocalButton
			// 
			this.ExecuteLocalButton.Location = new System.Drawing.Point(14, 315);
			this.ExecuteLocalButton.Name = "ExecuteLocalButton";
			this.ExecuteLocalButton.Size = new System.Drawing.Size(145, 23);
			this.ExecuteLocalButton.TabIndex = 7;
			this.ExecuteLocalButton.Text = "&Execute local query";
			this.ExecuteLocalButton.Click += new System.EventHandler(this.ExecuteLocalButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(356, 315);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(100, 23);
			this.CloseButton.TabIndex = 9;
			this.CloseButton.Text = "&Close";
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// QueryResult
			// 
			this.QueryResult.FullRowSelect = true;
			this.QueryResult.GridLines = true;
			this.QueryResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.QueryResult.Location = new System.Drawing.Point(14, 112);
			this.QueryResult.MultiSelect = false;
			this.QueryResult.Name = "QueryResult";
			this.QueryResult.Size = new System.Drawing.Size(442, 187);
			this.QueryResult.TabIndex = 6;
			this.QueryResult.View = System.Windows.Forms.View.Details;
			// 
			// ExecuteRemoteButton
			// 
			this.ExecuteRemoteButton.Location = new System.Drawing.Point(187, 315);
			this.ExecuteRemoteButton.Name = "ExecuteRemoteButton";
			this.ExecuteRemoteButton.Size = new System.Drawing.Size(139, 23);
			this.ExecuteRemoteButton.TabIndex = 8;
			this.ExecuteRemoteButton.Text = "E&xecute remote query";
			this.ExecuteRemoteButton.Click += new System.EventHandler(this.ExecuteRemoteButton_Click);
			// 
			// RemoteMachineNameLabel
			// 
			this.RemoteMachineNameLabel.AutoSize = true;
			this.RemoteMachineNameLabel.Location = new System.Drawing.Point(14, 13);
			this.RemoteMachineNameLabel.Name = "RemoteMachineNameLabel";
			this.RemoteMachineNameLabel.Size = new System.Drawing.Size(78, 16);
			this.RemoteMachineNameLabel.TabIndex = 0;
			this.RemoteMachineNameLabel.Text = "Machine name";
			// 
			// RemoteMachineName
			// 
			this.RemoteMachineName.Location = new System.Drawing.Point(93, 11);
			this.RemoteMachineName.Name = "RemoteMachineName";
			this.RemoteMachineName.Size = new System.Drawing.Size(363, 20);
			this.RemoteMachineName.TabIndex = 1;
			this.RemoteMachineName.Text = "";
			// 
			// RecordCountLabel
			// 
			this.RecordCountLabel.AutoSize = true;
			this.RecordCountLabel.Location = new System.Drawing.Point(14, 354);
			this.RecordCountLabel.Name = "RecordCountLabel";
			this.RecordCountLabel.Size = new System.Drawing.Size(71, 16);
			this.RecordCountLabel.TabIndex = 10;
			this.RecordCountLabel.Text = "Record count";
			// 
			// RecordCount
			// 
			this.RecordCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.RecordCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.RecordCount.Location = new System.Drawing.Point(93, 354);
			this.RecordCount.Name = "RecordCount";
			this.RecordCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.RecordCount.Size = new System.Drawing.Size(50, 15);
			this.RecordCount.TabIndex = 11;
			this.RecordCount.Text = "0";
			// 
			// InfoLabel
			// 
			this.InfoLabel.BackColor = System.Drawing.SystemColors.Info;
			this.InfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.InfoLabel.Location = new System.Drawing.Point(15, 380);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Size = new System.Drawing.Size(440, 45);
			this.InfoLabel.TabIndex = 12;
			this.InfoLabel.Text = "\'Execute local query\' runs the query against a local indexing server. \'Execute re" +
				"mote query\' runs against the indexing server on the remote machine entered. Ente" +
				"r a SQL statement to execute or select from one of the default ones.";
			// 
			// HelpButton
			// 
			this.HelpButton.Location = new System.Drawing.Point(235, 349);
			this.HelpButton.Name = "HelpButton";
			this.HelpButton.Size = new System.Drawing.Size(221, 23);
			this.HelpButton.TabIndex = 0;
			this.HelpButton.Text = "Show MSDN help for query language";
			this.HelpButton.Click += new System.EventHandler(this.MsdnHelpButton_Click);
			// 
			// IndexingServerManager
			// 
			this.AcceptButton = this.ExecuteLocalButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(468, 441);
			this.Controls.Add(this.HelpButton);
			this.Controls.Add(this.InfoLabel);
			this.Controls.Add(this.RecordCount);
			this.Controls.Add(this.RecordCountLabel);
			this.Controls.Add(this.RemoteMachineName);
			this.Controls.Add(this.RemoteMachineNameLabel);
			this.Controls.Add(this.ExecuteRemoteButton);
			this.Controls.Add(this.QueryResult);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.ExecuteLocalButton);
			this.Controls.Add(this.QueryToRun);
			this.Controls.Add(this.CatalogName);
			this.Controls.Add(this.QueryResultLabel);
			this.Controls.Add(this.QueryLabale);
			this.Controls.Add(this.CatalogLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "IndexingServerManager";
			this.Text = "Indexing Server query manager";
			this.Load += new System.EventHandler(this.IndexingServerManager_Load);
			this.ResumeLayout(false);

		}

		#endregion

		/// <summary>
		/// initialization
		/// </summary>
		public IndexingServerManager()
		{
			InitializeComponent();
		}

		/// <summary>
		/// refreshes the ListView with the latest query result
		/// </summary>
		/// <param name="DataReader">the query result</param>
		protected void ShowQueryResults(IDataReader DataReader)
		{
			try
			{
				int ResultRecordCount = 0;

				// remove any items and header columns shown so far
				QueryResult.Items.Clear();
				QueryResult.Columns.Clear();

				// get all the field names and add them to the list view
				for (int Count = 0; Count < DataReader.FieldCount; Count++)
					QueryResult.Columns.Add(DataReader.GetName(Count), DataReader.GetName(Count).Length * 15, HorizontalAlignment.Left);

				// now read all the returned records
				while (DataReader.Read())
				{
					// count the no of found records
					ResultRecordCount++;

					// create a list item which represents that record
					ListViewItem Item = new ListViewItem();

					// first column is the list view item text
					Item.Text = DataReader.GetValue(0).ToString();

					// loop through all remaining fields and add them to the list view item
					for (int Count = 1; Count < DataReader.FieldCount; Count++)
						Item.SubItems.Add(DataReader.GetValue(Count).ToString());

					// now add the list view item to the list view
					QueryResult.Items.Add(Item);
				}

				// shows the number of found records
				RecordCount.Text = ResultRecordCount.ToString();

				// close the data reader and underlying connection object
				DataReader.Close();

				// if the query is not yet in the drop down box then let's add it
				// so the user doesn't have to retype it again
				if (!QueryToRun.Items.Contains(QueryToRun.Text))
					QueryToRun.Items.Add(QueryToRun.Text);
			}

			// show any connection/querying error
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// execute the local query and show the result set
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="e">event arguments</param>
		private void ExecuteLocalButton_Click(object sender, EventArgs e)
		{
			// check to make sure that the user provided a query and catalog
			if ((CatalogName.Text.Length > 0) & (QueryToRun.Text.Length > 0))
			{
				try
				{
					// execute the data query against the local indexing server and obtain 
					// the data result
					IDataReader DataReader = CatalogQuery.LocalQuery(CatalogName.Text, QueryToRun.Text);

					// now show the nre result-set
					ShowQueryResults(DataReader);
				}

				// show any connection/querying error
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}

			// show a message that not all information has been provided
			else
				MessageBox.Show(this, ProvideQueryAndCatalog, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		/// <summary>
		/// execute the remote query and show the result set
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="e">event arguments</param>
		private void ExecuteRemoteButton_Click(object sender, EventArgs e)
		{
			// check to make sure that the user provided a machine name, query and catalog
			if ((RemoteMachineName.Text.Length > 0) & (CatalogName.Text.Length > 0) & (QueryToRun.Text.Length > 0))
			{
				try
				{
					// execute the data query against the remote indexing server and obtain 
					// the data result
					IDataReader DataReader = CatalogQuery.RemoteQuery(RemoteMachineName.Text, CatalogName.Text, QueryToRun.Text);

					// now show the nre result-set
					ShowQueryResults(DataReader);
				}

				// show any connection/querying error
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}

			// show a message that not all information has been provided
			else
				MessageBox.Show(this, RemoteProvideQueryAndCatalog, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		/// <summary>
		/// closes the form
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="e">event arguments</param>
		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// prefill the machine name with the local machine name
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="e">event arguments</param>
		private void IndexingServerManager_Load(object sender, EventArgs e)
		{
			RemoteMachineName.Text = Environment.MachineName;
		}

		/// <summary>
		/// launch a URL in the browser to show the MSDN help
		/// </summary>
		/// <param name="HelpUrl">the help URL to use</param>
		public static void ShowUrl(string HelpUrl)
		{
			// set the process start info - which is the launch URL and window style
			ProcessStartInfo StartInfo = new ProcessStartInfo(HelpUrl);
			StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

			// launch the URL, which opens it in the browser
			Process.Start(StartInfo);
		}

		/// <summary>
		/// show the MSDN help topic
		/// </summary>
		/// <param name="sender">event sender</param>
		/// <param name="e">event arguments</param>
		private void MsdnHelpButton_Click(object sender, EventArgs e)
		{
			ShowUrl(HelpUrl);
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new IndexingServerManager());
		}
	}
}
