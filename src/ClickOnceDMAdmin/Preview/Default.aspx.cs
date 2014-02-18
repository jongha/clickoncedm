using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClickOnceDMAdmin.Preview
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.Form["action"];
            if (string.IsNullOrEmpty(action))
            {
                string html = HttpContext.Current.Session["PreviewHtml"].ToString();
                HttpContext.Current.Session.Remove("PreviewHtml");
                Response.Write("<!DOCTYPE html>" + html);
            }
            else if (action == "save")
            {
                string html = Request.Form["html"];
                if (string.IsNullOrEmpty(html))
                {
                    html = string.Empty;
                }

                HttpContext.Current.Session.Timeout = 1;
                HttpContext.Current.Session["PreviewHtml"] = html;
            }
        }
    }
}