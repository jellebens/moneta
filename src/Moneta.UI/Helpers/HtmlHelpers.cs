using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Moneta.UI
{
    public static class HtmlHelpers
    {

        public static string IsSelected(this IHtmlHelper html, string page = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            
            string currentPage = (string)html.ViewContext.HttpContext.Request.Path;

            if (String.IsNullOrEmpty(page))
                page = currentPage;

            
            return page == currentPage ?
                cssClass : String.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }
}
