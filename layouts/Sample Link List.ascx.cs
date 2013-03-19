using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Monoco.CMS.Fields;

namespace Monoco.CMS.layouts
{
    public partial class SampleLinkList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var item = Sitecore.Context.Item;
            if (item.Fields["Links"] != null)
            {
                var relatedLinks = (LinkListField) item.Fields["Links"];
                if (relatedLinks != null && relatedLinks.Links.Any())
                {
                    relatedLinksRepeater.DataSource = relatedLinks.Links;
                    relatedLinksRepeater.DataBind();
                }
            }
        }
    }
}