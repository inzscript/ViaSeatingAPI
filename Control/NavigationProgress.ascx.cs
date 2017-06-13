using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Control_NavigationProgress : System.Web.UI.UserControl
{
    private string m_step_num = string.Empty;

    public string CurrentStepDisplay
    {
        get { return m_step_num; }
        set { m_step_num = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // ... Switch on the string.
            switch (m_step_num)
            {
                case "step1":
                    lSteps.Text = "<li class='first active'><a href='/'>Begin</a></li><li class='next'>Select</li><li class='next'>Build</li><li class='next'>Complete</li>";
                    break;
                case "step2":
                    lSteps.Text = "<li class='previous visited first'><a href='/'>Begin</a></li><li class='active'>Select</li><li class='next'>Build</li><li class='next'>Complete</li>";
                    break;
                case "step3":
                    lSteps.Text = "<li class='visited first'><a href='/'>Begin</a></li><li class='previous visited'>Select</li><li class='active'>Build</li><li class='next'>Complete</li>";
                    break;
                case "step4":
                    lSteps.Text = "<li class='visited first'><a href='/'>Begin</a></li><li class='previous visited'>Select</li><li class='previous visited'>Build</li><li class='active'>Complete</li>";
                    break;
                default:
                    lSteps.Text = "<li class='first active'><a href='/'>Begin</a></li><li class='next'>Select</li><li class='next'>Build</li><li class='next'>Complete</li>";
                    break;
            }

        }
    }
}