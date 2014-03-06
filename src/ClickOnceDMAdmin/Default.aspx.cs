using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.IO;
using ClickOnceDMAdmin.Configuration;
using System.Reflection;
using ClickOnceDMLib;
using System.Runtime.Serialization.Json;
using ClickOnceDMLib.Structs;
using ClickOnceDMLib.Path;
using ClickOnceDMLib.Process;

namespace ClickOnceDMAdmin
{
    public partial class Default : System.Web.UI.Page
    {
        private int id = 0;
        private Dictionary<string, Source> sourceList = new Dictionary<string, Source>();
        private ClickOnceDMLib.Data.History history = new ClickOnceDMLib.Data.History();

        protected void Page_Load(object sender, EventArgs e)
        {
            id = string.IsNullOrEmpty(Request.QueryString["id"]) ? 0 : Convert.ToInt32(Request.QueryString["id"]);

            PluginRetrieverSection plugins = ConfigurationManager.GetSection("pluginSettings") as PluginRetrieverSection;
            foreach (PluginElement el in plugins.Plugins)
            {
                ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[el.Source];

                sourceList.Add(el.Name, new Source
                {
                    Provider = connectionStringSetting.ProviderName,
                    Value = el.Value,
                    ConnectionString = connectionStringSetting.ConnectionString
                });

                rdoRecipeints.Items.Add(new ListItem(el.Name));
            }

            if (!Page.IsPostBack)
            {
                if (rdoRecipeints.Items.Count > 0)
                {
                    rdoRecipeints.Items[0].Selected = true;
                }

                if (id > 0)
                {
                    Ticket ticket = history.GetHistoryToTicket(this.id);
                    if (ticket != null)
                    {
                        txtSenderAddress.Text = ticket.SenderAddress;
                        txtSenderName.Text = ticket.SenderName;
                        txtSubject.Text = ticket.Subject;
                        txtHtml.Text = ticket.Body;
                    }
                }
            }

            ControlAttributeHelper.BindOnClickToFunction(btnSave, "return checkSendValidation()");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string recipient = rdoRecipeints.SelectedValue;

            Ticket ticket = new Ticket();

            ticket.SenderName = txtSenderName.Text.Trim();
            ticket.SenderAddress = txtSenderAddress.Text.Trim();
            ticket.Source = sourceList[recipient];
            ticket.Subject = txtSubject.Text.Trim();
            ticket.Body = txtHtml.Text;
            
            TicketProcess ticketProcess = new TicketProcess();

            ticketProcess.SaveTicket(ticket); // TODO: check result is success?

            try
            {
                history.SetHistory(ticket);
                history.DeleteHistory(100);
            }
            catch { }

            Response.Redirect("~/Logs.aspx", false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}