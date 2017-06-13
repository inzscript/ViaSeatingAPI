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
using System.Text.RegularExpressions;

public partial class builder3 : System.Web.UI.Page
{
    public class ChairOptions
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public string ImageURL = "";
    public string RuleSet = "";

    public DataSet dataset;
    public DataTable dataTable;
    public DataSet sessionDataset;
    public string list_properties = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the current step visual.
        NavProgress1.CurrentStepDisplay = "step3";

        if (Page.IsPostBack)
        {
            string pattern = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"[^\"]+\"";
            string replacement = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"" + SelectedOptionValue.Value + "\"";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(UpdatedChairSelect.Value, replacement);
            UpdatedChairSelect.Value = result;

            //Literal1.Text += UpdatedChairSelect.Value;

            // Get UpdatedChairSelect field for dataset to build page.
            dataset = JsonConvert.DeserializeObject<DataSet>(UpdatedChairSelect.Value);
            dataTable = dataset.Tables["Options"];

            getChairOptions();
            // Get the current option image link
            //ViaImageURL.Text = ImageURL;

            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: updateChairImage('" + ImageURL + "', '');", true);
            //ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "jQuery(function() {document.getElementById('preview-wrap').style.backgroundImage = 'url(" + ImageURL + "), url(/images/background_1.jpg)';jQuery('body').addClass('loaded'); jQuery('.ssticky').sticky({ topSpacing: 0 }); jQuery('.ssticky').sticky('update');}); ", true);
        }

