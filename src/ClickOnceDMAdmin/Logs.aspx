<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="ClickOnceDMAdmin.Logs" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1><i class="glyphicon glyphicon-floppy-disk"></i> <asp:Literal runat="server" Text="<%$Resources:Label_Logs%>" /></h1>
    </div>

    <div class="row">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>#</th>
                    <th><asp:Literal runat="server" Text="<%$Resources:Label_SenderName%>" /></th>
                    <th><asp:Literal runat="server" Text="<%$Resources:Label_SenderAddress%>" /></th>
                    <th><asp:Literal runat="server" Text="<%$Resources:Label_Subject%>" /></th>
                    <th><asp:Literal runat="server" Text="<%$Resources:Label_Timestamp%>" /></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptLog" OnItemCommand="rptLog_ItemCommand" OnItemDataBound="rptLog_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td><a href="./Default.aspx?id=<%# Eval("Id") %>"><%# Eval("Id") %></a></td>
                            <td><%# Eval("SenderName") %></td>
                            <td><%# Eval("SenderAddress") %></td>
                            <td><%# Eval("Subject") %></td>
                            <td><%# new DateTime(Convert.ToInt64(Eval("Timestamp"))) %></td>
                            <td><asp:Button ID="btnDelete" runat="server" Text="<%$Resources:Button_Delete%>" CssClass="btn btn-danger btn-xs" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <script>
        $(document).ready(function () {
            $("ul.nav.navbar-nav li:nth-child(2)").addClass("active");
        });
    </script>
</asp:Content>
