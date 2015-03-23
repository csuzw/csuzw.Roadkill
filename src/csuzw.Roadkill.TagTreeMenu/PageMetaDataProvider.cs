using Roadkill.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csuzw.Roadkill.TagTreeMenu
{
    internal class PageMetaDataProvider
    {
        private Lazy<TagComparer> _tagComparer;
        private readonly Lazy<IEnumerable<PageMetaData>> _pages;

        public PageMetaDataProvider(IRepository repository)
        {
            _pages = new Lazy<IEnumerable<PageMetaData>>(() => repository.AllPages().Select(p => new PageMetaData(p)));
            _tagComparer = new Lazy<TagComparer>(() => new TagComparer());
        }

        public IEnumerable<PageMetaData> GetPages(params TagKey[] keys)
        {
            var superset = keys.SelectMany(k => k.Tags.Select(t => t.Name)).Distinct();
            return _pages.Value.Where(p => keys.All(t => t.IsMatch(p.Tags, _tagComparer.Value)) && p.Tags.All(t => superset.Any(s => _tagComparer.Value.IsMatch(t, s)))).OrderBy(p => p.Name);
        }
    }
}
