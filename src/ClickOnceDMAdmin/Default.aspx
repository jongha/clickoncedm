<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClickOnceDMAdmin.Default"
    MasterPageFile="~/Master/Main.Master" ValidateRequest="false" UICulture="auto" %>

<asp:Content ID="content" ContentPlaceHolderID="mainContent" runat="server">
    <div class="page-header">
        <h1><i class="glyphicon glyphicon-envelope"></i> <asp:Literal runat="server" Text="<%$Resources:Label_SendMail%>" /></h1>
    </div>

    <div class="row">
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSubject.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_Title%>" /></label>
            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="<%$Resources:Label_Title%>" />
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSenderName.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_SenderName%>" /></label>
            <asp:TextBox ID="txtSenderName" runat="server" CssClass="form-control" placeholder="<%$Resources:Label_SenderName%>" />
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="<%=txtSenderAddress.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_SenderAddress%>" /></label>
            <asp:TextBox ID="txtSenderAddress" runat="server" CssClass="form-control" placeholder="<%$Resources:Label_SenderAddressPlaceHolder%>" />
            <p class="text-danger"><i class="glyphicon glyphicon-warning-sign"></i> <asp:Literal runat="server" Text="<%$Resources:Label_SenderAddressDesc%>" /></p>
        </div>
        <div class="form-group col-md-12 col-xs-12 recipient-manual recipient-selection" data-type="manual">
            <label for="<%=txtRecipientAddress.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_RecipientAddress%>" /></label>
            <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control" placeholder="<%$Resources:Label_RecipientAddressPlaceHolder%>" />
        </div>
        <div class="form-group col-md-12 col-xs-12 recipient-preset recipient-selection" data-type="preset">
            <label for="<%=rdoRecipeints.ClientID %>"><asp:Literal runat="server" Text="<%$Resources:Label_Recipients%>" /></label>
            <div><asp:RadioButtonList runat="server" ID="rdoRecipeints" RepeatLayout="Flow"></asp:RadioButtonList></div>
        </div>
        <div class="form-group col-md-12 col-xs-12">
            <label for="divHTML">Body</label>
            <ul class="nav nav-tabs mt5">
                <li class="active" id="tabHTML"><a href="#"><asp:Literal runat="server" Text="<%$Resources:Label_HTML%>" /></a></li>
                <li id="tabPreview"><a href="#"><asp:Literal runat="server" Text="<%$Resources:Label_Preview%>" /></a></li>
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
            <asp:Button ID="btnSave" runat="server" Text="<%$Resources:Button_SendMail%>" CssClass="btn btn-success"
                OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="<%$Resources:Button_Cancel%>"
                CssClass="btn btn-warning" OnClick="btnCancel_Click" />
        </div>
    </div>
    <script type="text/javascript">
        function checkSendValidation() {
            if ($("#<%=txtSubject.ClientID%>").val().trim() === "") {
                alert('<asp:Literal runat="server" Text="<%$Resources:Message_SubjectEmpty%>" />');
                $("#<%=txtSubject.ClientID%>").focus();

            } else if ($("#<%=txtSenderName.ClientID%>").val().trim() === "") {
                alert('<asp:Literal runat="server" Text="<%$Resources:Message_SenderNameEmpty%>" />');
                $("#<%=txtSenderName.ClientID%>").focus();

            } else if ($("#<%=txtSenderAddress.ClientID%>").val().trim() === "") {
                alert('<asp:Literal runat="server" Text="<%$Resources:Message_SenderAddressEmpty%>" />');
                $("#<%=txtSenderAddress.ClientID%>").focus();

            } else if ($("#<%=txtRecipientAddress.ClientID%>").val().trim() === "" &&
                $(".recipient-preset").find("input:checked").length == 0) {

                alert('<asp:Literal runat="server" Text="<%$Resources:Message_RecipientAddressEmpty%>" />');
                $("#<%=txtRecipientAddress.ClientID%>").click().focus();

            } else {
                if (confirm('<asp:Literal runat="server" Text="<%$Resources:Message_SendMailConfirm%>" />')) {
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

            var selectRecipient = function (type) {
                if (type === "preset") {
                    $(".recipient-preset").fadeTo("fast", 1);
                    $(".recipient-manual").fadeTo("fast", 0.3);
                } else {
                    $(".recipient-preset").fadeTo("fast", 0.3).find("input").prop("checked", false);
                    $(".recipient-manual").fadeTo("fast", 1);
                }

                if (!!!this.binded) {
                    $(".recipient-selection").bind("click", function () {
                        selectRecipient($(this).data("type"));
                    });

                    this.binded = true;
                }
            };

            selectRecipient();

            $("ul.nav.navbar-nav li:nth-child(1)").addClass("active");
        });
    </script>
</asp:Content>
