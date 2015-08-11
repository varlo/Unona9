<%@ Page language="c#" MasterPageFile="Site.Master" Codebehind="ImportFriends.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.ImportFriends" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"/>
		    <asp:MultiView ID="mvImportFriends" runat="server" ActiveViewIndex="0">
		        <asp:View ID="viewEmail" runat="server">
		            <div class="input-group">
                        <span class="input-group-addon"><%= Lang.Trans("Email address:") %></span>
                        <input class="form-control" type=text ID="txtEmail" Runat="server">
                        <span class="input-group-addon">@</span>
                        <asp:DropDownList  CssClass="form-control" ID="ddEmailProvider" runat="server">
                            <asp:ListItem>gmail.com</asp:ListItem>
                            <asp:ListItem>yahoo.com</asp:ListItem>
                            <asp:ListItem>live.com</asp:ListItem>
                            <asp:ListItem>hotmail.com</asp:ListItem>
                        </asp:DropDownList>
                        <span class="input-group-addon"><%= Lang.Trans("Password:") %></span>
                        <input class="form-control" id="txtPassword" type="password" runat="server">
                        <span class="input-group-btn"><asp:Button CssClass="btn btn-default" ID="btnImport" runat="server" onclick="btnImport_Click" /></span>
                    </div>
			        <asp:label CssClass="alert text-danger" id="lblError" EnableViewState="false" runat="server"/>
                    <p class="help-block">
						<%= "We don't store your email and password information.".Translate() %>
                    </p>
                    <div class="text-right"><a href="InviteFriend.aspx"><%= "... or click here to invite by e-mail".Translate() %></a></div>
			    </asp:View>
			    <asp:View ID="viewMessage" runat="server">
			        <div class="table-responsive">
			        <asp:DataGrid CssClass="table table-striped" ID="dgContacts" runat="server" AutoGenerateColumns="false">
			            <Columns>
			                <asp:TemplateColumn HeaderText="&lt;input type=checkbox onClick=&quot;a=0;for(i=0; i&lt;this.form.elements.length;i++){if(this.form.elements[i]==this) {a=3}; if ((this.form.elements[i].type=='checkbox') &amp;&amp; (a!=0) &amp;&amp; (i&gt;a)) {this.form.elements[i].checked=this.checked}}&quot;&gt;">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect" value='<%# Eval("Email") %>' runat="server" name="cbSelect" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
			                <asp:TemplateColumn>
			                    <ItemTemplate>
			                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'/>
			                    </ItemTemplate>
			                </asp:TemplateColumn>
			                <asp:TemplateColumn>
			                    <ItemTemplate>
			                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'/>
			                    </ItemTemplate>
			                </asp:TemplateColumn>
			            </Columns>
			        </asp:DataGrid>
			        </div>
			        <label><%= Lang.Trans("Add a personal note to this message") %></label>
			        <div class="fckeditor">
			            <asp:PlaceHolder ID="phEditor" runat="server"/>
			        </div>
			        <div class="actions">
				        <asp:button CssClass="btn btn-default" id="btnSubmit" tabIndex="1" runat="server"/>
			        </div>
			    </asp:View>
			</asp:MultiView>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"/>
</asp:Content>
