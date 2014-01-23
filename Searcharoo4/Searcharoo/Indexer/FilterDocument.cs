using System;
using System.Collections.Generic;
using System.Text;

namespace Searcharoo.Common
{
    public class FilterDocument : Document
    {
        private string _All;
        private string _WordsOnly;

        public FilterDocument(Uri location):base(location)
        {}

        /// <summary>
        /// Set 'all' and 'words only' to the same value (no parsing)
        /// </summary>
        public override string All
        {
            get { return _All; }
            set { 
                _All = value;
                _WordsOnly = _All;
            }
        }
        
        public override string WordsOnly
        {
            get { return _WordsOnly; }
        }

        public override string[] WordsArray
        {
            get { return this.WordsStringToArray(WordsOnly); }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public override void Parse()
        {
            // no parsing (for now). perhaps in future we can regex look for urls (www.xxx.com) and try to link to them...
        }

        public override bool GetResponse(System.Net.HttpWebResponse webresponse)
        {
            System.IO.Stream filestream = webresponse.GetResponseStream();
            this.Uri = webresponse.ResponseUri;
            string filename = System.IO.Path.Combine(Preferences.DownloadedTempFilePath, (System.IO.Path.GetFileName(this.Uri.LocalPath)));
            this.Title = System.IO.Path.GetFileNameWithoutExtension(filename);
           
            using (System.IO.BinaryReader reader = new System.IO.BinaryReader(filestream))
            {
                using (System.IO.FileStream iofilestream = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                {
                    int BUFFER_SIZE = 1024;
                    byte[] buf = new byte[BUFFER_SIZE];
                    int n = reader.Read(buf, 0, BUFFER_SIZE);
                    while (n > 0)
                    {
                        iofilestream.Write(buf, 0, n);
                        n = reader.Read(buf, 0, BUFFER_SIZE);
                    }

                    this.Uri = webresponse.ResponseUri;
                    this.Length = iofilestream.Length;
                    iofilestream.Close();
                    iofilestream.Dispose();
                } 
                reader.Close();
            } 
            try
            {
                EPocalipse.IFilter.FilterReader ifil = new EPocalipse.IFilter.FilterReader(filename);
                this.All = ifil.ReadToEnd();
                ifil.Close();
                System.IO.File.Delete(filename);    // clean up
            }
            catch (Exception)
            {
//                ProgressEvent(this, new ProgressEventArgs(2, "IFilter failed on " + this.Uri + " " + e.Message + ""));
            }
            if (this.All != string.Empty)
            {
                this.Description = base.GetDescriptionFromWordsOnly(WordsOnly);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}