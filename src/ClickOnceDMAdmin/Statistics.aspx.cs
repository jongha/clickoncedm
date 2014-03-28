using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin
{
    public partial class Statistics : System.Web.UI.Page
    {
        private int dataIndex = 0;
        private ClickOnceDMLib.Data.Statistics statisticsData = new ClickOnceDMLib.Data.Statistics();

        public int DataIndex
        {
            get { return ++this.dataIndex; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string query = !string.IsNullOrEmpty(Request.QueryString["query"]) ? Request.QueryString["query"].ToString().Trim() : string.Empty;

                txtQuery.Text = query;

                DataTable dt = statisticsData.GetSearchStatistics(query);
                rptStatistics.DataSource = dt;
                rptStatistics.DataBind();

                long success = 0, error = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    success += Convert.ToInt64(dr["Success"]);
                    error += Convert.ToInt64(dr["Error"]);
                }

                litSuccess.Text = success.ToString("###,###,##0");
                litError.Text = error.ToString("###,###,##0");
                litTotal.Text = (success + error).ToString("###,###,##0");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("~/Statistics.aspx?query={0}", txtQuery.Text),
                false
                );
        }

        protected void rptStatistics_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                statisticsData.DeleteStatisticsy(e.CommandArgument.ToString());
            }

            Response.Redirect(Request.RawUrl, false);
        }

        protected void rptStatistics_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string subject = DataBinder.Eval(e.Item.DataItem, "Subject").ToString();
            Button btnDelete = (Button)e.Item.FindControl("btnDelete");

            if (btnDelete != null)
            {
                btnDelete.CommandName = "Delete";
                btnDelete.CommandArgument = subject;
            }
        }
    }
}