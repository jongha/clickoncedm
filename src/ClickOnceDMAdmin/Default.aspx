<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClickOnceDMAdmin.Default"
    MasterPageFile="~/Master/Main.Master" ValidateRequest="false" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1>Send Mail</h1>
    </div>

    <div class="row">
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSubject.ClientID %>">Title</label>
            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Title" />
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSenderName.ClientID %>">Sender Name</label>
            <asp:TextBox ID="txtSenderName" runat="server" CssClass="form-control" placeholder="Sender Name" />
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSenderAddress.ClientID %>">Sender Address</label>
            <asp:TextBox ID="txtSenderAddress" runat="server" CssClass="form-control" placeholder="Sender Address (user@domain)" />
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=rdoRecipeints.ClientID %>">Recipients</label>
            <asp:RadioButtonList runat="server" ID="rdoRecipeints" CssClass="radio" RepeatLayout="Flow"></asp:RadioButtonList>
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="divHTML">Body</label>
            <ul class="nav nav-tabs mt5">
                <li class="active" id="tabHTML"><a href="#">HTML</a></li>
                <li id="tabPreview"><a href="#">Preview</a></li>
            </ul>
            <div class="mt5">
                <div id="divHTML">
                    <asp:TextBox ID="txtHtml" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="30" /></div>
                <div id="divPreview" style="display: none">
                    <iframe id="frmPreview" class="form-control" style="height: 600px"></iframe>
                </div>
            </div>
        </div>
        <div class="text-center">
            <asp:Button ID="btnSave" runat="server" Text="Send Mail" CssClass="btn btn-success"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                CssClass="btn btn-warning" OnClick="btnCancel_Click" />
        </div>
    </div>
    <script type="text/javascript">
        function checkSendValidation() {
            if ($("#<%=txtSubject.ClientID%>").val().trim() === "") {
                alert("The message must have title.");
                $("#<%=txtSubject.ClientID%>").focus();

            } else if ($("#<%=txtSenderName.ClientID%>").val().trim() === "") {
                alert("The message must have sender name.");
                $("#<%=txtSenderName.ClientID%>").focus();

            } else if ($("#<%=txtSenderAddress.ClientID%>").val().trim() === "") {
                alert("The message must have sender email address.");
                $("#<%=txtSenderAddress.ClientID%>").focus();

            } else {
                if (confirm("Are you sure?")) {
                    return true;
                }
            }
            return false;
        }

        $(document).ready(function () {
            $("#tabHTML").bind("click", function (e) {
                e.preventDefault();

                $(this).addClass("active");
                $("#divHTML").show();

                $("#tabPreview").removeClass("active");
                $("#divPreview").hide();
            });

            $("#tabPreview").bind("click", function (e) {
                e.preventDefault();

                var preview = "./Preview/Default.aspx?_=" + new Date().getTime();
                var _that = $(this);

                $.ajax({
                    type: "POST",
                    url: preview,
                    data: "action=save&html=" + encodeURIComponent($("#<%=txtHtml.ClientID %>").val()),
                    cache: false,
                    async: false,
                    success: function (data, textStatus, jqXHR) {
                        _that.addClass("active");

                        $("#frmPreview").attr({ "src": preview });
                        $("#divPreview").show();

                        $("#tabHTML").removeClass("active");
                        $("#divHTML").hide();
                    },
                    dataType: "html"
                });
            });
        });
    </script>
</asp:Content>
