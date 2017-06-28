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

public partial class complete : System.Web.UI.Page
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

    public DataSet summaryDataset;
    public DataTable summaryTable;

    public DataSet configuredDataset;
    public DataTable configuredTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set the current step visual.
            NavProgress1.CurrentStepDisplay = "step4";

            // Check for JSON Data from Step 2
            if (PreviousPage != null)
            {
                Control placeHolder = PreviousPage.Controls[0].FindControl("ContentPlaceHolder1");
                HiddenField SelectionSummaryTextBox = (HiddenField)placeHolder.FindControl("SelectionSummary");
                HiddenField ConfiguredPriceTextBox = (HiddenField)placeHolder.FindControl("ConfiguredPrice");
                HiddenField SessionTextBox = (HiddenField)placeHolder.FindControl("GlobalSessionID");
                if (SelectionSummaryTextBox != null && ConfiguredPriceTextBox != null && SessionTextBox != null)
                {
                    // Store Previous Page SessionID into Page Global SessionID
                    gSessionID = SessionTextBox.Value;

                    // Save Selection Summary to DataSet
                    summaryDataset = JsonConvert.DeserializeObject<DataSet>(SelectionSummaryTextBox.Value);
                    summaryTable = summaryDataset.Tables["Selections"];

                    // Save Configured Price to DataSet
                    configuredDataset = JsonConvert.DeserializeObject<DataSet>(ConfiguredPriceTextBox.Value);
                    configuredTable = configuredDataset.Tables["Details"];

                    initializeChairOptions();
                    finalizeChairOptions();
                }
            }
            else
            {
                Literal1.Text = error_timeout;
            }
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
        string  sList = "";

        // Check that chair options were found
        if (summaryTable.Rows.Count > 0)
        {
            // Output Selection Summary
            sList += "<ul class='order double'>";
            foreach (DataRow row in summaryTable.Rows)
            {
                // Output variable fields: Caption and Value
                string parmkey = row["caption"].ToString();
                string parmvalue = row["value"].ToString();

                sList += "<li><strong>" + parmkey + "</strong> <span>" + parmvalue + "</span></li>";
            }
            sList += "</ul> <br /><br />";

            // Output Configured Price
            sList += "<ul class='order double'>";
            foreach (DataRow row in configuredTable.Rows)
            {
                // Output variable fields: Caption, Value and Type
                string parmkey = row["caption"].ToString();
                string parmvalue = row["value"].ToString();
                string parmvisible = row["type"].ToString();

                sList += "<li><strong>" + parmkey + "</strong> <span>" + parmvalue + "</span></li>";
            }
            sList += "</ul> <br /><br />";

            Literal1.Text = sList;
        }
        else
        {
            // If it is not existing
            Literal1.Text = error_timeout;
        }
    }

    protected void finalizeChairOptions()
    {
        string instance = ConfigurationManager.AppSettings["via_instanceID"];
        string appId = ConfigurationManager.AppSettings["via_appID"];
        string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
        string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];

        try
        {
            // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
            var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
            var sessionId = gSessionID;

            // Finalize UIClient Session
            configUiClient.FinalizeConfiguration(sessionId);
        }
        catch (Exception ex)
        {
            // API Method call failed due to session timeout or undefined series ruleset.
            Literal1.Text = error_timeout;
        }
    }
}