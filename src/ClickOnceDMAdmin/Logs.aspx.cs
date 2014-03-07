using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin
{
    public partial class Logs : System.Web.UI.Page
    {
        private ClickOnceDMLib.Data.History history = new ClickOnceDMLib.Data.History();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                rptLog.DataSource = history.GetHistory();
                rptLog.DataBind();
            }
        }

        protected void rptLog_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string id = DataBinder.Eval(e.Item.DataItem, "Id").ToString();
            Button btnDelete = (Button)e.Item.FindControl("btnDelete");

            if (btnDelete != null)
            {
                btnDelete.CommandName = "Delete";
                btnDelete.CommandArgument = id;
            }
        }

        protected void rptLog_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                history.DeleteHistory(Convert.ToInt32(e.CommandArgument));
            }

            Response.Redirect(Request.RawUrl, false);
        }
    }
}