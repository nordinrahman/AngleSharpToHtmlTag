using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using HtmlTags;

namespace HtmlStringToHtmlTag
{
    /// <summary>
    /// Container for extension method to convert <see cref="IHtmlString"/> and <see cref="HtmlNode"/> to <see cref="HtmlTag"/>
    /// </summary>
    public static class HtmlTagExtensions
    {
        /// <summary>
        /// Convert <paramref name="htmlString"/> to <see cref="HtmlTag"/> instance
        /// </summary>
        /// <param name="htmlString"><see cref="IHtmlString"/> instance to be converted to <see cref="HtmlTag"/></param>
        /// <returns><see cref="HtmlTag"/> instance that is collerate with specified <paramref name="htmlString"/></returns>
        public static HtmlTag ToHtmlTag(this IHtmlString htmlString)
        {
            if (htmlString == null) return null;

            var htmlTag = htmlString as HtmlTag;
            if (htmlTag != null) return htmlTag;

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlString.ToHtmlString());
            var element = doc.DocumentNode.SelectSingleNode("//*");

            //var element = DocumentBuilder.Html(htmlString.ToHtmlString()).Body.FirstElementChild;

            return ToHtmlTag(element);
        }

        /// <summary>
        /// Convert <paramref name="element"/> to <see cref="HtmlTag"/> instance
        /// </summary>
        /// <param name="element"><see cref="HtmlNode"/> instance to be converted to <see cref="HtmlTag"/></param>
        /// <returns><see cref="HtmlTag"/> instance that is collerate with specified <paramref name="element"/></returns>
        public static HtmlTag ToHtmlTag(this HtmlNode element)
        {
            var tagName = element.Name.ToLowerInvariant();

            HtmlTag htmlTag;

            switch (tagName)
            {
                case "select":
                    htmlTag = new SelectTag();
                    break;

                case "dl":
                    htmlTag = new DLTag();
                    break;

                case "table":
                    htmlTag = new TableTag();
                    break;

                case "tr":
                    htmlTag = new TableRowTag();
                    break;

                case "form":
                    htmlTag = new FormTag();
                    break;

                case "br":
                    htmlTag = new BrTag();
                    break;

                case "div":
                    htmlTag = new DivTag();
                    break;

                case "a":
                    htmlTag = new LinkTag(element.InnerText, element.GetAttributeValue("src", string.Empty),
                        element.GetAttributeValue("class", string.Empty)
                            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    break;

                case "input":
                    switch (element.GetAttributeValue("type", string.Empty).ToLowerInvariant())
                    {
                        case "textbox":
                            htmlTag = new TextboxTag();
                            break;

                        case "hidden":
                            htmlTag = new HiddenTag();
                            break;

                        case "checkbox":
                            htmlTag = new CheckboxTag(element.Attributes.Contains("checked"));
                            break;

                        default:
                            htmlTag = new HtmlTag(tagName);
                            break;
                    }

                    break;

                default:
                    htmlTag = new HtmlTag(tagName);
                    break;
            }

            foreach (var attribute in element.Attributes)
            {
                if (!attribute.Name.Equals("id", StringComparison.OrdinalIgnoreCase)) htmlTag.Attr(attribute.Name, attribute.Value);
                else htmlTag.Id(attribute.Value);
            }

            if (element.HasChildNodes)
            {
                foreach (var child in element.ChildNodes)
                {
                    HtmlTag childTag;

                    switch (child.NodeType)
                    {
                        case HtmlNodeType.Element:
                            childTag = ToHtmlTag(child);
                            break;

                        case HtmlNodeType.Text:
                            childTag = new LiteralTag(child.InnerText);
                            break;

                        default:
                            childTag = new LiteralTag(child.OuterHtml);
                            break;
                    }

                    htmlTag.Append(childTag);
                }
            }

            return htmlTag;
        }

        /// <summary>
        /// Return all children of specified <see cref="HtmlTag"/> and their children all downwards under the hierarchy
        /// </summary>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlTag> AllChildrenTag(this HtmlTag htmlTag)
        {
            if (htmlTag == null || !htmlTag.Children.Any())
            {
                yield break;
            }

            foreach (var childTag in htmlTag.Children)
            {
                yield return childTag;

                foreach (var tag in AllChildrenTag(childTag))
                {
                    yield return tag;
                }
            }
        }
    }
}
