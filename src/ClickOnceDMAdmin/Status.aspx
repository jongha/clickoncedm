<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="ClickOnceDMAdmin.Status" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header"><h1><i class="glyphicon glyphicon-search"></i> <asp:Literal runat="server" Text="<%$Resources:Label_Status%>" /></h1></div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_TicketCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litTicketsCount" runat="server"></asp:Literal></strong></div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_QueueCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litQueueCount" runat="server"></asp:Literal></strong></div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_LogCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litLogCount" runat="server"></asp:Literal></strong></div>
    <div>
        <asp:Button ID="btnClearLog" runat="server" CssClass="btn btn-danger" Visible="false" Text="<%$Resources:Button_ClearLogs%>" OnClick="btnClearLog_Click" />
    </div>
    <script>
        $(document).ready(function () {
            $("ul.nav.navbar-nav li:nth-child(3)").addClass("active");
        });
    </script>
</asp:Content>