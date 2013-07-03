<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.DefaultPage" %>
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
            </div>
        </article>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />