using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using cd.net;

namespace Searcharoo.Common
{
    /// <summary>Instance of a word<summary>
    [Serializable]
    public class Word
    {
        #region Private fields: _Text, _FileCollection
        /// <summary>Collection of files the word appears in</summary>
        private System.Collections.Hashtable _FileCollection = new System.Collections.Hashtable();
        /// <summary>The word itself</summary>
        private string _Text;
        #endregion

        /// <summary>
        /// The cataloged word
        /// </summary>
        [XmlElement("t")]
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }
        /// <summary>
        /// Files that this Word appears in
        /// </summary>
        [XmlElement("fs")]
        public File[] Files
        {
            get
            {
                File[] fileArray = new File[_FileCollection.Count];
                _FileCollection.Keys.CopyTo(fileArray, 0);
                return fileArray;
            }
            set
            {
                File[] fileArray = value;
                Hashtable index = new Hashtable();
            }
        }

        /// <summary>
        /// Empty constructor required for serialization
        /// </summary>
        public Word() { }

        /// <summary>Constructor with first file reference</summary>
        public Word(string text, File infile, int position)
        {
            _Text = text;
            //WordInFile thefile = new WordInFile(filename, position);
            _FileCollection.Add(infile, 1);
        }

        /// <summary>Add a file referencing this word</summary>
        public void Add(File infile, int position)
        {
            if (_FileCollection.ContainsKey(infile))
            {
                int wordcount = (int)_FileCollection[infile];
                _FileCollection[infile] = wordcount + 1; //thefile.Add (position);
            }
            else
            {
                //WordInFile thefile = new WordInFile(filename, position);
                _FileCollection.Add(infile, 1);
            }
        }

        /// <summary>Collection of files containing this Word (Value=WordCount)</summary>
        public Hashtable InFiles()
        {
            return _FileCollection;
        }

        /// <summary>Debug string</summary>
        public override string ToString()
        {
            string temp = "";
            foreach (object tempFile in _FileCollection.Values) temp += ((File)tempFile).ToString();
            return "\tWORD :: " + _Text + "\n\t\t" + temp + "\n";
        }
    }  
}
