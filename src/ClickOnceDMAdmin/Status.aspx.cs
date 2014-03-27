using ClickOnceDMLib;
using ClickOnceDMLib.Path;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                litTicketsCount.Text = Directory.GetFiles(PathInfo.Ticket).Count().ToString("###,###,###,##0");
                litQueueCount.Text = Directory.GetFiles(PathInfo.Queue).Count().ToString("###,###,###,##0");

                // log file count
                int logCount = Directory.GetDirectories(PathInfo.Log).Count();
                litLogCount.Text = logCount.ToString("###,###,###,##0");

                if (logCount > 0)
                {
                    btnClearLog.Text += string.Format(" ({0})", logCount.ToString("###,###,###,##0"));

                    ControlAttributeHelper.BindOnClickToConfirm(
                        btnClearLog,
                        HttpContext.GetLocalResourceObject("~/Status.aspx", "Message_ClearLogsConfirm").ToString()
                        );
                }

                btnClearLog.Visible = (logCount > 0);
            }
        }

        protected void btnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete(PathInfo.Log, true); // empty log
            }
            catch { }
            
            try
            {
                Directory.Delete(PathInfo.Trash, true); // empty trash
            }
            catch { }

            Response.Redirect(Request.RawUrl, false);
        }
    }
}