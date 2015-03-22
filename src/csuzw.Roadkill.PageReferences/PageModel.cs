using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
using System;

namespace csuzw.Roadkill.PageReferences
{
    public class PageModel
    {
        private readonly Lazy<string> _encodedName;
        private readonly Lazy<string> _content;

        public string Name { get; private set; }
        public string EncodedName { get { return _encodedName.Value; } }
        public string Content { get { return _content.Value; } }

        public PageModel(Page page, IRepository repository)
        {
            Name = page.Title;
            _encodedName = new Lazy<string>(() => PageViewModel.EncodePageTitle(page.Title));
            _content = new Lazy<string>(() => repository.GetLatestPageContent(page.Id).Text);
        }
    }
}
