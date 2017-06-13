using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;
using System.IO;
using System.Data;
using Newtonsoft.Json;

public partial class builder2 : System.Web.UI.Page
{
    public class ChairOptions
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public string ImageURL = "";

    public DataSet dataset;
    public DataTable dataTable;
    public DataSet sessionDataset;

    protected void Page_Load(object seender, EventArgs e)
    {
        if (Page.IsAsync)
        {
            ViaImageURL.Text += "IsAsync";
        }

        if (Page.IsCallback)
        {
            ViaImageURL.Text += "IsCallBack";
        }
        if (Page.IsPostBack)
        {
            ViaImageURL.Text += "IsPostBack";

            if (Session["sessionDataset"] != null)
            {
                ViaImageURL.Text += " - Data Passed";
            }
            // Get Values from posted source page
            string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><a href='/' class='button'>View all chairs</a>";
            string sRuleSet = "";

            ////dataset = (DataSet)(Session["sessionDataset"]);
            //dataset = JsonConvert.DeserializeObject<DataSet>((string)(Session["sessionDataset"]));
            //dataTable = dataset.Tables["Options"];

        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        // Get Values from posted source page
        string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><a href='/' class='button'>View all chairs</a>";
        string sRuleSet = "";
        //DataSet dataset;
        //DataTable dataTable;

        //if (Page.IsAsync)
        //{
        //    ViaImageURL.Text += "IsAsync";
        //}

        //if (Page.IsCallback)
        //{
        //    ViaImageURL.Text += "IsCallBack";
        //}
        //if (Page.IsPostBack)
        //{
        //    ViaImageURL.Text += "IsPostBack";

        //    if (Session["sessionDataset"] != null)
        //    {
        //        ViaImageURL.Text += " - Data Passed";
        //    }
        //}
        if (Page.IsPostBack)
        {
            //Session.Abandon();
            ViaImageURL.Text += "Init - PostBack";
        }

        if (!Page.IsPostBack)
        {

            if (PreviousPage != null)
            {
                Control placeHolder = PreviousPage.Controls[0].FindControl("ContentPlaceHolder1");
                HiddenField SourceTextBox = (HiddenField)placeHolder.FindControl("ChairSelect");
                if (SourceTextBox != null)
                {
                    // Get the JSON data from step 2 select page.
                    Literal2.Text += "sourcetext found";
                    dataset = JsonConvert.DeserializeObject<DataSet>(SourceTextBox.Value);
                    Session["sessionDataset"] = SourceTextBox.Value;
                    //Session.Remove("sessionDataset");

                    dataTable = dataset.Tables["Options"];

                    Literal1.Text += "<br>Count:" + dataTable.Rows.Count + "<br>";

                    // Set the current step visual.
                    NavProgress1.CurrentStepDisplay = "step3";

                    // Check that chair options were passed
                    if (dataTable.Rows.Count > 0)
                    {
                        //URL Parameters were passed successfully, next try to call Configure Method
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string parmvalue = row["value"].ToString();
                            string parmkey = row["name"].ToString();
                            Literal1.Text += parmkey + ": " + parmvalue + "<br>";

                            if (parmkey == "RULE")
                            {
                                sRuleSet = parmvalue;
                            }
                        }

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

                            // Parse the json data passed from the select page.
                            var vidx = 1;
                            foreach (DataRow row in dataTable.Rows)
                            {

                                string parmvalue = row["value"].ToString();
                                string parmkey = row["name"].ToString();

                                if (parmkey != "RULE" && parmkey != "CHAIR_FAMILY")
                                {
                                    if (parmvalue == "TRUE")
                                    { parmvalue = "True"; }
                                    if (parmvalue == "FALSE")
                                    { parmvalue = "False"; }


                                    // if (parmvalue != "SINGLE UPH")
                                    CurrentID = getScreenSelection(configUiClient, sessionId, "", "", parmvalue);

                                    //if (CurrentID != "NotFound" && parmvalue != "SINGLE UPH")
                                    if (CurrentID != "NotFound")
                                    {
                                        //Literal1.Text += parmvalue + ",";
                                        CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, parmvalue, parmvalue);
                                    }
                                }
                                vidx++;
                            }

                            showScreenSelection(configUiClient, sessionId);
                        }
                        catch (Exception ex)
                        {
                            // API Method call failed due to session timeout or undefined series ruleset.
                            Literal2.Text = error_timeout;
                            Literal2.Text = ex.ToString();
                        }
                    }
                    else
                    {
                        // If it is not existing
                        Literal2.Text = error_timeout;
                    }
                }
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

        //Literal1.Text += numPages + ":" + numScreens + ":";

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
                            // Find the value  
                            if (select.Value == SearchValue)
                            {
                                screenOptionID = screenoption.ID;
                                Literal2.Text += SessionID + " : " + screenOptionID + " : " + SearchValue + "<br />";
                                // Get the current option image link
                                ImageURL = select.ImageLink;
                                ViaImageURL.Text += "Image URL:" + ImageURL;

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
            //Literal1.Text += "notfound,";
            return "NotFound";
        }
    }


