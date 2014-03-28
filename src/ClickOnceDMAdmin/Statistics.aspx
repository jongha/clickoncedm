<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="ClickOnceDMAdmin.Statistics" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header"><h1><i class="glyphicon glyphicon-list-alt"></i> <asp:Literal runat="server" Text="<%$Resources:Label_Statistics%>" /></h1></div>
    <div class="form-group">
        <div class="form-group">
            <label for="<%=txtQuery.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_Query%>" /></label>
            <asp:TextBox ID="txtQuery" CssClass="form-control" placeholder="<%$Resources:Label_QueryDesc%>" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSearch" CssClass="btn btn-danger btn-sm" Text="<%$Resources:Button_Search%>" runat="server" OnClick="btnSearch_Click" />
        </div>
    </div>
    <div class="well">
        <asp:Literal runat="server" Text="<%$Resources:Label_Success%>" />: <strong><asp:Literal ID="litSuccess" runat="server">0</asp:Literal></strong>,
        <asp:Literal runat="server" Text="<%$Resources:Label_Error%>" />: <strong><asp:Literal ID="litError" runat="server">0</asp:Literal></strong>,
        <asp:Literal runat="server" Text="<%$Resources:Label_Total%>" />: <strong><asp:Literal ID="litTotal" runat="server">0</asp:Literal></strong>
    </div>
    <table class="table table-striped">
        <thead>
            <tr>
                <th>#</th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Subject%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Success%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Error%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Total%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Timestamp%>" /></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptStatistics" runat="server" OnItemCommand="rptStatistics_ItemCommand" OnItemDataBound="rptStatistics_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td><%=DataIndex %></td>
                        <td><%# Eval("Subject") %></td>
                        <td><%# Convert.ToInt64(Eval("Success")).ToString("###,###,##0") %></td>
                        <td><%# Convert.ToInt64(Eval("Error")).ToString("###,###,##0") %></td>
                        <td><%# (Convert.ToInt64(Eval("Success")) + Convert.ToInt64(Eval("Error"))).ToString("###,###,##0") %></td>
                        <td><%# TimeZoneInfo.ConvertTime(Convert.ToDateTime(Eval("Timestamp")), TimeZoneInfo.Utc, TimeZoneInfo.Local) %></td>
                        <td><asp:Button ID="btnDelete" runat="server" Text="<%$Resources:Button_Delete%>" CssClass="btn btn-danger btn-xs" /></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <script>
        $(document).ready(function () {
            $("ul.nav.navbar-nav li:nth-child(4)").addClass("active");
        });
    </script>
</asp:Content>