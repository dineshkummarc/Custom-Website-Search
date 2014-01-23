using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace AgileVisions.SearchDocuments
{
	/// <summary>
	/// Summary description for SelectFolder.
	/// </summary>
	public class SelectFolder : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button button1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imglistFolder;
		private System.Windows.Forms.TreeView tvFolders;
		private static int depth =0;
		public DirectoryInfo selecteddirectory = null;
		public SelectFolder()
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
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectFolder));
			this.panel1 = new System.Windows.Forms.Panel();
			this.imglistFolder = new System.Windows.Forms.ImageList(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tvFolders = new System.Windows.Forms.TreeView();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.tvFolders);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(296, 232);
			this.panel1.TabIndex = 0;
			// 
			// imglistFolder
			// 
			this.imglistFolder.ImageSize = new System.Drawing.Size(16, 16);
			this.imglistFolder.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistFolder.ImageStream")));
			this.imglistFolder.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button1);
			this.panel2.Controls.Add(this.btnOK);
			this.panel2.Location = new System.Drawing.Point(0, 232);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(328, 32);
			this.panel2.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(224, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(64, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "&Cancel";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(160, 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// tvFolders
			// 
			this.tvFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvFolders.ImageList = this.imglistFolder;
			this.tvFolders.Location = new System.Drawing.Point(0, 0);
			this.tvFolders.Name = "tvFolders";
			this.tvFolders.Size = new System.Drawing.Size(296, 232);
			this.tvFolders.TabIndex = 0;
			this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
			this.tvFolders.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvFolders_BeforeExpand);
			// 
			// SelectFolder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 266);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectFolder";
			this.Text = "SelectFolder";
			this.Load += new System.EventHandler(this.SelectFolder_Load);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		private void SelectFolder_Load(object sender, System.EventArgs e)
		{
			LoadTree();
		}
		private void LoadTree()
		{
			tvFolders.Nodes.Clear();
			DirectoryInfo dirinfo;
			foreach(string drive in Directory.GetLogicalDrives())
			{
				dirinfo = new DirectoryInfo(drive);
				if(dirinfo.Exists)
				{
					TreeNode tn = new TreeNode(dirinfo.Name);
					tn.ImageIndex = 0;
					tn.Tag = dirinfo;
					tvFolders.Nodes.Add(tn);
					AddSubFoldersRecrusive(tn, dirinfo);
				}
			}
		}
		private void AddSubFoldersRecrusive(TreeNode tn, DirectoryInfo dirinfo)
		{
		//	depth++;
			foreach(DirectoryInfo tempdirinfo in dirinfo.GetDirectories())
			{
				TreeNode tnTemp = new TreeNode(tempdirinfo.Name);
				tnTemp.Tag = tempdirinfo;
				tnTemp.ImageIndex = 0;
				tn.Nodes.Add(tnTemp);
			/*	if(depth>10) 
				{
					depth = 0;
					break;
				}*/
			//	AddSubFoldersRecrusive(tnTemp, tempdirinfo);
			}
		}

		private void tvFolders_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			if(e.Node.Tag!=null)
			{
				DirectoryInfo dirinfo = (DirectoryInfo) e.Node.Tag;
				if(e.Node.Nodes.Count<=0)
				{
					AddSubFoldersRecrusive(e.Node,dirinfo);
				}
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(tvFolders.SelectedNode != null)
			{
				this.DialogResult = DialogResult.OK;
				selecteddirectory = (DirectoryInfo)tvFolders.SelectedNode.Tag; 
			}
			else
				this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void tvFolders_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(e.Node.Tag!=null)
			{
				DirectoryInfo dirinfo = (DirectoryInfo) e.Node.Tag;
				if(e.Node.Nodes.Count<=0)
				{
					AddSubFoldersRecrusive(e.Node,dirinfo);
				}
			}
		}
	}
}
