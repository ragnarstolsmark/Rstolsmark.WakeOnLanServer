using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Rstolsmark.WakeOnLanServer.TagHelpers;

[HtmlTargetElement(Attributes = nameof(If))]
public class IfTagHelper : TagHelper
{
    public bool If { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!If)
        {
            output.SuppressOutput();
        }
    }
}