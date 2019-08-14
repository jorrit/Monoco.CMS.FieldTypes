using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Links;
using System;

namespace Monoco.CMS.Fields
{
    public class Link
    {
        public string Anchor { get; set; }
        public string Class { get; set; }
        public string LinkType { get; set; }
        public string QueryString { get; set; }
        public string Target { get; set; }
        public Item TargetItem { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

        public string InternalPath
        {
            get
            {
                if (LinkType == "internal")
                {
                    string text = string.IsNullOrEmpty(Url) ? LinkManager.GetItemUrl(TargetItem) : Url;
                    if (text.Length > 0)
                    {
                        if (!text.StartsWith("/sitecore", StringComparison.OrdinalIgnoreCase))
                        {
                            text = FileUtil.MakePath("/sitecore/content", text);
                        }
                        return text;
                    }
                }

                return string.Empty;
            }
        }

        public string MediaPath
        {
            get
            {
                if (LinkType == "media")
                {
                    string text = Url;
                    if (text.Length > 0)
                    {
                        if (!text.StartsWith("/sitecore", StringComparison.OrdinalIgnoreCase))
                        {
                            text = FileUtil.MakePath("/sitecore/media library", text);
                        }
                        return text;
                    }
                }
                return string.Empty;
            }
        }

    }
}