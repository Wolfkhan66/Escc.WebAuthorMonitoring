<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getReport.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.getReport" %>

<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="Escc.WebAuthorMonitoring" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl runat="server"
        Title="get Report"
        DateCreated="2017-02-07"
        IpsvPreferredTerms="Internet"
        IsInSearch="False" />
    <EastSussexGovUK:ContextContainer runat="server" Desktop="true">
        <ClientDependency:Css runat="server" Files="FormsSmall" />
        <ClientDependency:Css runat="server" Files="FormsMedium" MediaConfiguration="Medium" />
        <ClientDependency:Css runat="server" Files="FormsLarge" MediaConfiguration="Large" />
    </EastSussexGovUK:ContextContainer>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <article>
            <div class="content text-content">
                <h1>Report a problem to web authors</h1>
                <asp:ValidationSummary runat="server" />

                <div class="form short-form" id="cms-search">
                    <asp:Label runat="server" AssociatedControlID="url">Channel or page url:</asp:Label>
                    <asp:TextBox ID="url" runat="server" CssClass="url" />
                    <Validators:UrlValidator runat="server" ControlToValidate="url" UriKind="RelativeOrAbsolute" ErrorMessage="Please enter a valid URL for the channel or page" />
                    <div>
                        <asp:Button ID="submit" runat="server" Text="Search" CssClass="button" /></div>
                </div>
            </div>
        </article>
    </div>
</asp:Content>
