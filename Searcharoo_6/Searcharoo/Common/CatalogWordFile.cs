using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Searcharoo.Common
{
    /// <summary>
    /// Used solely by the <see cref="Catalog"/> class to XmlSerialize the index
    /// to disk when binary serialization will not work due to Trust level issues.
    /// </summary>
    [Serializable]
    public class CatalogWordFile
    {
        #region Private Fields: _text, _fileIds
        private string _text;
        private List<int> _fileIds = new List<int>();
        #endregion

        /// <summary>
        /// The word that has been indexed
        /// </summary>
        [XmlElement("t")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        /// <summary>
        /// The 'generated identifiers' of the File objects associated with the Word
        /// </summary>
        [XmlElement("i")]
        public List<int> FileIds
        {
            get { return _fileIds; }
            set { _fileIds = value; }
        }
    }
}
