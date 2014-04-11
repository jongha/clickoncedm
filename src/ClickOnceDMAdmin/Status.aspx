<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="ClickOnceDMAdmin.Status" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header"><h1><i class="glyphicon glyphicon-search"></i> <asp:Literal runat="server" Text="<%$Resources:Label_Status%>" /></h1></div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_TicketCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litTicketsCount" runat="server"></asp:Literal></strong> <asp:Literal runat="server" Text="<%$Resources:Label_TicketCountDesc%>" /></div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_QueueCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litQueueCount" runat="server"></asp:Literal></strong> <asp:Literal runat="server" Text="<%$Resources:Label_QueueCountDesc%>" /></div>
    <div class="progress progress-striped active">
      <div id="progress" class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="<%=QueueProgressPercent %>" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
        <span><%=QueueProgressPercent %>% <asp:Literal runat="server" Text="<%$Resources:Label_Complete%>"></asp:Literal></span>
      </div>
    </div>
    <h3><asp:Literal runat="server" Text="<%$Resources:Label_LogCount%>" /></h3>
    <div class="alert alert-success"><strong><asp:Literal ID="litLogCount" runat="server"></asp:Literal></strong> <asp:Literal runat="server" Text="<%$Resources:Label_LogCountDesc%>" /></div>
    <div>
        <asp:Button ID="btnClearLog" runat="server" CssClass="btn btn-danger" Visible="false" Text="<%$Resources:Button_ClearLogs%>" OnClick="btnClearLog_Click" />
        <button id="refresh" class="btn btn-success" title="Refresh"><i class="glyphicon glyphicon-refresh"></i></button>
    </div>
    <script>
        $(document).ready(function () {
            var percent = "<%=QueueProgressPercent %>";
            if (percent === 0) { percent = 1; }
            $("#progress").animate({
                width: percent + "%",
                easing: 'easeInExpo'
            }, {
                duration: 500,
                always: function () {
                    if (percent >= 100) {
                        $(this).parent().removeClass("active progress-striped");
                    }
                }
            });

            $("#refresh").bind("click", function (e) {
                e.preventDefault();
                location.href = "./Status.aspx";
            })
            $("ul.nav.navbar-nav li:nth-child(3)").addClass("active");
        });
    </script>
</asp:Content>