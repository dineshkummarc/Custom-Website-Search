using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;



namespace AgileVisions.IndexManager.Analyzer
{
	/// <summary>
	/// Created by Mayukh Dutta (mayukh_dutta@homail.com)
	/// Date: 8/31/2006
	/// </summary>
	public enum StemmerLevel
	{
		PruralsandPastParticiples, All, None
	}

	
	/// <summary>
	/// Created by Mayukh Dutta (mayukh_dutta@homail.com)
	/// Date: 8/31/2006
	/// </summary>
	public class Indexer
	{
		private const float floatzero = 0F;

		Hashtable htGlobalWordList = null;
		ArrayList arrGlobalList = null;
		string indexpath = string.Empty;
		string[] stopwordslist = { "is", "can", "and", "the", "or", "that", "i", "we", "you" };
		
		public StemmerLevel stemlevel = StemmerLevel.None;

		DocumentManagement.Documents documentcollection = new IndexManager.DocumentManagement.Documents();

		
		public Indexer()
		{
			htGlobalWordList = new Hashtable();
			arrGlobalList = new ArrayList();
		}
		
		public string IndexPath
		{
			get { return indexpath;}
			set { indexpath = value;}
		}

		public DocumentManagement.Documents DocumentCollection
		{
			get { return documentcollection;}
		}

		public void AddDocument(DocumentManagement.Document doc, string documenttext)
		{
			LoadDocument(documenttext,ref arrGlobalList, true);
			arrGlobalList.Sort();
			htGlobalWordList = CreateCommonHash(arrGlobalList);
		}
		public void CreateDocumentIndices(DocumentManagement.Document doc, string documenttext)
		{
			ArrayList arrtemp = new ArrayList();
			ArrayList wordsfromdocument = LoadDocument(documenttext, ref arrtemp, false);
			doc.Index = new Hashtable();
			doc.Index = CalculateHash(htGlobalWordList, wordsfromdocument);
			documentcollection.Add(doc);
		}
		public void WriteIndices()
		{
			if (indexpath == string.Empty)
				throw new Exception("Index path not set");
			string localindexpath = System.IO.Path.Combine(Directory.GetCurrentDirectory() , indexpath);
			if(!Directory.Exists(localindexpath))
				Directory.CreateDirectory(localindexpath);
			if(Directory.Exists(localindexpath))
			{
				string documentcollectionfile = "dc";
				string globalindexfile = "gi";
				WriteToBinary(documentcollection,  Path.Combine(localindexpath, documentcollectionfile));
				WriteToBinary(htGlobalWordList, Path.Combine(localindexpath, globalindexfile));
			}
		}
		private void WriteToBinary(object obj, string filename)
		{
			Stream stream = File.Open(Path.Combine(indexpath, filename), FileMode.OpenOrCreate);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, obj);
			stream.Close();
		}

		private object ReadFromBinary(string filename)
		{
			object returnobject = null;
			Stream stream = File.OpenRead(filename);
			BinaryFormatter formatter = new BinaryFormatter();
			returnobject = formatter.Deserialize(stream);
			stream.Close();
			return returnobject;
		}
		public void ReadIndices()
		{
			string documentcollectionfile = "dc";
			string globalindexfile = "gi";
			documentcollection = (DocumentManagement.Documents) ReadFromBinary(Path.Combine(indexpath, documentcollectionfile));
			htGlobalWordList = (Hashtable) ReadFromBinary(Path.Combine(indexpath, globalindexfile)); 
		}
		public Results Search(string query)
		{
			ArrayList arrtemp = new ArrayList();
			ArrayList arrquerywords = new ArrayList();
			arrquerywords = LoadDocument(query, ref arrtemp, false);
			
			Hashtable htQuery = new Hashtable();
			htQuery = CalculateHash(htGlobalWordList, arrquerywords);
			Results results = new Results();
			foreach(DocumentManagement.Document doc in documentcollection)
			{
				float score = (float) CalculateDistance(htQuery, doc.Index);
				results.Add(new Result(doc, score));
			}
			results.Sort();
			return results;
		}

		private double CalculateDistance(Hashtable hta, Hashtable htb)
		{
			float f = Cosine(hta, htb);
			return (double)f;
		}
		
		public static float Cosine(Hashtable vectorA, Hashtable vectorB)
		{
			if (vectorA.Count != vectorB.Count)
				throw new Exception("Cannot perform the operation, vector lengths do not match");
			float norm = (Length(vectorA) * Length(vectorB));
			if (norm == floatzero)
				return floatzero;
			else
				return (Product(vectorA, vectorB) / norm);
		}

		public static float Product(Hashtable vectorA, Hashtable vectorB)
		{
			if (vectorA.Count != vectorB.Count)
				throw new Exception("Cannot perform the operation, vector lengths do not match");
			float result = floatzero;
			for (int i = 0; i < vectorA.Count; i++)
				result += float.Parse(vectorA[i].ToString()) * float.Parse(vectorB[i].ToString());
			return result;
		}

		public static float Length(Hashtable vector)
		{
			float sum = 0.0F;
			for (int i = 0; i < vector.Count; i++)
				sum = sum + (float.Parse(vector[i].ToString()) * float.Parse(vector[i].ToString()));
			return (float)Math.Sqrt(sum);
		}


		private Hashtable CreateCommonHash(ArrayList arrTemp)
		{
			Hashtable htCommon = new Hashtable();
			int i = 0;
			foreach (string strTemp in arrTemp)
			{
				htCommon.Add(i, strTemp);
				i++;
			}
			return htCommon;
		}
		private Hashtable CalculateHash(Hashtable htCommon, ArrayList ar)
		{
			Hashtable wordhash = new Hashtable();
			foreach (string strword in ar)
			{
				if (!wordhash.ContainsKey(strword))
					wordhash.Add(strword, 1);
				else
					wordhash[strword] = int.Parse(wordhash[strword].ToString()) + 1;
			}
			Hashtable htDocument = new Hashtable();
			for (int i = 0; i < htCommon.Count; i++)
			{
				if (wordhash.ContainsKey(htCommon[i].ToString()))
					htDocument.Add(i, wordhash[htCommon[i].ToString()]);
				else
					htDocument.Add(i, 0);
			}
			return htDocument;
		}

		
		private ArrayList LoadDocument(string sString, ref ArrayList ar, bool uniquewords)
		{
			WordProcessor.WordCollector wc = new IndexManager.WordProcessor.WordCollector(stopwordslist);
			wc.fetchunique = uniquewords;
			wc.BreakWords(sString);
			ar = Combine(ar, wc.arrCombined);
			return wc.arrCombined;
		}
		public ArrayList Combine(ArrayList arg1, ArrayList arg2)
		{
			ArrayList result = (ArrayList)arg1.Clone();
			IEnumerator e = arg2.GetEnumerator();

			while (e.MoveNext())
			{
				Object o = (Object)e.Current;
				if (!result.Contains(o))
					result.Add(o);
			}
			return result;
		}
	}
}
