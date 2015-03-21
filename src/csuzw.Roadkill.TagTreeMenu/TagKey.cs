using System.Collections.Generic;
using System.Linq;

namespace csuzw.Roadkill.TagTreeMenu
{
    internal class TagKey
    {
        public ICollection<Tag> Tags { get; private set; }

        public string Description { get { return string.Join(", ", Tags.Select(t => t.Description)); } }

        public TagKey(params Tag[] tags)
        {
            Tags = tags.ToList();
        }

        public bool IsMatch(IEnumerable<string> tags)
        {
            var isMatch = false;
            foreach (var tag in Tags)
            {
                if (tag.Operator == TagOperator.And)
                {
                    isMatch &= tags.Contains(tag.Name);
                }
                else
                {
                    isMatch |= tags.Contains(tag.Name);
                }
            }
            return isMatch;
        }
    }
}
