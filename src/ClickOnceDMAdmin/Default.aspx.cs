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

namespace ClickOnceDMAdmin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PluginRetrieverSection plugins = ConfigurationManager.GetSection("pluginSettings") as PluginRetrieverSection;
            foreach (PluginElement el in plugins.Plugins)
            {
                rdoRecipeints.Items.Add(new ListItem(el.Name));
            }

            if (!Page.IsPostBack)
            {
                ControlAttributeHelper.BindOnClickToConfirm(btnSave, "발송 하시겠습니까?");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string queuePath = ConfigurationManager.AppSettings["QueuePath"];
            if (!string.IsNullOrEmpty(queuePath))
            {
                queuePath = Path.Combine(queuePath, Guid.NewGuid().ToString());
                if (!Directory.Exists(queuePath))
                {
                    Directory.CreateDirectory(queuePath);

                    //DataContractJsonSerializer serializer = new DataContractJsonSerializer();
                    
                }
            }

            Response.Redirect(Request.RawUrl, false);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}