        if (!Page.IsPostBack)
        {
            //ViaImageURL.Text += "PageLoad";
            if (PreviousPage != null)
            {
                Control placeHolder = PreviousPage.Controls[0].FindControl("ContentPlaceHolder1");
                HiddenField SourceTextBox = (HiddenField)placeHolder.FindControl("ChairSelect");
                if (SourceTextBox != null)
                {
                    // Get the JSON data from step 2 select page.
                    dataset = JsonConvert.DeserializeObject<DataSet>(SourceTextBox.Value);
                    dataTable = dataset.Tables["Options"];

                    getChairOptions();
                    //ViaImageURL.Text = ImageURL;
                    //ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript:updateChairImagen(ImageURL, '');", true);
                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "jQuery(function() {document.getElementById('preview-wrap').style.backgroundImage = 'url(" + ImageURL + "), url(/images/background_1.jpg)';jQuery('body').addClass('loaded');}); ", true);
                }
            }
        }
    }

    protected void getChairOptions()
    {
        // Get Values from posted source page
        string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><a href='/' class='button'>View all chairs</a>";
        string sRuleSet = "";

        //Literal1.Text += "<br>Count:" + dataTable.Rows.Count + "<br>";

        // Check that chair options were passed
        if (dataTable.Rows.Count > 0)
        {
            //URL Parameters were passed successfully, next try to call Configure Method
            foreach (DataRow row in dataTable.Rows)
            {
                string parmvalue = row["value"].ToString();
                string parmkey = row["name"].ToString();
                //Literal1.Text += parmkey + ": " + parmvalue + "<br>";

                if (parmkey == "RULE")
                {
                    sRuleSet = parmvalue;
                    RuleSet = parmvalue;
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
                    
                    // Skip the parmkeys that have the RULESET value - RULE and CHAIR_FAMILY
                    if (parmkey != "RULE" && parmkey != "CHAIR_FAMILY")
                    {
                        if (parmvalue == "TRUE")
                        { parmvalue = "True"; }
                        if (parmvalue == "FALSE")
                        { parmvalue = "False"; }

                        CurrentID = getScreenSelection(configUiClient, sessionId, "", "", parmvalue);

                        if (CurrentID != "NotFound")
                        {
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
                                //Literal2.Text += SessionID + " : " + screenOptionID + " : " + SearchValue + "<br />";

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
            // Get the current option image link
            ImageURL = UiData.ImageUrl.ToString();
        }
        
        if (selectionFound)
        {
            return screenOptionID;
        } else
        {
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

        var sListOption1 = "";
        var sListOption2 = "";
        var sListOption3 = "";
        var sListOption4 = "";
        var sListOption5 = "";
        var sListOption6 = "";
        var inputcheck = "";

        SectionOptions.Text = "";

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                list_properties = "{\"Options\" : [{\"name\" : \"RULE\", \"value\" : \"" + RuleSet + "\"}";
                foreach (var screen in UiData.Pages[0].Screens)
                {
                    if (screenIDX > -1 && screen.IsVisible == true)
                    {
                        screenOptIDX = 0;
                        foreach (var screenoption in screen.ScreenOptions)
                        {
                            switch (screenoption.DisplayType)
                            {
                                case "TypeableDropDown":
                                    create_DropDown(screenOptIDX, screenoption, screenoption.Value);
                                    break;
                                case "DropDown":
                                    create_DropDown(screenOptIDX, screenoption, screenoption.Value);
                                    break;
                                case "CheckBox":
                                    create_CheckBox(screenOptIDX, screenoption, screenoption.Value);
                                    break;
                                default:
                                    create_RadioBtn_Group(screenOptIDX, screenoption);
                                    break;
                            }
                            screenOptIDX++;
                        }
                    }
                    screenIDX++;
                }
                list_properties += "] }";
                UpdatedChairSelect.Value = list_properties;
            }
        }
    }

    public void create_DropDown(int screenIndex, ProdConfigUI.ScreenOption screenoption, string defaultValue)
    {
        if (screenIndex == 0)
        {
            // Dynamically create Chair Options ListBox
            SectionOptions.Text += "<section class='" + screenoption.Name + "' id='" + screenoption.Name + "'>";
            SectionOptions.Text += "<div class='heading'><h4>" + screenoption.Caption + "</h4></div>";
            SectionOptions.Text += "<select class='cs-select cs-skin-border'>";
        }

        // Reset Section Option List
        var sListOption1 = ""; var sListOption2 = "";
        var sFirstOptionSelected = ""; var sFirstOptionNotSelected = "";
        var setDefault_list_properties = "";
        var checkboxvalue = defaultValue;
        Boolean foundOption = false;
        var selectIDX = 0;
        sListOption1 = "<option value='' disabled>" + screenoption.Caption + "</option>";
        foreach (var select in screenoption.SelectableValues)
        {
            var inputSelected = "";
            // Dynamically create Literal for Screen Option Selectable Values
            if (IsInChairOption(screenoption.Name, select.Value))
            {
                list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\"}";
                inputSelected = "selected";
                foundOption = true;
                //ViaImageURL.Text += "Name:" + screenoption.Name + " Value:" + select.Value + " ";
            }

            var sListOption3 = "";
            foreach (var property in select.CustomProperties)
            {
                // Check Custom Properties for price and display if found
                if (property.Name == "Price" || property.Name == "PRICE")
                {
                    if (property.Value != "0" && property.Value != null && property.Value != "")
                    {
                        sListOption3 = " [+$" + property.Value + ".00]";
                    }
                    else
                    {
                        sListOption3 = "";
                    }
                }
            }

            // Prepare first option in loop
            if (selectIDX == 0)
            {
                sFirstOptionSelected = "<option value='" + select.Value + ":" + screenoption.Name + "' selected >" + select.Caption + sListOption3 + "</option>";
                sFirstOptionNotSelected = "<option value='" + select.Value + ":" + screenoption.Name + "' " + inputSelected + ">" + select.Caption + sListOption3 + "</option>";
                setDefault_list_properties = ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\"}";
            } else { 
                sListOption2 += "<option value='" + select.Value + ":" + screenoption.Name + "' " + inputSelected + ">" + select.Caption + sListOption3 + "</option>";
            }
            selectIDX++;
        }

        if (foundOption)
        {
            // Could be selected if foundOption is the first selection.
            SectionOptions.Text += sListOption1 + sFirstOptionNotSelected + sListOption2;
        } else
        {
            // Set Default DropDown to First Selection because NOT set.
            list_properties += setDefault_list_properties;
            SectionOptions.Text += sListOption1 + sFirstOptionSelected + sListOption2;
        }
        
        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</select>";
            SectionOptions.Text += "</section>";
        }
    }

    public void create_CheckBox(int screenIndex, ProdConfigUI.ScreenOption screenoption, string defaultValue)
    {
        if (screenIndex == 0)
        {
            // Dynamically create Chair Options ListBox
            SectionOptions.Text += "<section class='" + screenoption.Name + "' id='" + screenoption.Name + "'>";
            SectionOptions.Text += "<div class='heading'><h4>" + screenoption.Caption + "</h4></div>";
            SectionOptions.Text += "<div class='switch'>";
        }

        // Reset Section Option List
        var sListOption1 = ""; var sListOption2 = ""; var sListOption3 = ""; var sListOption4 = ""; var sListOption5 = ""; var sListOption6 = "";
        var selectIDX = 0;
        var inputcheck = "";
        var checkboxvalue = defaultValue;
        var found = false;
        foreach (var select in screenoption.SelectableValues)
        {
            // Dynamically create Literal for Screen Option Selectable Values
            
            if (IsInChairOption(screenoption.Name, select.Value))
            {
                //list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\"}";
                checkboxvalue = select.Value;
                found = true;
            }
        }

        list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + checkboxvalue + "\"}";

        if (checkboxvalue == "True")
            inputcheck = " checked='checked'";

        sListOption1 = "";
        sListOption2 = "<input type='checkbox' id='cmn-toggle-" + screenoption.Name + "' class='cmn-toggle cmn-toggle-yes-no'" + inputcheck + " onclick='javascript:onCheckBoxClick(\"" + screenoption.Name + "\");' > ";
        sListOption3 = "<label for='cmn-toggle-" + screenoption.Name + "' data-on='Yes' data-off='No'></label>";

        SectionOptions.Text += sListOption1 + sListOption2 + sListOption3;

        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</div>";
            SectionOptions.Text += "</section>";
        }
    }

    public void create_RadioBtn_Group(int screenIndex, ProdConfigUI.ScreenOption screenoption)
    {
        if (screenIndex == 0)
        {
            // Dynamically create Chair Options ListBox
            SectionOptions.Text += "<section class='" + screenoption.Name + "' id='" + screenoption.Name + "'>";
            SectionOptions.Text += "<div class='heading'><h4>" + screenoption.Caption + "</h4></div>";
            SectionOptions.Text += "<ul class='options unstyled'>";
        }

        // Reset Section Option List
        var sListOptions = ""; var sListOption2 = ""; var sListOption3 = ""; var sListOption4 = "";
        var sFirstOptionSelected = ""; var sFirstOptionNotSelected = "";
        var setDefault_list_properties = "";
        Boolean foundOption = false;
        var selectIDX = 0;
        foreach (var select in screenoption.SelectableValues)
        {
            // Dynamically create Literal for Screen Option Selectable Values
            var inputcheck = "";
            if (IsInChairOption(screenoption.Name, select.Value))
            {
                inputcheck = " checked='checked'";
                list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\"}";
                foundOption = true;
            }

            var sOptionPrice = "";
            foreach (var property in select.CustomProperties)
            {
                // Check Custom Properties for price and display if found
                if (property.Name == "Price" || property.Name == "PRICE")
                {
                    if (property.Value != "0" && property.Value != null && property.Value != "")
                        { sOptionPrice = "<span class='price-delta'> [+$" + property.Value + ".00]</span>"; }
                    else
                        { sOptionPrice = ""; }
                }
            }

            // Prepare first option in loop
            if (selectIDX == 0)
            {
                sFirstOptionSelected = "<li class='part extras radio-option'>";
                sFirstOptionSelected += "<input type='radio' id='" + select.Value + "' name='" + screenoption.Name + "' product-id='" + select.Value + "' section-id='" + screenoption.Name + "' checked='checked' onclick='javascript:onRadioClick(\"" + screenoption.Name + "\",\"" + select.Value + "\");'></input>";
                sFirstOptionSelected += "<label for='" + select.Value + "' title='" + select.Caption + "' class=''>" + select.Caption + sOptionPrice + "</label>";
                sFirstOptionSelected += "</li>";

                sFirstOptionNotSelected = "<li class='part extras radio-option'>";
                sFirstOptionNotSelected += "<input type='radio' id='" + select.Value + "' name='" + screenoption.Name + "' product-id='" + select.Value + "' section-id='" + screenoption.Name + "'" + inputcheck + " onclick='javascript:onRadioClick(\"" + screenoption.Name + "\",\"" + select.Value + "\");'></input>";
                sFirstOptionNotSelected += "<label for='" + select.Value + "' title='" + select.Caption + "' class=''>" + select.Caption + sOptionPrice + "</label>";
                sFirstOptionNotSelected += "</li>";

                setDefault_list_properties = ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\"}";
            } else {
                sListOptions += "<li class='part extras radio-option'>";
                sListOptions += "<input type='radio' id='" + select.Value + "' name='" + screenoption.Name + "' product-id='" + select.Value + "' section-id='" + screenoption.Name + "'" + inputcheck + " onclick='javascript:onRadioClick(\"" + screenoption.Name + "\",\"" + select.Value + "\");'></input>";
                sListOptions += "<label for='" + select.Value + "' title='" + select.Caption + "' class=''>" + select.Caption + sOptionPrice + "</label>";
                sListOptions += "</li>";
            }
            selectIDX++;
        }
        // Build Section Option Output
        if (foundOption)
        {
            // Could be selected if foundOption is the first selection.
            SectionOptions.Text += sFirstOptionNotSelected + sListOptions;
        }
        else
        {
            // Set Default DropDown to First Selection because NOT set.
            list_properties += setDefault_list_properties;
            SectionOptions.Text += sFirstOptionSelected + sListOptions;
        }

        //SectionOptions.Text += sListOption1 + sListOption2 + sListOption3 + sListOption4;

        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</ul>";
            SectionOptions.Text += "</section>";
        }
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton btn = sender as RadioButton;
    }

    // Function: ReplaceChairOption
    // Parameters: OptionName, OptionValue
    // Decription: Rplaces the chair strings passed in OptionName and OptionValue if they exist in the 
    //    default parameters passed in the JSON data.
    public bool ReplaceChairOption(string OptionName, string OptionValue)
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

                if (parmkey == OptionName)
                {
                    row["value"] = OptionValue;
                    selectionFound = true;
                    SectionOptions.Text = "Found:" + OptionName + ":" + OptionValue;
                    break;
                }
            }

            //dataset.AcceptChanges();
            //UpdatedChairSelect.Value = JsonConvert.SerializeObject(dataset, Formatting.Indented);
        }
        // Return boolean option if found
        return selectionFound;

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

    protected void btnPostBack_Click(object sender, EventArgs e)
    {
        // Call server.transfer on ajax enabled postback click and saved code from page request Manager exception
        //ViaImageURL.Text += "Time(" + DateTime.Now.ToLongTimeString() + ") : " + SelectedOptionValue.Value;
        
        //string pattern = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"[^\"]+\"";
        //string replacement = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"" + SelectedOptionValue.Value + "\"";
        //Regex rgx = new Regex(pattern);
        //string result = rgx.Replace(UpdatedChairSelect.Value, replacement);
        //UpdatedChairSelect.Value = result;
    }
}