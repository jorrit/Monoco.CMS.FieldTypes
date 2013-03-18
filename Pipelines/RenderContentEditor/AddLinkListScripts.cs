using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sitecore.Pipelines;

namespace Monoco.CMS.Pipelines.RenderContentEditor
{
    public class AddLinkListScripts
    {
        /// <summary>
        /// Called by the pipeline.
        /// </summary>
        /// <param name="args"></param>
        public void Process(PipelineArgs args)
        {
            if (Sitecore.Context.ClientPage.IsEvent)
            {
                return;
            }
            var context = HttpContext.Current;
            if (context == null)
            {
                return;
            }

            var page = context.Handler as Page;
            if (page == null)
            {
                return;
            }
            // Inserts the javascript into the Sitecore client.
            page.Header.Controls.Add(new LiteralControl("<script type=\"text/javascript\""
                                                        + " src=\"/sitecore modules/Shell/FieldTypes/LinkList.js\">" +
                                                        "</script>")); 
        }
    }
}