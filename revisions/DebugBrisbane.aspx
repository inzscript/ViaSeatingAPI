<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="DebugBrisbane.aspx.cs" Inherits="DebugBrisbane" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>
<%@ Register Src="~/Control/HeaderContent.ascx" TagPrefix="uc1" TagName="HeaderContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- content holder -->
    <div class="content_holder">

        <uc1:HeaderContent runat="server" ID="HeaderContent" />
         
        <div class="content_second_background">
        <div class="content_area clearfix">

            <div id="row-664892-1" class="content_block_background template_builder "> 
            <section class="content_block clearfix">
                <section id="row-664892-1-content" class="content full  clearfix">
                    <div class="row clearfix">
                        <div id="products-707941-54923" class="product_holder product-showcase clearfix">
                            <div class="product_boxes" data-rt-animation-group="group">

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
                                
                            </div>
                        </div>
                    </div>
                </section>
            </section>
            </div>

        </div><!-- / end div .content_area -->

        <uc1:FooterContent runat="server" ID="FooterContent" />

        </div><!-- / end div .content_second_background -->
    </div><!-- / end div .content_holder -->

</asp:Content>

