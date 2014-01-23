<%@ Page Language="c#" autoeventwireup="true" Inherits="Searcharoo.WebApplication.SearchPageBase"%>
<%@ import Namespace="System" %>
<%@ import Namespace="System.Xml.Serialization" %>
<%@ import Namespace="System.Collections.Specialized" %>
<%@ import Namespace="Searcharoo.Common" %>
<%@ Register TagPrefix="roo" TagName="SearchPanel" Src="SearchControl.ascx" %>
<script runat="server">
    /*
* (c) 2004-2008 Craig Dunn - ConceptDevelopment.NET
* v1 30-Jun-04
* v2 02-Jul-04
* v3 30-Mar-06
* v4 13-Mar-07
* v5 25-Apr-07
* v6    Jun-08
* 
* More info:
*    http://www.searcharoo.net/
*    http://www.conceptdevelopment.net/search/searcharooV1/
*    http://www.conceptdevelopment.net/search/searcharooV2/
*    http://www.conceptdevelopment.net/search/searcharooV3/
*    http://www.conceptdevelopment.net/search/searcharoo-V4/
*    http://searcharoo.net/SearcharooV5/
*/
    /// <summary>
    /// This method implements a 'rolling window' page-number index
    /// for the underlying PagedDataSource
    /// </summary>
    /// <remarks>
    /// http://www.sitepoint.com/article/asp-nets-pageddatasource
    /// http://www.uberasp.net/ArticlePrint.aspx?id=29
    /// 
    /// http://www.codeproject.com/KB/aspnet/Mastering_DataBinding.aspx
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
    <title>Searcharoo.Net Version 6</title>
    <meta http-equiv="robots" content="none">
    <style type="text/css">
		body {margin:10px 10px 10px 10px;background-color:white;}
		.heading {font-size:xx-large;font-weight:bold;color:darkgrey;filter:DropShadow (Color=#cccccc, OffX=5, OffY=5, Positive=true);}
		.copyright {font-size:xx-small;}
		body, td, a {font-family:trebuchet ms, verdana, arial, sans-serif;font-size:small;}
		.subheading {font-size:large;font-weight:bold;color:darkgrey;}
		.geo {font-size:xx-small; font-color: grey;}
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
			    <p><%=_NumberOfMatches%> results for <%=_Matches%> took <%=_DisplayTime%> (<%=_Geocoded%> geocoded
			    <% if (_Geocoded >0) {%><a href="/SearchKml/<%=Request["searchfor"]%>.kml">view in Google Earth</a><%} %>)</p>
		    </HeaderTemplate>
		    <ItemTemplate>
		        <font color="blue" size="-1"><asp:literal runat="server" 
		            Visible='<%# (string)DataBinder.Eval(Container.DataItem, "Extension") != "html" %>'
			        Text='<%# DataBinder.Eval(Container.DataItem, "Extension") %>' /></font>
			    <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>"><b><%# DataBinder.Eval(Container.DataItem, "Title") %></b></a>
			    <!--(infile.Title==""?"&laquo; no title &raquo;":infile.Title)-->
			    <a href="<%# DataBinder.Eval(Container.DataItem, "Url") %>" target="_blank" title="open in new window" style="font-size:x-small">&uarr;</a>
			    <font color=gray>(<%# DataBinder.Eval(Container.DataItem, "Rank") %>)</font>
			    <%# DataBinder.Eval(Container.DataItem, "GpsLocationHtml")%>
			    <br><%# DataBinder.Eval(Container.DataItem, "Description") %>...
			    <font color=brown><asp:literal ID="Literal1" runat="server" 
			        Visible='<%# (string)DataBinder.Eval(Container.DataItem, "KeywordString") != "" %>'
			        Text='<br />' /><asp:literal runat="server" 
			        Visible='<%# (string)DataBinder.Eval(Container.DataItem, "KeywordString") != "" %>'
			        Text='<%# DataBinder.Eval(Container.DataItem, "KeywordString") %>' /></font>
			    <br><font color=green><%# DataBinder.Eval(Container.DataItem, "Url") %> - <%# DataBinder.Eval(Container.DataItem, "Size") %>
			    bytes</font>
			    <font color=gray>- <%# DataBinder.Eval(Container.DataItem, "Extension") %> - <%# DataBinder.Eval(Container.DataItem, "CrawledDate") %></font><p>
		    </ItemTemplate>
		    <FooterTemplate>
			    <p><%=CreatePagerLinks(_PagedResults, Request.Url.ToString() )%></p>
		    </FooterTemplate>
		</asp:Repeater>

		<roo:SearchPanel id="ucSearchPanelFooter" runat="server" visible="false" IsSearchResultsPage="true" IsFooter="true"/>
		
	</body>
</html>
