using System;
using System.Collections.Generic;

namespace Pretorianie.Tytan.Data
{
    /// <summary>
    /// Class for name generation. The specific sets of prefixes, bodies and postfixes might be given as a parameters, so it can be used
    /// to generate any kind of names.
    /// </summary>
    public class NameProvider
    {
        private readonly Random r;
        private readonly ICollection<string> pre;
        private readonly ICollection<string> body;
        private readonly ICollection<string> post;
        private readonly double probabilityPre;
        private readonly double probabilityBody;
        private readonly double probabilityPost;

        /// <summary>
        /// Init constructor. Accepts the set of elements that generator will work on.
        /// </summary>
        public NameProvider(ICollection<string> pre, double probabilityPre,
                            ICollection<string> body, double probabilityBody,
                            ICollection<string> post, double probabilityPost)
        {
            r = new Random();
            this.pre = pre;
            this.probabilityPre = probabilityPre;
            this.body = body;
            this.probabilityBody = probabilityBody;
            this.post = post;
            this.probabilityPost = probabilityPost;
        }

        /// <summary>
        /// Generate new name based on defined sets of prefixes, bodies and postfixes.
        /// </summary>
        public string NextName()
        {
            return string.Format("{0}{1}{2}", GetRandomItem(pre, probabilityPre),
                            GetRandomItem(body, probabilityBody),
                            GetRandomItem(post, probabilityPost));
        }

        /// <summary>
        /// Gets the random element of the collection.
        /// </summary>
        private string GetRandomItem(ICollection<string> c, double probability)
        {
            if (c == null || c.Count == 0)
                return string.Empty;

            // if user-defined probability not occured...
            // don't generate the content:
            if(r.NextDouble() >= probability && probability < 1)
                return string.Empty;

            // get the random element:
            int n = r.Next(c.Count);
            IEnumerator<string> e = c.GetEnumerator();

            // and then drive to specific element:
            for (int i = 0; i < n; i++)
                e.MoveNext();

            return e.Current;
        }
    }
}
