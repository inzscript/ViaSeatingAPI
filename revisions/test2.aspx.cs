using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        RadioButton OptionRadioBtn = new RadioButton();
        OptionRadioBtn.ID = "RadioBtn_1";
        OptionRadioBtn.GroupName = "Test";
        OptionRadioBtn.Checked = false;
        OptionRadioBtn.Enabled = true;
        OptionRadioBtn.Text = "Test Button";
        OptionRadioBtn.EnableViewState = true;
        OptionRadioBtn.Attributes.Add("Value", "Brisbane");
        OptionRadioBtn.CheckedChanged += (se, ev) => radioButton_CheckedChanged(se, ev);
        OptionRadioBtn.AutoPostBack = true;
        Panel1.Controls.Add(OptionRadioBtn);

        RadioButton OptionRadioBtn2 = new RadioButton();
        OptionRadioBtn2.ID = "RadioBtn_2";
        OptionRadioBtn2.GroupName = "Test";
        OptionRadioBtn2.Checked = false;
        OptionRadioBtn2.Enabled = true;
        OptionRadioBtn2.Text = "Test Button 2";
        OptionRadioBtn2.EnableViewState = true;
        OptionRadioBtn2.Attributes.Add("Value", "Brisbane 2");
        OptionRadioBtn2.CheckedChanged += (se, ev) => radioButton_CheckedChanged(se, ev);
        OptionRadioBtn2.AutoPostBack = true;
        Panel1.Controls.Add(OptionRadioBtn2);
    }

    protected void radioButton_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton btn = sender as RadioButton;
        lblHelloWorld.Text = "What up Player: " + DateTime.Now.ToLongTimeString();
    }

    void b_Command(object sender, CommandEventArgs e)
    {
        lblHelloWorld.Text = "What up Player: " + DateTime.Now.ToLongTimeString();
    }

    protected void btnHelloWorld_Click(object sender, EventArgs e)
    {
        lblHelloWorld.Text = "Hello, world - this is a fresh message from ASP.NET AJAX! The time right now is: " + DateTime.Now.ToLongTimeString();
    }
}