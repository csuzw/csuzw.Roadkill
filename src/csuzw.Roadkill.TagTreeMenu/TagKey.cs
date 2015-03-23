using System.Collections.Generic;
using System.Linq;

namespace csuzw.Roadkill.TagTreeMenu
{
    internal class TagKey
    {
        public ICollection<Tag> Tags { get; private set; }

        public string Description { get { return string.Join(", ", Tags.Where(t => t.Operator != TagOperator.Null).Select(t => t.Description)); } }

        public TagKey(params Tag[] tags)
        {
            Tags = tags.ToList();
        }

        public bool IsMatch(IEnumerable<string> tags, TagComparer comparer)
        {
            var isMatch = false;
            foreach (var tag in Tags)
            {
                switch (tag.Operator)
                {
                    case TagOperator.And:
                        isMatch &= tags.Any(t => comparer.IsMatch(t, tag.Name));
                        break;
                    case TagOperator.Or:
                    case TagOperator.None:
                        isMatch |= tags.Any(t => comparer.IsMatch(t, tag.Name));
                        break;
                    case TagOperator.Null:
                    default:
                        break;
                }
            }
            return isMatch;
        }
    }
}
