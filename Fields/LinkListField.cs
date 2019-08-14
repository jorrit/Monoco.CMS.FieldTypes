using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Monoco.CMS.Fields
{
    public class LinkListField : XmlField
    {
        private IEnumerable<Link> _links;

        /// <summary>
        /// Contains all links.
        /// </summary>
        public IEnumerable<Link> Links
        {
            get
            {
                if (_links == null)
                {
                    ParseLinks();
                }
                return _links;
            }

            set
            {
                WriteLinks(value);
                _links = null;
            }
        }

        /// <summary>
        /// Parses the XML document and populates the links collection.
        /// </summary>
        private void ParseLinks()
        {
            XDocument document = XDocument.Parse(Value);
            _links = from link in document.Descendants("link")
                     select ParseLink(link);
        }

        /// <summary>
        /// Writes _links to the field value.
        /// </summary>
        private void WriteLinks(IEnumerable<Link> links)
        {
            var stringWriter = new StringWriter();
            using (var xmlTextWriter = new XmlTextWriter(stringWriter))
            {
                xmlTextWriter.WriteStartElement("links");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        xmlTextWriter.WriteStartElement("link");
                        xmlTextWriter.WriteAttributeString("text", link.Text);
                        xmlTextWriter.WriteAttributeString("linktype", link.LinkType);
                        xmlTextWriter.WriteAttributeString("class", link.Class);
                        xmlTextWriter.WriteAttributeString("title", link.Title);
                        xmlTextWriter.WriteAttributeString("target", link.Target);
                        xmlTextWriter.WriteAttributeString("querystring", link.QueryString);
                        if (link.LinkType == "internal" || link.LinkType == "media")
                        {
                            xmlTextWriter.WriteAttributeString("id", link.TargetItem?.ID.ToString());
                        }
                        if (!string.IsNullOrEmpty(link.Anchor))
                        {
                            xmlTextWriter.WriteAttributeString("anchor", link.Anchor);
                        }
                        xmlTextWriter.WriteEndElement();
                    }
                }

                xmlTextWriter.WriteEndElement();
                Value = stringWriter.ToString();
            }
        }

        /// <summary>
        /// Gets the url for a link, based on link type.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private Link ParseLink(XElement link)
        {
            var result = new Link
            {
                Anchor = GetElementAttribute(link, "anchor"),
                Class = GetElementAttribute(link, "class"),
                LinkType = GetElementAttribute(link, "linktype"),
                QueryString = GetElementAttribute(link, "querystring"),
                Target = GetElementAttribute(link, "target"),
                Title = GetElementAttribute(link, "title"),
                Text = GetElementAttribute(link, "text"),
                Url = string.Empty,
            };

            switch (result.LinkType.ToLowerInvariant())
            {
                case "internal": // Internal link, the the target item and return it's URL.
                    var id = GetElementAttribute(link, "id");
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if (ID.TryParse(id, out ID itemId))
                        {
                            var item = Sitecore.Context.Database.GetItem(itemId);

                            if (item != null)
                            {
                                result.Url = Sitecore.Links.LinkManager.GetItemUrl(item);
                                result.TargetItem = item;
                            }
                        }
                    }
                    break;

                case "media": // Link is a media item, get the resource's URL.
                    var id2 = GetElementAttribute(link, "id");
                    if (!string.IsNullOrWhiteSpace(id2))
                    {
                        if (ID.TryParse(id2, out ID mediaId))
                        {
                            var item = Sitecore.Context.Database.GetItem(mediaId);
                            if (item != null)
                            {
                                var mediaItem = (MediaItem)item;
                                result.Url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem);
                                result.TargetItem = item;
                            }
                        }
                    }
                    break;

                default: // all other links are considered external.
                    result.Url = GetElementAttribute(link, "url");
                    break;
            }

            return result;
        }

        private string GetElementAttribute(XElement element, string name)
        {
            return element.Attribute(name)?.Value ?? string.Empty;
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
            {
                return new LinkListField(field);
            }

            return null;
        }

        public override void ValidateLinks(LinksValidationResult result)
        {
            foreach (var link in Links)
            {
                if (link.LinkType == "internal")
                {
                    if (link.TargetItem != null || !string.IsNullOrEmpty(link.Url))
                    {
                        Item targetItem = link.TargetItem;
                        if (targetItem != null)
                        {
                            result.AddValidLink(targetItem, link.InternalPath);
                        }
                        else
                        {
                            result.AddBrokenLink(link.InternalPath);
                        }
                    }
                }
                else if (link.LinkType == "media" && (link.TargetItem != null || !string.IsNullOrEmpty(link.MediaPath)))
                {
                    var targetItem2 = link.TargetItem;
                    if (targetItem2 != null)
                    {
                        result.AddValidLink(targetItem2, link.MediaPath);
                    }
                    else
                    {
                        result.AddBrokenLink(link.MediaPath);
                    }
                }
            }
        }
    }
}
