using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;

namespace DutchVerbs;

public static class StringExtensions
{
    private static readonly string Space = "&nbsp;";
    public static MarkupString HtmlPadLeft(this string str, int length)
    {
        var htmlStr = HtmlEncoder.Default.Encode(str);
        var spacesToAdd = length - str.Length;
        if (spacesToAdd < 0)
        {
            spacesToAdd = 0;
        }

        var stringBuilder = new StringBuilder(htmlStr.Length + spacesToAdd * Space.Length);

        for (var i = 0; i < spacesToAdd; i++)
        {
            stringBuilder.Append(Space);
        }
        stringBuilder.Append(htmlStr);

        var result = new MarkupString(stringBuilder.ToString());

        return result;
    }
}
