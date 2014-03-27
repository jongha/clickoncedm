<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="ClickOnceDMAdmin.Statistics" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header"><h1><i class="glyphicon glyphicon-list-alt"></i> <asp:Literal runat="server" Text="<%$Resources:Label_Statistics%>" /></h1></div>
    <div class="form-group">
        <div class="form-group">
            <label for="<%=txtStartTime.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_StartDate%>" /></label>
            <div class="datetimepicker input-group date">
                <asp:TextBox data-format="YYYY/MM/DD HH:mm:ss" ID="txtStartTime" CssClass="form-control" placeholder="YYYY/MM/DD 24HH:MI:SS" MaxLength="19" runat="server"></asp:TextBox>
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
        <div class="form-group">
            <label for="<%=txtEndTime.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_EndDate%>" /></label>
            <div class="datetimepicker input-group date">
                <asp:TextBox data-format="YYYY/MM/DD HH:mm:ss" ID="txtEndTime" CssClass="form-control" placeholder="YYYY/MM/DD 24HH:MI:SS" MaxLength="19" runat="server"></asp:TextBox>
                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>
            </div>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSearch" CssClass="btn btn-danger btn-sm" Text="<%$Resources:Button_Search%>" runat="server" OnClick="btnSearch_Click" />
        </div>
    </div>

    <table class="table table-striped">
        <thead>
            <tr>
                <th>#</th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Success%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Error%>" /></th>
                <th><asp:Literal runat="server" Text="<%$Resources:Label_Total%>" /></th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptStatistics" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%=DataIndex %></td>
                        <td><%# Convert.ToInt64(Eval("Success")).ToString("###,###,##0") %></td>
                        <td><%# Convert.ToInt64(Eval("Error")).ToString("###,###,##0") %></td>
                        <td><%# (Convert.ToInt64(Eval("Success")) + Convert.ToInt64(Eval("Error"))).ToString("###,###,##0") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    

    <script>
        $(document).ready(function () {
            $(function () {
                $('.datetimepicker').datetimepicker(
                    { format: 'YYYY/MM/DD HH:mm:ss' }
                );
            });

            $("ul.nav.navbar-nav li:nth-child(4)").addClass("active");
        });
    </script>
</asp:Content>