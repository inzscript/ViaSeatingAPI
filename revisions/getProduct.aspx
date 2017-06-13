<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getProduct.aspx.cs" Inherits="getProduct" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
    </form>
</body>
</html>
