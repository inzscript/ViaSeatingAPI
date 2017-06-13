<%@ Page Title="Sample Page" Language="C#" MasterPageFile="~/Via.master" AutoEventWireup="true" CodeFile="sample.aspx.cs" Inherits="sample" %>

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
                                <div class="row clearfix">
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/1.jpg" alt="executive seating" class="">
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">vista II</h3>
                                        <button type="button" class="button">stools</button>
                                        </div>
                                    </div>                        
                                    </div>
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/2.jpg" alt="executive seating" class=""> 
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">3dee</h3>
                                        <button type="button" class="button">motion</button>
                                        </div>
                                    </div>                        
                                    </div>
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/3.jpg" alt="executive seating" class="">
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">oyo</h3>
                                        <button type="button" class="button">motion</button>
                                        </div>
                                    </div>                        
                                    </div>
                                </div>

                                <br />
                                <div class="row clearfix">
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/4.jpg" alt="executive seating" class="">
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">sienna</h3>
                                        <button type="button" class="button">lounge</button>
                                        </div>
                                    </div>                    
                                    </div>
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/5.jpg" alt="executive seating" class="">
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">muvman
                                        </h3>
                                        <button type="button" class="button">motion</button>
                                        </div>
                                    </div>                    
                                    </div>
                                    <div class="box three ">
                                    <div class="featured_image img_loaded"> 
                                        <img src="/images/6.jpg" alt="executive seating" class="">
                                    </div>
                                    <div class="row clearfix">
                                        <div>
                                        <h3 class="featured_article_title fade animated" data-rt-animate="animate" data-rt-animation-type="fade" data-rt-animation-group="single" style="-webkit-animation: 0s;">chico
                                        </h3>
                                        <button type="button" class="button">lounge</button>
                                        </div>
                                    </div>                    
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