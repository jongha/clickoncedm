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

namespace ClickOnceDMAdmin
{
    public partial class Default : System.Web.UI.Page
    {
        private Dictionary<string, SourceInfo> sourceList = new Dictionary<string, SourceInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            PluginRetrieverSection plugins = ConfigurationManager.GetSection("pluginSettings") as PluginRetrieverSection;
            foreach (PluginElement el in plugins.Plugins)
            {
                sourceList.Add(el.Name, new SourceInfo
                {
                    Source = el.Source,
                    Value = el.Value,
                    ConnectionString = ConfigurationManager.ConnectionStrings[el.Source].ConnectionString
                });

                rdoRecipeints.Items.Add(new ListItem(el.Name));
            }

            if (!Page.IsPostBack)
            {
                ControlAttributeHelper.BindOnClickToConfirm(btnSave, "발송 하시겠습니까?");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string recipient = rdoRecipeints.SelectedValue;
            SourceInfo sourceInfo = sourceList[recipient];

            TicketInfo ticketInfo = new TicketInfo();
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N").Substring(0, 16).ToUpper() + ".ticket";
            ticketInfo.SaveTicket(fileName, sourceInfo); // TODO: check result is success?

            Response.Redirect(Request.RawUrl, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}