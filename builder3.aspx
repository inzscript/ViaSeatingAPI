﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ViaBuilder.master" AutoEventWireup="true" CodeFile="builder3.aspx.cs" Inherits="builder3" EnableViewState="true" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>
<%@ Register Src="~/Control/NavigationProgress.ascx" TagPrefix="uc1" TagName="NavigationProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="Scripts/classie.js"></script>
    <script src="Scripts/selectFx.js"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" language="javascript">

        //ToDo: Used to call button on which we fire server.transfer
        function onFinalizePostBack() {
            var btn1Name = $get("<%=btnFinalize.ClientID%>").name;
            jQuery('body').removeClass('loaded');
            __doPostBack(btn1Name, "");
        }

        function onDropDownChange(dropdownValue) {
            var dstring = dropdownValue.split(":");

            if (dstring.length > 1) {

                document.getElementById("<%=SelectedOptionName.ClientID %>").value = dstring[1];
                document.getElementById("<%=SelectedOptionValue.ClientID %>").value = dstring[0];

                //alert("DropDown Selection:" + dstring[0]);
            }
            
            <%-- Do PostBack --%>
            jQuery('body').removeClass('loaded');
            var btnName = $get("<%=btnPostBack.ClientID%>").name;
            __doPostBack(btnName, "");
        }

        function onCheckBoxClick(checkboxName) {

            var x = document.getElementById("cmn-toggle-" + checkboxName).checked;
            var checkboxValue = "False";

            if (x)
                checkboxValue = "True";
            else
                checkboxValue = "False";

            document.getElementById("<%=SelectedOptionName.ClientID %>").value = checkboxName;
            document.getElementById("<%=SelectedOptionValue.ClientID %>").value = checkboxValue;
            
            <%-- Do PostBack --%>
            jQuery('body').removeClass('loaded');
            var btnName = $get("<%=btnPostBack.ClientID%>").name;
            __doPostBack(btnName, "");
        }

        function onRadioClick(radioName, radioValue) {
            document.getElementById("<%=SelectedOptionName.ClientID %>").value = radioName;
            document.getElementById("<%=SelectedOptionValue.ClientID %>").value = radioValue;

            <%--str = document.getElementById("<%=UpdatedChairSelect.ClientID %>").value;
            str.replace("\"name\" : \"SERIES\", \"value\" : \"2803\"", "\"name\" : \"" + radioName + "\", \"value\" : \"" + radioValue + "\"");
            alert(radioName + ":" + str);
            document.getElementById("<%=UpdatedChairSelect.ClientID %>").value = str;--%>

            <%-- Do PostBack --%>
            var btnName = $get("<%=btnPostBack.ClientID%>").name;
            jQuery('body').removeClass('loaded');
            __doPostBack(btnName, "");
        }

        function errorDisplay() {
            jQuery('.ssticky').unstick();
            jQuery('.preview-wrap').addClass('builder-hidden');
            jQuery('.ssticky').addClass('builder-hidden');
            jQuery('.finish').addClass('builder-hidden');
        }

        function updateChairImage(chairImg, backgroundImg) {
            var number = 1 + Math.floor(Math.random() * 200);
            document.getElementById('preview-wrap').style.backgroundImage = 'url(' + chairImg + '?v=' + number + '), url(/images/background_1.jpg)';
            jQuery('body').addClass('loaded');
            jQuery('.ssticky').sticky({ topSpacing: 0 });
            jQuery('.ssticky').sticky('update');
            jQuery(function () {
                [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
                    new SelectFx(el, { stickyPlaceholder: true, onChange: function (selectedValue) { onDropDownChange(selectedValue); } });
                });
             });
        }

        function initializeChairImage(chairImg, backgroundImg) {
            document.getElementById('preview-wrap').style.backgroundImage = 'url(' + chairImg + '), url(/images/background_1.jpg)';
            jQuery('body').addClass('loaded');
            jQuery('.ssticky').sticky({ topSpacing: 0 });
            jQuery('.ssticky').sticky('update');
        }

    </script>

    <!-- content holder -->
    <div class="content_holder builder_holder">
        <section class="top_content clearfix">
        <section class="info_bar clearfix ">

        <uc1:NavigationProgress runat="server" ID="NavProgress1" />
        <%--<uc1:HeaderContent runat="server" ID="HeaderContent" />--%>

        </section>
        </section>
        
        <div class="content_second_background">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>

                    <div class="content_area clearfix">


                        <section id="row-content" class="content right">

                            <!-- Image Area -->
                            <div class="apply ssticky">
                                <div class="preview-wrap" id="preview-wrap" style="background-color: #ffffff; background-image: url('/images/brisbane720_720.png'), url('/images/background_1.jpg');">

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
                                                    <button class="small" id="view-spec">SPEC</button>
                                                </li>
                                                <li class="border" id="change-event">
                                                    <button class="small" id="btnPDF" runat="server" onserverclick="btnGeneratePDF_Click" onclick="return false;"><span class="icon-download"></span>PDF</button>
                                                </li>
                                                <li>
                                                    <button class="final">FINALIZE SPEC</button>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>

                        </section>

                        <section id="row-sidebar" class="sidebar left">

                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            <asp:Literal ID="ViaImageURL" runat="server"></asp:Literal>
                            <asp:Literal ID="SectionOptions" runat="server"></asp:Literal>
                            <asp:HiddenField ID="SelectedOptionValue" runat="server" Value="" />
                            <asp:HiddenField ID="SelectedOptionName" runat="server" Value="" />
                            <asp:HiddenField ID="UpdatedChairSelect" runat="server" Value="" />
                            <asp:HiddenField ID="GlobalSessionID" runat="server" Value="" />
                            <asp:Panel ID="Panel1" runat="server"></asp:Panel>

                            <div class="finish">
                                <hr>

                                <div style="display: none">
                                    <asp:Button runat="server" ID="btnPostBack" OnClick="btnPostBack_Click" Text="PostBack" />
                                </div>
                                <asp:Button runat="server" ID="btnFinalizePostBack" OnClick="btnPostFinal_Click" Text="Finalize" CssClass="button" />
                            </div>

                        </section>

                    </div>

                    <uc1:FooterContent runat="server" ID="FooterContent" />

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Button ID="createPDF" runat="server" OnClick="btnGeneratePDF_Click" CssClass="button" Text="PDF" />
            

            <div style="display: none">
                <asp:Button runat="server" ID="btnFinalize" OnClick="btnFinalize_Click" Text="Finalize PostBack" />
            </div>

            <script>
                (function () {
                    [].slice.call(document.querySelectorAll('select.cs-select')).forEach(function (el) {
                        new SelectFx(el, { stickyPlaceholder: true, onChange: function (selectedValue) { onDropDownChange(selectedValue); } });
                    });
                })();
            </script>

        </div>
    </div>
</asp:Content>

