using System;
using System.Collections.Generic;
using System.Text;
using Bo.Cuyahoga.Extensions.Search;
namespace BoLuceneSearchTest
{
	public class PersonContent
	{
		private string _id;

		[SearchContentField(SearchContentFieldType.Keyword,IsKeyField=true)]
		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		private string _keyword;
		[SearchContentField(SearchContentFieldType.Keyword)]
		public string Keyword
		{
			get { return _keyword; }
			set { _keyword = value; }
		}

		private string _fullName;
		[SearchContentField(SearchContentFieldType.Text,IsQueryField=true)]
		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; }
		}

		private string _notes;
		[SearchContentField(SearchContentFieldType.UnStored,IsResultField=false,IsQueryField=true)]
		public string Notes
		{
			get { return _notes; }
			set { _notes = value; }
		}

		private int _age;
		[SearchContentField(SearchContentFieldType.UnIndexed)]
		public int Age
		{
			get { return _age; }
			set { _age = value; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("Id = " + _id);
			sb.AppendLine("FullName = " + _fullName);
			sb.AppendLine("Age = " + _age.ToString());

			return sb.ToString();

		}


	}
}
