using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class getProduct : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Step 1: Prepare Host Services Input Parameters
        // HeaderId = "QUO101"
        var inputParams = new InputParameters
        {
            DetailId = Guid.NewGuid().ToString(),
            HeaderId = Guid.NewGuid().ToString(),
            PartNamespace = "Chair",
            PartNumber = "ALL_CHAIRS",
            Profile = "Default"
        };

        inputParams.IntegrationParameters.Add("Quantity", 1);

        string instance = ConfigurationManager.AppSettings["via_instanceID"];
        string appId = ConfigurationManager.AppSettings["via_appID"];
        string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];

        // Step 2: Call the PrepareForInteractive Host services methods
        string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];
        var hostServices = new HostServices(instance, appId, serviceUrl);

        const string redirectUrl = "http://viaseatingdev.azurewebsites.net/export.aspx";
        var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

        //http://<ServerName>/PCM/ConfiguratorService/v2/ProductConfiguratorUI.svc
        //const string productServiceUrl = "http://216.162.180.77/PCMDEV/ConfiguratorService/v2/ProductConfiguratorUI.svc?wsdl";

        // Step 3: Call the UI Service InitializeConfiguration method
        var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient(endpoint_configuration_name);
        var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);

        // Step 4: Call the UI Service Configure method to retrieve the first set of user interface elements
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = configUiClient.Configure(sessionId, selections);

        RuleLiteral.Text = inputParams.PartNumber;

        Literal1.Text = "<p>SessionID=<br />" + sessionId.ToString() + "</p>";
        Literal1.Text += "<hr>";

        Literal2.Text = "<p>DetailID/ConfigurationID=<br />" + inputParams.DetailId.ToString() + "</p>";
        Literal2.Text += "<p>HeaderID=<br />" + inputParams.HeaderId.ToString() + "</p>";
        Literal2.Text += "<hr>";

        Literal3.Text = "<p>Pages=" + UiData.Pages[0].Caption + "</p>";
        Literal3.Text += "<p>Screens=" + UiData.Pages[0].Screens[0].Description + "</p>";
        Literal3.Text += "<p>Opt=" + UiData.Pages[0].Screens[0].ScreenOptions[0].Caption + "</p>";
        Literal3.Text += "<p>OptID=" + UiData.Pages[0].Screens[0].ScreenOptions[0].ID + "</p>";
        Literal3.Text += "<p>OptInfoLink=" + UiData.Pages[0].Screens[0].ScreenOptions[0].InformationLink + "</p>";
        Literal3.Text += "<p>OptInfoMessage=" + UiData.Pages[0].Screens[0].ScreenOptions[0].InformationMessage + "</p>";
        Literal3.Text += "<p>OptRanges=" + UiData.Pages[0].Screens[0].ScreenOptions[0].Ranges + "</p>";
        Literal3.Text += "<p>OptDType=" + UiData.Pages[0].Screens[0].ScreenOptions[0].DisplayType + "</p>";
        Literal3.Text += "<p>OptType=" + UiData.Pages[0].Screens[0].ScreenOptions[0].Type + "</p>";
        Literal3.Text += "<p>OptValue=" + UiData.Pages[0].Screens[0].ScreenOptions[0].Value + "</p>";

        foreach (var i in UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues)
        {
            Literal3.Text += "<p>SelValues=" + i.Value + "</p>";
        }


        Literal3.Text += "<p>Details=" + UiData.Details + "</p>";
        Literal3.Text += "<p>ImageURL=" + UiData.ImageUrl + "</p>";
        Literal3.Text += "<p>Messages=" + UiData.Messages + "</p>";
        Literal3.Text += "<p>Summary=" + UiData.SelectionSummary[0].Caption + " - " + UiData.SelectionSummary[0].Type + " - " + UiData.SelectionSummary[0].Value + "</p>";
        Literal3.Text += "<hr>";

        var changedScreenOption = UiData.Pages[0].Screens[0].ScreenOptions[0];
        var optionSelection = new ProdConfigUI.OptionSelection
        {
            ID = changedScreenOption.ID,
            Value = changedScreenOption.Value
        };

        var selections2 = new[] { optionSelection };
        var UiData2 = configUiClient.Configure(sessionId, selections2);

        Literal3.Text += "<p>Pages=" + UiData2.Pages[0].Caption + "</p>";
        Literal3.Text += "<p>Screens=" + UiData2.Pages[0].Screens[0].Description + "</p>";
        Literal3.Text += "<p>Opt=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].Caption + "</p>";
        Literal3.Text += "<p>OptID=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].ID + "</p>";
        Literal3.Text += "<p>OptInfoLink=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].InformationLink + "</p>";
        Literal3.Text += "<p>OptInfoMessage=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].InformationMessage + "</p>";
        Literal3.Text += "<p>OptRanges=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].Ranges + "</p>";
        Literal3.Text += "<p>OptDType=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].DisplayType + "</p>";
        Literal3.Text += "<p>OptType=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].Type + "</p>";
        Literal3.Text += "<p>OptValue=" + UiData2.Pages[0].Screens[0].ScreenOptions[0].Value + "</p>";
        Literal3.Text += "<p>Details=" + UiData2.Details + "</p>";
        Literal3.Text += "<p>ImageURL=" + UiData2.ImageUrl + "</p>";
        Literal3.Text += "<p>Messages=" + UiData2.Messages + "</p>";
        Literal3.Text += "<hr>";

        // Step 5: Call the UI Service FinalizeConfiguration method :: TBD
        // Step 6: Retrieve the Configuration Summary :: TBD
    }
}