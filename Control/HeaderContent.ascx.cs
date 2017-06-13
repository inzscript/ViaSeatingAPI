using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Control_HeaderContent : System.Web.UI.UserControl
{
    private string m_SeriesTitle = string.Empty;

    public string SeriesTitle
    {
        get { return m_SeriesTitle; }
        set { m_SeriesTitle = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ltlSeriesHdr.Text = m_SeriesTitle;
        }
    }
}