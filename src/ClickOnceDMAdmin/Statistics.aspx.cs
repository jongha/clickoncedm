using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin
{
    public partial class Statistics : System.Web.UI.Page
    {
        private int dataIndex = 0;
        private DateTime startDateTime;
        private DateTime endDateTime;

        public int DataIndex
        {
            get { return ++this.dataIndex; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string start = !string.IsNullOrEmpty(Request.QueryString["start"]) ? Request.QueryString["start"].ToString().Trim() : string.Empty;
                string end = !string.IsNullOrEmpty(Request.QueryString["end"]) ? Request.QueryString["end"].ToString().Trim() : string.Empty;

                if (!string.IsNullOrEmpty(start))
                {
                    this.startDateTime = Convert.ToDateTime(start);
                    txtStartTime.Text = start;
                }
                else
                {
                    this.startDateTime = new DateTime();
                }

                if (!string.IsNullOrEmpty(end))
                {
                    this.endDateTime = Convert.ToDateTime(end);
                    txtEndTime.Text = end;
                }
                else
                {
                    this.endDateTime = new DateTime();
                }


                ClickOnceDMLib.Data.Statistics statisticsData = new ClickOnceDMLib.Data.Statistics();

                rptStatistics.DataSource = statisticsData.GetStatistics(this.startDateTime, this.endDateTime);
                rptStatistics.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("~/Statistics.aspx?start={0}&end={1}", txtStartTime.Text, txtEndTime.Text),
                false
                );
        }
    }
}