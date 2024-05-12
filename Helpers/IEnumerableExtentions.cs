using System;
using System.Collections.Generic;
using System.Linq;

namespace PullPitcher.Helpers
{
    public static class IEnumerableExtentions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
