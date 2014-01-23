using System;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace AgileVisions.IndexManager
{
	/// <summary>
	/// Created by Mayukh Dutta (mayukh_dutta@homail.com)
	/// Date: 8/31/2006
	/// </summary>
	public class Results : CollectionBase
	{
		public ArrayList arrlist = new ArrayList();
		public Results()
		{
			
		}
		public void Add(DocumentManagement.Document doc, float score)
		{
			arrlist.Add(new Result(doc, score));
		}
		public void Add(Result result)
		{
			arrlist.Add(result);
		}
		public void Remove(Result result)
		{
			arrlist.Remove(result);
		}
		
		public Result this[int index]
		{
			get{ return (Result)arrlist[index];}
			set { arrlist[index] = value;}
		}
		public void Sort()
		{
			ScoreComparer comparer = new ScoreComparer();
			comparer.SortOrder = SortOrderEnum.Descending;
			comparer.SortProperty = "Score";
			arrlist.Sort(comparer);
		}
		
	}
	public enum SortOrderEnum
	{
		Ascending,
		Descending
	}
	public class ScoreComparer : IComparer
	{
		private String _Property = null;
		private SortOrderEnum _SortOrder = SortOrderEnum.Ascending;
		public String SortProperty
		{
			get { return _Property; }
			set { _Property = value; }
		}
		public SortOrderEnum SortOrder
		{
			get { return _SortOrder; }
			set { _SortOrder = value; }
		}
		public int Compare(object x, object y)
		{
			Result ing1;
			Result ing2;
        
			if (x is Result)
				ing1 = (Result) x;
			else
				throw new ArgumentException("Object is not of type Result");

			if (y is Result)
				ing2 = (Result) y;
			else
				throw new ArgumentException("Object is not of type Result");
        
			if (this.SortOrder.Equals(SortOrderEnum.Ascending))
				return ing1.CompareTo(ing2, this.SortProperty);
			else
				return ing2.CompareTo(ing1, this.SortProperty);
		}
		
	}

	public class Result
	{
		DocumentManagement.Document document = new IndexManager.DocumentManagement.Document();
		float _score;
		
		public Result(DocumentManagement.Document doc, float score)
		{
			document = doc;
			_score = score;
		}
		public DocumentManagement.Document Doc 
		{
			get { return document;}
			set { document = value;}
		}
		public float Score
		{
			get { return _score;}
			set { _score = value;}
		}
		public int CompareTo(object obj, string Property)
		{
			try
			{
				Type type = this.GetType();
				PropertyInfo prop = type.GetProperty(Property);
				Type type2 = obj.GetType();
				PropertyInfo prop2 = type2.GetProperty(Property);
				object[] index = null;
				object Obj1 = prop.GetValue(this, index);
				object Obj2 = prop2.GetValue(obj, index);
				IComparable Ic1 = (IComparable) Obj1;
				IComparable Ic2 = (IComparable) Obj2;
				int returnValue = Ic1.CompareTo(Ic2);
				return returnValue;
			}
			catch (Exception Ex)
			{
				throw new ArgumentException("CompareTo is not possible !");
			}
		}
	}
	
}

