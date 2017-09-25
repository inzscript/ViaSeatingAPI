<%@ Page Title="" Language="C#" MasterPageFile="~/Via.Master" AutoEventWireup="true" CodeFile="complete.aspx.cs" Inherits="complete" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>
<%@ Register Src="~/Control/HeaderContent.ascx" TagPrefix="uc1" TagName="HeaderContent" %>
<%@ Register Src="~/Control/NavigationProgress.ascx" TagPrefix="uc1" TagName="NavigationProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="content_holder">
        <section class="top_content clearfix">
            <section class="info_bar clearfix">

                <uc1:NavigationProgress runat="server" ID="NavProgress1" />
                <%--<uc1:HeaderContent runat="server" ID="HeaderContent" />--%>

            </section>
        </section>

        <div class="content_second_background">
            <div class="content_area clearfix">

                <section id="row-pricelist" class="content title">
                    <div class="family left triple"><span><asp:Literal ID="lFamily" runat="server"></asp:Literal></span><br /><asp:Literal ID="lSeries" runat="server"></asp:Literal></div>
                    <div class="price left triple"><span>List Price</span><br /><asp:Literal ID="lListPrice" runat="server"></asp:Literal></div>
                    <div class="other left triple"><asp:Button ID="createPDF" runat="server" OnClick="btnGeneratePDF_Click" CssClass="button orange" Text="Save PDF" /><br/><a class="icon-back button" href="/" itemprop="url"> Start Over</a></div>
                </section>

                <asp:Literal ID="Literal1" runat="server"></asp:Literal>

            </div>

            <asp:Literal ID="Literal2" runat="server"></asp:Literal>

            <asp:HiddenField ID="SelectionSummary" runat="server" Value="" />
            <asp:HiddenField ID="ConfiguredPrice" runat="server" Value="" />
            <asp:HiddenField ID="ChairImageURL" runat="server" Value="" />
            <asp:HiddenField ID="ChairSelect" runat="server" Value="" />

            <uc1:FooterContent runat="server" ID="FooterContent" />

        </div>
    </div>

</asp:Content>

