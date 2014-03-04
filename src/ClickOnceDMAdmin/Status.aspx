<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="ClickOnceDMAdmin.Tickets" MasterPageFile="~/Master/Main.Master" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1>Status</h1>
    </div>

    <h3>Ticket Count</h3>
    <div class="alert alert-success">
        <strong>
            <asp:Literal ID="litTicketsCount" runat="server"></asp:Literal></strong>
    </div>

    <h3>Queue Count</h3>
    <div class="alert alert-success">
        <strong>
            <asp:Literal ID="litQueueCount" runat="server"></asp:Literal></strong>
    </div>
</asp:Content>
