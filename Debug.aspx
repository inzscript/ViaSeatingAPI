<%@ Page Title="Debug" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Debug.aspx.cs" Inherits="_Debug" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="jumbotron">
        <h1><asp:Literal ID="RuleLiteral" runat="server"></asp:Literal></h1>
        <p class="lead">Debug Rule Set</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>InitializeConfiguration</h2>
            <p>
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Configure Calls</h2>
            <p>
                <asp:Literal ID="Literal2" runat="server"></asp:Literal>
            </p>
        </div>
        <div class="col-md-4">
            <h2>UiData</h2>
            <p>
                <asp:Literal ID="Literal3" runat="server"></asp:Literal>
            </p>

        </div>
    </div>

</asp:Content>
