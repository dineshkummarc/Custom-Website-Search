<%@ Page Language="c#" autoeventwireup="true" %>
<%@ import Namespace="System" %>
<%@ import Namespace="System.Xml.Serialization" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="Searcharoo.Common" %>
<%@ Register TagPrefix="roo" TagName="SearchPanel" Src="SearchControl.ascx" %>
<script runat="server">
/*
* (c) 2004-2006 Craig Dunn - ConceptDevelopment.NET
* v1 30-Jun-04
* v2 02-Jul-04
* v3 30-Mar-06
* v4 13-Mar-07
* 
* More info:
*    http://www.searcharoo.net/
*    http://www.conceptdevelopment.net/search/searcharooV1/
*    http://www.conceptdevelopment.net/search/searcharooV2/
*    http://www.conceptdevelopment.net/search/searcharooV3/
*    http://www.conceptdevelopment.net/search/searcharoo-V4/
*
* CIA World Factbook
* 268 files	10.8   Mb
* Binary	 4,731 Kb
* Xml		76,917 Kb !!!
*/
    #region Private Fields: _WordCount, _ErrorMessage, _Catalog, _SearchTerm, _PagedResults, _DisplayTime, _Matches, _NumberOfMatches
    /// <summary>Displayed in HTML - count of words IN CATALOG (not results)</summary>
    private int _WordCount;
    /// <summary>Displayed in HTML - error message IF an error occurred</summary>
    private string _ErrorMessage = String.Empty;
    /// <summary>Get from Cache</summary>
    private Catalog _Catalog = null;

    private string _SearchTerm = String.Empty;

    /// <summary>Datasource to bind the results collection to, for paged display</summary>
    private PagedDataSource _PagedResults = new PagedDataSource();
    /// <summary>Display string: time the search too</summary>
    private string _DisplayTime;
    /// <summary>Display string: matches (links and number of)</summary>
    private string _Matches = "";
    /// <summary>Display string: Number of pages that match the query</summary>
    private string _NumberOfMatches;
    #endregion
    
    string SearchQuery {
        get {
            if (string.IsNullOrEmpty(Request.QueryString[Preferences.QuerystringParameterName]))
            {
                return string.Empty;
            }
            else
            {
                return Request.QueryString[Preferences.QuerystringParameterName].ToString().Trim(' ');
            }
        }
    }
    /// <summary>
    /// ALL processing happens here, since we are not using ASP.NET controls or events.
    /// Page_Load will:
    /// * check the Cache for a catalog to use 
    /// * if not, check the filesystem for a serialized cache
    /// * and if STILL not, Server.Transfer to the Spider to build a new cache
    /// * check the QueryString for search arguments (and if so, do a search)
    /// * otherwise just show the HTML of this page - a blank search form
    /// </summary>
    public void Page_Load ()
    {
        // prevent Searcharoo from indexing itself (ie. it's own results page)
        if (Request.UserAgent.ToLower().IndexOf("searcharoo") >0 ) {Response.Clear();Response.End();return;}
        
        bool getCatalog = false;
        try
        {   // see if there is a catalog object in the cache
            _Catalog = (Catalog)Cache["Searcharoo_Catalog"];
            _WordCount = _Catalog.Length; // if so, get the _WordCount
        }
        catch (Exception ex)
        {
            // otherwise, we'll need to build the catalog
            Response.Write ("Catalog object unavailable : building a new one ! <!--" + ex.ToString() + "-->");
            _Catalog = null; // in case
        }
        
        ucSearchPanelHeader.WordCount = _WordCount;
	    ucSearchPanelFooter.WordCount = _WordCount;
        
        if (null == _Catalog)
        {
            getCatalog = true;
        }
        else if  (_Catalog.Length == 0)
        {
            getCatalog = true;
        }
	    if (getCatalog)
	    {
		    // No catalog 'in memory', so let's look for one
		    // First, for a serialized version on disk	
		    _Catalog = Catalog.Load();	// returns null if not found
    		
		    // Still no Catalog, so we have to start building a new one
		    if (null == _Catalog)
		    {	
			    Server.Transfer("SearchSpider.aspx");
			    _Catalog = (Catalog)Cache["Searcharoo_Catalog"];
		    }
		    else 
		    {	// Yep, there was a serialized catalog file
			    // Don't forget to add to cache for next time (the Spider does this too)
			    Cache["Searcharoo_Catalog"] = _Catalog;
			    Response.Write ("Deserialized catalog " + _Catalog.Words);
		    }
        }

        if (this.SearchQuery == "")
        {
            //ucSearchPanelHeader.ErrorMessage = "Please type a word (or words) to search for<br>";
            ucSearchPanelFooter.Visible = false;
            ucSearchPanelFooter.IsFooter = true;
            ucSearchPanelHeader.IsSearchResultsPage = false;
        }
        else
        {
            //refactored into class - catalog can be build via a console application as well as the SearchSpider.aspx page
            Searcharoo.Engine.Search se = new Searcharoo.Engine.Search();
            SortedList output = se.GetResults(this.SearchQuery, _Catalog);

            _NumberOfMatches = output.Count.ToString();
            if (output.Count > 0)
            {
                _PagedResults.DataSource = output.GetValueList();
                _PagedResults.AllowPaging = true;
                _PagedResults.PageSize = Preferences.ResultsPerPage; //10;
                _PagedResults.CurrentPageIndex = Request.QueryString["page"] == null ? 0 : Convert.ToInt32(Request.QueryString["page"]) - 1;

                _Matches = se.SearchQueryMatchHtml;
                _DisplayTime = se.DisplayTime;
                
                SearchResults.DataSource = _PagedResults;
                SearchResults.DataBind();
            }
            else
            {
                lblNoSearchResults.Visible = true;
            }
           // Set the display info in the top & bottom user controls
            ucSearchPanelHeader.Word = ucSearchPanelFooter.Word = this.SearchQuery;
            ucSearchPanelFooter.Visible = true;
            ucSearchPanelFooter.IsFooter = true;
            ucSearchPanelHeader.IsSearchResultsPage = true;  
        }
        
    } // Page_Load

        
    public string CreatePageUrl (string searchFor, int pageNumber)
    {
        return "Search.aspx?" + Preferences.QuerystringParameterName + "=" + this.SearchQuery + "&page=" + pageNumber;
    }

    /// <summary>
    /// This method implements a 'rolling window' page-number index
    /// for the underlying PagedDataSource
    /// </summary>
    /// <remarks>
    /// http://www.sitepoint.com/article/asp-nets-pageddatasource
    /// http://www.uberasp.net/ArticlePrint.aspx?id=29
    /// </remarks>
    public string CreatePagerLinks(PagedDataSource objPds, string BaseUrl)
    {
	    StringBuilder sbPager  = new StringBuilder();
	    StringBuilder sbPager1 = new StringBuilder();

	    sbPager1.Append ("<td><font color=black>S</font><font color=red>e</font><font color=blue>a</font><font color=green>r</font><font color=darkgrey>c</font><font color=purple>h</font><font color=brown>a</font><font color=darkpink>r</font></td>");
    	
	    if (objPds.IsFirstPage)
	    {	// lower link is blank
		    sbPager.Append ("<td></td>");
	    }
	    else
	    {	// first+prev link
		    sbPager.Append ("<td align=right>");
		    // first page link
		    sbPager.Append ("<a href=\"");
		    sbPager.Append (CreatePageUrl (BaseUrl, 1) );
		    sbPager.Append ("\" alt=\"First Page\" title=\"First Page\">|&lt;</a>&nbsp;");
		    if (objPds.CurrentPageIndex != 1)
		    {
			    // previous page link
			    sbPager.Append ("<a href=\"");
			    sbPager.Append (CreatePageUrl (BaseUrl, objPds.CurrentPageIndex ) );
			    sbPager.Append ("\" alt=\"Previous Page\" title=\"Previous Page\">&laquo;</a>&nbsp;");
		    }
		    sbPager.Append ("</td>");
	    }
	    // calc low and high limits for numeric links
	    int intLow = objPds.CurrentPageIndex - 1;
	    int intHigh = objPds.CurrentPageIndex + 3;
	    if (intLow < 1) intLow = 1;
	    if (intHigh > objPds.PageCount) intHigh = objPds.PageCount;
	    if (intHigh - intLow < 5) while ((intHigh < intLow + 4) && intHigh < objPds.PageCount) intHigh++;
	    if (intHigh - intLow < 5) while ((intLow > intHigh - 4) && intLow > 1) intLow--;
	    for (int x = intLow; x < intHigh + 1; x++)
	    {
		    // numeric links
		    if (x == objPds.CurrentPageIndex + 1)
		    {
			    sbPager1.Append("<td width=10 align=center><font color=orange><b>o</b></td>");
			    sbPager.Append ("<td>" + x.ToString() + "</td>");
		    }
		    else
		    {
			    sbPager1.Append("<td width=10 align=center><font color=orange><b>o</b></td>");
			    sbPager.Append ("<td>");
			    sbPager.Append ("<a href=\"");
			    sbPager.Append (CreatePageUrl (BaseUrl, x ) );
			    sbPager.Append ("\" alt=\"Go to page\" title=\"Go to page\">");
			    sbPager.Append (x.ToString());
			    sbPager.Append ("</a> " );
			    sbPager.Append ("</td>");
		    }
	    }
	    if (!objPds.IsLastPage)
	    {
		    sbPager.Append("<td>");
		    if ((objPds.CurrentPageIndex + 2) != objPds.PageCount)
		    {
			    // next page link
			    sbPager.Append ("&nbsp;<a href=\"");
			    sbPager.Append (CreatePageUrl (BaseUrl, objPds.CurrentPageIndex + 2));
			    sbPager.Append ("\" alt=\"Next Page\" title=\"Next Page\">&raquo;</a> ");
		    }
		    // last page link
		    sbPager.Append ("&nbsp;<a href=\"");
		    sbPager.Append (CreatePageUrl (BaseUrl,objPds.PageCount ));
		    sbPager.Append ("\" alt=\"Last Page\" title=\"Last Page\">&gt;|</a>");
		    sbPager.Append ("</td>");
	    }
	    else
	    {
		    if (objPds.PageCount == 1) sbPager.Append ("<td> of 1</td>");
	    }
	    // convert the final links to a string and assign to labels
	    return "<table cellpadding=0 cellspacing=1 border=0><tr>" + sbPager1.ToString() + "</tr><tr>" + sbPager.ToString() + "</tr></table>";
    }
