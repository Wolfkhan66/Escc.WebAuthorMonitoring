﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="Escc.WebAuthorMonitoring.Website.view" %>
<asp:Content runat="server" ContentPlaceHolderID="metadata">
    <Metadata:MetadataControl runat="server"
        Title="View a problem problemReport"
        IpsvPreferredTerms="Internet"
        IsInSearch="False"
        DateCreated="2013-07-04"
        ID="headContent"
    />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="content">
    <div class="full-page">
        <article id="container" runat="server">
            <div class="content text-content">
                <h1 id="subject" runat="server"></h1>
                <dl>
                    <dt>To</dt>
                    <dd><ul id="webAuthors" runat="server"></ul></dd>
                    <dt>Date</dt>
                    <dd id="reportDate" runat="server"></dd>
                </dl>
                <asp:Literal runat="server" ID="messageHtml" />
            </div>
        </article>
    </div>
</asp:Content>