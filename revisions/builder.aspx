<%@ Page Title="" Language="C#" MasterPageFile="~/ViaBuilder.master" AutoEventWireup="true" CodeFile="builder.aspx.cs" Inherits="builder" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>
<%@ Register Src="~/Control/NavigationProgress.ascx" TagPrefix="uc1" TagName="NavigationProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- content holder -->
    <div class="content_holder builder_holder">
        <section class="top_content clearfix">
        <section class="info_bar clearfix ">

        <uc1:NavigationProgress runat="server" ID="NavProgress1" />
        <%--<uc1:HeaderContent runat="server" ID="HeaderContent" />--%>

        </section>
        </section>

        <div class="content_second_background">
            <div class="content_area clearfix">

                <section id="row-content" class="content right">
                    <!-- Image Area -->
                    <div class="apply ssticky">
                        <div class="preview-wrap" style="background-color: #ffffff; background-image: url('/images/brisbane720_720.png'), url('/images/background_1.jpg');">
                            <div id="overlay" style="display: none;">
                                <div class="loading dots">
                                    <span class="fa fa-circle"></span>
                                    <span class="fa fa-circle"></span>
                                    <span class="fa fa-circle"></span>
                                </div>
                            </div>
                            <div class="image-options">
                                <div class="utility-bar">
                                    <ul class="unstyled">
                                        <li>
                                            <label>
                                                <input type="radio" value="Front" name="view" id="front" class="view" checked="checked">
                                                <span>FRONT</span>
                                            </label>
                                            <label>
                                                <input type="radio" value="Back" name="view" id="back" class="view">
                                                <span>BACK</span>
                                            </label>
                                        </li>
                                        <li>
                                            <button class="small" id="view-spec">SPEC</button></li>
                                        <li class="border" id="change-event">
                                            <button class="small"><span class="icon-download"></span> PDF</button></li>
                                        <li><button class="final">FINALIZE SPEC</button></li>
                                    </ul>
                                </div>

                                

                            </div>

                        </div>
                    </div>

                    <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                </section>
                <section id="row-sidebar" class="sidebar left">
                    <form class="specsForm">

<%--                        <section class="summary" id="Amplify-Material-UP">

                            <div class="heading loading">
                                <h4>SUMMARY</h4>
                            </div>

                            <div class="material-container">

                                <div class="fabric-select">
                                    <div class="selected-fabric">
                                        <div class="selected-fabric-info">
                                            <dl>--%>
                                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                           <%-- </dl>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </section>--%>

                        <asp:Literal ID="SectionOptions" runat="server"></asp:Literal>

                        <div class="finish">
                            <hr>
                            <button class="full-width primary" id="finalize">finalize spec</button>
                        </div>

                    </form>
                </section>
                
            </div>

            <uc1:FooterContent runat="server" ID="FooterContent" />

        </div>
    </div>
</asp:Content>

