using System.Collections.Generic;

namespace Analogy.DataTypes
{
    public class ItemStatistics
    {
        public string Name { get; private set; }
        public int TotalMessages { get; private set; }
        public int Errors { get; private set; }
        public int Warnings { get; private set; }
        public int Critical { get; private set; }
        public int Events { get; private set; }
        public int Debug { get; private set; }
        public int Verbose { get; private set; }

        public ItemStatistics(string name, int total, int errors, int warnings, int critical, int events, int debug, int verbose)
            => (Name, TotalMessages, Errors, Warnings, Critical, Events, Debug, Verbose) =
                (name, total, errors, warnings, critical, events, debug, verbose);

        public IEnumerable<Statistics> AsList()
        {
            yield return new Statistics(nameof(Critical), Critical);
            yield return new Statistics(nameof(Errors), Errors);
            yield return new Statistics(nameof(Warnings), Warnings);
            yield return new Statistics(nameof(Verbose), Verbose);
            yield return new Statistics(nameof(Debug), Debug);
            yield return new Statistics(nameof(Events), Events);

        }
    }

    public class Statistics
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public Statistics(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
