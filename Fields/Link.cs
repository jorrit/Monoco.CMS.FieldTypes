using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Monoco.CMS.Fields
{
    public class Link
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public string LinkType { get; set; }

        public Item TargetItem { get; set; }
    }
}