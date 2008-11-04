using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Helper class for string comparison.
    /// </summary>
    public static class StringHelper
    {
        #region Public Interfaces

        /// <summary>
        /// Interface that allowes matching string values.
        /// </summary>
        public interface IStringFilter
        {
            /// <summary>
            /// Checks if the specified text matches the comparison cirteria defined for current object.
            /// </summary>
            bool Match(string text);
            
            /// <summary>
            /// Initialize the filter fields for future matching.
            /// </summary>
            void Update(string filter);

            /// <summary>
            /// Checks if current filter is currently not initialized and will always return 'true' for Match.
            /// </summary>
            bool IsAlwaysMatch
            { get; }
        }

        /// <summary>
        /// Class that uses star as a delimeter for multi-char comparison.
        /// </summary>
        public class StarFilter : IStringFilter
        {
            private string[] filters;
            private bool filterFirst;
            private bool filterLast;

            /// <summary>
            /// Default constructor.
            /// </summary>
            public StarFilter()
            {
            }

            /// <summary>
            /// Init constructor that initializes filter fields.
            /// </summary>
            public StarFilter(string filter)
            {
                Parse(filter);
            }

            #region Public Methods

            /// <summary>
            /// Initialize the filter fields for future matching.
            /// </summary>
            public void Parse(string filter)
            {
                if (!string.IsNullOrEmpty(filter))
                    filter = filter.Trim().ToLower();

                if (string.IsNullOrEmpty(filter))
                {
                    filters = null;
                    filterFirst = true;
                    filterLast = true;
                }
                else
                {
                    filters = filter.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);

                    filterFirst = (filter.Length > 0 ? filter[0] == '*' : false);
                    filterLast = (filter.Length > 1 ? filter[filter.Length - 1] == '*' : false);
                }
            }

            #endregion

            #region IStringFilter Members

            /// <summary>
            /// Checks if the specified text matches the comparison cirteria defined for current object.
            /// </summary>
            public bool Match(string text)
            {
                if (filters == null || filters.Length == 0)
                    return true;
                if (string.IsNullOrEmpty(text))
                    return false;

                int start = (filterFirst ? 0 : 1);
                int length = (filterLast ? filters.Length : filters.Length - 1);
                int pos = 0;
                int end = text.Length;

                text = text.ToLower();

                // check if starts/ends properly:
                if (!filterFirst)
                {
                    if (!text.StartsWith(filters[0]))
                        return false;

                    pos += filters[0].Length;
                }
                if (!filterLast)
                {
                    if (!text.EndsWith(filters[filters.Length - 1]))
                        return false;
                    end -= filters[filters.Length - 1].Length;
                }

                // validate:
                for (int i = start; i < length; i++)
                {
                    if ((pos = text.IndexOf(filters[i], pos)) < 0)
                        return false;

                    pos += filters[i].Length;
                    if (pos > end)
                        return false;
                }

                return true;
            }

            /// <summary>
            /// Checks if current filter is currently not initialized and will always return 'true' for Match.
            /// </summary>
            public bool IsAlwaysMatch
            {
                get { return filters == null || filters.Length == 0; }
            }

            /// <summary>
            /// Initialize the filter fields for future matching.
            /// </summary>
            public void Update(string filter)
            {
                Parse(filter);
            }

            #endregion
        }

        /// <summary>
        /// Delegate the detects the string recognizer object based on the formula fragment.
        /// </summary>
        public delegate void RenamerRecognizeHandler(string formulaFragment, out IStringRenamer r);

        /// <summary>
        /// Interface implemented by string renamers.
        /// </summary>
        public interface IStringRenamer
        {
            /// <summary>
            /// Renames the input text according to internal settings.
            /// </summary>
            string Rename(string inputText);
        }

        /// <summary>
        /// Class that describes the counter.
        /// </summary>
        public class CounterDetails
        {
            private uint start;
            private readonly uint step;
            private readonly int digits;
            private readonly string zeros;

            /// <summary>
            /// Init constructor of CounterDetails.
            /// </summary>
            public CounterDetails(uint start, uint step, int digits)
            {
                this.start = start;
                this.step = step;
                this.digits = digits;

                // generate zeros:
                while (digits > 0)
                {
                    digits--;
                    zeros += "0";
                }
            }

            /// <summary>
            /// Init constructor of CounterDetails.
            /// </summary>
            public CounterDetails(CounterDetails counter)
                : this(counter.Current, counter.Step, counter.Digits)
            {
            }

            #region Properties

            /// <summary>
            /// Current value of the counter.
            /// </summary>
            public uint Current
            {
                get
                {
                    return start;
                }
            }

            /// <summary>
            /// Step when moving to next value.
            /// </summary>
            public uint Step
            {
                get
                {
                    return step;
                }
            }

            /// <summary>
            /// Number of digits.
            /// </summary>
            public int Digits
            {
                get
                {
                    return digits;
                }
            }

            #endregion

            /// <summary>
            /// Moves to the next value.
            /// </summary>
            public uint MoveNext()
            {
                start += step;
                return start;
            }

            /// <summary>
            /// Returns the string representation of the counter.
            /// </summary>
            public override string ToString()
            {
                string value = start.ToString();
                if (value.Length > digits)
                    return value;
                else
                {
                    return (zeros + value).Substring(value.Length);
                }
            }
        }

        /// <summary>
        /// Class renaming the give input string.
        /// </summary>
        public class NameRenamer : IStringRenamer
        {
            private readonly int start;
            private readonly int end;

            /// <summary>
            /// Default constructor, that will copy whole name.
            /// </summary>
            public NameRenamer()
            {
                start = -1;
                end = -1;
            }

            /// <summary>
            /// Init constructor.
            /// </summary>
            public NameRenamer(int start, int end)
            {
                this.start = start;
                this.end = end;
            }

            /// <summary>
            /// Init constructor.
            /// </summary>
            public NameRenamer(string start, string end)
            {
                if (!int.TryParse(start, out this.start))
                    this.start = -1;
                if (!int.TryParse(end, out this.end))
                    this.end = -1;
            }

            #region IStringRenamer Members

            /// <summary>
            /// Renames the input text according to internal settings.
            /// </summary>
            public string Rename(string inputText)
            {
                if (start < 0 && end < 0)
                    return inputText;

                if ((end >= 0 && end < start) || string.IsNullOrEmpty(inputText))
                    return string.Empty;

                if (start - 1 < 0 || start - 1 >= inputText.Length)
                    return string.Empty;

                if (end < 0 || end - 1 >= inputText.Length)

                    return inputText.Substring(start - 1);

                return inputText.Substring(start - 1, end - start + 1);
            }

            #endregion
        }

        /// <summary>
        /// Class that returns always a given text.
        /// </summary>
        public class StaticRenamer : IStringRenamer
        {
            private readonly string staticText;

            /// <summary>
            /// Init constructor.
            /// </summary>
            public StaticRenamer(string staticText)
            {
                this.staticText = staticText;
            }

            #region IStringRenamer Members

            /// <summary>
            /// Renames the input text according to internal settings.
            /// </summary>
            public string Rename(string inputText)
            {
                return staticText;
            }

            #endregion
        }

        /// <summary>
        /// Class that generates the counter values and completelly ignores the input text.
        /// </summary>
        public class CounterRenamer : IStringRenamer
        {
            private readonly CounterDetails counter;

            /// <summary>
            /// Init constructor of CounterRenamer.
            /// </summary>
            public CounterRenamer(CounterDetails counter)
            {
                this.counter = counter;
            }

            #region IStringRenamer Members

            /// <summary>
            /// Renames the input text according to internal settings.
            /// </summary>
            public string Rename(string inputText)
            {
                string text = counter.ToString();
                counter.MoveNext();

                return text;
            }

            #endregion
        }

        /// <summary>
        /// Applies multiply rename capabilities.
        /// </summary>
        public class CombineRenamer : IStringRenamer
        {
            IList<IStringRenamer> renamers;

            /// <summary>
            /// Adds new renamer for input text.
            /// </summary>
            public void Add(IStringRenamer renamer)
            {
                if (renamers == null)
                    renamers = new List<IStringRenamer>();

                renamers.Add(renamer);
            }

            #region IStringRenamer Members

            /// <summary>
            /// Renames the input text according to internal settings.
            /// </summary>
            public string Rename(string inputText)
            {
                if (string.IsNullOrEmpty(inputText) || renamers == null)
                    return string.Empty;

                StringBuilder result = new StringBuilder();
                
                // apply all renamers:
                foreach (IStringRenamer r in renamers)
                    result.Append(r.Rename(inputText));

                return result.ToString();
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the filter that uses '*' sign for multi-char matches.
        /// </summary>
        public static IStringFilter CreateStarFilter(string filter)
        {
            return new StarFilter(filter);
        }

        /// <summary>
        /// Creates the renamer tool cumulating set of options based on given formula.
        /// </summary>
        public static IStringRenamer CreateCombineRenamer(string formula, RenamerRecognizeHandler recognizer)
        {
            CombineRenamer r = new CombineRenamer();
            IList<string> p = new List<string>();
            int i = 0;
            int j = 0;
            int start = 0;

            // split the text and generate proper renamers:
            if (string.IsNullOrEmpty(formula))
                r.Add(new StaticRenamer(string.Empty));
            else
                if (recognizer == null)
                    r.Add(new StaticRenamer(formula));
                else
                {
                    try
                    {
                        int length = formula.Length;
                        while (i < length)
                        {
                            if (formula[i] == '[')
                            {
                                if (i > j)
                                    p.Add(formula.Substring(j, i - j));
                                start = i;
                                j = i;
                            }
                            else
                                if (formula[i] == ']')
                                {
                                    if (i > start)
                                        p.Add(formula.Substring(start, i - start + 1));
                                    start = 0;
                                    j = i + 1;
                                }

                            i++;
                        }

                        // add last text:
                        if (i != j)
                            p.Add(formula.Substring(j, i - j));

                        // now the list contains the elements to interprete:
                        foreach (string formulaFragment in p)
                        {
                            IStringRenamer outR;
                            recognizer(formulaFragment, out outR);
                            if (outR != null)
                                r.Add(outR);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                }

            return r;
        }
    }
}