    // Function: showScreenSelection
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
        var screenOptIDX = 0;
        var selectIDX = 0;

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    if (screenIDX > -1)
                    {
                        screenOptIDX = 0;
                        foreach (var screenoption in screen.ScreenOptions)
                        {
                            if (screenOptIDX == 0) {
                                // Dynamically create Chair Options ListBox
                                Literal ltitle = new Literal();
                                ltitle.ID = "OptionTitle" + screenOptIDX.ToString();

                                ltitle.Text = "<section class='" + screenoption.Name + "' id='" + screenoption.Name + "'>";
                                ltitle.Text += "<div class='heading'><h4>" + screenoption.Caption + "</h4></div>";
                                ltitle.Text += "<ul class='options unstyled'>";

                                //UpdatePanel1.ContentTemplateContainer.Controls.Clear();
                                Panel1.Controls.Add(ltitle);
                            }

                            selectIDX = 0;
                            foreach (var select in screenoption.SelectableValues)
                            {
                                // Dynamically create Literal for Screen Option Selectable Values
                                Literal lbegin = new Literal();
                                lbegin.ID = "OptionBegin" + screenOptIDX.ToString() + selectIDX.ToString();
                                Literal lend = new Literal();
                                lend.ID = "OptionEnd" + screenOptIDX.ToString() + selectIDX.ToString();

                                RadioButton OptionRadioBtn = new RadioButton();
                                OptionRadioBtn.ID = "RadioBtn_" + screenoption.Name + selectIDX.ToString();
                                OptionRadioBtn.GroupName = screenoption.Name;
                                OptionRadioBtn.Checked = false;
                                OptionRadioBtn.Enabled = true;
                                OptionRadioBtn.Text = select.Caption;
                                OptionRadioBtn.EnableViewState = true;
                                OptionRadioBtn.Attributes.Add("Value", select.Value);
                                //OptionRadioBtn.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                                OptionRadioBtn.CheckedChanged += (se, ev) => radioButton_CheckedChanged(se, ev);
                                OptionRadioBtn.AutoPostBack = true;
                                OptionRadioBtn.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                                if (IsInChairOption(screenoption.Name, select.Value))
                                {
                                    OptionRadioBtn.Checked = true;
                                }

                                lbegin.Text = "<li class='part extras radio-option'>";

                                foreach (var property in select.CustomProperties)
                                {
                                    // Check Custom Properties of Select Options for price and display if found
                                    if (property.Name == "Price" || property.Name == "PRICE")
                                    {
                                        if (property.Value != "0" && property.Value != null && property.Value != "") {
                                            OptionRadioBtn.Text += "<span class='price-delta'> [+$" + property.Value + ".00]</span>";
                                        } 
                                    }
                                }

                                lend.Text += "</li>";
                                Panel1.Controls.Add(lbegin);
                                Panel1.Controls.Add(OptionRadioBtn);
                                Panel1.Controls.Add(lend);

                                //AsyncPostBackTrigger addTrigger = new AsyncPostBackTrigger();
                                //addTrigger.ControlID = OptionRadioBtn.ID;
                                //addTrigger.EventName = "radioButton_CheckedChanged";
                                //UpdatePanel1.Triggers.Add(addTrigger);

                                selectIDX++;
                            }

                            if (screenOptIDX == 0)
                            {
                                // Dynamically create Chair Options ListBox
                                Literal lfooter = new Literal();
                                lfooter.ID = "OptionFooter" + screenOptIDX.ToString();

                                lfooter.Text = "</ul>";
                                lfooter.Text += "</section>";
                                Panel1.Controls.Add(lfooter);
                            }
                            screenOptIDX++;
                        }
                    }
                    screenIDX++;
                }
            }
        }
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
        Session["sessionDataset"] = dataset;
        RadioButton btn = sender as RadioButton;
        Literal1.Text = "What up Player";
    }

    // Function: IsInChairOption
    // Parameters: OptionName, OptionValue
    // Decription: Checks the chair strings passed in OptionName and OptionValue if they exist in the 
    //    default parameters passed in the JSON data.
    public bool IsInChairOption(string OptionName, string OptionValue)
    {
        bool selectionFound = false;

        if (dataTable.Rows.Count > 0)
        {
            // Check each parameter to see if found
            foreach (DataRow row in dataTable.Rows)
            {
                string parmkey = row["name"].ToString();
                string parmvalue = row["value"].ToString();

                // Convert boolean values to proper case
                if (parmvalue == "TRUE")
                { parmvalue = "True"; }
                if (parmvalue == "FALSE")
                { parmvalue = "False"; }

                if (parmkey == OptionName && parmvalue == OptionValue)
                {
                    selectionFound = true;
                    break;
                }
            }
        }
        // Return boolean option if found
        return selectionFound;

    }

    protected void lb_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListBox listBox = (ListBox)sender;
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