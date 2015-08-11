<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EditStrings.aspx.cs" Inherits="AspNetDating.Admin.EditStrings" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
	<div class="panel-heading"><h4 class="panel-title"><%= Lang.TransA("Languages") %></h4></div>
	<div class="panel-body">
        <div class="form-horizontal">
            <div class="medium-width">
                <div class="form-group" id="pnlLanguage" runat="server">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Language") %>:</label>
                    <div class="col-sm-9">
                        <asp:dropdownlist CssClass="form-control" id="ddLanguage" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
                            <asp:ListItem/>
                        </asp:dropdownlist>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-3 control-label"><%= Lang.TransA("Location") %>:</label>
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control" ID="ddTranslationType" runat="server" AutoPostBack="true" onselectedindexchanged="ddTranslationType_SelectedIndexChanged"/>
                    </div>
                </div>
            </div>
         </div>
    </div>
</div>
<asp:datagrid id="dgTranslations" Runat="server" CssClass="table table-striped" AutoGenerateColumns="False" AllowPaging="False" GridLines="None">
    <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
	<Columns>
		<asp:TemplateColumn>
			<ItemTemplate>
                <span data-keytext="1"></span>
                <input type="hidden" value='<%# ((string)Eval("Key")).Replace("'", "&#039;") %>' data-key="1" />
			</ItemTemplate>
		</asp:TemplateColumn>
		<asp:TemplateColumn>
		    <ItemStyle CssClass="pointer" Width="50%"></ItemStyle>
			<ItemTemplate>
                <div data-translation="1"><%# HttpUtility.HtmlEncode((string)Eval("Value")) %></div>
			</ItemTemplate>
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>

<script type="text/javascript">
    $(function () {
        $("[data-keytext]").each(function () { $(this).text($(this).siblings('[data-key]').val()); });
        $("[data-translation]").editInPlace({
            url: '<%= string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port == -1 ? "" : (":"+Request.Url.Port), Request.ApplicationPath.TrimEnd('/')) + "/Admin/Translate.ashx" %>',
            text_size: 70,
            params: 'languageId=<%= ddLanguage.SelectedValue %>&adminPanel=<%= ddTranslationType.SelectedValue %>',
            validate_input: function (dom, newValue) {
                var keyVal = dom.parent().prev().find('[data-key]').val();
                for (var i = 0; i < 5; ++i) {
                    if (keyVal.indexOf("{" + i + "}") != -1 &&
                        newValue.indexOf("{" + i + "}") == -1) {
                        alert('<%= Lang.TransA("You have missed a placeholder in your translation") %>');
                        return false;
                    }
                }

                return true;
            },
            dynamic_params: function (dom) { return "keyValue=" + encodeURIComponent(dom.parent().prev().find('[data-key]').val()); }
        });
    });
</script>
</asp:Content>
