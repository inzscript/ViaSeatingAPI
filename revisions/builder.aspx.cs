    using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class builder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Get Values from posted source page
        string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><a href='/' class='button'>View all chairs</a>";
        string sSeries = "";
        string sModel = "";
        string sArm = "";
        string sArmCap = "";
        string sBase = "";
        string sCasters = "";
        string sRuleSet = "";

        if (!IsPostBack)
        {
            // Set the current step visual.
            NavProgress1.CurrentStepDisplay = "step3";
            //if ((Request.QueryString["SERIES"] != null) && (Request.QueryString["MODEL"] != null) && (Request.QueryString["ARM"] != null) && (Request.QueryString["ARMCAP"] != null) && (Request.QueryString["BASE"] != null) && (Request.QueryString["CASTERS"] != null) && (Request.QueryString["rule"] != null))
            if (Request.QueryString.AllKeys.Length > 0)
            {
                //URL Parameters were passed successfully, next try to call Configure Method

                string[,] chairparms = new string[30,2];

                // Retreive chair selections from url query parameters.
                int idx = 0;
                foreach (var key in Request.QueryString.AllKeys)
                {
                    if (key == "rule")
                    {
                        sRuleSet = Request.QueryString[key];
                    }
                    else
                    {
                        if (key != "CHAIR_FAMILY")
                        {
                            chairparms[idx, 0] = Server.UrlDecode(key.ToString());
                            chairparms[idx, 1] = Server.UrlDecode(Request.QueryString[key]);
                            idx = idx + 1;
                            Literal1.Text += Server.UrlDecode(key.ToString()) + " : " + Server.UrlDecode(Request.QueryString[key]) + "<br>";
                        }
                    }
                }

                sSeries = Request.QueryString["SERIES"];
                sModel = Request.QueryString["MODEL"];
                sArm = Request.QueryString["ARM"];
                sArmCap = Request.QueryString["ARMCAP"];
                sBase = Request.QueryString["BASE"];
                sCasters = Request.QueryString["CASTERS"];
                //sRuleSet = Request.QueryString["rule"];
                //sSeries = "1803";
                //sModel = "3C";
                //sArm = "10A";
                //sArmCap = "5";
                //sBase = "18BB";
                //sCasters = "16HP";

                string redirectUrl = "http://viaseatingdev.azurewebsites.net/export.aspx";
                string instance = ConfigurationManager.AppSettings["via_instanceID"];
                string appId = ConfigurationManager.AppSettings["via_appID"];
                string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
                string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];

                // Step 1: Prepare Host Services Input Parameters
                var inputParams = new InputParameters
                {
                    DetailId = Guid.NewGuid().ToString(),
                    HeaderId = Guid.NewGuid().ToString(),
                    PartNamespace = ConfigurationManager.AppSettings["via_partNamespace"],
                    PartNumber = sRuleSet,
                    Profile = ConfigurationManager.AppSettings["via_profile"]
                };
                // Quantity needed for Brisbane Rule Set
                inputParams.IntegrationParameters.Add("Quantity", 1);

                try
                {

                    // Step 2: Call the PrepareForInteractive Host services methods
                    var hostServices = new HostServices(instance, appId, serviceUrl);
                    var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

                    // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
                    var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
                    var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);

                    var CurrentID = "";

                    // Select default series chair selection options
                    int keynum = chairparms.GetUpperBound(0);
                    //int value = chairparms.GetUpperBound(1);

                    for (int keyid = 0; keyid <= keynum; keyid++)
                    {
                        string parmvalue = chairparms[keyid, 1];
                        string parmkey = chairparms[keyid, 0];

                        // Skip the Shipping Status due to function call on Infor
                        if (parmkey != "CHAIRUPHSTATUSXX")
                        { 

                            if (parmvalue != "TRUE" && parmvalue != "FALSE")
                            {
                                Literal1.Text += parmvalue;
                                if (keyid == 0)
                                { CurrentID = getScreenSelection(configUiClient, sessionId, "", "", parmvalue); }
                                CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, parmvalue, parmvalue);
                            }
                        }
                    }

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sSeries);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sSeries, sSeries);

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sModel);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sModel, sModel);

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sArm);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sArm, sArm);

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sArmCap);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sArmCap, sArmCap);

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sBase);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sBase, sBase);

                    //CurrentID = getScreenSelection(configUiClient, sessionId, "", "", sCasters);
                    //CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, sCasters, sCasters);

                    // showScreenOptions(configUiClient, sessionId);
                    showScreenSelection(configUiClient, sessionId);
                }
                catch (Exception ex)
                {
                    // API Method call failed due to session timeout or undefined series ruleset.
                    Literal2.Text = error_timeout;
                    Literal2.Text = ex.ToString();
                }
            } else
            {
                    // If it is not existing
                    Literal2.Text = error_timeout;
            }
        }
    }

    // Function: getScreenSelection
    // Parameters: UiClient, SessionID, OptionSelectionID, OptionSelectionValue, SearchValue
    // Decription:
    protected string getScreenSelection(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID, string OptSelectID, string OptSelectValue, string SearchValue)
    {
        // Initialize Variables
        // Setup API Call: Configure( SessionID, OptionSelection(ID, Value))
        var selections = new ProdConfigUI.OptionSelection[0];
        
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

        if (numPages >= 1) {
            if (numScreens >= 1)
            {
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    foreach (var screenoption in screen.ScreenOptions)
                    { 
                        // ScreenOptions is only 1 Dimension Deep [0] - Loop through ScreensOptions/ScreenOption/SelectableValues/
                        foreach (var select in screenoption.SelectableValues)
                        {
                            if (select.Value == SearchValue)
                            {
                                screenOptionID = screenoption.ID;
                                //Literal1.Text += screenoption.Caption + "(" + select.Value + ":" + numScreens + ") - ";
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
        } else
        {
            return "NotFound";
        }
    }


    // Function: getScreenSelection
    // Parameters: UiClient, SessionID, OptionSelectionID, OptionSelectionValue, SearchValue
    // Decription:
    protected void showScreenSelection(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID)
    {
        // Initialize Variables
        // Setup API Call: Configure( SessionID, OptionSelection(ID, Value))
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = UIClient.Configure(SessionID, selections);
        var numPages = UiData.Pages.Length;
        var numScreens = UiData.Pages[0].Screens.Length;
        var screenIDX = 0;
        var screenOptionIDX = 0;
        var sListOption1 = "";
        var sListOption2 = "";
        var sListOption3 = "";
        var sListOption4 = "";
        var sListOption5 = "";
        var sListOption6 = "";

        var sSeries = Request.QueryString["SERIES"];
        var sModel = Request.QueryString["MODEL"];
        var sArm = Request.QueryString["ARM"];
        var sArmCap = Request.QueryString["ARMCAP"];
        var sBase = Request.QueryString["BASE"];
        var sCasters = Request.QueryString["CASTERS"];
        var inputcheck = "";

        SectionOptions.Text = "";

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    if (screenIDX > -1)
                    {
                        screenOptionIDX = 0;
                        foreach (var screenoption in screen.ScreenOptions)
                        {
                            if (screenOptionIDX == 0) {
                                SectionOptions.Text += "<section class='" + screenoption.Name + "' id='" + screenoption.Name + "'>";
                                SectionOptions.Text += "<div class='heading'><h4>" + screenoption.Caption + "</h4></div>";
                                SectionOptions.Text += "<ul class='options unstyled'>";
                            }

                            sListOption1 = ""; sListOption2 = ""; sListOption3 = ""; sListOption4 = ""; sListOption5 = ""; sListOption6 = "";
                            foreach (var select in screenoption.SelectableValues)
                            {
                                if (select.Value == sSeries || select.Value == sModel || select.Value == sArm || select.Value == sArmCap || select.Value == sBase || select.Value == sCasters)
                                {
                                    inputcheck = " checked='checked'";
                                } else {
                                    inputcheck = "";
                                }

                                sListOption1 = "<li class='part extras radio-option'>";
                                sListOption2= "<input type='radio' id='" + select.Value + "' name='" + screenoption.Name + "' product-id='" + select.Value + "' section-id='" + screenoption.Name + "'" + inputcheck + "></input>";
                                sListOption3= "<label for='" + select.Value + "' title='" + select.Caption + "' class=''>" + select.Caption;

                                foreach (var property in select.CustomProperties)
                                {
                                    if (property.Name == "Price" || property.Name == "PRICE")
                                    {
                                        if (property.Value != "0" && property.Value != null && property.Value != "") {
                                            sListOption4 = "<span class='price-delta'> [+$" + property.Value + ".00]</span>";
                                        } else {
                                            sListOption4 = "";
                                            sListOption2 = "<input type='radio' id='" + select.Value + "' name='" + screenoption.Name + "' product-id='" + select.Value + "' section-id='" + screenoption.Name + "' checked='checked'></input>";
                                        }
                                    }
                                }

                                sListOption5= "</label>";
                                sListOption6= "</li>";

                                SectionOptions.Text += sListOption1 + sListOption2 + sListOption3 + sListOption4 + sListOption5 + sListOption6;
                            }
                            if (screenOptionIDX == 0)
                            {
                                SectionOptions.Text += "</ul>";
                                SectionOptions.Text += "</section>";
                            }
                            screenOptionIDX++;
                        }
                    }
                    screenIDX++;
                }
            }
        }
    }


    // Function: getScreenSelection
    // Parameters: UiClient, SessionID, OptionSelectionID, OptionSelectionValue, SearchValue
    // Decription:
    protected void showScreenOptions(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID)
    {
        // Initialize Variables
        // Setup API Call: Configure( SessionID, OptionSelection(ID, Value))
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = UIClient.Configure(SessionID, selections);
        var numPages = UiData.Pages.Length;
        var numScreens = UiData.Pages[0].Screens.Length;
        var selectIDX = 0;

        var screenOptionID = "";
        bool selectionFound = false;
        bool exitloop = false;

        String[] chairDetails = { "1803", "3C", "10A", "5", "18BB", "16HP" };
        Literal1.Text = "";

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
                            if (selectIDX < 9)
                            {
                                // Display Chair Summary Section
                                foreach (var detail in chairDetails)
                                {
                                    if (select.Value == detail)
                                    {
                                        Literal1.Text += "<dt>" + String.Format(TitleCaseString(screenoption.Name)) + "</dt><dd>" + String.Format(TitleCaseString(select.Caption)) + "</dd>";
                                        selectionFound = true;
                                    }
                                }
                            }
                            else
                            {
                                // Display Chair Selection Options
                            }
                        }
                    }
                }
            }
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

    private static void ShowArrayInfo(string Caption, Array arr)
    {
        Console.WriteLine("Length of Array:      {0,3}", arr.Length);
        Console.WriteLine("Number of Dimensions: {0,3}", arr.Rank);
        // For multidimensional arrays, show number of elements in each dimension.
        if (arr.Rank > 1)
        {
            for (int dimension = 1; dimension <= arr.Rank; dimension++)
                Console.WriteLine("   Dimension {0}: {1,3}", dimension,
                                  arr.GetUpperBound(dimension - 1) + 1);
        }
        Console.WriteLine();
    }
}