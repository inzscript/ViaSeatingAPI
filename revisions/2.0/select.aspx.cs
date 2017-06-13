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
    protected void Page_Init(object sender, EventArgs e)
    {
        // Get Values from posted source page
        string sessionId = "";
        string selectionId = "";
        string selectionValue = "";
        string screenOptionID = "";
        string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><a href='/' class='button'>View all chairs</a>";
        

        //if (!IsPostBack)
        //{
            // Set the current step visual.
            NavProgress1.CurrentStepDisplay = "step2";

            if ((Request.QueryString["sid"] != null) && (Request.QueryString["id"] != null) && (Request.QueryString["value"] != null) && (Request.QueryString["soid"] != null))
            {               
                // URL Parameters were passed successfully, next try to call Configure Method
                sessionId = Request.QueryString["sid"];
                selectionId = Request.QueryString["sid"];
                selectionValue = Request.QueryString["value"];
                screenOptionID = Request.QueryString["soid"];

                string instance = ConfigurationManager.AppSettings["via_instanceID"];
                string appId = ConfigurationManager.AppSettings["via_appID"];
                string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
                string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];
                
                try
                {
                    // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
                    var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
                    // Step 4: Call the UI Service Configure method to retrieve the first set of user interface elements
                    var optionSelection = new ProdConfigUI.OptionSelection
                    {
                        ID = selectionId,
                        Value = selectionValue
                    };

                    var selections = new[] { optionSelection };
                    var UiData = configUiClient.Configure(sessionId, selections);
                
                    // Re-initiate the session call to Configuration Method :: Begin
                    int optionIndex = Convert.ToInt32(screenOptionID);
                    var changedScreenOption = UiData.Pages[0].Screens[0].ScreenOptions[0];
                    var optionSelection2 = new ProdConfigUI.OptionSelection
                    {
                        ID = UiData.Pages[0].Screens[0].ScreenOptions[0].ID,
                        Value = UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues[optionIndex].Value
                    };
                    var selections2 = new[] { optionSelection2 };
                    var UiData2 = configUiClient.Configure(sessionId, selections2);
                    // Re-initiate :: End

                    // Set the Page SubTitle the name of the series
                    var SeriesValue = UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues[optionIndex].Value;
                    HeaderContent.SeriesTitle = " > " + SeriesValue;
                    Literal1.Text = "";
                    
                    int count = 0;
                    foreach (var select in UiData2.Pages[0].Screens[1].ScreenOptions[0].SelectableValues)
                    {
                        // Build the url parameters for the chair builder page
                        // Set first parameter to the Chair Servies value
                        var list_properties = "{\"Options\" : [{\"name\" : \"RULE\", \"value\" : \"" + SeriesValue + "\"}";
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
                catch (Exception ex)
                {
                    // API Method call failed due to session timeout or undefined series ruleset.
                    Literal1.Text = error_timeout;
                }
            }
            else
            {
                // If it is not existing
                Literal1.Text = error_timeout;
            }
        //}
        
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