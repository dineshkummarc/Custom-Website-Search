using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Searcharoo.Common
{
    public abstract class Document
    {
        private Uri _Uri;
        private string _All;
        private string _ContentType;
        private string _MimeType = String.Empty;
        private string _Title;
        private long _Length;
        /// <summary>Html &lt;meta http-equiv='description'&gt; tag</summary>
        private string _Description = String.Empty;
        
        /// <summary>
        /// Subclasses must implement GetResponse
        /// </summary>
        public abstract bool GetResponse(System.Net.HttpWebResponse webresponse);
        public abstract void Parse();

        public ArrayList LocalLinks;
        public ArrayList ExternalLinks;

        public virtual string All
        {
            get { return _All; }
            set { _All = value; }
        }
        public virtual string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        public virtual string MimeType
        {
            get { return _MimeType; }
            set { _MimeType = value; }
        }

        public abstract string WordsOnly { get; }
        
        public virtual string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public virtual long Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        public virtual string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        /// <summary>
        /// http://www.ietf.org/rfc/rfc2396.txt
        /// </summary>
        public virtual Uri Uri
        {
            get { return _Uri; }
            set { _Uri = value; }
        }

        public virtual string[] WordsArray
        {
            get { return this.WordsStringToArray(WordsOnly); }
        }

        /// <summary>
        /// Most document types don't have embedded robot information
        /// so they'll always be allowed to be followed 
        /// (assuming there are links to follow)
        /// </summary>
        public virtual bool RobotFollowOK
        {
            get { return true; }
        }
        /// <summary>
        /// Most document types don't have embedded robot information
        /// so they'll always be allowed to be indexed 
        /// (assuming there is content to index)
        /// </summary>
        public virtual bool RobotIndexOK
        {
            get { return true; }
        }

        /// <summary>
        /// Constructor for any document requires the Uri be specified
        /// </summary>
        public Document(Uri uri)
        {
            _Uri = uri;
        }

       

        protected string[] WordsStringToArray(string words)
        {
            // COMPRESS ALL WHITESPACE into a single space, seperating words
            if (words.Length > 0)
            {
                Regex r = new Regex(@"\s+");            //remove all whitespace
                string compressed = r.Replace(words, " ");
                return compressed.Split(' ');
            }
            else
            {
                return new string[0];
            }
        }
        protected string GetDescriptionFromWordsOnly(string wordsonly)
        {
            string description = string.Empty;
            if (wordsonly.Length > Preferences.SummaryCharacters)
            {
                description = wordsonly.Substring(0, Preferences.SummaryCharacters);
            }
            else
            {
                description = WordsOnly;
            }
            description = System.Text.RegularExpressions.Regex.Replace(description, @"\s+", " ").Trim();
            return description;
        }
    }
}