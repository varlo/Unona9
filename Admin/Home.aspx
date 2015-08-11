<%@ Page Language="C#" MasterPageFile="~/Admin/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="AspNetDating.Admin.Home" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Admin.SiteAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
    <div id="divRegistrationsChart" runat="server">
        <asp:Chart ID="ChartNewRegistrations" runat="server" Width="790" Height="200" Palette="BrightPastel" BackColor="#D3DFF0" BorderDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
            <Titles>
                <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105">
                </asp:Title>
            </Titles>
            <Series>
                <asp:Series Name="Default" ChartType="Column" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                    BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                    <Area3DStyle Rotation="10" Inclination="30" Perspective="5" PointDepth="200" IsRightAngleAxes="False" Enable3D="true" WallWidth="0" IsClustered="False" />
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    <div id="divUsersPie" runat="server">
        <asp:Chart ID="ChartTotalRegistrations" runat="server" Width="790" Height="300">
            <Titles>
                <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105">
                </asp:Title>
            </Titles>
            <Legends>
                <asp:Legend Name="Default" BackColor="Transparent" Alignment="Center" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" LegendStyle="Column">
                </asp:Legend>
            </Legends>
            <Series>
                <asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ToolTip="#VALX" LegendText="#VALX">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                    BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                    <Area3DStyle Rotation="0" Inclination="55" Enable3D="true" />
                    <AxisY LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisY>
                    <AxisX LineColor="64, 64, 64, 64">
                        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                        <MajorGrid LineColor="64, 64, 64, 64" />
                    </AxisX>
                    <InnerPlotPosition Width="100" Height="100" />
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </div>
    <asp:Repeater ID="rptPlugins" runat="server">
        <HeaderTemplate>
            <div class="panel">
                <div class="panel-heading">
                    <h3 class="panel-title"><%= "Plug-ins status".TranslateA() %></h3>
                </div>
                <div class="panel-body">
                    <ul class="list-group list-group-striped">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="list-group-item">
                <img src='images/<%# Eval("Icon") %>.png' border="0" />
                <%# Eval("Name") %>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></div></div></FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptHelpdeskTopics" runat="server">
        <HeaderTemplate>
            <div class="panel">
                <div class="panel-heading">
                    <h3 class="panel-title"><%= "The Latest Helpdesk Topics".TranslateA() %></h3>
                </div>
                <div class="panel-body">
                    <ul class="list-group list-group-striped">
        </HeaderTemplate>
        <ItemTemplate>
            <li class="list-group-item">
                    <a href='<%# Eval("Link") %>' target="_blank"><%# Eval("Title") %></a><br />
                    <%# String.Format("Published on <b>{0}</b> in category <b>{1}</b>".TranslateA(), Convert.ToDateTime(Eval("PubDate")).ToShortDateString(), Eval("Category")) %>
                    <%# ((string)Eval("Description")).StripHTML().Shorten(100) %>
                    <a class="small" href='<%# Eval("Link") %>' target="_blank"><%# "read".TranslateA() %>&nbsp;<i class="fa fa-angle-double-right"></i></a>
            </li>
        </ItemTemplate>
        <FooterTemplate></ul></div></div></FooterTemplate>
    </asp:Repeater>
</asp:Content>
