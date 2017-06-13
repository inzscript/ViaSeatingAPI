using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TDCI.BuyDesign.Configurator.Integration.Data;
using TDCI.BuyDesign.Configurator.Integration.Web;

public partial class index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var inputParams = new InputParameters
        {
            DetailId = "Line1",
            HeaderId = "QUO101",
            PartNamespace = "AMC",
            PartNumber = "Bike",
            Profile = "Default",
            VariantKey = "MFMWRED"
        };

        //inputParams.IntegrationParameters.Add("Quantity", 1);

        inputParams.IntegrationParameters.Add("Quantity", 10);
        inputParams.IntegrationParameters.Add("BasePrice", 100.00);
        inputParams.SetExchangeRate(1.2m, "EUR");
        inputParams.RapidOptions.Add("COLOR", "RED");
        inputParams.RapidOptions.Add("HANDLEBARS", "UPRIGHT");

        const string instance = "CustomChair";
        const string appId = "ChairBuilder";
        const string serviceUrl = "http://localhost/PCMDEV/ConfiguratorService/IntegrationService.svc";
        var hostServices = new HostServices(instance, appId, serviceUrl);

        // this is where it goes when the user clicks finish
        const string redirectUrl = "http://localhost/PCMDEV/ConfiguratiorService/custom/HandlesTheConfigurationResult.aspx";
        var url = hostServices.Export(inputParams.HeaderId, inputParams.DetailId);
        //var url = hostServices.PrepareForInteractiveConfiguration(inputParams, "Seat Builder", redirectUrl);

        //http://<ServerName>/PCM/ConfiguratorService/v2/ProductConfiguratorUI.svc
        var configUiClient = new ProdConfigUI.ProductConfiguratorUIServiceProxyClient();
        var sessionId = configUiClient.InitializeConfiguration(instance, appId, inputParams.HeaderId, inputParams.DetailId);

        // assume no screen option on initial 
        var selections = new ProdConfigUI.OptionSelection[0];
        var UiData = configUiClient.Configure(sessionId, selections);
        // present this value to the UI using the properties available 
        
        // fake response data from user selection in your ui
        var changedScreenOption = UiData.Pages[0].Screens[0].ScreenOptions[0];
        var optionSelection = new ProdConfigUI.OptionSelection
        {
            ID = changedScreenOption.ID, 
            Value = changedScreenOption.Value
        };

        selections = new[] { optionSelection };
        UiData = configUiClient.Configure(sessionId, selections);

    }

    //private void uidatachanged(object sender, propertychangedeventargs e)
    //{

    //}

}