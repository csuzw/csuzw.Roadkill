using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csuzw.Roadkill.TagTreeMenu
{
    internal class PageMetaData
    {
        private readonly Lazy<string> _encodedName;
        private readonly Lazy<ICollection<string>> _tags;

        public string Name { get; private set; }
        public string EncodedName { get { return _encodedName.Value; } }
        public ICollection<string> Tags { get { return _tags.Value; } }

        public PageMetaData(Page page)
        {
            Name = page.Title;
            _encodedName = new Lazy<string>(() => PageViewModel.EncodePageTitle(page.Title));
            _tags = new Lazy<ICollection<string>>(() => PageViewModel.ParseTags(page.Tags).ToList());
        }

        public bool IsExactTagMatch(IEnumerable<string> tags)
        {
            return !Tags.Except(tags).Any();
        }
    }
}
