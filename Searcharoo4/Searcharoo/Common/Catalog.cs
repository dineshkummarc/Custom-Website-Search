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
    /// <summary>
    /// Catalog of Words (and Files)
    /// <summary>
    /// <remarks>
    /// XmlInclude
    /// http://pluralsight.com/blogs/craig/archive/2004/07/08/1580.aspx
    /// </remarks>
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(Searcharoo.Common.Word))]
    public class Catalog
    {
        /// <summary>
        /// Internal datastore of Words referencing Files
        /// </summary>
        /// <remarks>
        /// Hashtable
        /// key    = STRING representation of the word, 
        /// value  = Word OBJECT (with File collection, etc)
        /// </remarks>
        private System.Collections.Hashtable _Index;	//TODO: implement collection with faster searching

        /// <summary>
        /// Words in the Catalog
        /// </summary>
        /// <remarks>
        /// Added property to allow Serialization to disk
        /// NOTE: the XmlInclude attribute on the Catalog class, which is what
        /// enables this array of 'non-standard' objects to be serialized correctly
        /// </remarks>
        [XmlElement("words")]
        public Word[] Words
        {
            get
            {
                Word[] wordArray = new Word[_Index.Count];
                _Index.Values.CopyTo(wordArray, 0);
                return wordArray;
            }
            set
            {
                Word[] wordArray = value;
                _Index = new Hashtable();
            }
        }

        /// <summary>
        /// String array representing the list of words. 
        /// Used mainly for debugging - ie in the Save() method - so you can 
        /// see what the Spider found.
        /// </summary>
        [XmlAttribute("dict")]
        public string[] Dictionary
        {
            get
            {
                string[] wordArray = new string[_Index.Count];
                _Index.Keys.CopyTo(wordArray, 0);
                return wordArray;
            }
        }
        /// <summary>
        /// Number of Words in the Catalog
        /// </summary>
        public int Length
        {
            get { return _Index.Count; }
        }

        /// <summary>
        /// Constructor - creates the Hashtable for internal data storage.
        /// </summary>
        public Catalog()
        {
            _Index = new System.Collections.Hashtable();
        }
        /// <summary>
        /// Add a new Word/File pair to the Catalog
        /// </summary>
        public bool Add(string word, File infile, int position)
        {
            // ### Make sure the Word object is in the index ONCE only
            if (_Index.ContainsKey(word))
            {
                Word theword = (Word)_Index[word];	// add this file reference to the Word
                theword.Add(infile, position);
            }
            else
            {
                Word theword = new Word(word, infile, position);	// create a new Word object
                _Index.Add(word, theword);
            }
            return true;
        }
        /// <summary>
        /// Returns all the Files which contain the searchWord
        /// </summary>
        /// <returns>
        /// Hashtable where:
        /// </returns>
        public Hashtable Search(string searchWord)
        {
            // apply the same 'trim' as when we're building the catalog
            //searchWord = searchWord.Trim(' ','?','\"',',','\'',';',':','.','(', ')','[',']','%','*','$','-').ToLower();
            Hashtable retval = null;
            if (_Index.ContainsKey(searchWord))
            {	// does all the work !!!
                Word thematch = (Word)_Index[searchWord];
                retval = thematch.InFiles(); // return the collection of File objects
            }
            return retval;
        }

        /// <summary>
        /// Debug string
        /// </summary>
        public override string ToString()
        {
            string wordlist = "";
            //foreach (object w in index.Keys) temp += ((Word)w).ToString();	// output ALL words, will take a long time
            return "CATALOG :: " + _Index.Values.Count.ToString() + " words.\n" + wordlist;
        }

        /// <summary>
        /// Save the catalog to disk by BINARY serializing the object graph as a *.DAT file.
        /// </summary>
        /// <remarks>
        /// For 'reference', the method also saves XmlSerialized copies of the Catalog (which
        /// can get quite large) and just the list of Words that were found. In production, you
        /// would probably comment out/remove the Debugging code.
        /// 
        /// You may also wish to use a difficult-to-guess filename for the serialized data, 
        /// or else change the .DAT file extension to something that will be not be served by
        /// IIS (so that other people can't download your catalog).
        /// </remarks>
        public void Save()
        {
            // BINARY http://www.dotnetspider.com/technology/kbpages/454.aspx
            System.IO.Stream stream = new System.IO.FileStream(Preferences.CatalogFileName, System.IO.FileMode.Create);
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();

            #region Debugging Serialization - these are only really useful for looking at; they're not re-loaded
            if (Preferences.DebugSerializeXml)
            {
                //Kelvin<Catalog>.ToBinaryFile(this, Path.GetFileNameWithoutExtension(Preferences.CatalogFileName) + "_Kelvin" + Path.GetExtension(Preferences.CatalogFileName));
                //Kelvin<Catalog>.ToXmlFile(this, Path.GetFileNameWithoutExtension(Preferences.CatalogFileName) + "_Kelvin.xml");
                Kelvin<string[]>.ToXmlFile(this.Dictionary, Path.GetFileNameWithoutExtension(Preferences.CatalogFileName) + "_words.xml");

                // TODO: Maybe use to save as ZIP - save space on disk? http://www.123aspx.com/redir.aspx?res=31602
                // XML http://www.devhood.com/Tutorials/tutorial_details.aspx?tutorial_id=236
                XmlSerializer serializerXmlCatalog = new XmlSerializer(typeof(Catalog));
                System.IO.TextWriter writer = new System.IO.StreamWriter(Path.GetFileNameWithoutExtension(Preferences.CatalogFileName) + ".xml");
                serializerXmlCatalog.Serialize(writer, this);
                writer.Close();

                // XML http://www.devhood.com/Tutorials/tutorial_details.aspx?tutorial_id=236
                XmlSerializer serializerXmlWords = new XmlSerializer(typeof(string[]));
                System.IO.TextWriter writerW = new System.IO.StreamWriter(Path.GetFileNameWithoutExtension(Preferences.CatalogFileName) + "_words.xml");
                serializerXmlWords.Serialize(writerW, this.Dictionary);
                writerW.Close();
            }
            #endregion
        }

        /// <summary>
        /// Use Kelvin too
        /// </summary>
        /// <returns>the catalog deserialized from disk, or NULL</returns>
        public static Catalog Load()
        {
            //Catalog c0 = Kelvin<Catalog>.FromBinaryFile(Preferences.CatalogFileName + "_Kelvin.dat");
            //Catalog c1 = Kelvin<Catalog>.FromXmlFile(Preferences.CatalogFileName + "_Kelvin.xml");
            //string[] words = Kelvin<string[]>.FromXmlFile(Preferences.CatalogFileName + "_words_Kelvin.xml");

            if (System.IO.File.Exists(Preferences.CatalogFileName))
            {
                object deserializedCatalogObject;
                using (System.IO.Stream stream = new System.IO.FileStream(Preferences.CatalogFileName, System.IO.FileMode.Open))
                {
                    System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    //object m = formatter.Deserialize (stream); // This doesn't work, SerializationException "Cannot find the assembly <random name>"
                    formatter.Binder = new CatalogBinder();	// This custom Binder is REQUIRED to find the classes in our current 'Temporary ASP.NET Files' assembly
                    deserializedCatalogObject = formatter.Deserialize(stream);
                } //stream.Close();
                Catalog catalog = deserializedCatalogObject as Catalog;
                return catalog;
            }
            else
            {
                return null;
            }
        }
    }
}
