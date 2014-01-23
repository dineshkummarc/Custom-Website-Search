using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace AgileVisions.IndexManager.WordProcessor
{
	public class WordCollector
	{
		Hashtable StopWords = null;
		public bool fetchunique = true;
		public ArrayList arrCombined = new ArrayList();
		public WordCollector(string [] stopwords)
		{
			if (StopWords == null)
			{
				StopWords = new Hashtable();
				double dummy = 0;
				foreach (string word in stopwords)
				{
					AddWords(StopWords, word, dummy);
				}
			}
		}
		private object AddWords(IDictionary collection, Object key, object newValue)
		{
			object element = collection[key];
			collection[key] = newValue;
			return element;
		}
		string [] regexpatterns = {" ","\\t","{","}","(",")",":",";",",","\n","\r","\\s","|"};
		public void BreakWords(string sString)
		{
			Regex regEx=new Regex("([ \\t{}():;.,| \n\r\\s*])");		
			string []  strArray = regEx.Split(sString.ToLower());
			foreach(string str in strArray)
			{
				if(str == string.Empty) continue;
				if(str.Length == 1)
				{
					bool found = false;
					foreach(string c in regexpatterns)
					{
						if (str == c)
						{
							found = true;
							break;
						}
					}
					if (found) continue;
				}
				if (fetchunique)
				{
					if (!arrCombined.Contains(str) && !StopWords.ContainsKey(str))
						arrCombined.Add(str);
				}
				else
					if (!StopWords.ContainsKey(str))
					arrCombined.Add(str);
			}
		}
	}

	}
