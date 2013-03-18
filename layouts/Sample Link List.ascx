<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sample Link List.ascx.cs" Inherits="Monoco.CMS.layouts.SampleLinkList" %>
<%@ Import Namespace="Monoco.CMS.Fields" %>
<h2>Links</h2>
<asp:Repeater runat="server" ID="relatedLinksRepeater">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
        <a href="<%# ((Link) Container.DataItem).Url %>" class="<%# ((Link) Container.DataItem).LinkType %>"><%# ((Link) Container.DataItem).Title %></a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>