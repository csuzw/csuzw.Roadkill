using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using Roadkill.Core.Services;
using System.Linq;
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
            get { return "Generates links for all pages that reference current page.  Usage: {PageReference=Example-Page}"; }
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
            var pageName = match.Groups["page"].Value;
            var searchTerm = string.Format("]({0})", pageName);
            // SearchService might be more performant but the results it gives are bullshit!
            var pages = _repository.AllPages().Select(p => new PageModel(p, _repository)).Where(p => p.EncodedName != pageName && p.Content.Contains(searchTerm));
            var text = string.Join(" | ", pages.Select(p => string.Format("[{0}]({1})", p.Name, p.EncodedName)));
            return text;
        }
    }
}
