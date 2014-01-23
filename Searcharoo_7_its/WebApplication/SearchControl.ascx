<%@ Control Language="c#" AutoEventWireup="true" Inherits="Searcharoo.WebApplication.SearchControlBase" %>
<%@ Import Namespace="Searcharoo.Common" %>
<script runat="server">

</script>
<%--
Panel that is visible when the search page is first visited
--%>
<asp:Panel id="pnlHomeSearch" runat="server">
<form method="get" action="Search.aspx" style="margin:0px;padding:0px;">
<center>
<p class="heading"><font color=darkgray>Search</font><font color=red>a</font><font color=blue>r</font><font color=green>o</font><font color=orange>o</font> <font color=darkgray><sup>7</sup></font></p>
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
		<td><p class="copyright">&copy; 2004-2009 ConceptDevelopment.Net - Searching <%=WordCount%> words</p></td>
	</tr>
</table></center></form>
</asp:Panel>
<%--
Panel that is visible when search results are being shown
--%>
<asp:Panel id="pnlResultsSearch" runat="server">
<form method="get" id="bottom" action="Search.aspx" style="margin:0px;padding:0px;">
        <center>
        <p class="heading" id="pHeading" runat="server"><font color=darkgray>Search</font><font color=red>a</font><font color=blue>r</font><font color=green>o</font><font color=orange>o</font> <font color=darkgray><sup>7</sup></font></p>
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
            <tr id="rowFooter2" visible="false" runat="server"><td><p class="copyright">&copy; 2004 -2009<a href="http://www.conceptdevelopment.net/">ConceptDevelopment.Net</a> - Searching <%=WordCount%> words</p></td></tr>
        </table>
        </center>
    </form>
</asp:Panel>