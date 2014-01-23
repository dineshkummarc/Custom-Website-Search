using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace Searcharoo.Common
{
    /// <summary>
    /// File attributes
    /// </summary>
    /// <remarks>
    /// *Beware* ambiguity with System.IO.File - always fully qualify File object references
    /// </remarks>
    [Serializable]
    public class File
    {
        #region Fields - can't remember why these were made public!
        [XmlIgnore]
        private string _Url;
        [XmlIgnore]
        private string _Title;
        [XmlIgnore]
        private string _Description;
        [XmlIgnore]
        private DateTime _CrawledDate;
        [XmlIgnore]
        private long _Size;
        #endregion

        [XmlAttribute("u")]
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }
        [XmlAttribute("t")]
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        [XmlElement("d")]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        [XmlAttribute("d")]
        public DateTime CrawledDate
        {
            get { return _CrawledDate; }
            set { _CrawledDate = value; }
        }
        [XmlAttribute("s")]
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }
        /// <summary>
        /// Required for serialization
        /// </summary>
        public File() { }

        /// <summary>
        /// Constructor requires all File attributes
        /// </summary>
        public File(string url, string title, string description, DateTime datecrawl, long length)
        {
            _Title = title;
            _Description = description;
            _CrawledDate = datecrawl;
            _Url = url;
            _Size = length;
        }

        /// <summary>
        /// Debug string
        /// </summary>
        public override string ToString()
        {
            return "\tFILE :: " + Url + " -- " + Title + " - " + Size + " bytes + \n\t" + Description + "\n";
        }
    }
}
