using Roadkill.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csuzw.Roadkill.TagTreeMenu
{
    internal class PageMetaDataProvider
    {
        private readonly Lazy<IEnumerable<PageMetaData>> _pages;

        public PageMetaDataProvider(IRepository repository)
        {
            _pages = new Lazy<IEnumerable<PageMetaData>>(() => repository.AllPages().Select(p => new PageMetaData(p)));
        }

        public IEnumerable<PageMetaData> GetPages(params TagKey[] keys)
        {
            var comparer = StringComparer.CurrentCultureIgnoreCase;
            var superset = keys.SelectMany(k => k.Tags.Select(t => t.Name)).Distinct();
            return _pages.Value.Where(p => keys.All(t => t.IsMatch(p.Tags, comparer)) && !p.Tags.Except(superset, comparer).Any()).OrderBy(p => p.Name);
        }
    }
}
