using ClickOnceDMLib;
using ClickOnceDMLib.Path;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin
{
    public partial class Tickets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                litTicketsCount.Text = Directory.GetFiles(PathInfo.Ticket).Count().ToString("###,###,###,###");
                litQueueCount.Text = Directory.GetFiles(PathInfo.Queue).Count().ToString("###,###,###,###");
            }
        }
    }
}