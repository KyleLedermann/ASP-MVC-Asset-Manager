using System;
using System.Collections.Generic;
using System.Text;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static AssetHelper Assets(this HtmlHelper htmlHelper)
        {
            return AssetHelper.GetInstance(htmlHelper);
        }
    }

    public class AssetHelper
    {
        private static string _assetsKey = Guid.NewGuid().ToString();
#if DEBUG
        private static bool _assetsInDebug = true;
#else
        private static bool _assetsInDebug = false;
#endif

        private static AssetHelper _instance = null;

        public static AssetHelper Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new AssetHelper(_assetsInDebug);
                }
                return _instance;
            }
        }

        public static AssetHelper GetInstance(HtmlHelper htmlHelper)
        {
            HttpContextBase context = htmlHelper.ViewContext.HttpContext;
            if (null == context)
            {
                return null;
            }

            if (!context.Items.Contains(_assetsKey))
            {
                context.Items.Add(_assetsKey, Instance);
            }

            return Instance;
        }

        private bool inDebugMode = false;

        public AssetHelper(bool mode)
        {
            this.inDebugMode = true;

            this.Styles = new AssetRegistrar(this, AssetRegistrarFormatters.StyleFormat);
            this.Scripts = new AssetRegistrar(this, AssetRegistrarFormatters.ScriptFormat);
            this.InlineScripts = new AssetRegistrar(this, AssetRegistrarFormatters.InlineScriptFormat);
        }

        public AssetRegistrar Styles
        {
            get;
            private set;
        }

        public AssetRegistrar Scripts
        {
            get;
            private set;
        }

        public AssetRegistrar InlineScripts
        {
            get;
            private set;
        }

        public bool IsModeDebug
        {
            get { return this.inDebugMode; }
        }
    }

    public class AssetRegistrar
    {
        private readonly AssetHelper helper;
        private readonly string format;
        private readonly IList<string> items;

        public AssetRegistrar(AssetHelper helper, string format)
        {
            this.helper = helper;
            this.format = format;
            this.items = new List<string>();
        }

        public AssetRegistrar Add(string uri)
        {
            if (!this.items.Contains(uri))
            {
                this.items.Add(uri);
            }

            return this;
        }

        public IHtmlString Render()
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (string item in this.items)
            {
                string path = item;
                if (path.StartsWith("~"))
                {
                    path = VirtualPathUtility.ToAbsolute(item);
                    if (this.helper.IsModeDebug)
                    {
                        double timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                        path = string.Format("{0}?d={1}", path, timestamp);
                    }
                }

                string str = string.Format(this.format, path);
                strBuilder.Append(str);
            }

            return new HtmlString(strBuilder.ToString());
        }
    }

    public class AssetRegistrarFormatters
    {
        public const string StyleFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"type/css\" />";
        public const string ScriptFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";
        public const string InlineScriptFormat = "<script type=\"text/javascript\">{0}</script>";
    }
}