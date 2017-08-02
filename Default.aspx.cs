using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class _Default : Page
{
    public string gCategoryList = "";
    public string gSeriesList = "";
    public int gCount = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the current step visual.
        NavProgress1.CurrentStepDisplay = "step1";

        // Step 1: Prepare Host Services Input Parameters
        var inputParams = new InputParameters
        {
            DetailId = Guid.NewGuid().ToString(),
            HeaderId = Guid.NewGuid().ToString(),
            PartNamespace = ConfigurationManager.AppSettings["via_partNamespace"],
            PartNumber = "ALL_CHAIRS",
            Profile = ConfigurationManager.AppSettings["via_profile"]
        };

        string error_timeout = "<h2>Chair Builder Session Error</h2><p>Please standby as we determine why your session failed..</p><a href='/' class='button'>Retry Chair Builder</a>";
        string instance = ConfigurationManager.AppSettings["via_instanceID"];
        string appId = ConfigurationManager.AppSettings["via_appID"];
        string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
        string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];
        string redirectUrl = "http://viaseatingdev.azurewebsites.net/export.aspx";

        try
        {
            // Step 2: Call the PrepareForInteractive Host services methods
            var hostServices = new HostServices(instance, appId, serviceUrl);
            var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

            // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
            var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
            var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);

            // Step 4: Call the UI Service Configure method to retrieve the first set of user interface elements
            var selections = new ProdConfigUI.OptionSelection[0];
            var UiData = configUiClient.Configure(sessionId, selections);

            Literal1.Text = "<div class='row clearfix'>";
            Literal2.Text = "<div id='tabs'>";
            gCategoryList = "<ul>";
            var CurrentID = "";

            int count = 0;
            foreach (var select in UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues)
            {
                count += 1;
                gCount = count;
                // First ScreenSelection call will return chair categories.
                CurrentID = getScreenSelection(configUiClient, sessionId, "", "", select.Value);

                // Second ScreenSelection call will return each chair in families.
                if (CurrentID != "NotFound")
                {
                    gSeriesList += "<div id='tabs-" + count + "'>";
                    gSeriesList += "<div class='row clearfix'>";
                    CurrentID = displayScreenSelection(configUiClient, sessionId, CurrentID, select.Value, select.Value);
                    gSeriesList += "</div>";
                }
            }

            Literal2.Text += gCategoryList + "</ul>" + gSeriesList + "</div></div>";
        }
        catch (Exception ex)
        {
            // API Method call failed due to session timeout or undefined series ruleset.
            Literal1.Text = error_timeout;
            Literal1.Text = ex.ToString();
        }
    }

    protected string displayScreenSelection(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID, string OptSelectID, string OptSelectValue, string SearchValue)
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
                var screenIDX = 0;
                foreach (var screen in UiData.Pages[0].Screens)
                {

                    if (screenIDX > 0)
                    {
                        var screenOptIDX = 0;
                        foreach (var screenoption in screen.ScreenOptions)
                        {
                            int count = 0;
                            foreach (var select in screenoption.SelectableValues)
                            {
                                count += 1;
                                //gSeriesList += "<p>Chair: " + select.Value + "</p>";
                                gSeriesList += "<div class='box three'>";
                                gSeriesList += "<div class='featured_image img_loaded'>";
                                gSeriesList += "<img src='" + select.ImageLink + "' alt='" + select.Value + "' />";
                                gSeriesList += "</div>";
                                gSeriesList += "<div class='row clearfix'><div>";
                                
                                gSeriesList += "<a href='/select.aspx?sid=" + SessionID + "&id=" + screenoption.ID + "&value=" + select.Value + "&catid=" + SearchValue + "&soid=" + (count - 1).ToString() + "' class='button'>" + select.Value + "</a>";
                                gSeriesList += "<h2 class='featured_article_title fade animated' style='-webkit-animation: 0s;'>" + TitleCaseString(select.Caption) + "</h2>";
                                gSeriesList += "</div></div>";
                                gSeriesList += "</div>";
                                //// Close div if the item was third.
                                if ((count % 3) == 0)
                                {
                                    gSeriesList += "</div><br />";
                                    gSeriesList += "<div class='row clearfix'>";
                                }
                            }
                            // Close div when row contains less than three items.
                            //if ((count % 3) != 0)
                            //{
                                gSeriesList += "</div><br />";
                            //}
                            screenOptIDX++;
                        }
                    }
                    screenIDX++;
                }
            }
            // Set Global Variable ImageURL with the current ScreenOption image link
            //ImageURL = UiData.ImageUrl.ToString();
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
                                //Literal2.Text += SessionID + " : " + screenOptionID + " : " + SearchValue + "<br />";
                                gCategoryList += "<li><a href='#tabs-" + gCount + "'>" + select.Value + "</a></li>";

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

    public static String TitleCaseString(String s)
    {
        if (s == null) return s;

        String[] words = s.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length == 0) continue;

            Char firstChar = Char.ToUpper(words[i][0]);
            String rest = "";
            if (words[i].Length > 1)
            {
                rest = words[i].Substring(1).ToLower();
            }
            words[i] = firstChar + rest;
        }
        return String.Join(" ", words);
    }
}