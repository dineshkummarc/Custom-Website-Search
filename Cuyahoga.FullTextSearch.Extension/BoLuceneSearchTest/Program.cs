using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Bo.Cuyahoga.Extensions.Search;

namespace BoLuceneSearchTest
{
	class Program
	{
		private const string dirName = "IndexingTest";
		private static IList<PersonContent> sampleContent = new List<PersonContent>();
		static void Main(string[] args)
		{
			if (Directory.Exists(dirName))
			{
				DeleteDir(dirName);
			}
			Directory.CreateDirectory(dirName);

			PrepareSamples();
			BuildFullTextIndex(false);
			Console.WriteLine("Index was built.");

			while(true)
			{
				Console.Write("-> Enter query text (Ctrl+C to quit): ");
				string qry = Console.ReadLine();
				if (String.IsNullOrEmpty(qry))
					continue;
				SearchResultCollection<PersonContent> result = Find(qry);
				if (result.Count == 0)
				{
					Console.WriteLine("No mathches.");
				}
				else
				{
					Console.WriteLine("{0} mathches found.",result.Count);
					int i = 1;
					foreach (PersonContent p in result)
					{
						Console.WriteLine("Match {0}", i);
						Console.WriteLine(p.ToString());
						i++;
					}
				}
			}
		}

		private static void DeleteDir(string dir)
		{
			DirectoryInfo di = new DirectoryInfo(dir);
			FileInfo[] files = di.GetFiles();
			foreach (FileInfo fi in files)
			{
				File.Delete(fi.FullName);
			}

			Directory.Delete(dir);
		}

		private static void BuildFullTextIndex(bool rebuild)
		{
			using (IndexBuilderEx<PersonContent> ib = new IndexBuilderEx<PersonContent>(dirName, rebuild))
			{
				foreach (PersonContent p in sampleContent)
				{
					ib.AddContent(p);
				}
			}
		}

		private static void DeleteSampleFromIndex()
		{
			using (IndexBuilderEx<PersonContent> ib = new IndexBuilderEx<PersonContent>(dirName, false))
			{
				foreach (PersonContent p in sampleContent)
				{
					if (p.FullName == "Mustafa")
					{
						ib.DeleteContent(p);
						Console.WriteLine("Deleted content with name Mustafa");
					}
				}
			}
		}

		private static SearchResultCollection<PersonContent> Find(string qry)
		{
			IndexQueryEx<PersonContent> idQry = new IndexQueryEx<PersonContent>(dirName);
			Hashtable keywordFilter = new Hashtable();
			keywordFilter.Add("Keyword", "PersonContent");

			return idQry.Find(qry, keywordFilter, 0, 200);
		}

		private static void PrepareSamples()
		{
			PersonContent p = new PersonContent();
			p.Id = Guid.NewGuid().ToString();
			p.FullName = "Ali";
			p.Notes = "Software developer, özgür family member";
			p.Age = 29;
			p.Keyword = "PersonContent";
			sampleContent.Add(p);
			Console.WriteLine("Sample 1:");
			Console.WriteLine(p.ToString());

			p = new PersonContent();
			p.Id = Guid.NewGuid().ToString();
			p.FullName = "Seniha";
			p.Notes = "Sales Rep and future mother, özgür family member";
			p.Age = 30;
			p.Keyword = "PersonContent";
			sampleContent.Add(p);
			Console.WriteLine("Sample 2:");
			Console.WriteLine(p.ToString());

			p = new PersonContent();
			p.Id = Guid.NewGuid().ToString();
			p.FullName = "Mustafa";
			p.Notes = "Department Manager";
			p.Age = 43;
			p.Keyword = "PersonContent";
			sampleContent.Add(p);
			Console.WriteLine("Sample 3:");
			Console.WriteLine(p.ToString());


		}
	}
}
