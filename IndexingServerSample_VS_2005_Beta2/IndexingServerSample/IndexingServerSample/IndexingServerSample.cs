using System;
using System.Collections.Generic;
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
	public partial class IndexingServerManager : Form
	{
		const string HelpUrl = "http://msdn.microsoft.com/library/default.asp?url=/library/en-us/indexsrv/html/ixrefqls_3wj7.asp";
		const string ProvideQueryAndCatalog = "You must provide a catalog name and query string!";
		const string RemoteProvideQueryAndCatalog = "You must provide a remote machine name, catalog name and query string!";

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
					QueryResult.Columns.Add(DataReader.GetName(Count), DataReader.GetName(Count).Length * 15);

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
	}
}
