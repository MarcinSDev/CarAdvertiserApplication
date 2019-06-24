using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CarAdvertiser.DAL;
using CarAdvertiser.DTO;
using Microsoft.AspNet.Identity;

namespace CarAdvertiser.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetEmail(this IIdentity identy)
        {
            if (!int.TryParse(identy.GetUserId(), out int userId)) return null;

            using (CarAdvertiserContext context = new CarAdvertiserContext())
            {
                AppUserV2 user = context.GetDbSet<AppUserV2>().FirstOrDefault(x => x.Id == userId);
                return user?.Email;
            }
        }
    }



    // https://forums.asp.net/t/1758990.aspx?How+to+get+access+to+a+list+of+Models+in+jQuery+
    public static class JavascriptExtensions
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString JSVariableFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return JSVariable(htmlHelper,
                                ExpressionHelper.GetExpressionText(expression),
                                metadata.Model);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString JSVariable(this HtmlHelper htmlHelper, string varName, object value)
        {
            return MvcHtmlString.Create(string.Format("var {0} = {1};", varName, serializeValue(value)));
        }


        public static MvcHtmlString JSValue(this HtmlHelper helper, object value)
        {
            return MvcHtmlString.Create(serializeValue(value));
        }

        public static MvcHtmlString PageID(this HtmlHelper helper)
        {
            var controller = helper.ViewContext.RouteData.Values["controller"].ToString();
            var action = helper.ViewContext.RouteData.Values["action"].ToString();
            return MvcHtmlString.Create(string.Format("pgid:{0}.{1}", controller, action));
        }


        private static string serializeValue(object value)
        {
            var varValue = "";
            if (value == null) varValue = "null";
            else if (value is string) varValue = quoteString(value.ToString());
            else if (value is DateTime) value = quoteString(Convert.ToString((DateTime)value));
            else if (value is DateTime?) value = quoteString(Convert.ToString((DateTime?)value));
            else if (value is bool || value is bool?) varValue = value.ToString().ToLower();
            else if (value is int || value is int?
                    || value is byte || value is byte?
                    || value is long || value is long?
                    || value is float || value is float?
                    || value is double || value is double?
                    || value is decimal || value is decimal) varValue = value.ToString();
            else if (value is char || value is char?) varValue = quoteString(value.ToString());
            else
            {
                var sb = new StringBuilder();
                var serializer = new JavaScriptSerializer();
                serializer.Serialize(value, sb);
                varValue = sb.ToString();
            }
            return varValue;
        }

        private static string quoteString(string s)
        {
            return "'"
                + s
                    .Replace("\\", "\\\\")
                    .Replace("'", "\'")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                + "'";
        }
    }
}