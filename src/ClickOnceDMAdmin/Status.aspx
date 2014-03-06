﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="ClickOnceDMAdmin.Tickets" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1><asp:Literal runat="server" Text="<%$Resources:Label_Status%>" /></h1>
    </div>

    <h3><asp:Literal runat="server" Text="<%$Resources:Label_TicketCount%>" /></h3>
    <div class="alert alert-success">
        <strong>
            <asp:Literal ID="litTicketsCount" runat="server"></asp:Literal></strong>
    </div>

    <h3><asp:Literal runat="server" Text="<%$Resources:Label_QueueCount%>" /></h3>
    <div class="alert alert-success">
        <strong>
            <asp:Literal ID="litQueueCount" runat="server"></asp:Literal></strong>
    </div>
</asp:Content>