﻿using System;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;



public partial class builder3 : System.Web.UI.Page
{
    // Define Global Variable
    public string gSessionID = "";
    public string ImageURL = "";
    public string RuleSet = "";
    public string list_properties = "";
    public string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><p><a href='/' class='button'>View all chairs</a></p>";

    // Define Global DataSet for JSON Data
    public DataSet dataset;
    public DataTable dataTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Set the current step visual.
        NavProgress1.CurrentStepDisplay = "step3";

        // Initialize PostBack (AJAX)
        if (Page.IsPostBack)
        {
            // Update the previous Chair Options stored as JSON data in HiddenField UPdatedChairSelect. 
            // Selected options name and value are stored in HiddenFields SelectedOptionName and SelectedOptionValue
            //string pattern = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"[^\"]+\"";
            string pattern = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"[^\"]+\", \"visible\" : \"[^\"]+\"";
            string replacement = "\"name\" : \"" + SelectedOptionName.Value + "\", \"value\" : \"" + SelectedOptionValue.Value + "\", \"visible\" : \"true\"";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(UpdatedChairSelect.Value, replacement);
            UpdatedChairSelect.Value = result;
            //Literal1.Text += UpdatedChairSelect.Value;
            // Store the JSON data from PostBack HiddenFields into Global DataSet
            dataset = JsonConvert.DeserializeObject<DataSet>(UpdatedChairSelect.Value);
            dataTable = dataset.Tables["Options"];

            updateChairOptions();

            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: updateChairImage('" + ImageURL + "', '');", true);
            //ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "jQuery(function() {document.getElementById('preview-wrap').style.backgroundImage = 'url(" + ImageURL + "), url(/images/background_1.jpg)';jQuery('body').addClass('loaded'); jQuery('.ssticky').sticky({ topSpacing: 0 }); jQuery('.ssticky').sticky('update');}); ", true);
        }

