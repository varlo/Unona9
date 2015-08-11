<%@ Control Language="c#" AutoEventWireup="True" Codebehind="NewsBox.ascx.cs" Inherits="AspNetDating.Components.NewsBox" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="News" Src="News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<div class="panel panel-news">
    <div class="panel-heading">
        <h3 class="panel-title"><%= AspNetDating.Classes.Lang.Trans("News") %></h3>
    </div>
    <div class="panel-body">
	    <div class="news-wrap"><uc1:News id="News1" runat="server" /></div>
    </div>
</div>
