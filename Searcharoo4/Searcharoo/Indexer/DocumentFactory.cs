using System;

namespace Searcharoo.Common
{
    public static class DocumentFactory
    {
        public static Document New (Uri uri, System.Net.HttpWebResponse contentType)
        {
            Document newDoc = new IgnoreDocument(uri);
            string mimeType = ParseMimeType(contentType.ContentType.ToString()).ToLower();
            string encoding = ParseEncoding(contentType.ToString()).ToLower();
            switch (mimeType)
            {
                case "text/css":
                    
                    break;
                case "application/x-msdownload":
                case "application/octet-stream":

                    break;
                case "application/vnd.ms-powerpoint":
                case "application/pdf":
                case "application/msword":
                    newDoc = new FilterDocument(uri);
                    break;

                case "text/plain":
                    newDoc = new TextDocument(uri);
                    break;
                
                case "text/xml":
                case "application/xml":
                    newDoc = new HtmlDocument(uri); // TODO: XmlDocument parser
                    break;
                case "application/rss+xml":
                case "application/rdf+xml":
                case "application/atom+xml":
                    newDoc = new HtmlDocument(uri); // TODO: RssDocument parser
                    break;
                case "application/xhtml+xml":
                    newDoc = new HtmlDocument(uri); // TODO: XhtmlDocument parser
                    break;
                case "text/html":
                default:
                    //newDoc = new HtmlDocument(uri);
                    if (mimeType.IndexOf("text") >= 0)
                    {   // If we got 'text' data (not images)
                        newDoc = new HtmlDocument(uri);
                    }
                    break;
            } // switch
            newDoc.MimeType = mimeType;
            
            return newDoc;
        }

        private static string ParseMimeType(string contentType)
        {
            string mimeType = string.Empty;
            string[] contentTypeArray = contentType.Split(';');
            // Set MimeType if it's blank
            if (mimeType == String.Empty && contentTypeArray.Length >= 1)
            {
                mimeType = contentTypeArray[0];
            }
            return mimeType;
        }

        private static string ParseEncoding(string contentType)
        {
            string encoding = string.Empty;
            string[] contentTypeArray = contentType.Split(';');
            // Set Encoding if it's blank
            if (encoding == String.Empty && contentTypeArray.Length >= 2)
            {
                int charsetpos = contentTypeArray[1].IndexOf("charset");
                if (charsetpos > 0)
                {
                    encoding = contentTypeArray[1].Substring(charsetpos + 8, contentTypeArray[1].Length - charsetpos - 8);
                }
            }
            return encoding;
        }
    }
}
