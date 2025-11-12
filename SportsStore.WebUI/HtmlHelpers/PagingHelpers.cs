using SportsStore.WebUI.Models;
using System.Text;
using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportsStore.WebUI.HtmlHelpers
{
	public static class PagingHelpers
	{
		public static IHtmlContent PageLinks(this IHtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
		{
			var builder = new HtmlContentBuilder(pagingInfo.TotalPages);

			for (int i = 1; i <= pagingInfo.TotalPages; i++)
			{
				var tag = new TagBuilder("a");
				tag.Attributes["href"] = pageUrl(i);
				tag.InnerHtml.Append(i.ToString());
				if (i == pagingInfo.CurrentPage)
					tag.AddCssClass("selected");

				builder.AppendHtml(tag);
			}

			return builder;
		}
	}
}
