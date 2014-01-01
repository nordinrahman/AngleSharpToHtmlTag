using System.Web;

using AngleSharp;
using AngleSharp.DOM;

using HtmlTags;

namespace AngleSharpToHtmlTag
{
    /// <summary>
    /// Container for extension method to convert <see cref="IHtmlString"/> and <see cref="Element"/> to <see cref="HtmlTag"/>
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

            var element = DocumentBuilder.Html(htmlString.ToHtmlString()).Body.FirstElementChild;

            return ToHtmlTag(element);
        }

        /// <summary>
        /// Convert <paramref name="element"/> to <see cref="HtmlTag"/> instance
        /// </summary>
        /// <param name="element"><see cref="Element"/> instance to be converted to <see cref="HtmlTag"/></param>
        /// <returns><see cref="HtmlTag"/> instance that is collerate with specified <paramref name="element"/></returns>
        public static HtmlTag ToHtmlTag(this Element element)
        {
            var tagName = element.NodeName;

            var htmlTag = new HtmlTag(tagName);

            foreach (var attribute in element.Attributes)
            {
                if (!attribute.IsId) htmlTag.Attr(attribute.Name, attribute.Value);
                else htmlTag.Id(attribute.Value);
            }

            if (element.HasChildNodes)
            {
                foreach (var child in element.ChildNodes)
                {
                    HtmlTag childTag;

                    switch (child.NodeType)
                    {
                        case NodeType.Element:
                            childTag = ToHtmlTag((Element)child);
                            break;

                        case NodeType.Text:
                            childTag = new LiteralTag(child.TextContent);
                            break;

                        default:
                            childTag = new LiteralTag(child.ToHtml());
                            break;
                    }

                    htmlTag.Append(childTag);
                }
            }

            return htmlTag;
        }
    }
}
