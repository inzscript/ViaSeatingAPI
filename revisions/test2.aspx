<%@ Page Title="" Language="C#" MasterPageFile="~/Via.Master" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="/Scripts/image-picker.min.js"></script>
    <script>
        //jQuery.noConflict();
        jQuery(function () {
            jQuery.widget("custom.iconselectmenu", jQuery.ui.selectmenu, {
                _renderItem: function (ul, item) {
                    var li = jQuery("<li>"),
                      wrapper = jQuery("<div>", { text: item.label });

                    if (item.disabled) {
                        li.addClass("ui-state-disabled");
                    }

                    jQuery("<span>", {
                        style: item.element.attr("data-style"),
                        "class": "ui-icon " + item.element.attr("data-class")
                    })
                      .appendTo(wrapper);

                    return li.append(wrapper).appendTo(ul);
                }
            });

            jQuery("#people")
              .iconselectmenu()
              .iconselectmenu("menuWidget")
                .addClass("ui-menu-icons avatar");
        });

        // Image Picker
        jQuery(".image-picker").imagepicker({
            hide_select: false,
        });

        jQuery("select.image-picker.show-labels").imagepicker({
            hide_select: false,
            show_label: true,
        });

        jQuery("select.image-picker.limit_callback").imagepicker({
            limit_reached: function () { alert('We are full!') },
            hide_select: false
        });

    </script>

    <style>
        h2 {
            margin: 30px 0 0 0;
        }

        fieldset {
            border: 0;
        }

        label {
            display: block;
        }

        /* select with custom icons */
        .ui-selectmenu-menu .ui-menu.customicons .ui-menu-item-wrapper {
            padding: 0.5em 0 0.5em 3em;
        }

        .ui-selectmenu-menu .ui-menu.customicons .ui-menu-item .ui-icon {
            height: 30px;
            width: 30px;
            top: 0.1em;
        }

        .ui-icon.video {
            background: url("images/24-video-square.png") 0 0 no-repeat;
        }

        .ui-icon.podcast {
            background: url("images/24-podcast-square.png") 0 0 no-repeat;
        }

        .ui-icon.rss {
            background: url("images/24-rss-square.png") 0 0 no-repeat;
        }

        /* select with CSS avatar icons */
        option.avatar {
            background-repeat: no-repeat !important;
            padding-left: 20px;
        }

        .avatar .ui-icon {
            background-position: left top;
        }

        /* Image Picker CSS */
        ul.thumbnails.image_picker_selector {
            overflow: auto;
            list-style-image: none;
            list-style-position: outside;
            list-style-type: none;
            padding: 0px;
            margin: 0px;
        }

        ul.thumbnails.image_picker_selector ul {
            overflow: auto;
            list-style-image: none;
            list-style-position: outside;
            list-style-type: none;
            padding: 0px;
            margin: 0px;
        }

        ul.thumbnails.image_picker_selector li.group {
            width: 100%;
        }

        ul.thumbnails.image_picker_selector li.group_title {
            float: none;
        }

        ul.thumbnails.image_picker_selector li {
            margin: 0px 12px 12px 0px;
            float: left;
        }

        ul.thumbnails.image_picker_selector li .thumbnail {
            padding: 6px;
            border: 1px solid #dddddd;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
        }

        ul.thumbnails.image_picker_selector li .thumbnail img {
            -webkit-user-drag: none;
        }

        ul.thumbnails.image_picker_selector li .thumbnail.selected {
            background: #0088cc;
        }
    </style>

    <link href="stylesheets/css/builder.css" rel="stylesheet">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:UpdatePanel ID="pnlHelloWorld" runat="server">
            <ContentTemplate>

                <h2>Selectmenu with custom avatar 16x16 images as CSS background</h2>
                <fieldset>
                    <label for="people">Select a Person:</label>
                    <select name="people" id="people">
                        <option value="1" data-class="avatar" data-style="background-image: url(&apos;http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SATSUMA.JPG&apos;);">John Resig</option>
                        <option value="2" data-class="avatar" data-style="background-image: url(&apos;http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SNOW.JPG&apos;);">Tauren Mills</option>
                        <option value="3" data-class="avatar" data-style="background-image: url(&apos;http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SPICE.JPG&apos;);">Jane Doe</option>
                    </select>
                </fieldset>

                <br /><br />

                <select class="image-picker show-html">
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SATSUMA.JPG" data-img-class="first" data-img-alt="MESSENGER SATSUMA" value="MESSENGER SATSUMA">MESSENGER SATSUMA</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SNOW.JPG" data-img-alt="MESSENGER SNOW" value="MESSENGER SNOW">MESSENGER SNOW</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SPICE.JPG" data-img-alt="MESSENGER SPICE" value="MESSENGER SPICE">MESSENGER SPICE</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-SQUALL.JPG" data-img-alt="MESSENGER SQUALL" value="MESSENGER SQUALL">MESSENGER SQUALL</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-TANGELO.JPG" data-img-alt="MESSENGER TANGELO" value="MESSENGER TANGELO">MESSENGER TANGELO</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-TUSK.JPG" data-img-alt="MESSENGER TUSK" value="MESSENGER TUSK">MESSENGER TUSK</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-ULTRAMARINE.JPG" data-img-alt="MESSENGER ULTRAMARINE" value="MESSENGER ULTRAMARINE">MESSENGER ULTRAMARINE</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-VIBRANT.JPG" data-img-alt="MESSENGER VIBRANT" value="MESSENGER VIBRANT">MESSENGER VIBRANT</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MESSENGER/MESSENGER-VOYAGE.JPG" data-img-alt="MESSENGER VOYAGE" value="MESSENGER VOYAGE">MESSENGER VOYAGE</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MILESTONE/MILESTONE-AURORA.JPG" data-img-alt="MILESTONE AURORA" value="MILESTONE AURORA">MILESTONE AURORA</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MILESTONE/MILESTONE-BASIL.JPG" data-img-alt="MILESTONE BASIL" value="MILESTONE BASIL">MILESTONE BASIL</option>
                    <option data-img-src="http://app02.viaseating.com/pcm/configuratorcontent/IMAGES/TEXTILES/MILESTONE/MILESTONE-BISON.JPG" data-img-alt="MILESTONE BISON" data-img-class="last" value="MILESTONE BISON">MILESTONE BISON</option>
                </select>

                <br /><br />

                <div class="crow">
                    <div class="question">
                        Do you like Trinidad &amp; Tobago?
                    </div>
                    <div class="switch">
                        <input id="cmn-toggle-4" class="cmn-toggle cmn-toggle-round-flat" type="checkbox">
                        <label for="cmn-toggle-4"></label>
                    </div>
                </div>
                <!-- /row -->

                <br /><br />

                
                <select class="cs-select cs-skin-border">
                    <option value="" disabled selected>Preferred contact method</option>
                    <option value="email">E-Mail</option>
                    <option value="twitter">Twitter</option>
                    <option value="linkedin">LinkedIn</option>
                </select>

                <script src="Scripts/classie.js"></script>
                <script src="Scripts/selectFx.js"></script>
                <script>
			        (function() {
				        [].slice.call( document.querySelectorAll( 'select.cs-select' ) ).forEach( function(el) {	
					        new SelectFx(el);
				        } );
			        })();
		        </script>

                <br /><br />

                <div class="control-group">
                    <h1>Select boxes</h1>
                    <div class="select">
                        <select>
                            <option>FIRST SELECT</option>
                            <option>OPTION1</option>
                            <option>OPTION2</option>
                        </select>
                        <div class="select__arrow"></div>
                    </div>
                    <div class="select">
                        <select>
                            <option>SECOND SELECT IS QUITE LONG</option>
                            <option>OPTION3</option>
                            <option>OPTION4</option>
                        </select>
                        <div class="select__arrow"></div>
                    </div>
                    <div class="select">
                        <select disabled="disabled">
                            <option>DISABLED</option>
                            <option>Option</option>
                            <option>Option</option>
                        </select>
                        <div class="select__arrow"></div>
                    </div>
                </div>
                
                <br /><br />

                

                <br /><br />
                
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>

                <br />
                <asp:Label runat="server" ID="lblHelloWorld" Text="Click the button!" />
                <br />
                <asp:Button runat="server" ID="btnHelloWorld" OnClick="btnHelloWorld_Click" Text="Update label!" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

