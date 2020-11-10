using System;
using System.Collections.Generic;
using System.Linq;
using Analogy.Interfaces;

namespace Analogy.DataTypes
{
    public class LogStatistics
    {
        private Func<List<AnalogyLogMessage>> MessagesSource { get; set; }
        private List<AnalogyLogMessage> Messages => MessagesSource();
        private List<string> Sources => Messages.Select(m => m.Source).Distinct().ToList();
        private List<string> Modules => Messages.Select(m => m.Module).Distinct().ToList();
        private List<string> Texts = new List<string>();
        public LogStatistics(List<AnalogyLogMessage> messages)
        {
            MessagesSource = () => messages;
        }
        public LogStatistics(Func<List<AnalogyLogMessage>> messagesFunc)
        {
            MessagesSource = messagesFunc;
        }
        public ItemStatistics CalculateGlobalStatistics()
        {
            return new ItemStatistics("Global", Messages.Count, CountMessages(Messages, AnalogyLogLevel.Error),
                CountMessages(Messages, AnalogyLogLevel.Warning), CountMessages(Messages, AnalogyLogLevel.Critical),
                CountMessages(Messages, AnalogyLogLevel.Information), CountMessages(Messages, AnalogyLogLevel.Debug),
                CountMessages(Messages, AnalogyLogLevel.Verbose));
        }

        public List<Statistics> CalculateTextStatistics()
        {
            var total = Messages.Count;
            List<Statistics> items = new List<Statistics>();
            //items.Add(new Statistics("Total messages", total));
            foreach (string text in Texts)
            {
                items.Add(new Statistics(text, Messages.Count(m => m.Text.Contains(text, StringComparison.InvariantCultureIgnoreCase))));
            }

            return items;
        }

        public void AddText(string text) => Texts.Add(text);
        public void ClearTexts() => Texts.Clear();
        public IEnumerable<ItemStatistics> CalculateModulesStatistics()
        {
            foreach (var module in Modules.Where(m => m != null))
            {
                yield return CalculateSingleStatistics(module);

            }

        }

        public ItemStatistics CalculateSingleStatistics(string module)
        {
            return new ItemStatistics(module, Messages.Count(m => module.Equals(m.Module)),
                CountModuleMessages(module, AnalogyLogLevel.Error), CountModuleMessages(module, AnalogyLogLevel.Warning),
                CountModuleMessages(module, AnalogyLogLevel.Critical), CountModuleMessages(module, AnalogyLogLevel.Information),
                CountModuleMessages(module, AnalogyLogLevel.Debug), CountModuleMessages(module, AnalogyLogLevel.Verbose));

        }

        public IEnumerable<ItemStatistics> CalculateSourcesStatistics()
        {
            foreach (var source in Sources.Where(s => s != null))
            {
                yield return new ItemStatistics(source, Messages.Count(m => source.Equals(m.Source)),
                    CountSourceMessages(source, AnalogyLogLevel.Error), CountSourceMessages(source, AnalogyLogLevel.Warning),
                    CountSourceMessages(source, AnalogyLogLevel.Critical), CountSourceMessages(source, AnalogyLogLevel.Information),
                    CountSourceMessages(source, AnalogyLogLevel.Debug), CountSourceMessages(source, AnalogyLogLevel.Verbose));
            }

        }

        private int CountMessages(List<AnalogyLogMessage> messages, AnalogyLogLevel level) => messages.Count(m => m.Level == level);
        private int CountModuleMessages(string module, AnalogyLogLevel level) => Messages.Count(m => m.Level == level && module.Equals(m.Module));
        private int CountSourceMessages(string source, AnalogyLogLevel level) => Messages.Count(m => m.Level == level && source.Equals(m.Source));
    }
}
