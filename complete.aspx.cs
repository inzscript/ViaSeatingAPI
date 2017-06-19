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
                HiddenField SourceTextBox = (HiddenField)placeHolder.FindControl("UpdatedChairSelect");
                HiddenField SessionTextBox = (HiddenField)placeHolder.FindControl("GlobalSessionID");
                if (SourceTextBox != null && SessionTextBox != null)
                {
                    // Store Previous Page SessionID into Page Global SessionID
                    gSessionID = SessionTextBox.Value;
                    // Store the JSON data from Step 2 into Global DataSet
                    dataset = JsonConvert.DeserializeObject<DataSet>(SourceTextBox.Value);
                    dataTable = dataset.Tables["Options"];

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
        string sRuleSet = "";

        // Check that chair options were found
        if (dataTable.Rows.Count > 0)
        {
            sList = "<ul class='order double'>";
            // Locate and store the RuleSet name in Global Variable
            foreach (DataRow row in dataTable.Rows)
            {
                
                string parmkey = row["name"].ToString();
                string parmvalue = row["value"].ToString();
                string parmvisible = row["visible"].ToString();

                sList += "<li><strong>" + parmkey + "</strong> <span>" + parmvalue + "</span></li>";
            }
            sList += "</ul>";
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