</script>
<html>
  <head>
    <title>Searcharoo.Net Version 4</title>
    <meta http-equiv="robots" content="none">
    <style type="text/css">
		body {margin:10px 10px 10px 10px;background-color:white;}
		.heading {font-size:xx-large;font-weight:bold;color:darkgrey;filter:DropShadow (Color=#cccccc, OffX=5, OffY=5, Positive=true);}
		.copyright {font-size:xx-small;}
		body, td, a {font-family:trebuchet ms, verdana, arial, sans-serif;font-size:small;}
		.subheading {font-size:large;font-weight:bold;color:darkgrey;}
	</style>
</head>
   <body>
		<roo:SearchPanel id="ucSearchPanelHeader" runat="server" IsSearchResultsPage="false" />
		
		<asp:Panel id="lblNoSearchResults" visible="false" runat="server">
			Your search - <b><%=_SearchTerm%></b> - did not match any documents. 
			<br><br>
			It took <%=_DisplayTime%>.

			<p>Suggestions:</p>
			<ul>
			<li>Check your spelling</li>
			<li>Try similar meaning words (synonyms)</li>
			<li>Try fewer keywords: <%=_Matches%></li>
			</ul>
		</asp:Panel>
		
		<asp:Repeater id="SearchResults" runat="server">
		    <HeaderTemplate>
			    <p><%=_NumberOfMatches%> results for <%=_Matches%> took <%=_DisplayTime%></p>
		    </HeaderTemplate>
		    <ItemTemplate>
			    <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>"><b><%# DataBinder.Eval(Container.DataItem, "Title") %></b></a>
			    <!--(infile.Title==""?"&laquo; no title &raquo;":infile.Title)-->
			    <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>" target="_blank" title="open in new window" style="font-size:x-small">&uarr;</a>
			    <font color=gray>(<%# DataBinder.Eval(Container.DataItem, "Rank") %>)</font>
			    <br><%# DataBinder.Eval(Container.DataItem, "Description") %>...
			    <br><font color=green><%# DataBinder.Eval(Container.DataItem, "Url") %> - <%# DataBinder.Eval(Container.DataItem, "Size") %>
			    bytes</font>
			    <font color=gray>- <%# DataBinder.Eval(Container.DataItem, "CrawledDate") %></font><p>
		    </ItemTemplate>
		    <FooterTemplate>
			    <p><%=CreatePagerLinks(_PagedResults, Request.Url.ToString() )%></p>
		    </FooterTemplate>
		</asp:Repeater>

		<roo:SearchPanel id="ucSearchPanelFooter" runat="server" visible="false" IsSearchResultsPage="true" IsFooter="true"/>
		
	</body>
</html>
