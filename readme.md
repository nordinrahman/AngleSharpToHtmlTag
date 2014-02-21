# HtmlStringToHtmlTag

Helper extension methods to convert instance of [IHtmlString][1] to [HtmlTag][2].

It provides "ToHtmlTag" extension method, that could be applied against instance of IHtmlString (such as from HtmlHelper.XXX or HtmlHelper.XXXFor method).

The motivation behind this project, is to minimized effort working on legacy codes whilst still benefit from HtmlTag, especifically to easily use HtmlTag library against:

 - existing Razor code page, without extra need to convert existing HtmlHelper methods usage
 - existing legacy HTML constructed via StringBuilder

  [1]: http://msdn.microsoft.com/en-us/library/system.web.ihtmlstring%28v=vs.110%29.aspx
  [2]: https://github.com/DarthFubuMVC/htmltags
