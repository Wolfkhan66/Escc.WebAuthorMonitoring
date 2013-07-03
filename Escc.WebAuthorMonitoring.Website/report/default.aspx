<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.report.DefaultPage" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Egms:MetadataControl runat="server"
        Title="Report a problem to web authors"
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
    <div class="full-page" runat="server" id="container">
        <article>
            <div class="text">
                <h1>Report a problem to web authors</h1>
                <div class="form service-form">
                    <asp:ValidationSummary runat="server"/>
                    <div class="formBox">
                        <p class="read-only">
                            <span class="formLabel">To</span>
                            <ul runat="server" id="toList"></ul>
                        </p>

                        <p class="read-only">
                            <span class="formLabel">Regarding page</span>
                            <a href="" id="regardingPage" runat="server"></a>
                        </p>

                        <fieldset class="formPart">
                            <legend class="formLabel">Subject</legend>
                            <asp:CheckBoxList runat="server" ID="problemTypes" CssClass="radioButtonList problem-types" RepeatDirection="Horizontal" RepeatLayout="Flow"/>
                        </fieldset>
                        
                        <div class="formPart">
                            <asp:Label runat="server" AssociatedControlID="message">Message</asp:Label>
                            <asp:TextBox runat="server" ID="message" TextMode="MultiLine" CssClass="message" />
                        </div>
                    </div>
                    
                    <asp:Button runat="server" Text="Report it"/>
                </div>
            </div>
        </article>
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="supporting" />

<asp:Content runat="server" ContentPlaceHolderID="javascript">
    <script src="default.js"></script>
</asp:Content>