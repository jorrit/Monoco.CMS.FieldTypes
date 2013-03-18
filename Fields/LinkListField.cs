using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Monoco.CMS.Fields
{
    public class LinkListField : Sitecore.Data.Fields.XmlField
    {
        private IEnumerable<Link> _links;
        /// <summary>
        /// Contains all links.
        /// </summary>
        public IEnumerable<Link> Links
        {
            get { 
                if (_links == null)
                {
                    ParseLinks();
                }
                return _links; 
            }
        }
        /// <summary>
        /// Parses the XML document and populates the links collection.
        /// </summary>
        private void ParseLinks()
        {
            XDocument document = XDocument.Parse(Value);
            _links = from link in document.Descendants("link")
                     select new Link
                                {
                                    Target = GetElementAttribute(link, "target"),
                                    Title = GetElementAttribute(link, "text"),
                                    LinkType = GetElementAttribute(link, "linktype"),
                                    Url = GetUrlFromLink(link)
                                };
        }
        /// <summary>
        /// Gets the url for a link, based on link type.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private string GetUrlFromLink(XElement link)
        {
            switch (GetElementAttribute(link, "linktype").ToLowerInvariant())
            {
                case "internal": // Internal link, the the target item and return it's URL.
                    var id = GetElementAttribute(link, "id");
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        Sitecore.Data.ID itemId;
                        if (Sitecore.Data.ID.TryParse(id, out itemId))
                        {
                            var item = Sitecore.Context.Database.GetItem(itemId);

                            return item != null ? Sitecore.Links.LinkManager.GetItemUrl(item) : String.Empty;
                        }
                        return String.Empty;
                    }
                    return String.Empty;
                case "media": // Link is a media item, get the resource's URL.
                    var id2 = GetElementAttribute(link, "id");
                    if (!String.IsNullOrWhiteSpace(id2))
                    {
                        Sitecore.Data.ID mediaId;
                        if (Sitecore.Data.ID.TryParse(id2, out mediaId))
                        {
                            var item = Sitecore.Context.Database.GetItem(mediaId);
                            if (item == null)
                                return String.Empty;
                            var mediaItem = (MediaItem) item;
                            return Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);
                        }
                    }
                    return String.Empty;
                default: // all other links are considered external.
                    return GetElementAttribute(link, "url");
            }
        }

        private string GetElementAttribute(XElement element, string name)
        {
            return element != null && element.Attribute(name) != null
                       ? element.Attribute(name) != null ? element.Attribute(name).Value : String.Empty
                       : String.Empty;
        }

        public LinkListField(Field innerField) : base(innerField, "links")
        {
            
        }
        
        public LinkListField(Field innerField, string root, string runtimeValue) : base(innerField, root, runtimeValue)
        {
        }
        /// <summary>
        /// Implicitly converts a Sitecore Field do a LinkListField.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static implicit operator LinkListField(Field field)
        {
            if (field != null)
                return new LinkListField(field);
            return null;
        }
    }
}