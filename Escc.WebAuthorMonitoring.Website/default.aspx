﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.DefaultPage" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Escc.WebAuthorMonitoring" %>
<%@ Import Namespace="eastsussexgovuk.webservices.TextXhtml.HouseStyle" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Egms:MetadataControl runat="server"
        Title="Problems reported to web authors"
        DateCreated="2013-07-03"
        IpsvPreferredTerms="Internet"
        IsInSearch="False"
    />
    <Egms:Css runat="server" Files="FormsSmall" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <Egms:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <Egms:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <article>
            <div class="text">
                <h1>Problems reported to web authors</h1>
                <asp:ValidationSummary runat="server"/>
            </div>
                
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
            
            <div class="text">
                <asp:Repeater runat="server" ID="table">
                    <HeaderTemplate>
                        <table>
                            <thead><tr><th>To</th><th>Subject</th><th>Date</th></tr></thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr><td><%# HttpUtility.HtmlEncode(((ProblemReport)Container.DataItem).WebAuthorPermissionsGroupName) %> web authors</td>
                            <td><a href="<%# HttpUtility.HtmlEncode(String.Format(CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Escc.WebAuthorMonitoring.ViewReportUrl"], ((ProblemReport)Container.DataItem).ProblemReportId)) %>"><%# HttpUtility.HtmlEncode(((ProblemReport)Container.DataItem).SubjectLine()) %></a></td>
                            <td><%# HttpUtility.HtmlEncode(DateTimeFormatter.ShortBritishDate(((ProblemReport)Container.DataItem).ReportDate))%></td></tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                
                <p id="noneFound" runat="server" Visible="False">Sorry, no reports matched your search.</p>
            </div>
        </article>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />