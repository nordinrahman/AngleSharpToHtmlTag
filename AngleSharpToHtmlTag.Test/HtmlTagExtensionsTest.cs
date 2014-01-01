using System.Web;
using System.Web.Mvc;

using HtmlTags;

using NFluent;

using NSubstitute;

using Xunit;

namespace AngleSharpToHtmlTag.Test
{
    public class HtmlTagExtensionsTest
    {
        [Fact]
        public void ToHtmlTag_WhenHtmlStringIsNull_ShouldReturnNull()
        {
            IHtmlString htmlString = null;

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag).IsNull();
        }

        [Fact]
        public void ToHtmlTag_WhenHtmlStringIsAlreadyHtmlTag_ShouldReturnHtmlTagAsItIs()
        {
            var htmlString = Substitute.For<HtmlTag>("div");

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag).IsSameReferenceThan(htmlString);
        }
        
        [Fact]
        public void ToHtmlTag_WhenHtmlStringIsAlreadyDivTag_ShouldReturnHtmlTagAsItIs()
        {
            var htmlString = Substitute.For<DivTag>();

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag).IsSameReferenceThan(htmlString);
        }

        [Fact]
        public void ToHtmlTag_IfHtmlStringWithOneElement_ShouldReturnValidHtmlTag()
        {
            var htmlString = new HtmlString(new TagBuilder("div").ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenStringWithIdAttribute_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.GenerateId("divid");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div id=\"divid\"></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenStringWithIdAttributeAndClassAttribute_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.GenerateId("divid");
            tagBuilder.AddCssClass("class2");
            tagBuilder.AddCssClass("class1");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div id=\"divid\" class=\"class1 class2\"></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenStringWithIdAttributeAndNonStandardAttribute_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.GenerateId("divid");
            tagBuilder.Attributes.Add("xxx", "xxx");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div id=\"divid\" xxx=\"xxx\"></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenStringWithIdAttributeAndDataAttribute_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.GenerateId("divid");
            tagBuilder.Attributes.Add("data-id", "dataid");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div data-id=\"dataid\" id=\"divid\"></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenImageTag_ShouldReturnImageHtmlSelfEnclose()
        {
            var tagBuilder = new TagBuilder("img");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<img />");
        }

        [Fact]
        public void ToHtmlTag_GivenDivWithChildDiv_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.InnerHtml = new TagBuilder("div").ToString();
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div><div></div></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenDivWithInnerText_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.SetInnerText("inner text");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div>inner text</div>");
        }

        [Fact]
        public void ToHtmlTag_GivenDivWithCommentInside_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.InnerHtml = "<!-- Comment -->";
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div><!-- Comment --></div>");
        }

        [Fact]
        public void ToHtmlTag_GivenDivWithChildrenTextAndCommentAndElement_ShouldReturnValidHtmlTag()
        {
            var tagBuilder = new TagBuilder("div");
            tagBuilder.InnerHtml = "inner text<!-- Comment -->" + new TagBuilder("div");
            var htmlString = new HtmlString(tagBuilder.ToString());

            var htmlTag = HtmlTagExtensions.ToHtmlTag(htmlString);

            Check.That(htmlTag.ToHtmlString()).IsEqualTo("<div>inner text<!-- Comment --><div></div></div>");
        }
    }
}
