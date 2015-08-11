<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="NewUsersStats.aspx.cs" Inherits="AspNetDating.Admin.NewUsersStats" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="panel panel-filter">
    <div class="panel-heading">
        <h4 class="panel-title">
            <i class="fa fa-filter"></i>&nbsp;<%= Lang.TransA("Filter") %>
            <span class="pull-right" id="tblHideSearch" runat="server" visible="false">
               <!-- <a onclick="document.getElementById('tblSearch').style.display = 'block'; document.getElementById('<%= tblHideSearch.ClientID %>').style.display = 'none';" href="javascript: void(0);" title="<%= Lang.TransA("Expand filter") %>">
                    <i class="fa fa-expand"></i>
                </a>-->
                <a data-toggle="collapse" data-parent=".panel-filter" href="#collapseFilter" title="<%= Lang.TransA("Expand/Collapse Filter") %>"><i class="fa fa-expand"></i></a>
            </span>
        </h4>
    </div>
    <div id="collapseFilter" class="panel-collapse collapse in">
        <div class="panel-body">
            <div class="form-horizontal form-sm">
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("From") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtFrom" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("To") %>:</label>
                    <div class="col-sm-8">
                        <asp:TextBox CssClass="form-control" ID="txtTo" runat="server"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-sm-4 control-label"><%= Lang.TransA("Type") %>:</label>
                    <div class="col-sm-8">
                        <asp:DropDownList CssClass="form-control" ID="ddType" runat="server" OnSelectedIndexChanged="ddType_SelectedIndexChanged" AutoPostBack="true"/>
                    </div>
                </div>
                <div class="actions">
                    <asp:Button CssClass="btn btn-primary" ID="btnShowStatistics" runat="server" OnClick="btnShowStatistics_Click"/>
                </div>
            </div>
        </div>
    </div>
</div>
    <script type="text/javascript">
        if (document.getElementById('<%= tblHideSearch.ClientID %>'))
            document.getElementById('tblSearch').style.display = 'none';
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc1:messagebox id="MessageBox" runat="server" />
        <div id="pnlNewUsersChart" runat="server">
        <asp:Chart ID="ChartNewRegistrations" runat="server" Width="600" Height="200" Palette="BrightPastel" BackColor="#D3DFF0" BorderDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
            <Titles>
                <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105"/>
            </Titles>
            <series>
				<asp:Series Name="Default" ChartType="Column" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
				</asp:Series>
			</series>
			<ChartAreas>
				<asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
				    <area3dstyle Rotation="10" Inclination="30" Perspective="5" PointDepth="200" IsRightAngleAxes="False" Enable3D="true" WallWidth="0" IsClustered="False"/>
				    <axisy LineColor="64, 64, 64, 64">
						<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
						<MajorGrid LineColor="64, 64, 64, 64" />
					</axisy>
					<axisx LineColor="64, 64, 64, 64">
						<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
						<MajorGrid LineColor="64, 64, 64, 64" />
					</axisx>
				</asp:ChartArea>
			</ChartAreas>
        </asp:Chart>
        </div>
        <div class="table-responsive">
        <asp:DataGrid CssClass="table table-striped" ID="dgNewUsers" runat="server" AllowPaging="False" AutoGenerateColumns="False" AllowSorting="True" GridLines="None" OnItemCreated="dgNewUsers_ItemCreated">
        <HeaderStyle Font-Bold="True" Wrap="False"></HeaderStyle>
        <Columns>
            <asp:TemplateColumn>
	            <ItemTemplate>
	                <%# Eval("Time")%>
	            </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <%# Eval("NewUsers") %>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>    
    <div class="text-right"><asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnGetCSV" runat="server" Visible="false" onclick="btnGetCSV_Click"/></div>
</asp:Content>
