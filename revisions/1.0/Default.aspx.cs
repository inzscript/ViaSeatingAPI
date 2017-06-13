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
            int count = 0;
            foreach (var select in UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues)
            {
                count += 1;
                
                Literal1.Text += "<div class='box three'>";
                Literal1.Text += "<div class='featured_image img_loaded'>";
                Literal1.Text += "<img src='" + select.ImageLink + "' alt='" + select.Caption + "'>";
                Literal1.Text += "</div>";
                Literal1.Text += "<div class='row clearfix'><div>";
                Literal1.Text += "<h3 class='featured_article_title fade animated' data-rt-animate='animate' data-rt-animation-type='fade' data-rt-animation-group='single' style='-webkit-animation: 0s;'>" + select.Caption + "</h3>";
                Literal1.Text += "<a href='/select.aspx?sid=" + sessionId.ToString() + "&id=" + UiData.Pages[0].Screens[0].ScreenOptions[0].ID + "&value=" + select.Value + "&soid=" + (count-1).ToString() + "' class='button'>" + select.Value + "</a>";
                Literal1.Text += "</div></div>";
                Literal1.Text += "</div>";
                // Close div if the item was third.
                if (((count % 3) == 0))
                {
                    Literal1.Text += "</div><br />";
                    Literal1.Text += "<div class='row clearfix'>";
                }
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
            Literal1.Text = ex.ToString();
        }
    }
}