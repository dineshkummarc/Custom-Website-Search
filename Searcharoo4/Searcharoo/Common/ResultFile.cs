using System;
using System.Collections.Generic;
using System.Text;

namespace Searcharoo.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ResultFile : File
    {
        private int _Rank;
        public ResultFile(File sourceFile)
        {
            this.Url = sourceFile.Url;
            this.Title = sourceFile.Title;
            this.Description = sourceFile.Description;
            this.CrawledDate = sourceFile.CrawledDate;
            this.Size = sourceFile.Size;
            this.Rank = -1;
        }
        public int Rank
        {
            get { return _Rank; }
            set { _Rank = value; }
        }
    }
}
