<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="ClickOnceDMAdmin.Logs" MasterPageFile="~/Master/Main.Master" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1>
            <asp:Literal runat="server" Text="<%$Resources:Label_Logs%>" /></h1>
    </div>

    <div class="row">
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Sender Name</th>
                    <th>Sender Address</th>
                    <th>Subject</th>
                    <th>Timestamp</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptLog">
                    <ItemTemplate>
                        <tr>
                            <td><a href="./Default.aspx?id=<%# Eval("Id") %>"><%# Eval("Id") %></a></td>
                            <td><%# Eval("SenderName") %></td>
                            <td><%# Eval("SenderAddress") %></td>
                            <td><%# Eval("Subject") %></td>
                            <td><%# Eval("Timestamp") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
</asp:Content>
