<%@ Page Title="" Language="C#" MasterPageFile="~/ViaBuilder.master" AutoEventWireup="true" CodeFile="builder-sample.aspx.cs" Inherits="builder_sample" %>

<%@ Register Src="~/Control/FooterContent.ascx" TagPrefix="uc1" TagName="FooterContent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- content holder -->
    <div class="content_holder builder_holder">
        <div class="content_second_background">
            <div class="content_area clearfix">

                <section id="row-content" class="content right">
                    <!-- Image Area -->
                    <div class="apply ssticky">
                        <div class="preview-wrap" style="background-color: #ffffff; background-image: url('//s7d4.scene7.com/ir/render/ExemplisRender/Amplify_0000?wid=720&amp;fmt=png-alpha&amp;qlt=100&amp;obj=root/uphl1/fabric&amp;src=sitonit_sugar_licorice&amp;show&amp;obj=root/stat&amp;show&amp;obj=root/mechs/dflt/stat&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/fram1/amplifyfc1&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/uphl1/fabric&amp;src=sitonit_sugar_licorice&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/stat&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/fram2/b17&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/drop&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/stat&amp;show&amp;obj=root&amp;req=object'),
                        url('../../includes/img/customizer/backgrounds/StudioShot_CoolGray.jpg');">
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
                                            <button class="small">CHANGE BACKGROUND</button></li>
                                        <li class="pdf custom"><a class="download-pdf">PDF<span data-tooltip="" aria-haspopup="true" data-options="disable_for_touch:true" class="has-tip" data-selector="tooltip-j0k6n4z60" aria-describedby="tooltip-j0k6n4z60" title=""><i class="fa fa-arrow-down"></i></span></a></li>
                                        <li>
                                            <a class="download-image" href="//s7d4.scene7.com/ir/render/ExemplisRender/Amplify_0000?wid=3000&amp;fmt=png-alpha&amp;qlt=100&amp;obj=root/uphl1/fabric&amp;src=sitonit_sugar_licorice&amp;show&amp;obj=root/stat&amp;show&amp;obj=root/mechs/dflt/stat&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/fram1/amplifyfc1&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/uphl1/fabric&amp;src=sitonit_sugar_licorice&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/stat&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/fram2/b17&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/drop&amp;show&amp;obj=root/mechs/dflt/backs/amplifyb3/bases/dflt/feet/dflt/stat&amp;show&amp;obj=root&amp;req=object" download="Chair Preview Front">
                                                <span data-tooltip="" aria-haspopup="true" data-options="disable_for_touch:true" class="has-tip" data-selector="tooltip-j0k6n4z61" aria-describedby="tooltip-j0k6n4z61" title="">
                                                    <i class="fa fa-camera"></i>
                                                </span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>

                                <button class="final">FINALIZE SPEC</button>

                            </div>

                        </div>
                    </div>
                </section>
                <section id="row-sidebar" class="sidebar left">
                    <form class="specsForm">

                        <section class="back-style" id="Amplify-Back-Style">

                            <div class="heading">
                                <h4>BACK STYLE</h4>
                            </div>

                            <ul class="options unstyled">

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="Amplify.B1" name="BACK STYLE" product-id="Amplify.B1" section-id="Amplify-Back-Style">
                                    <label for="Amplify.B1" title="Mesh - B1" class=" ">
                                        Mesh - B1<span class="price-delta"> [-$46.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="Amplify.B3" name="BACK STYLE" product-id="Amplify.B3" section-id="Amplify-Back-Style" checked="checked">
                                    <label for="Amplify.B3" title="Upholstered - B3" class=" ">
                                        Upholstered - B3<span class="price-delta"> </span>
                                    </label>
                                </li>

                            </ul>

                        </section>

                        <section class="back" id="Amplify-UP-Back">

                            <div class="heading">
                                <h4>BACK</h4>
                            </div>

                            <ul class="options unstyled">

                                <li class="sub-heading">SIZE:
							        <span>Highback with Adjustable Lumbar</span>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="Amplify.2723" name="SIZE" product-id="Amplify.2723" subsection-id="Amplify-Back-UPSize" section-id="Amplify-UP-Back" checked="checked">
                                    <label for="Amplify.2723" title="Highback with Adjustable Lumbar - 2723" class=" ">
                                        Highback with Adjustable Lumbar - 2723<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="sub-heading">FRAME COLOR:
							        <span>Black</span>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="Amplify.FC1" name="FRAME COLOR" product-id="Amplify.FC1" subsection-id="Amplify-UP-Back-FrameColor" section-id="Amplify-UP-Back" checked="checked">
                                    <label for="Amplify.FC1" title="Black - FC1" class=" ">
                                        Black - FC1<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="Amplify.FC13" name="FRAME COLOR" product-id="Amplify.FC13" subsection-id="Amplify-UP-Back-FrameColor" section-id="Amplify-UP-Back">
                                    <label for="Amplify.FC13" title="Fog - FC13" class=" ">
                                        Fog - FC13<span class="price-delta"> [+$35.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="Amplify.FC14" name="FRAME COLOR" product-id="Amplify.FC14" subsection-id="Amplify-UP-Back-FrameColor" section-id="Amplify-UP-Back">
                                    <label for="Amplify.FC14" title="Fog with Silver - FC14" class=" ">
                                        Fog with Silver - FC14<span class="price-delta"> [+$35.00]</span>
                                    </label>
                                </li>

                            </ul>

                        </section>

                        <section class="material" id="Amplify-Material-UP">

                            <div class="heading loading">
                                <h4>MATERIAL</h4>
                            </div>

                            <div class="material-container">

                                <div class="fabric-select">
                                    <div class="selected-fabric">
                                        <div class="selected-fabric-info">
                                            <h4>Sugar, Licorice</h4>
                                            <dl>
                                                <dt>Manufacturer</dt>
                                                <dd class="manufacturer">SitOnIt</dd>

                                                <dt>Part number</dt>
                                                <dd class="partNumber">26-1430002-0010</dd>

                                                <dt>Grade</dt>
                                                <dd class="grade">1</dd>

                                                <dt>Lead Time</dt>
                                                <dd class="leadTime">2</dd>
                                            </dl>

                                        </div>

                                    </div>
                                </div>

                                <hr>

                                <div class="fabric-search fabric-search-params">
                                    <div>
                                        <div class="fabric-type-toggle">
                                            <label>
                                                <input type="radio" value="Fabric" name="fabric-toggle" id="tabric-toggle-1" checked="checked">
                                                <span>TEXTILE</span>
                                            </label>
                                            <label>
                                                <input type="radio" value="Leather" name="fabric-toggle" id="tabric-toggle-2">
                                                <span>LEATHER</span>
                                            </label>
                                        </div>

                                        <label class="cal133 active">
                                            <input type="checkbox">
                                            <span>Cal 133
                <span class="price-delta">[+$99.0]
            
                </span>
                                            </span>
                                        </label>

                                        <div style="clear: both"></div>

                                        <div class="fabric-fitlers">
                                            <div>
                                                <div class="sort-action">
                                                    <button data-dropdown="drop-colors" aria-controls="drop-colors" aria-expanded="true" class="small secondary button dropdown open">
                                                        <span>COLORS</span>
                                                        <i class="fa fa-chevron-down"></i>
                                                    </button>
                                                    <ul id="drop-colors" data-dropdown-content="" class="f-dropdown open f-open-dropdown" aria-hidden="false">
                                                        <li><a>All Colors</a></li>
                                                        <li><a data-color="Beige">Beige</a></li>
                                                        <li><a data-color="Black">Black</a></li>
                                                        <li><a data-color="Blue">Blue</a></li>
                                                        <li><a data-color="Brown">Brown</a></li>
                                                        <li><a data-color="Gray">Gray</a></li>
                                                        <li><a data-color="Green">Green</a></li>
                                                        <li><a data-color="Orange">Orange</a></li>
                                                        <li><a data-color="Purple">Purple</a></li>
                                                        <li><a data-color="Red">Red</a></li>
                                                        <li><a data-color="White">White</a></li>
                                                        <li><a data-color="Yellow">Yellow</a></li>
                                                    </ul>
                                                </div>
                                                <div class="sort-action">
                                                    <button data-dropdown="drop-manufacturers" aria-controls="drop-manufacturers" aria-expanded="true" class="small secondary button dropdown open">
                                                        <span>MANUFACTURERS</span>
                                                        <i class="fa fa-chevron-down"></i>
                                                    </button>
                                                    <ul id="drop-manufacturers" data-dropdown-content="" class="f-dropdown open f-open-dropdown" aria-hidden="false">
                                                        <li><a>All Manufacturers</a></li>
                                                        <li><a data-manufacturer="Arc Com">Arc Com</a></li>
                                                        <li><a data-manufacturer="Knoll Textiles">Knoll Textiles</a></li>
                                                        <li><a data-manufacturer="Maharam">Maharam</a></li>
                                                        <li><a data-manufacturer="Mayer">Mayer</a></li>
                                                        <li><a data-manufacturer="Momentum">Momentum</a></li>
                                                        <li><a data-manufacturer="SitOnIt">SitOnIt</a></li>
                                                        <li><a data-manufacturer="Ultrafabrics">Ultrafabrics</a></li>
                                                    </ul>
                                                </div>
                                                <div class="sort-action">
                                                    <button data-dropdown="drop-patterns" aria-controls="drop-patterns" aria-expanded="true" class="small secondary button dropdown open">
                                                        <span>PATTERNS</span>
                                                        <i class="fa fa-chevron-down"></i>
                                                    </button>
                                                    <ul id="drop-patterns" data-dropdown-content="" class="f-dropdown open f-open-dropdown" aria-hidden="false">
                                                        <li><a>All Patterns</a></li>
                                                        <li><a data-pattern="Abacus Standard" data-manufacturer="Maharam">Abacus Standard</a></li>
                                                        <li><a data-pattern="Align" data-manufacturer="Mayer">Align</a></li>
                                                        <li><a data-pattern="Amuse" data-manufacturer="Momentum">Amuse</a></li>
                                                        <li><a data-pattern="Ascend" data-manufacturer="Momentum">Ascend</a></li>
                                                        <li><a data-pattern="Avenue" data-manufacturer="Momentum">Avenue</a></li>
                                                        <li><a data-pattern="Bar" data-manufacturer="Maharam">Bar</a></li>
                                                        <li><a data-pattern="Beeline" data-manufacturer="Momentum">Beeline</a></li>
                                                        <li><a data-pattern="Bevel" data-manufacturer="Maharam">Bevel</a></li>
                                                        <li><a data-pattern="Bravo II" data-manufacturer="Momentum">Bravo II</a></li>
                                                        <li><a data-pattern="Brisa" data-manufacturer="Ultrafabrics">Brisa</a></li>
                                                        <li><a data-pattern="Brisa Distressed" data-manufacturer="Ultrafabrics">Brisa Distressed</a></li>
                                                        <li><a data-pattern="Brisa Fresco" data-manufacturer="Ultrafabrics">Brisa Fresco</a></li>
                                                        <li><a data-pattern="Canter" data-manufacturer="Momentum">Canter</a></li>
                                                        <li><a data-pattern="Chroma" data-manufacturer="Arc Com">Chroma</a></li>
                                                        <li><a data-pattern="Chronicle" data-manufacturer="Knoll Textiles">Chronicle</a></li>
                                                        <li><a data-pattern="Clarion" data-manufacturer="Arc Com">Clarion</a></li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="grade-select">
                                            <label for="amount">GRADE</label>

                                            <input type="text" class="amount" name="Amplify-Material-UP-amount" readonly="">
                                            <div class="grade-range-container fabric-grade-range active" data-type="Fabric">
                                                <div class="grade-range ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all ui-slider-pips" data-min="1" data-max="7" aria-disabled="false">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all" style="left: 0%; width: 100%;"></div>
                                                    <a class="ui-slider-handle ui-state-default ui-corner-all" href="#" style="left: 0%;"></a><a class="ui-slider-handle ui-state-default ui-corner-all last" href="#" style="left: 100%;"></a><span class="ui-slider-pip ui-slider-pip-first ui-slider-pip-label ui-slider-pip-1" style="left: 0%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="1">1</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-2" style="left: 16.6667%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="2">2</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-3" style="left: 33.3333%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="3">3</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-4" style="left: 50.0000%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="4">4</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-5" style="left: 66.6667%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="5">5</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-6" style="left: 83.3333%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="6">6</span></span><span class="ui-slider-pip ui-slider-pip-last ui-slider-pip-label ui-slider-pip-7" style="left: 100%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="7">7</span></span></div>
                                            </div>
                                            <div class="grade-range-container leather-grade-range " data-type="Leather">
                                                <div class="grade-range ui-slider ui-slider-horizontal ui-widget ui-widget-content ui-corner-all ui-slider-pips" data-min="1" data-max="3" aria-disabled="false">
                                                    <div class="ui-slider-range ui-widget-header ui-corner-all" style="left: 0%; width: 100%;"></div>
                                                    <a class="ui-slider-handle ui-state-default ui-corner-all" href="#" style="left: 0%;"></a><a class="ui-slider-handle ui-state-default ui-corner-all last" href="#" style="left: 100%;"></a><span class="ui-slider-pip ui-slider-pip-first ui-slider-pip-label ui-slider-pip-1" style="left: 0%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="1">1</span></span><span class="ui-slider-pip ui-slider-pip-label ui-slider-pip-2" style="left: 50.0000%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="2">2</span></span><span class="ui-slider-pip ui-slider-pip-last ui-slider-pip-label ui-slider-pip-3" style="left: 100%"><span class="ui-slider-line"></span><span class="ui-slider-label" data-value="3">3</span></span></div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="fabric-search fabric-search-results">
                                    <div>
                                        <div class="fabric-carousel">
                                            <div class="fabric-slide">


                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010805-0532" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Span Incase, Voyage by Momentum" alt="Span Incase, Voyage by Momentum" src="/content/dam/exemplis/textiles/momentum/span_incase/high_res/voyage.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Span Incase</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Voyage</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>4</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010805-1304" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Span Incase, Oat by Momentum" alt="Span Incase, Oat by Momentum" src="/content/dam/exemplis/textiles/momentum/span_incase/high_res/oat.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Span Incase</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Oat</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>4</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010805-0415" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Span Incase, Bark by Momentum" alt="Span Incase, Bark by Momentum" src="/content/dam/exemplis/textiles/momentum/span_incase/high_res/bark.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Span Incase</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Bark</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>4</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010805-0770" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Span Incase, Downtown by Momentum" alt="Span Incase, Downtown by Momentum" src="/content/dam/exemplis/textiles/momentum/span_incase/high_res/downtown.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Span Incase</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Downtown</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>4</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010805-1368" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Span Incase, Resort by Momentum" alt="Span Incase, Resort by Momentum" src="/content/dam/exemplis/textiles/momentum/span_incase/high_res/resort.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Span Incase</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Resort</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>4</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010792-0432" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Parkside Crypton, Pebble by Momentum" alt="Parkside Crypton, Pebble by Momentum" src="/content/dam/exemplis/textiles/momentum/parkside_crypton/high_res/pebble.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Parkside Crypton</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Pebble</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>5</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010792-0901" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Parkside Crypton, Parchment by Momentum" alt="Parkside Crypton, Parchment by Momentum" src="/content/dam/exemplis/textiles/momentum/parkside_crypton/high_res/parchment.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Parkside Crypton</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Parchment</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>5</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010792-0280" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Parkside Crypton, Annatto by Momentum" alt="Parkside Crypton, Annatto by Momentum" src="/content/dam/exemplis/textiles/momentum/parkside_crypton/high_res/annatto.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Parkside Crypton</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Annatto</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>5</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010792-0221" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Parkside Crypton, Breeze by Momentum" alt="Parkside Crypton, Breeze by Momentum" src="/content/dam/exemplis/textiles/momentum/parkside_crypton/high_res/breeze.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Parkside Crypton</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Breeze</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>5</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="26-0010792-0004" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Parkside Crypton, Antique by Momentum" alt="Parkside Crypton, Antique by Momentum" src="/content/dam/exemplis/textiles/momentum/parkside_crypton/high_res/antique.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Parkside Crypton</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Antique</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Momentum</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>5</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>5</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0072" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Sea Glass by Ultrafabrics" alt="Brisa Fresco, Sea Glass by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/sea_glass.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Sea Glass</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0096" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Tangerine by Ultrafabrics" alt="Brisa Fresco, Tangerine by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/tangerine.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Tangerine</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0087" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Elderberry by Ultrafabrics" alt="Brisa Fresco, Elderberry by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/elderberry.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Elderberry</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0084" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Azurite by Ultrafabrics" alt="Brisa Fresco, Azurite by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/azurite.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Azurite</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0099" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Whey by Ultrafabrics" alt="Brisa Fresco, Whey by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/whey.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Whey</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>

                                                <div class="fabric-swatch" klass="" data-partnumber="25-0830011-0092" data-sel="26-1430002-0010">

                                                    <figure>
                                                        <img title="Brisa Fresco, Patina by Ultrafabrics" alt="Brisa Fresco, Patina by Ultrafabrics" src="/content/dam/exemplis/textiles/ultrafabrics/brisa_fresco/high_res/patina.jpeg/_jcr_content/renditions/cq5dam.thumbnail.76.76.png">
                                                    </figure>
                                                    <div class="fabric-details hidden">
                                                        <table class="fabric-hover" cellpadding="10">

                                                            <tbody>
                                                                <tr>
                                                                    <td>Pattern:</td>
                                                                    <td>Brisa Fresco</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="54%">Colorway:</td>
                                                                    <td>Patina</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>By:</td>
                                                                    <td>Ultrafabrics</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Price:</td>
                                                                    <td class="price"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Grade</td>
                                                                    <td>6</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Lead Time:</td>
                                                                    <td>10</td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                        <button class="small text-center apply-btn">APPLY</button>
                                                        <button class="small text-center undo-btn">UNDO</button>

                                                    </div>

                                                </div>


                                            </div>
                                        </div>

                                        <div class="fabric-carousel-controls rows clearfix" style="display: block;">

                                            <div class="large-3 small-3 medium-3 columns no-pad-left"><a class="button tiny fabric-back disabled"><span class="fa fa-chevron-left"></span></a></div>
                                            <div class="fabric-pagination large-6 small-8 medium-6 columns text-center">Page 1 of 68</div>
                                            <div class="large-3 small-3 medium-3 columns"><a class="button tiny right fabric-next"><span class="fa fa-chevron-right"></span></a></div>

                                        </div>
                                    </div>
                                </div>

                            </div>

                        </section>

                        <section class="arm-style" id="Amplify-Arm">

                            <div class="heading">
                                <h4>ARM STYLE</h4>
                            </div>

                            <ul class="options unstyled">
                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="A0" name="ARM STYLE" product-id="A0" section-id="Amplify-Arm" checked="checked">
                                    <label for="A0" title="Armless - A0" class=" ">
                                        Armless - A0<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="A140" name="ARM STYLE" product-id="A140" section-id="Amplify-Arm">
                                    <label for="A140" title="Fixed Polyurethane - A140" class=" ">
                                        Fixed Polyurethane - A140<span class="price-delta"> [+$38.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="A141" name="ARM STYLE" product-id="A141" section-id="Amplify-Arm">
                                    <label for="A141" title="Height Adjustable - A141" class=" ">
                                        Height Adjustable - A141<span class="price-delta"> [+$48.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="A142" name="ARM STYLE" product-id="A142" section-id="Amplify-Arm">
                                    <label for="A142" title="Multi-Adjustable - A142" class=" ">
                                        Multi-Adjustable - A142<span class="price-delta"> [+$78.00]</span>
                                    </label>
                                </li>

                            </ul>

                        </section>

                        <section class="mechanism" id="Amplify-Mechanism">

                            <div class="heading">
                                <h4>MECHANISM</h4>
                            </div>

                            <ul class="options unstyled">
                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="T" name="MECHANISM" product-id="T" section-id="Amplify-Mechanism">
                                    <label for="T" title="Swivel Tilt - T" class=" ">
                                        Swivel Tilt - T<span class="price-delta"> [-$50.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="Amplify.Y" name="MECHANISM" product-id="Amplify.Y" section-id="Amplify-Mechanism" checked="checked">
                                    <label for="Amplify.Y" title="Enhanced Synchro - Y" class=" ">
                                        Enhanced Synchro - Y<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="Amplify.Ye3" name="MECHANISM" product-id="Amplify.Ye3" section-id="Amplify-Mechanism">
                                    <label for="Amplify.Ye3" title="Enhanced Synchro with Seat Depth Adjustment - Y/e3" class=" ">
                                        Enhanced Synchro with Seat Depth Adjustment - Y/e3<span class="price-delta"> [+$79.00]</span>
                                    </label>
                                </li>
                            </ul>

                        </section>

                        <section class="base" id="Amplify-Base-Black">

                            <div class="heading">
                                <h4>BASE</h4>
                            </div>

                            <ul class="options unstyled">

                                <li class="sub-heading">BASE MATERIAL:
							        <span>Black Nylon</span>

                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="B17" name="BASE MATERIAL" product-id="B17" subsection-id="Amplify-Base-Black-BaseColor" section-id="Amplify-Base-Black" checked="checked">
                                    <label for="B17" title="Black Nylon - B17" class=" ">
                                        Black Nylon - B17<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="B18" name="BASE MATERIAL" product-id="B18" subsection-id="Amplify-Base-Black-BaseColor" section-id="Amplify-Base-Black">
                                    <label for="B18" title="Polished Aluminum - B18" class=" ">
                                        Polished Aluminum - B18<span class="price-delta"> [+$113.00]</span>
                                    </label>
                                </li>

                                <li class="sub-heading">CYLINDER HEIGHT:
							        <span>Standard Cylinder - *N</span>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="*SN" name="CYLINDER HEIGHT" product-id="*SN" subsection-id="Amplify-Base-CylinderHeight" section-id="Amplify-Base-Black" checked="checked">
                                    <label for="*SN" title="Standard Cylinder" class=" ">
                                        Standard Cylinder<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="S13" name="CYLINDER HEIGHT" product-id="S13" subsection-id="Amplify-Base-CylinderHeight" section-id="Amplify-Base-Black">
                                    <label for="S13" title="Higher Cylinder - S13" class=" ">
                                        Higher Cylinder - S13<span class="price-delta"> [$0.00]</span>
                                    </label>
                                </li>

                                <li class="sub-heading">CASTERS:
							        <span>Carpet</span>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="C16" name="CASTERS" product-id="C16" subsection-id="Amplify-Base-Casters" section-id="Amplify-Base-Black" checked="checked">
                                    <label for="C16" title="Carpet - C16" class=" ">
                                        Carpet - C16<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="C17" name="CASTERS" product-id="C17" subsection-id="Amplify-Base-Casters" section-id="Amplify-Base-Black">
                                    <label for="C17" title="Hard Floor - C17" class=" ">
                                        Hard Floor - C17<span class="price-delta"> [+$30.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" type="radio" id="C3" name="CASTERS" product-id="C3" subsection-id="Amplify-Base-Casters" section-id="Amplify-Base-Black">
                                    <label for="C3" title="Glides for Task Chair - C3" class=" ">
                                        Glides for Task Chair - C3<span class="price-delta"> [+$30.00]</span>
                                    </label>
                                </li>

                            </ul>

                        </section>

                        <section class="packaging" id="Amplify-Packaging">

                            <div class="heading">
                                <h4>PACKAGING</h4>
                            </div>

                            <ul class="options unstyled">

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="KD" name="PACKAGING" product-id="KD" section-id="Amplify-Packaging" checked="checked">
                                    <label for="KD" title="Knocked Down - KD" class=" ">
                                        Knocked Down - KD<span class="price-delta"> </span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="UC" name="PACKAGING" product-id="UC" section-id="Amplify-Packaging">
                                    <label for="UC" title="Back Attached to Seat, Base Separate - UC" class=" ">
                                        Back Attached to Seat, Base Separate - UC<span class="price-delta"> [+$30.00]</span>
                                    </label>
                                </li>

                                <li class="part extras radio-option">
                                    <input data-dependentproducts="" data-rendering="0" type="radio" id="AC" name="PACKAGING" product-id="AC" section-id="Amplify-Packaging">
                                    <label for="AC" title="Fully Assembled in a Carton - AC" class=" ">
                                        Fully Assembled in a Carton - AC<span class="price-delta"> [+$80.00]</span>
                                    </label>
                                </li>
                            </ul>

                        </section>

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

