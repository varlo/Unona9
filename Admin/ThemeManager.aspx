<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ThemeManager.aspx.cs" Inherits="AspNetDating.Admin.ThemeManager" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel clear-panel">
    <div class="panel-heading">
        <h4 class="panel-title"><%= "Themes".TranslateA() %></h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">
            <asp:DataList ID="dlThemes" RepeatDirection="Horizontal" RepeatColumns="3" CssClass="table table-striped" runat="server" OnItemCommand="dlThemes_ItemCommand" GridLines="None" OnItemDataBound="dlThemes_ItemDataBound">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <h4><%# Eval("Name") %></h4>
                    <p>
                    <asp:Repeater ID="rptPreview" DataSource='<%# Eval("Previews") %>' runat="server">
                        <ItemTemplate>
                            <a href='<%# Eval("Value") %>'><img class="img-thumbnail" src='<%# Eval("Key") %>' /></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    </p>
                    <div id="Div1" runat="server" visible='<%# Config.Misc.SiteTheme != (string) Eval("Name") %>'>
                        <div class="btn-group btn-group-sm">
                            <a class="btn btn-default" href='<%# Config.Urls.Home + "?theme=" + Eval("Name") %>' target="preview"><i class="fa fa-eye"></i>&nbsp;preview</a>
                            <asp:LinkButton CssClass="btn btn-secondary" ID="btnSelect" CommandName="Select" CommandArgument='<%# Eval("Name") %>' runat="server"><i class="fa fa-check-square-o"></i>&nbsp;select</asp:LinkButton>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</div>
<div class="panel clear-panel">
    <div class="panel-heading">
        <h4 class="panel-title"><%= "Themes Online".TranslateA() %></h4>
    </div>
    <div class="panel-body">
        <div class="table-responsive">
            <asp:DataList ID="dlOnlineThemes" RepeatDirection="Horizontal" RepeatColumns="3" runat="server" OnItemCommand="dlOnlineThemes_ItemCommand" GridLines="None" CssClass="table table-striped" OnItemDataBound="dlOnlineThemes_ItemDataBound">
            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <h4><%# Eval("Name") %></h4>
                    <small><%# Eval("Description") %></small>
                    <p>
                    <asp:Repeater ID="rptPreview" DataSource='<%# Eval("Previews") %>' runat="server">
                        <ItemTemplate>
                            <a href='<%# Eval("ImageUrl") %>'><img class="img-thumbnail" src='<%# Eval("ThumbnailUrl") %>' /></a>
                        </ItemTemplate>
                    </asp:Repeater>
                    </p>
                    <div class="btn-group btn-group-sm">
                        <asp:LinkButton CssClass="btn btn-primary" ID="btnDownload" CommandName="Download" CommandArgument='<%# Eval("Name") %>' Visible='<%# Eval("DownloadUrl") != null %>' runat="server"><i class="fa fa-download"></i>&nbsp;download</asp:LinkButton>
                        <asp:HyperLink CssClass="btn btn-primary" ID="lnkPurchase" NavigateUrl='<%# Eval("PurchaseUrl") %>' Visible='<%# Eval("PurchaseUrl") != null %>' Target="_blank" runat="server"><i class="fa fa-shopping-cart"></i>&nbsp;purchase</asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
</div>
<div class="panel clear-panel">
    <div class="panel-heading">
        <h4 class="panel-title"><%= "Custom Themes".TranslateA() %></h4>
    </div>
    <div class="panel-body">
        <div class="medium-width">
            <p>Our designers will create a unique theme for your site. The new design can be based on your specifications, a mockup or existing design.</p>
            <p>The service includes custom theme based on your requirements. The custom theme service does not include programming changes or adding new features.</p>
            <div class="actions">
                <a class="btn btn-secondary btn-lg" href="http://sites.fastspring.com/aspnetdating/product/customdesign" target="_blank"><i class="fa fa-shopping-cart"></i>&nbsp;Order custom theme for AspNetDating</a>
            </div>
        </div>
        <div class="medium-width">
            <hr />
            If you've already received your theme you can upload and install it below:
            <div class="input-group">
                <span class="input-group-addon"><%= "Theme:".TranslateA() %></span>
                <p class="form-control-static"><asp:FileUpload ID="fileUploadTheme" runat="server" /></p>
                <span class="input-group-btn"><asp:LinkButton CssClass="btn btn-primary" ID="btnUploadTheme" runat="server" OnClick="btnUploadTheme_Click" /></span>
            </div>
        </div>
    </div>
</div>
<script>
$(document).ready(function () {
  $('*[data-thumbnail]').hover(
    /* onmouseenter: */ function () {
      // remove a possible current thumbnail:
      $('#thumb').remove();

      var x = 0, y = 0, $this = $(this);
      // we get the URL from the data attribute
      var url = $this.attr('data-thumbnail');
      y = $this.offset().top;
      x = $this.offset().left - 330;
      // generate the thumbnail
      var $thumb = $('<iframe src="'+url+'" id="thumb" '+
                     'style="position: absolute; '+
                     'top:'+y+'px;left:'+x+'px;"></iframe>');
      $thumb.appendTo('body').delay(500).css({
        'width': '960px',
        'height': '480px'
      });
    },
    /* onmouseleave: */ function () {
      $('#thumb').remove();
    }
  );
});
</script>
</asp:Content>