        // Initialize First Page Load,
        if (!Page.IsPostBack)
        {
            // Check for JSON Data from Step 2
            if (PreviousPage != null)
            {
                Control placeHolder = PreviousPage.Controls[0].FindControl("ContentPlaceHolder1");
                HiddenField SourceTextBox = (HiddenField)placeHolder.FindControl("ChairSelect");
                if (SourceTextBox != null)
                {
                    // Store the JSON data from Step 2 into Global DataSet
                    dataset = JsonConvert.DeserializeObject<DataSet>(SourceTextBox.Value);
                    dataTable = dataset.Tables["Options"];

                    initializeChairOptions();

                    ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: initializeChairImage('" + ImageURL + "', '');", true);
                    //ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "jQuery(function() {document.getElementById('preview-wrap').style.backgroundImage = 'url(" + ImageURL + "), url(/images/background_1.jpg)';jQuery('body').addClass('loaded');}); ", true);
                }
            } else
            {
                Literal2.Text = error_timeout;
                ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
            }
        }
    }

    /// <summary>
    /// Initialize the page load by connecting to Infor API 
    /// </summary>
    /// <remarks>
    /// Recursivlely calls API method, Configure() with ScreenOption Name and Value to preload Chair selection options.
    /// </remarks>
    protected void updateChairOptions()
    {
        string sRuleSet = "";

        // Check that chair options were found
        if (dataTable.Rows.Count > 0)
        {
            // Locate and store the RuleSet name in Global Variable
            foreach (DataRow row in dataTable.Rows)
            {
                string parmvalue = row["value"].ToString();
                string parmkey = row["name"].ToString();

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
            // Set Quantity needed for initialize RuleSet
            inputParams.IntegrationParameters.Add("Quantity", 1);

            try
            {
                // Step 2: Call the PrepareForInteractive Host services methods
                var hostServices = new HostServices(instance, appId, serviceUrl);
                var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

                // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
                var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
                //var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);
                var CurrentID = "";
                var sessionId = GlobalSessionID.Value;


                string parmvalue = SelectedOptionValue.Value;
                string parmkey = SelectedOptionName.Value;

                // Skip the parmkeys that have the RULESET values = RULE and CHAIR_FAMILY
                if (parmkey != "RULE" && parmkey != "CHAIR_FAMILY")
                {
                    // Convert TRUE FALSE to Proper Case
                    if (parmvalue == "TRUE") { parmvalue = "True"; }
                    if (parmvalue == "FALSE") { parmvalue = "False"; }

                    CurrentID = getScreenSelection(configUiClient, sessionId, "", "", parmvalue);

                    if (CurrentID != "NotFound")
                    {
                        CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, parmvalue, parmvalue);
                    }
                }

                showScreenSelection(configUiClient, sessionId);

                updateChairRebuild("");
            }
            catch (Exception ex)
            {
                // API Method call failed due to session timeout or undefined series ruleset.
                Literal2.Text = error_timeout;
                Literal2.Text = ex.ToString();
                ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
            }
        }
        else
        {
            // If it is not existing
            Literal2.Text = error_timeout;
            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
        }
    }

    protected void updateChairRebuild(string lastParm)
    {
        string sRuleSet = "";
        
        // Get rebuild DataSet after selection is made
        dataset = JsonConvert.DeserializeObject<DataSet>(UpdatedChairSelect.Value);
        dataTable = dataset.Tables["Options"];

        // Check that chair options were found
        if (dataTable.Rows.Count > 0)
        {
            // Locate and store the RuleSet name in Global Variable
            foreach (DataRow row in dataTable.Rows)
            {
                string parmvalue = row["value"].ToString();
                string parmkey = row["name"].ToString();

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
            // Set Quantity needed for initialize RuleSet
            inputParams.IntegrationParameters.Add("Quantity", 1);

            try
            {
                // Step 2: Call the PrepareForInteractive Host services methods
                var hostServices = new HostServices(instance, appId, serviceUrl);
                var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

                // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
                var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
                //var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);
                var CurrentID = "";
                var sessionId = GlobalSessionID.Value;


                string lastparmvalue = SelectedOptionValue.Value;
                string lastparmkey = SelectedOptionName.Value;

                // Parse the DataSet for ScreenOptionValues
                var vidx = 1;
                Boolean beginRebuild = false;
                foreach (DataRow row in dataTable.Rows)
                {
                    string parmvalue = row["value"].ToString();
                    string parmkey = row["name"].ToString();

                    if (lastparmkey == parmkey)
                        beginRebuild = true;

                    if (beginRebuild) { 
                        // Skip the parmkeys that have the RULESET values = RULE and CHAIR_FAMILY
                        if (parmkey != "RULE" && parmkey != "CHAIR_FAMILY")
                        {
                            // Convert TRUE FALSE to Proper Case
                            if (parmvalue == "TRUE") { parmvalue = "True"; }
                            if (parmvalue == "FALSE") { parmvalue = "False"; }

                            CurrentID = getScreenSelection(configUiClient, sessionId, "", "", parmvalue);

                            Literal1.Text = "Rebuiling in Progress...";

                            if (CurrentID != "NotFound")
                            {
                                CurrentID = getScreenSelection(configUiClient, sessionId, CurrentID, parmvalue, parmvalue);
                            }

                            ViaImageURL.Text += UpdatedChairSelect.Value + "<br><br>";
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
                ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
            }
        }
        else
        {
            // If it is not existing
            Literal2.Text = error_timeout;
            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
        }
    }

    /// <summary>
    /// Initialize the page load by connecting to Infor API 
    /// </summary>
    /// <remarks>
    /// Recursivlely calls API method, Configure() with ScreenOption Name and Value to preload Chair selection options.
    /// </remarks>
    protected void initializeChairOptions()
    {
        string sRuleSet = "";

        // Check that chair options were found
        if (dataTable.Rows.Count > 0)
        {
            // Locate and store the RuleSet name in Global Variable
            foreach (DataRow row in dataTable.Rows)
            {
                string parmvalue = row["value"].ToString();
                string parmkey = row["name"].ToString();

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
            // Set Quantity needed for initialize RuleSet
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
                GlobalSessionID.Value = sessionId;

                // Parse the DataSet for ScreenOptionValues
                var vidx = 1;
                foreach (DataRow row in dataTable.Rows)
                {
                    string parmvalue = row["value"].ToString();
                    string parmkey = row["name"].ToString();
                    
                    // Skip the parmkeys that have the RULESET values = RULE and CHAIR_FAMILY
                    if (parmkey != "RULE" && parmkey != "CHAIR_FAMILY")
                    {
                        // Convert TRUE FALSE to Proper Case
                        if (parmvalue == "TRUE") { parmvalue = "True"; }
                        if (parmvalue == "FALSE") { parmvalue = "False"; }

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
                ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
            }
        }
        else
        {
            // If it is not existing
            Literal2.Text = error_timeout;
            ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: errorDisplay();", true);
        }
    }

    /// <summary>
    /// Get the ScreenOption ID of the ScrenOption Name
    /// </summary>
    /// <remarks>
    /// Searches XML dataset from Infor API method call to Configure() for the ScreenOption Name and returns ScreenOption ID if found
    /// </remarks>
    /// <param name="UIClient"></param>
    /// <param name="SessionID"></param>
    /// <param name="OptSelectID"></param>
    /// <param name="OptSelectValue"></param>
    /// <param name="SearchValue"></param>
    /// <returns>ScreenOption ID</returns>
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
                                ViaImageURL.Text += SessionID + " : " + screenOptionID + " : " + SearchValue + "<br />";
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
            // Set Global Variable ImageURL with the current ScreenOption image link
            ImageURL = UiData.ImageUrl.ToString();
            //Literal2.Text += UiData.ImageUrl.ToString() + "<br />";
        }
        
        if (selectionFound)
        {
            return screenOptionID;
        } else
        {
            return "NotFound";
        }
    }

    /// <summary>
    /// Creates Selection form fields based on ScreenOption Display Types
    /// </summary>
    /// <param name="UIClient"></param>
    /// <param name="SessionID"></param>
    protected void showScreenSelection(ProdConfigUI.ProductConfiguratorUIServiceProxyClient UIClient, string SessionID)
    {
        // Setup API Call: Configure( SessionID, OptionSelection(ID, Value))
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = UIClient.Configure(SessionID, selections);
        var numPages = UiData.Pages.Length;
        var numScreens = UiData.Pages[0].Screens.Length;
        var screenIDX = 0;
        var screenOptIDX = 0;

        SectionOptions.Text = "";

        if (numPages >= 1)
        {
            if (numScreens >= 1)
            {
                // Create JSON data to track current ScreenOption selected values.
                list_properties = "{\"Options\" : [{\"name\" : \"RULE\", \"value\" : \"" + RuleSet + "\", \"visible\" : \"true\"}";
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
                // Store the current selected values as JSON data in HiddenField used for PostBack.
                list_properties += "] }";
                UpdatedChairSelect.Value = list_properties;
            }
        }
    }

    /// <summary>
    /// Create Display Type DropDown form element
    /// </summary>
    /// <remarks>
    /// Sets the default value of DropDown base on:
    /// 1. If initial value in JSON
    /// 2. If no intial value is set retreived XML data default value
    /// 3. If the above is not found set default value to the first value defined in XML data.
    /// </remarks>
    /// <param name="screenIndex"></param>
    /// <param name="screenoption"></param>
    /// <param name="defaultValue"></param>
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
                list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\", \"visible\" : \"true\"}";
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

                // Prepare to add first option as selected both on the form and DataSet. (disabled - set visible to false)
                sFirstOptionNotSelected = "<option value='" + select.Value + ":" + screenoption.Name + "' " + inputSelected + ">" + select.Caption + sListOption3 + "</option>";
                setDefault_list_properties = ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\", \"visible\" : \"false\"}";
            } else { 
                sListOption2 += "<option value='" + select.Value + ":" + screenoption.Name + "' " + inputSelected + ">" + select.Caption + sListOption3 + "</option>";
            }
            selectIDX++;
        }

        if (foundOption)
        {
            // Could be selected if found Option is the first selection.
            SectionOptions.Text += sListOption1 + sFirstOptionNotSelected + sListOption2;
        } else
        {
            // Set Default DropDown to First Selection because NOT set. (disabled - set visible to false)
            list_properties += setDefault_list_properties;

            // Set Dropdown with first Option Selected
            //SectionOptions.Text += sListOption1 + sFirstOptionSelected + sListOption2;
            SectionOptions.Text += sListOption1 + sFirstOptionNotSelected + sListOption2;
        }
        
        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</select>";
            SectionOptions.Text += "</section>";
        }
    }

    /// <summary>
    /// Create Display Type CheckBox form element
    /// </summary>
    /// <remarks>
    /// Sets the default value of DropDown base on:
    /// 1. If initial value in JSON
    /// 2. If no intial JSON value is set retreived XML data default value
    /// 3. If the above is not found set default value to NO or FALSE.
    /// </remarks>
    /// <param name="screenIndex"></param>
    /// <param name="screenoption"></param>
    /// <param name="defaultValue"></param>
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
        var sListOption1 = ""; var sListOption2 = ""; var sListOption3 = "";
        var selectIDX = 0;
        var inputcheck = "";
        var checkboxvalue = defaultValue;
        //var checkboxvalue = "";
        Boolean foundOption = false;
        foreach (var select in screenoption.SelectableValues)
        {
            // Dynamically create Literal for Screen Option Selectable Values
            if (IsInChairOption(screenoption.Name, select.Value))
            {
                list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\", \"visible\" : \"true\"}";
                checkboxvalue = select.Value;
                foundOption = true;
            }
        }

        // Auto select
        //list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + checkboxvalue + "\", \"visible\" : \"true\"}";
        //if (checkboxvalue == "True")
        //    inputcheck = " checked='checked'";

        if (foundOption)
        {
            // Set checked
            if (checkboxvalue == "True")
                inputcheck = " checked='checked'";
        }
        else
        {
            // Set Default Checkbox. (disabled - set visible to false)
            list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + checkboxvalue + "\", \"visible\" : \"false\"}";
        }

        sListOption1 = "<input type='checkbox' id='cmn-toggle-" + screenoption.Name + "' class='cmn-toggle cmn-toggle-yes-no'" + inputcheck + " onclick='javascript:onCheckBoxClick(\"" + screenoption.Name + "\");' > ";
        sListOption2 = "<label for='cmn-toggle-" + screenoption.Name + "' data-on='Yes' data-off='No'></label>";

        SectionOptions.Text += sListOption1 + sListOption2 + sListOption3;

        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</div>";
            SectionOptions.Text += "</section>";
        }
    }

    /// <summary>
    /// Create Display Type RadioButton form element
    /// </summary>
    /// <remarks>
    /// 1. If initial value in JSON
    /// 2. If no intial JSON value is set retreived XML data default value
    /// 3. If the above is not found set default value to the first value defined in XML data.
    /// </remarks>
    /// <param name="screenIndex"></param>
    /// <param name="screenoption"></param>
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
                list_properties += ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\", \"visible\" : \"true\"}";
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

                // Prepare to add first option as selected for the DataSet. (disabled - set visible to false)
                setDefault_list_properties = ", {\"name\" : \"" + screenoption.Name + "\", \"value\" : \"" + select.Value + "\", \"visible\" : \"false\"}";
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
            // Set Default RadioBtn to first selection because NOT set.(disabled - set visible to false)
            list_properties += setDefault_list_properties;
            //SectionOptions.Text += sFirstOptionSelected + sListOptions;
            SectionOptions.Text += sFirstOptionNotSelected + sListOptions;
        }

        //SectionOptions.Text += sListOption1 + sListOption2 + sListOption3 + sListOption4;

        if (screenIndex == 0)
        {
            // End Chair Options List
            SectionOptions.Text += "</ul>";
            SectionOptions.Text += "</section>";
        }
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

    // Function: IsInChairOptionByName
    // Parameters: OptionName, OptionValue
    // Decription: Checks if OptionName exist in the default parameters passed in the JSON data.
    public bool IsInChairOptionByName(string OptionName, string OptionValue)
    {
        bool selectionFound = false;

        if (dataTable.Rows.Count > 0)
        {
            // Check each parameter to see if found
            foreach (DataRow row in dataTable.Rows)
            {
                string parmkey = row["name"].ToString();
                

                if (parmkey == OptionName)
                {
                    selectionFound = true;
                    break;
                }
            }
        }
        // Return boolean option if found
        return selectionFound;
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

        /// <summary>
    /// Convert string to Upper Case
    /// </summary>
    /// <param name="s"></param>
    /// <returns>Converted String</returns>
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

    /// <summary>
    /// Process anything before PostBack occurs.
    /// </summary>
    /// <remarks>
    /// Not used at this point.
    /// </remarks>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    protected void btnGeneratePDF_Click(object sender, EventArgs e)
    {
        Literal1.Text += "</br> Hello </br>";
        try
        {
            string LOGOURL = Server.MapPath(".") + "/images/via-logo-with-sweet-spot-tagline.gif";
            string CHAIRURL = ImageURL;

            iTextSharp.text.Font bodyFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font bodyBoldFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font headBoldFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);

            BaseColor primary = new BaseColor(255, 175, 88); // color: #FFAF58
            BaseColor light1 = new BaseColor(255, 249, 243); // color: #FFF9F3
            BaseColor light2 = new BaseColor(255, 207, 154); // color: #FFCF9A
            BaseColor dark1 = new BaseColor(255, 145, 25); // color: #FF9119
            BaseColor dark2 = new BaseColor(255, 133, 0); // color: #FF8500
            BaseColor white = new BaseColor(255, 255, 255); // color: #FFF

            Document pdfDoc = new Document(PageSize.LETTER, 25, 25, 25, 25);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

            pdfDoc.Open();

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(LOGOURL);
            logo.ScaleToFit(140f, 120f);
            logo.SpacingBefore = 10f;
            logo.SpacingAfter = 30f;
            logo.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(logo);

            iTextSharp.text.Image chair = iTextSharp.text.Image.GetInstance(CHAIRURL);
            chair.ScaleToFit(130f, 130f);
            chair.SpacingBefore = 10f;
            chair.SpacingAfter = 10f;
            chair.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(chair);

            Paragraph header = new Paragraph("Brisbane");
            header.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(header);            

            PdfPTable table = new PdfPTable(5);
            table.DefaultCell.Border = 0;
            table.TotalWidth = 400f;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 5f;

            float[] widths = new float[] { 95f, 95f, 20f, 95f, 95f };
            table.SetWidths(widths);
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell cell = new PdfPCell(new Phrase("Specification Sheet", bodyBoldFont));
            //cell.BackgroundColor = white;
            //cell.Colspan = 5;
            //cell.Border = 0;
            //cell.FixedHeight = 40f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
            //table.AddCell(cell);

            PdfPCell cell = new PdfPCell(new Phrase("ROW2COL1", headBoldFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row2, col2", bodyFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(" "));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("ROW2COL4", headBoldFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row2, col5", bodyFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("ROW3COL1", headBoldFont));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row3, col2", bodyFont));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(" "));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("ROW2COL4", headBoldFont));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row3, col5", bodyFont));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("ROW4COL1", headBoldFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row4, col2", bodyFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(" "));
            cell.BackgroundColor = white;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("ROW4COL4", headBoldFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Row4, col5", bodyFont));
            cell.BackgroundColor = light1;
            cell.Border = 0;
            cell.FixedHeight = 40f;
            cell.PaddingLeft = 2f;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            
            pdfDoc.Add(table);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Spec-Sheet.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void btnPostFinal_Click(object send, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "displayNone", "<script language='JavaScript'>onFinalizePostBack();</script>", false);
        //ScriptManager.RegisterStartupScript(UpdatePanel2, GetType(), "Javascript", "javascript: onFinalizePostBack();", true);
    }

    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        // Call server.transfer on ajax enabled postback click and saved code from page request Manager exception
        Server.Transfer("complete.aspx", false);
    }

}