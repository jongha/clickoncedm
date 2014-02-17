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
        private Dictionary<string, Source> sourceList = new Dictionary<string, Source>();
        protected void Page_Load(object sender, EventArgs e)
        {
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
                ControlAttributeHelper.BindOnClickToConfirm(btnSave, "Are you sure?");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string recipient = rdoRecipeints.SelectedValue;
            Source sourceInfo = sourceList[recipient];

            TicketProcess ticketProcess = new TicketProcess();

            string fileName = string.Empty;
            ticketProcess.SaveTicket(sourceInfo, out fileName); // TODO: check result is success?

            Response.Redirect(Request.RawUrl, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}