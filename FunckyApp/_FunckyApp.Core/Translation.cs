using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FunckyApp.Core
{
    public class Translation
    {
        private readonly List<Fragment> _fragments;
        public Translation(): this(Enumerable.Empty<Fragment>()) { }

        public Translation(IEnumerable<Fragment> fragments)
        {
            _fragments = fragments.ToList();
            Fragments = new ReadOnlyCollection<Fragment>(_fragments);
        }

        public ReadOnlyCollection<Fragment> Fragments { get; private set; }

        public Translation AddFragment(Fragment fragment)
        {
            _fragments.Add(fragment);
            return this;
        }

        public int InflationRate { get; set; }
        public string InflatedPhrase
        {
            get { return Fragments.Aggregate(string.Empty, (prev, curr) => prev + curr.InflatedText); }
        }

        public string OriginalPhrase
        {
            get { return Fragments.Aggregate(string.Empty, (prev, curr) => prev + curr.OriginalText); }
        }

    }
}