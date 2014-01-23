<%@ Control Language="c#" AutoEventWireup="true"%>
<%@ Import Namespace="Searcharoo.Common" %>
<script runat="server">
/// <summary>Size of the searchable catalog (number of unique words)</summary>
public int WordCount = -1;

/// <summary>Word/s displayed in search input box</summary>
public string Word = "";

/// <summary>
/// Error message - on Home Page version ONLY
/// ie. ONLY when IsSearchResultsPage = true
/// </summary>
public string _ErrorMessage;

/// <summary>Whether the standalone home page version, or the on Search Results page</summary>
private bool _IsSearchResultsPage;

/// <summary>Whether the control is placed at the Header or Footer</summary>
protected bool _IsFooter;

/// <summary>
/// Value is either
///   false: being displayed on the 'home page' - only thing on the page
///   true:  on the Results page (at the top _and_ bottom)
/// <summary>
public bool IsSearchResultsPage
{
	get {return _IsSearchResultsPage;}
	set {
		_IsSearchResultsPage = value;
		if (_IsSearchResultsPage) 
		{
			pnlHomeSearch.Visible = false;
			pnlResultsSearch.Visible = true;
		}
		else
		{
			pnlHomeSearch.Visible = true;
			pnlResultsSearch.Visible = false;	
		}
	}
}
/// <summary>
/// Footer control has more 'display items' than the one shown
/// in the Header - setting this property shows/hides them
/// </summary>
public bool IsFooter
{
	set {
		_IsFooter = value;
		pHeading.Visible = !_IsFooter;
		rowFooter1.Visible = _IsFooter;
		rowFooter2.Visible = _IsFooter;
		rowSummary.Visible = !_IsFooter;
	}
}
/// <summary>
/// Error message to be displayed if search input box is empty
/// </summary>
public string ErrorMessage
{
	set {
		_ErrorMessage = value;
	}
}
/// <summary>
/// Nothing actually happens on the User Control Page_Load () 
/// ... for now
/// <summary>
protected void Page_Load (object sender, EventArgs ea) 
{	
}

/// <summary>
/// Was originally used in Searcharoo3.aspx to generate the top and bottom 
/// search boxes from a single User Control 'instance'. Decided not to use
/// that approach - but left this in for reference.
/// <summary>
[Obsolete("Render control to string to embed mulitple times in a page; but no longer required.")]
public override string ToString() 
{
	System.IO.StringWriter writer = new System.IO.StringWriter();
	System.Web.UI.HtmlTextWriter buffer = new System.Web.UI.HtmlTextWriter(writer);
	this.Render(buffer);
	return writer.ToString();
}
</script>
<%--
Panel that is visible when the search page is first visited
--%>
<asp:Panel id="pnlHomeSearch" runat="server">
<form method="get" action="Search.aspx" style="margin:0px;padding:0px;">
<center>
<p class="heading"><font color=darkgray>Search</font><font color=red>a</font><font color=blue>r</font><font color=green>o</font><font color=orange>o</font> <font color=darkgray><sup>4</sup></font></p>
<table align="center" cellspacing="0" cellpadding="4" frame="box" bordercolor="#dcdcdc" bgcolor="lightyellow" rules="none" style="BORDER-COLLAPSE: collapse">
    <tr>
        <td>
        <p class="intro">Search for ...<br><font color=red><%=_ErrorMessage%></font>
            <input name="<%=Preferences.QuerystringParameterName%>" id="<%=Preferences.QuerystringParameterName%>1" size="40" value="<%=Word%>" /> 
        </p>
        </td>
    </tr>
    <tr>
		<td align="center"><input type="submit" value="Searcharoo!" class="button" /></td>
	</tr>
    <tr>
		<td><a href="http://www.searcharoo.net/">Searcharoo.Net</a> - <a href="http://www.conceptdevelopment.net/">ConceptDevelopment.Net</a></td>
	</tr>
    <tr>
		<td><p class="copyright">&copy; 2007 ConceptDevelopment.Net - Searching <%=WordCount%> words</p></td>
	</tr>
</table></center></form>
</asp:Panel>
<%--
Panel that is visible when search results are being shown
--%>
<asp:Panel id="pnlResultsSearch" runat="server">
<form method="get" id="bottom" action="Search.aspx" style="margin:0px;padding:0px;">
        <center>
        <p class="heading" id="pHeading" runat="server"><font color=darkgray>Search</font><font color=red>a</font><font color=blue>r</font><font color=green>o</font><font color=orange>o</font> <font color=darkgray><sup>4</sup></font></p>
        <table cellspacing=0 cellpadding=4 frame=box bordercolor=#dcdcdc rules=none style="BORDER-COLLAPSE: collapse" width="100%" bgcolor="#E1FFFF">
            <tr>
                <td>
                <p>Search for :
                    <input type="text" name="<%=Preferences.QuerystringParameterName%>" id="<%=Preferences.QuerystringParameterName%>2" width="400" value="<%=Word%>" />
                    <input type="submit" value="Searcharoo!" class="button" />
                </p>
                </td>
            </tr>
			<tr id="rowSummary" visible="true" runat="server"><td><p class="copyright">Searching <%=WordCount%> words</p></td></tr>
            <tr id="rowFooter1" visible="false" runat="server"><td><a href="http://www.searcharoo.net/">Searcharoo.Net</a> - <a href="http://www.conceptdevelopment.net/">ConceptDevelopment.Net</a></td></tr>
            <tr id="rowFooter2" visible="false" runat="server"><td><p class="copyright">&copy;2006 <a href="http://www.conceptdevelopment.net/">ConceptDevelopment.Net</a> - Searching <%=WordCount%> words</p></td></tr>
        </table>
        </center>
    </form>
</asp:Panel>