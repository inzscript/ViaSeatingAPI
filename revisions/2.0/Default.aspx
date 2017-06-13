<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Via.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableSessionState="True" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>
<%@ Register Src="~/Control/HeaderContent.ascx" TagPrefix="uc1" TagName="HeaderContent" %>
<%@ Register Src="~/Control/NavigationProgress.ascx" TagPrefix="uc1" TagName="NavigationProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
      jQuery( function() {
          jQuery("#tabs").tabs();
      } );
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- content holder -->
    <div class="content_holder">
        <section class="top_content clearfix">
        <section class="info_bar clearfix ">

        <uc1:NavigationProgress runat="server" ID="NavProgress1" />
        <uc1:HeaderContent runat="server" ID="HeaderContent" />
        
        </section>
        </section>
         
        <div class="content_second_background">
        <div class="content_area clearfix">

            <div id="row-664892-1" class="content_block_background template_builder ">

                <asp:Literal ID="Literal2" runat="server"></asp:Literal>

            <section class="content_block clearfix">
                <section id="row-664892-1-content" class="content full  clearfix">
                    <div class="row clearfix">
                        <div id="products-707941-54923" class="product_holder product-showcase clearfix">
                            <div class="product_boxes" data-rt-animation-group="group">

                                
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                
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
