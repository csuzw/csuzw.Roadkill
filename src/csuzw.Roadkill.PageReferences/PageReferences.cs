using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using System.Text.RegularExpressions;

namespace csuzw.Roadkill.PageReferences
{
    public class PageReferences : TextPlugin
    {
        private static readonly Regex _regex = new Regex(@"(?<!\{)\{PageReferences=(?<page>[\w-]+?)\}(?!\})", RegexOptions.Singleline | RegexOptions.Compiled);

        private readonly IRepository _repository;

        #region Properties

        public override string Id
        {
            get { return "PageReferences"; }
        }

        public override string Name
        {
            get { return "Page References"; }
        }

        public override string Description
        {
            get { return "Creates footer links for all pages that reference current page.  Usage: {PageReference=Test-Page}"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        #endregion

        public PageReferences(IRepository repository)
        {
            _repository = repository;
        }

        public override string BeforeParse(string markupText)
        {
            markupText = _regex.Replace(markupText, m => GetPageReferences(m));

            return markupText;
        }

        private string GetPageReferences(Match match)
        {
            // TODO get current page name/encoded name

            // TODO run regex against all current pages (performance!!!)
            return "Hello Rob!";
        }
    }
}
