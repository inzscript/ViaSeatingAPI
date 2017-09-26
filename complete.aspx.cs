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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;

public partial class complete : System.Web.UI.Page
{
    // Define Global Variable
    public string gSessionID = "";
    public string ImageURL = "";
    public string RuleSet = "";
    public string ListPrice = "";
    public string list_properties = "";
    public string error_timeout = "<h2>Chair Builder Session Timed Out</h2><p>Please start your session over.</p><p><a href='/' class='button'>View all chairs</a></p>";

    // Define Global DataSet for JSON Data
    public DataSet dataset;
    public DataTable dataTable;

    public DataSet summaryDataset;
    public DataTable summaryTable;

    public DataSet configuredDataset;
    public DataTable configuredTable;

    public DataSet chairselectDataset;
    public DataTable chairselectTable;

    public string FamilyRuleSet = "";
    public string ListPriceValue = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set the current step visual.
            NavProgress1.CurrentStepDisplay = "step4";

            // Check for JSON Data from Step 2
            if (PreviousPage != null)
            {
                // Get Data from PreviousPage
                Control placeHolder = PreviousPage.Controls[0].FindControl("ContentPlaceHolder1");
                HiddenField SelectionSummaryTextBox = (HiddenField)placeHolder.FindControl("SelectionSummary");
                HiddenField ConfiguredPriceTextBox = (HiddenField)placeHolder.FindControl("ConfiguredPrice");
                HiddenField SessionTextBox = (HiddenField)placeHolder.FindControl("GlobalSessionID");
                HiddenField ChairImageURLTextBox = (HiddenField)placeHolder.FindControl("ChairImageURL");
                HiddenField ChairSelectTextBox = (HiddenField)placeHolder.FindControl("UpdatedChairSelect");
                HiddenField RuleSetTextBox = (HiddenField)placeHolder.FindControl("SelectedRuleSet");

                if (SelectionSummaryTextBox != null && ConfiguredPriceTextBox != null && SessionTextBox != null && ChairImageURLTextBox != null)
                {
                    // Store Previous Page SessionID into Page Global SessionID
                    gSessionID = SessionTextBox.Value;
                    RuleSet = RuleSetTextBox.Value;
                    lFamily.Text = RuleSetTextBox.Value;
                    HRuleset.Value = RuleSetTextBox.Value;

                    // Save Chair Image URL to ImageURL
                    ChairImageURL.Value = ChairImageURLTextBox.Value;
                    ImageURL = ChairImageURLTextBox.Value;

                    // Save Selection Summary to DataSet
                    SelectionSummary.Value = SelectionSummaryTextBox.Value;
                    summaryDataset = JsonConvert.DeserializeObject<DataSet>(SelectionSummaryTextBox.Value);
                    summaryTable = summaryDataset.Tables["Selections"];

                    // Save Configured Price to DataSet
                    ConfiguredPrice.Value = ConfiguredPriceTextBox.Value;
                    configuredDataset = JsonConvert.DeserializeObject<DataSet>(ConfiguredPriceTextBox.Value);
                    configuredTable = configuredDataset.Tables["Details"];

                    // Save Configured Price to DataSet
                    ChairSelect.Value = ChairSelectTextBox.Value;
                    chairselectDataset = JsonConvert.DeserializeObject<DataSet>(ChairSelectTextBox.Value);
                    chairselectTable = chairselectDataset.Tables["Options"];

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
    public void initializeChairOptions()
    {
        string  sList = "";

        // Check that chair select options were found
        //if (chairselectTable.Rows.Count > 0)
        //{
        //    foreach (DataRow row in chairselectTable.Rows)
        //    {
        //        // Output variable fields: Caption and Value
        //        string parmkey = row["name"].ToString();
        //        string parmvalue = row["value"].ToString();

        //        if (parmkey == "RULE")
        //        {
        //            sList += "<p><strong>" + RuleSet + "</strong></p>";
        //        }
        //    }
        //}

        // Display Chair Image
        sList += "<section id=\"row-content-complete\" class=\"content complete left\">";
        sList += "<img src=\"" + ImageURL + "\" alt=\"Via Chair\">";
        sList += "</section>";

        // Check that chair options were found
        if (summaryTable.Rows.Count > 0)
        {
            // Output Selection Summary
            sList += "<section id=\"row-sidebar-complete\" class=\"sidebar complete right\">";
            sList += "<ul class='order single'>";
            foreach (DataRow row in summaryTable.Rows)
            {
                // Output variable fields: Caption and Value
                string parmkey = row["caption"].ToString();
                string parmvalue = row["value"].ToString();

                if (parmkey == "SERIES:" || parmkey == "SERIES")
                {
                    lSeries.Text = parmvalue;
                    HSeries.Value = parmvalue;
                }

                sList += "<li><strong>" + parmkey + "</strong> <span>" + parmvalue + "</span></li>";
            }
            sList += "</ul> <br /><br />";

            // Output Configured Price
            //sList += "<ul class='order single'>";
            foreach (DataRow row in configuredTable.Rows)
            {
                // Output variable fields: Caption, Value and Type
                string parmkey = row["caption"].ToString();
                string parmvalue = row["value"].ToString();
                string parmvisible = row["type"].ToString();

                if (parmkey == "LIST PRICE:")
                {
                    lListPrice.Text = parmvalue;
                    ListPriceValue = parmvalue;
                    HPrice.Value = parmvalue;
                }

                //sList += "<li><strong>" + parmkey + "</strong> <span>" + parmvalue + "</span></li>";
            }
            //sList += "</ul> <br /><br />";
            sList += "</section>";

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

    public void btnGeneratePDF_Click(object sender, EventArgs e)
    {
        try
        {

            // Local Variables
            var sPrice = "";
            var sFamily = "";

            //// Get current SummarySelection and convert to Json Data
            summaryDataset = JsonConvert.DeserializeObject<DataSet>(SelectionSummary.Value);
            summaryTable = summaryDataset.Tables["Selections"];

            // Initialize VIA logo, Fonts and Colors Scheme
            string LOGOURL = Server.MapPath(".") + "/images/via-logo-with-sweet-spot-tagline.gif";
            string CHAIRURL = ChairImageURL.Value;

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

            // Create VIA Logo to PDF
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(LOGOURL);
            logo.ScaleToFit(140f, 120f);
            logo.SpacingBefore = 10f;
            logo.SpacingAfter = 30f;
            logo.Alignment = Element.ALIGN_CENTER;
            

            

            // Create selected chair image url to PDF
            iTextSharp.text.Image chair = iTextSharp.text.Image.GetInstance(CHAIRURL);
            chair.ScaleToFit(150f, 150f);
            chair.SpacingBefore = 10f;
            chair.SpacingAfter = 10f;
            chair.Alignment = Element.ALIGN_CENTER;
            

            // Write type to header
            //Paragraph header = new Paragraph("Brisbane");
            //header.Alignment = Element.ALIGN_CENTER;
            //pdfDoc.Add(header);

            // Create Table with option selection for Specification Sheet
            PdfPTable table = new PdfPTable(1);
            table.DefaultCell.Border = 0;
            table.TotalWidth = 200f;
            table.SpacingBefore = 5f;
            table.SpacingAfter = 5f;

            float[] widths = new float[] { 200f };
            table.SetWidths(widths);
            //table.HorizontalAlignment = Element.ALIGN_CENTER;

            var rowIDX = 1;
            PdfPCell cell;

            foreach (DataRow row in summaryTable.Rows)
            {
                Literal2.Text += row["caption"].ToString() + "<br>";
                cell = new PdfPCell();
                cell.BackgroundColor = white;
                cell.Border = 0;
                cell.FixedHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                var phraseSelection = new Phrase();
                //var phrase1 = new Phrase(row["caption"].ToString(), bodyBoldFont);
                //var phrase2 = new Phrase(row["value"].ToString(), bodyFont);
                phraseSelection.Add(new Phrase(row["caption"].ToString(), bodyBoldFont));
                phraseSelection.Add(new Phrase(" ", bodyFont));
                phraseSelection.Add(new Phrase(row["value"].ToString(), bodyFont));
                cell.AddElement(phraseSelection);
                //cell.AddElement(phrase2);
                table.AddCell(cell);


                rowIDX++;
            }


            // Create Table with Family and List Price
            PdfPTable tablePrice = new PdfPTable(3);
            tablePrice.DefaultCell.Border = 0;
            tablePrice.TotalWidth = 400f;
            tablePrice.SpacingBefore = 40f;
            tablePrice.SpacingAfter = 5f;
            float[] widthsPrice = new float[] { 190f, 10f, 190f };
            tablePrice.SetWidths(widthsPrice);
            tablePrice.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cellPrice;

            // First Row - First Column - Add Family Title
            cellPrice = new PdfPCell(new Phrase(HRuleset.Value.ToString(), bodyBoldFont));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_LEFT;
            tablePrice.AddCell(cellPrice);

            // First Row - Second Column - Add Column Gap
            cellPrice = new PdfPCell(new Phrase(" "));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_LEFT;
            tablePrice.AddCell(cellPrice);

            // First Row - Third Column - Add List Price
            var sListPrice = "LIST PRICE";
            cellPrice = new PdfPCell(new Phrase(sListPrice.ToString(), bodyBoldFont));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablePrice.AddCell(cellPrice);

            // Second Row - First Column - Add Family Title Value
            cellPrice = new PdfPCell(new Phrase(HSeries.Value.ToString(), bodyFont));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_LEFT;
            tablePrice.AddCell(cellPrice);

            // Second Row - Second Column - Add Column Gap
            cellPrice = new PdfPCell(new Phrase(" "));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_LEFT;
            tablePrice.AddCell(cellPrice);

            // Second Row - Third Column - Add List Price Value
            cellPrice = new PdfPCell(new Phrase(HPrice.Value.ToString(), bodyFont));
            cellPrice.BackgroundColor = white;
            cellPrice.Border = 0;
            cellPrice.FixedHeight = 15f;
            cellPrice.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellPrice.HorizontalAlignment = Element.ALIGN_RIGHT;
            tablePrice.AddCell(cellPrice);


            // Create Layout Table -
            PdfPTable tableLayout = new PdfPTable(3);
            tableLayout.DefaultCell.Border = 0;
            tableLayout.TotalWidth = 400f;
            tableLayout.SpacingBefore = 40f;
            tableLayout.SpacingAfter = 5f;
            float[] widthsLayout = new float[] { 190f, 10f, 190f };
            tableLayout.SetWidths(widthsLayout);
            tableLayout.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell cellLayout;

            // First Row - First Column - Add Chair Image
            tableLayout.AddCell(chair);

            // First Row - Second Column - Add Column Gap
            cellLayout = new PdfPCell(new Phrase(" "));
            cellLayout.BackgroundColor = white;
            cellLayout.Border = 0;
            cellLayout.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellLayout.HorizontalAlignment = Element.ALIGN_LEFT;
            tableLayout.AddCell(cellLayout);

            // First Row - Third Column - Add List
            tableLayout.AddCell(table);

            pdfDoc.Add(logo);
            pdfDoc.Add(tablePrice);
            pdfDoc.Add(tableLayout);
            //pdfDoc.Add(chair);
            //pdfDoc.Add(table);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Via-Specification-Sheet2.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
}