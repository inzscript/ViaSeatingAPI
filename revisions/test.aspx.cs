using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        RadioButton OptionRadioBtn = new RadioButton();
        OptionRadioBtn.ID = "RadioBtn_1";
        OptionRadioBtn.GroupName = "Test Button";
        OptionRadioBtn.Checked = false;
        OptionRadioBtn.Enabled = true;
        OptionRadioBtn.Text = "Test Button";
        OptionRadioBtn.EnableViewState = true;
        OptionRadioBtn.AutoPostBack = true;
        OptionRadioBtn.Attributes.Add("Value", "Brisbane");
        OptionRadioBtn.CheckedChanged += (se, ev) => radioButton_CheckedChanged(se, ev);

        Panel1.Controls.Add(OptionRadioBtn);
    }

    protected void radioButton_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton btn = sender as RadioButton;
        lblHelloWorld.Text = "What up Player";
    }

    protected void btnHelloWorld_Click(object sender, EventArgs e)
    {
        lblHelloWorld.Text = "Hello, world - this is a fresh message from ASP.NET AJAX! The time right now is: " + DateTime.Now.ToLongTimeString();
    }
}