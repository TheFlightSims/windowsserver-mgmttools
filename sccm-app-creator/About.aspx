<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="CM_App_Creator.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="alertError" class="alert alert-danger" runat="server" visible="false">
        <strong>Error!</strong> <span id="spanError" runat="server"></span>
    </div>
    <div id="contentdiv" runat="server">
        <h2><%: Title %>.</h2>
        <div>
        <p runat="server" id="txtfileNews" style="font-size: 12px; font-family:Calibri"></p>
        </div>
        <div class="fa-4x">
            <i class="fas fa-wheelchair fa-spin"></i>
        </div>
    </div>
</asp:Content>
