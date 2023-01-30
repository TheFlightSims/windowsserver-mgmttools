<%@ Page Title="Eventlog" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Eventlog.aspx.cs" Inherits="CM_App_Creator.Eventlog" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="alertError" class="alert alert-danger" runat="server" visible="false">
        <strong>Error!</strong> <span id="spanError" runat="server"></span>
    </div>
    <div id="contentdiv" runat="server">
        <h3><%: Title %>.</h3><p id="nrofappscreated" runat="server"></p>
        <div>
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-sm smalltext table-hover table-striped grideventlog" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Time" HeaderStyle-CssClass="w-25">
                    <ItemTemplate>
                        <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Creator" HeaderStyle-CssClass="w-25">
                    <ItemTemplate>
                        <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("Creator") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="w-50">
                    <ItemTemplate>
                        <asp:Label ID="lblCreator" runat="server" Text='<%# Eval("Action") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            </asp:GridView>

        </div>
        <div>
        <p runat="server" id="txtfileNews" style="font-size: 12px; font-family:Calibri"></p>
        </div>
    </div>
</asp:Content>
