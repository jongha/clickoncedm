using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ControlAttributeHelper
/// </summary>
public class ControlAttributeHelper
{
    public ControlAttributeHelper()
    {
    }

    public static void BindOnClickToReturnFunction(WebControl ctrl, string function)
    {
        ctrl.Attributes.Add("onclick", string.Format("return {0};", function));
    }

    public static void BindOnClickToFunction(WebControl ctrl, string function)
    {
        ctrl.Attributes.Add("onclick", string.Format("{0};", function));
    }

    public static void BindOnClickToConfirm(WebControl ctrl, string message)
    {
        ctrl.Attributes.Add("onclick", string.Format("return confirm('{0}');", message));
    }

    public static void BindOnClickToAlert(WebControl ctrl, string message)
    {
        ctrl.Attributes.Add("onclick", string.Format("alert('{0}'); return false;", message));
    }

    public static void BindOnKeyPressToReturnFalse(WebControl ctrl)
    {
        ctrl.Attributes.Add("onkeypress", "if(event.keyCode==13){return false;}");
    }

    public static void BindOnKeyPressToTargetCtrl(WebControl ctrl, WebControl ctrlTarget, Page page)
    {
        ctrl.Attributes.Add("onkeypress", "if(event.keyCode==13){ "
                + page.ClientScript.GetPostBackEventReference(ctrlTarget, null) + "; return false;}");
    }

    public static void BindOnKeyPressToTargetCtrl(WebControl ctrl, WebControl ctrlTarget, string successCondition, Page page)
    {
        ctrl.Attributes.Add("onkeypress", "if(event.keyCode==13){if(" + successCondition + "){"
                + page.ClientScript.GetPostBackEventReference(ctrlTarget, null) + ";}return false;}");
    }
}
