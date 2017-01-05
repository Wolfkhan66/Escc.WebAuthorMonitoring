<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.DefaultPage" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Escc.WebAuthorMonitoring" %>
<%@ Import Namespace="Escc.Dates" %>
<asp:content runat="server" contentplaceholderid="metadata">
    <Metadata:MetadataControl runat="server"
        Title="Problems reported to web authors"
        DateCreated="2013-07-03"
        IpsvPreferredTerms="Internet"
        IsInSearch="False"
    />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="FormsSmall" />
        <ClientDependency:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:content>

<asp:content runat="server" contentplaceholderid="content">
    <div class="full-page">
        <article>
            <div class="content text-content">
                <h1>Problems reported to web authors</h1>
                <asp:ValidationSummary runat="server"/>
                
                <div class="form short-form" id="cms-search">
	                <asp:label runat="server" AssociatedControlID="url">Channel or page:</asp:label>
	                <asp:Textbox id="url" runat="server" CssClass="url"/>
                    <Validators:UrlValidator runat="server" ControlToValidate="url" UriKind="RelativeOrAbsolute" ErrorMessage="Please enter a valid URL for the channel or page" />
                
	                <asp:label runat="server" AssociatedControlID="webAuthor">Web author:</asp:label>
	                <asp:Textbox id="webAuthor" runat="server"/>

                    <asp:Label runat="server" AssociatedControlID="from" Text="From date:" />
                    <asp:TextBox runat="server" ID="from" type="date" />
                    <asp:RangeValidator runat="server" ControlToValidate="from" ErrorMessage="Please enter a valid start date" Type="Date" MinimumValue="1753-01-01" MaximumValue="9999-12-31" />
                    <asp:RangeValidator runat="server" ControlToValidate="from" ErrorMessage="We haven't had any reports sent back from the future" Type="Date" MinimumValue="0001-01-01" ID="future1" />

                    <asp:Label runat="server" AssociatedControlID="to" Text="To date:" />
                    <asp:TextBox runat="server" ID="to" type="date" />
                    <asp:RangeValidator runat="server" ControlToValidate="to" ErrorMessage="Please enter a valid end date" Type="Date" MinimumValue="1753-01-01" MaximumValue="9999-12-31" />

	                <div><asp:button id="submit" runat="server" text="Search" CssClass="button" /></div>
                </div>

                <NavigationControls:PagingController id="paging" runat="server" ResultsTextSingular="result" ResultsTextPlural="results" />
                <NavigationControls:PagingBarControl id="pagingTop" runat="server" PagingControllerId="paging" />
            
                <asp:Repeater runat="server" ID="table">
                    <HeaderTemplate>
                        <table>
                            <thead><tr><th>To</th><th>Subject</th><th>Date</th></tr></thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr><td><%# HttpUtility.HtmlEncode(((ProblemReport)Container.DataItem).WebAuthorPermissionsGroupName) %> </td>
                            <td><a href="<%# HttpUtility.HtmlEncode(String.Format(CultureInfo.InvariantCulture, ResolveUrl("~/view.aspx?report={0}"), ((ProblemReport)Container.DataItem).ProblemReportId)) %>"><%# HttpUtility.HtmlEncode(((ProblemReport)Container.DataItem).SubjectLine()) %></a></td>
                            <td><%# HttpUtility.HtmlEncode(((ProblemReport)Container.DataItem).ReportDate.ToShortBritishDate())%></td></tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <NavigationControls:PagingBarControl id="pagingBottom" runat="server" PagingControllerId="paging" />

                 
                <p id="noneFound" runat="server" Visible="False">Sorry, no reports matched your search.</p>
            </div>
        </article>
    </div>
</asp:content>
