using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class _Debug : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Step 1: Prepare Host Services Input Parameters
        var inputParams = new InputParameters
        {
            DetailId = Guid.NewGuid().ToString(),
            HeaderId = Guid.NewGuid().ToString(),
            PartNamespace = "Chair",
            PartNumber = "ALL_CHAIRS",
            //PartNumber = "BRISBANE",
            Profile = "Default"
        };

        string instance = ConfigurationManager.AppSettings["via_instanceID"];
        string appId = ConfigurationManager.AppSettings["via_appID"];
        string endpoint_configuration_name = ConfigurationManager.AppSettings["via_endpoint_configname"];
        string serviceUrl = ConfigurationManager.AppSettings["via_endpoint_integrationservice"];
        string redirectUrl = "http://viaseatingdev.azurewebsites.net/export.aspx";

        // Step 2: Call the PrepareForInteractive Host services methods
        var hostServices = new HostServices(instance, appId, serviceUrl);
        var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

        // Step 3: Call the UI Service InitializeConfiguration method :: Defined in Web References
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
            ID = UiData.Pages[0].Screens[0].ScreenOptions[0].ID,
            Value = UiData.Pages[0].Screens[0].ScreenOptions[0].SelectableValues[0].Value
        };
        var selections2 = new[] { optionSelection };
        var UiData2 = configUiClient.Configure(sessionId, selections2);

        Literal3.Text += "<p>PageLength=" + UiData2.Pages.Length.ToString() + "</p>";
        Literal3.Text += "<p>ScreenLenght=" + UiData2.Pages[0].Screens.Length.ToString() + "</p>";

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

        foreach (var i in UiData2.Pages[0].Screens[1].ScreenOptions[0].SelectableValues)
        {
            Literal3.Text += "<p>SelValues=" + i.Value + "</p>";
        }

        Literal3.Text += "<p>ImageURL=" + UiData2.ImageUrl + "</p>";
        Literal3.Text += "<p>Messages=" + UiData2.Messages + "</p>";
        Literal3.Text += "<hr>";
    }
}