using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class selector : System.Web.UI.Page
{
    // Define Global Variable
    public string gSessionID = "";
    public string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><p><a href='/' class='button'>View all chairs</a></p>";

    protected void Page_Init(object sender, EventArgs e)
    {
        // Set the current step visual.
        NavProgress1.CurrentStepDisplay = "step2";

        if ((Request.QueryString["sid"] != null) && (Request.QueryString["id"] != null) && (Request.QueryString["value"] != null) && (Request.QueryString["catid"] != null) && (Request.QueryString["soid"] != null))
        {
            initializeChairOptions();
        }
        else
        {
            // If it is not existing
            Literal1.Text = error_timeout;
        }
        
    }

    protected void initializeChairOptions()
    {

        // URL Parameters were passed successfully, next try to call Configure Method
        var sessionId = Server.UrlDecode(Request.QueryString["sid"]);
        var selectionId = Server.UrlDecode(Request.QueryString["sid"]);
        var selectionValue = Server.UrlDecode(Request.QueryString["value"]);
        var screenCategoryID = Server.UrlDecode(Request.QueryString["catid"]);
        var screenOptionID = Server.UrlDecode(Request.QueryString["soid"]);

        // Step 1: Prepare Host Services Input Parameters
        var inputParams = new InputParameters
        {
            DetailId = Guid.NewGuid().ToString(),
            HeaderId = Guid.NewGuid().ToString(),
            PartNamespace = ConfigurationManager.AppSettings["via_partNamespace"],
            PartNumber = "ALL_CHAIRS",
            Profile = ConfigurationManager.AppSettings["via_profile"]
        };

        string redirectUrl = "http://viaseatingdev.azurewebsites.net/export.aspx";
        string instance = ConfigurationManager.AppSettings["via_instanceID"];
        string appId = ConfigurationManager.AppSettings["via_appID"];
        string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
        string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];

        try
        {
            // Step 2: Call the PrepareForInteractive Host services methods
            var hostServices = new HostServices(instance, appId, serviceUrl);
            var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

            // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
            var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
            //var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);
            var CurrentID = "";
            gSessionID = sessionId;

            // First Init get the Chair Category selection.
            CurrentID = getScreenSelection(configUiClient, sessionId, "", "", screenCategoryID);

            // First Init get the Chair Category selection.
            if (CurrentID != "NotFound")
            {
                CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, screenCategoryID, screenCategoryID);
            }

            // Second Init get the Chair Family
            if (CurrentID != "NotFound")
            {
                CurrentID = getScreenSelection(configUiClient, sessionId, "", "", selectionValue);
            }

            // Second get the Chair Family and Show Options
            if (CurrentID != "NotFound")
            {
                CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, selectionValue, selectionValue);
                showChairOptions(configUiClient, sessionId, CurrentID, selectionValue, "GOOD_BETTER_BEST");
            }
        }
        catch (Exception ex)
        {
            // API Method call failed due to session timeout or undefined series ruleset.
            Literal1.Text = error_timeout;
            Literal2.Text = ex.ToString();
        }

    }

    protected string getScreenSelection(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID, string OptSelectID, string OptSelectValue, string SearchValue)
    {
        // Setup selections for ScreenOptions
        var selections = new ProdConfigUI.OptionSelection[0];

        // Set selection variable for ScreenOption ID and Value 
        if ((!string.IsNullOrEmpty(OptSelectID)) && (!string.IsNullOrEmpty(OptSelectValue)))
        {
            var optionSelections = new ProdConfigUI.OptionSelection
            {
                ID = OptSelectID,
                Value = OptSelectValue
            };
            selections = new[] { optionSelections };
        }

        var UiData = UIClient.Configure(SessionID, selections);
        var numPages = UiData.Pages.Length;
        var numScreens = UiData.Pages[0].Screens.Length;
        var screenOptionID = "";
        bool selectionFound = false;
        bool exitloop = false;

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    foreach (var screenoption in screen.ScreenOptions)
                    {
                        // ScreenOptions is only 1 Dimension Deep [0] - Loop through ScreensOptions/ScreenOption/SelectableValues/
                        foreach (var select in screenoption.SelectableValues)
                        {
                            // Find the ScreenOption Value  
                            if (select.Value == SearchValue)
                            {
                                screenOptionID = screenoption.ID;
                                //Literal2.Text += "SID=" +SessionID + " ID=" + screenOptionID + " VALUE=" + SearchValue + "<br />";

                                selectionFound = true;
                                exitloop = true;
                                break;
                            }
                        }
                        if (exitloop) break;
                    }
                    if (exitloop) break;
                }
            }
        }

        if (selectionFound)
        {
            return screenOptionID;
        }
        else
        {
            return "NotFound";
        }
    }

    protected void showChairOptions(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID, string OptSelectID, string OptSelectValue, string SearchValue)
    {
        // Setup selections for ScreenOptions
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = UIClient.Configure(SessionID, selections);
        var numPages = UiData.Pages.Length;
        var numScreens = UiData.Pages[0].Screens.Length;

        // Set the Page SubTitle the name of the series
        HeaderContent.SeriesTitle = " > " + OptSelectValue;

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    foreach (var screenoption in screen.ScreenOptions)
                    {
                        // Find the ScreenOption Value  
                        if (screenoption.Name == SearchValue)
                        {
                            int count = 0;
                            // ScreenOptions is only 1 Dimension Deep [0] - Loop through ScreensOptions/ScreenOption/SelectableValues/
                            foreach (var select in screenoption.SelectableValues)
                            {
                                //Literal2.Text += "Name=" + screenoption.Name + " Value=" + select.Value + "<br />";
                                // Build the url parameters for the chair builder page
                                // Set first parameter to the Chair Servies value
                                var list_properties = "{\"Options\" : [{\"name\" : \"RULE\", \"value\" : \"" + OptSelectValue + "\"}";
                                var btn_name = "";
                                var selectTop = "";
                                var selectBottom = "";

                                foreach (var property in select.CustomProperties)
                                {
                                    // Add each Custom Property as a URL parameter. Skip Model_String and Starting_At.
                                    if (property.Name != "MODEL_STRING")
                                    {
                                        // Get the price value to display for the button name.
                                        if (property.Name == "STARTING_AT")
                                        {
                                            btn_name = property.Value;
                                        }
                                        else
                                        {
                                            list_properties += ", {\"name\" : \"" + property.Name + "\", \"value\" : \"" + property.Value + "\"}";
                                        }
                                    }
                                }

                                list_properties += "] }";
                                count += 1;
                                // Check if for the first item in a row.
                                if ((count - 1 % 3) == 0)
                                {
                                    selectTop = "<div class='row clearfix'>";
                                }

                                selectTop += "<div class='box three'>";
                                selectTop += "<div class='featured_image img_loaded'>";
                                selectTop += "<img src='" + select.ImageLink + "' alt='" + select.Caption + "'>";
                                selectTop += "</div>";
                                selectTop += "<div class='row clearfix'><div>";
                                selectTop += "<h3 class='featured_article_title fade animated' data-rt-animate='animate' data-rt-animation-type='fade' data-rt-animation-group='single' style='-webkit-animation: 0s;'>" + select.Caption + "</h3>";

                                // Check if there is a custom property value set to be used for button name. If not the set to the selection value.
                                if (btn_name == "")
                                { btn_name = select.Value; }

                                //Literal1.Text += "<a href='/builder.aspx" + list_properties + "' class='button'>" + btn_name + "</a>";
                                selectBottom = "</div></div>";
                                selectBottom += "</div>";

                                // Close div if the item was third.
                                if (((count % 3) == 0))
                                {
                                    selectBottom += "</div><br />";
                                }

                                // Dynamically build chair selections
                                // Create Chair Header HTML
                                Literal lt = new Literal();
                                lt.ID = "LiteralTop" + count.ToString();
                                lt.Text = selectTop;

                                // Create Button
                                Button b = new Button();
                                b.ID = "Button" + count.ToString();
                                b.Text = btn_name;
                                b.CssClass = "button";
                                //b.PostBackUrl = "~/builder2.aspx";
                                b.CommandArgument = list_properties;
                                //b.OnClientClick = "b_Click";
                                //b.Click += new EventHandler(b_Click);
                                b.Click += (se, ev) => b_Click(se, ev, list_properties);
                                //b.Click += (se, ev) => b_Click(se, ev);

                                //Create Hidden Chair Properties (Options)
                                HiddenField h = new HiddenField();
                                h.ID = "Hidden" + count.ToString();
                                h.Value = list_properties;

                                // Create Chair Header HTML
                                Literal lb = new Literal();
                                lb.ID = "LiteralBottom" + count.ToString();
                                lb.Text = selectBottom;

                                // Write the Controls to the Panel
                                Panel1.Controls.Add(lt);
                                Panel1.Controls.Add(b);
                                Panel1.Controls.Add(h);
                                Panel1.Controls.Add(lb);

                            }

                            // Close div when row contains less than three items.
                            if (((count % 3) != 0))
                            {
                                Literal1.Text += "</div><br />";
                            }
                        }
                    }
                }
            }
        }
    }

    protected void b_Click(object sender, EventArgs e, string chairparms)
    {
        // Your process you want to do on click.
        // put 2 buttons one inside update panel and another outside of update panel so when inside update panel button's click event will 
        // fire then it will do database operations and after successful db operations it will call a java script function from server side

        ChairSelect.Value = chairparms;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "displayNote", "   <script language='JavaScript'>onAsyncPostclick();</script>", false);
    }

    protected void btnHelloWorld_Click(object sender, EventArgs e)
    {
        // Call server.transfer on ajax enabled postback click and saved code from page request Manager exception
        Literal2.Text = "Hello, world - this is a fresh message from ASP.NET AJAX! The time right now is: " + DateTime.Now.ToLongTimeString();
        Server.Transfer("builder3.aspx", false);
    }

}