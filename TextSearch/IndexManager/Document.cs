using System;
using System.Text;
using System.Collections;

namespace AgileVisions.IndexManager.DocumentManagement
{
	/// <summary>
	/// Created by Mayukh Dutta (mayukh_dutta@homail.com)
	/// Date: 8/31/2006
	/// </summary>
	[Serializable]
	public class Documents : CollectionBase
	{
		public void Add(Document document)
		{
			List.Add(document);
		}
		public void Remove(Document document)
		{
			List.Remove(document);
		}
		public Document this[int index]
		{
			get { return (Document) List[index];}
			set { List[index] = value;}
		}
	}
	/// <summary>
	/// Created by Mayukh Dutta (mayukh_dutta@homail.com)
	/// Date: 8/31/2006
	/// </summary>
	[Serializable]
	public class Document
	{
		private string _name = string.Empty;
		private string _filename = string.Empty;
		private string _applicationdirectory = string.Empty;
		private string _virtualdirectory = string.Empty;
		private Hashtable _index;
		

		public Document(){}
		public void SetContent(string sContent){}
		
		public Hashtable Index
		{
			get { return _index;}
			set { _index = value;}
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public string FileName
		{
			get { return _filename; }
			set { _filename = value; }
		}
		
		public string ApplicationDirectory
		{
			set {_applicationdirectory = value;}
			get { return _applicationdirectory;}
		}

		public string VirtualDirectory
		{
			get { return _virtualdirectory;}
			set { _virtualdirectory = value;}
		}
	}
}
