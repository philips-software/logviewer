using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Analogy.DataTypes;
using Analogy.Interfaces;

namespace Analogy
{
    public class FilterCriteriaObject
    {
        private static FilterCriteriaObject alertFilterCriteria = new FilterCriteriaObject();
        private readonly AnalogyLogLevel[] _allLevels = Enum.GetValues(typeof(AnalogyLogLevel)) as AnalogyLogLevel[];
        public string[] Sources;
        public string[] ExcludedSources;
        public string[] Modules;
        public string[] ExcludedModules;

        public string TextInclude { get; set; }
        public DateTime NewerThan { get; set; }
        public DateTime OlderThan { get; set; }
        public string TextExclude { get; set; }
        private AnalogyLogLevel[] _arrLevels;
        public AnalogyLogLevel[] Levels
        {
            get => _arrLevels ?? _allLevels;
            set => _arrLevels = value;
        }

        public List<FilterCriteriaUIOption> IncludeFilterCriteriaUIOptions { get; set; }
        public List<FilterCriteriaUIOption> ExcludeFilterCriteriaUIOptions { get; set; }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            foreach (var c in value)
            {
                switch (c)
                {


                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    case '@':
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetModules(string modules)
        {
            if (string.IsNullOrEmpty(modules))
            {
                Modules = null;
                ExcludedModules = null;
            }
            else
            {
                var items = modules.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var includeItems = items.Where(i => !i.StartsWith("-"));
                var excludeItems = items.Where(i => i.StartsWith("-") && i.Length > 1)
                    .Select(i => i.Substring(1, i.Length - 1));

                Modules = includeItems.Select(val => val.Trim()).ToArray();
                ExcludedModules = excludeItems.Select(val => val.Trim()).ToArray();
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetSources(string sources)
        {
            if (string.IsNullOrEmpty(sources))
            {
                Sources = null;
                ExcludedModules = null;
            }
            else
            {
                var items = sources.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var includeItems = items.Where(i => !i.StartsWith("-"));
                var excludeItems = items.Where(i => i.StartsWith("-") && i.Length > 1)
                    .Select(i => i.Substring(1, i.Length - 1));

                Sources = includeItems.Select(val => val.Trim()).ToArray();
                ExcludedSources = excludeItems.Select(val => val.Trim()).ToArray();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetSqlExpression()
        {

            StringBuilder sqlString = new StringBuilder();
            List<string> includeTexts = new List<string> { EscapeLikeValue(TextInclude.Trim()) };

            bool orOperationInInclude = false;
            bool orOperationInexclude = false;
            var text = EscapeLikeValue(TextInclude.Trim());
            if (text.Contains('|'))
            {
                orOperationInInclude = true;
                var split = text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                includeTexts = split.Select(itm => itm.Trim()).ToList();
            }

            if (text.Contains("&") || text.Contains('+'))
            {
                var split = text.Split(new[] { '&', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                includeTexts = split.Select(itm => itm.Trim()).ToList();
            }
            List<string> excludedTexts = new List<string>(0);
            if (!string.IsNullOrEmpty(TextExclude))
            {
                excludedTexts.Add(EscapeLikeValue(TextExclude.Trim()));
            }

            text = EscapeLikeValue(TextExclude.Trim());
            if (text.Contains("|"))
            {
                orOperationInexclude = true;
                var split = text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                excludedTexts = split.Select(itm => itm.Trim()).Where(w => !string.IsNullOrEmpty(w)).ToList();
            }
            if (text.Contains("&") || text.Contains("+"))
            {
                var split = text.Split(new[] { '&', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                excludedTexts = split.Select(itm => itm.Trim()).ToList();
            }
            if (UserSettingsManager.UserSettings.SearchAlsoInSourceAndModule)
            {
                sqlString.Append("((");
            }
            else
            {
                sqlString.Append("(");
            }

            if (orOperationInInclude)
            {
                sqlString.Append(string.Join(" Or ", includeTexts.Select(t => $" Text like '%{t}%'")));
            }
            else
            {
                sqlString.Append(string.Join(" and ", includeTexts.Select(t => $" Text like '%{t}%'")));
            }

            sqlString.Append(")");

            if (UserSettingsManager.UserSettings.SearchAlsoInSourceAndModule)
            {
                //also in source
                sqlString.Append(" or (");
                if (orOperationInInclude)
                {
                    sqlString.Append(string.Join(" Or ", includeTexts.Select(t => $" Source like '%{t}%'")));
                }
                else
                {
                    sqlString.Append(string.Join(" And ", includeTexts.Select(t => $" Source like '%{t}%'")));
                }

                sqlString.Append(")");
                //also in module
                sqlString.Append(" or (");
                if (orOperationInInclude)
                {
                    sqlString.Append(string.Join(" Or ", includeTexts.Select(t => $" Module like '%{t}%'")));
                }
                else
                {
                    sqlString.Append(string.Join(" And ", includeTexts.Select(t => $" Module like '%{t}%'")));
                }

                sqlString.Append("))");
            }

            if (excludedTexts.Any())
            {
                sqlString.Append(" and (");
                if (orOperationInexclude)
                {
                    sqlString.Append(string.Join(" and ", excludedTexts.Select(t => $" NOT Text like '%{t}%'")));
                }
                else
                {
                    sqlString.Append(string.Join(" Or ", excludedTexts.Select(t => $" NOT Text like '%{t}%'")));
                }

                sqlString.Append(")");
            }

            if (Sources != null && Sources.Any())
            {
                sqlString.Append(" and (");
                sqlString.Append(string.Join(" Or ", Sources.Select(l => $" Source like '%{EscapeLikeValue(l)}%'")));
                sqlString.Append(")");
            }
            if (ExcludedSources != null && ExcludedSources.Any())
            {
                sqlString.Append(" and (");
                sqlString.Append(string.Join(" and ", ExcludedSources.Select(l => $" NOT Source like '%{EscapeLikeValue(l)}%'")));
                sqlString.Append(")");
            }
            if (ExcludedModules != null && ExcludedModules.Any())
            {
                sqlString.Append(" and (");
                sqlString.Append(string.Join(" and", ExcludedModules.Select(l => $" NOT Module like '%{EscapeLikeValue(l)}%'")));
                sqlString.Append(")");
            }

            if (Modules != null && Modules.Any())
            {
                sqlString.Append(" and (");
                sqlString.Append(string.Join(" Or ", Modules.Select(l => $" Module like '%{EscapeLikeValue(l)}%'")));
                sqlString.Append(")");
            }

            if (Levels != null && Levels.Any())
            {
                string sTemp = string.Join(",", Levels.Select(l => $"'{l}'"));
                sqlString.Append(" and Level in (" + sTemp + ")");
            }

            string dateFilter = $" AND (Date >= '{NewerThan}' and Date <= '{OlderThan}')";

            sqlString.Append(dateFilter);

            if (includeTexts.Any())
            {
                var includeColumns = IncludeFilterCriteriaUIOptions.Where(f => f.CheckMember);
                foreach (FilterCriteriaUIOption include in includeColumns)
                {
                    sqlString.Append(" or (");
                    string op = (orOperationInInclude) ? "or" : "and";
                    sqlString.Append(string.Join($" {op} ",
                        includeTexts.Select(l =>
                            $" [{EscapeLikeValue(include.ValueMember)}] like '%{EscapeLikeValue(l)}%'")));
                    sqlString.Append(")");
                }
            }

            if (excludedTexts.Any())
            {
                var excludeColumns = ExcludeFilterCriteriaUIOptions.Where(f => f.CheckMember);
                foreach (FilterCriteriaUIOption exclude in excludeColumns)
                {
                    sqlString.Append(" and (");
                    string op = (orOperationInexclude) ? "and" : "or";
                    sqlString.Append(string.Join($" {op} ",
                        excludedTexts.Select(l =>
                            $" NOT [{EscapeLikeValue(exclude.ValueMember)}] like '%{EscapeLikeValue(l)}%'")));
                    sqlString.Append(")");
                }
            }

            return sqlString.ToString();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Match(string rowLine, string criteria, PreDefinedQueryType type)
        {
            if (string.IsNullOrEmpty(criteria))
            {
                return false;
            }

            List<string> includeTexts = new List<string>(1) { criteria.Trim() };

            bool orOperationInInclude = false;
            if (criteria.Contains('|'))
            {
                orOperationInInclude = true;
                var split = criteria.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                includeTexts = split.Select(itm => itm.Trim()).ToList();
            }

            if (criteria.Contains("&") || criteria.Contains('+'))
            {
                var split = criteria.Split(new[] { '&', '+' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                includeTexts = split.Select(itm => itm.Trim()).ToList();
            }

            if (orOperationInInclude)
            {
                switch (type)
                {
                    case PreDefinedQueryType.Contains:
                        return includeTexts.Any(t =>
                            rowLine.Contains(t, StringComparison.InvariantCultureIgnoreCase) &&
                            !string.IsNullOrEmpty(t));
                    case PreDefinedQueryType.Equals:
                        return includeTexts.Any(t =>
                            rowLine.Equals(t, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(t));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            else
            {
                switch (type)
                {
                    case PreDefinedQueryType.Contains:
                        return includeTexts.All(t => rowLine.Contains(t, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(t));
                    case PreDefinedQueryType.Equals:
                        return includeTexts.All(t => rowLine.Equals(t, StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(t));
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        public static bool MatchAlert(AnalogyLogMessage analogyLogMessage, PreDefineAlert preDefineAlert)
        {
            alertFilterCriteria.SetModules(preDefineAlert.Modules);
            alertFilterCriteria.SetSources(preDefineAlert.Sources);
            alertFilterCriteria.TextInclude = preDefineAlert.IncludeText ?? string.Empty;
            alertFilterCriteria.TextExclude = preDefineAlert.ExcludeText ?? string.Empty;
            return alertFilterCriteria.MatchAlert(analogyLogMessage);

        }

        private bool MatchAlert(AnalogyLogMessage message)
        {
            bool match = true;
            if (!string.IsNullOrEmpty(TextInclude))
            {
                match = message.Text.Contains(TextInclude, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!match)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(TextExclude))
            {
                match = !message.Text.Contains(TextExclude, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!match)
            {
                return false;
            }

            if (Modules != null && Modules.Any())
            {
                match = Modules.Any(m => message.Module.Contains(m, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!match)
            {
                return false;
            }

            if (ExcludedModules != null && ExcludedModules.Any())
            {
                match = ExcludedModules.All(m => !message.Module.Contains(m, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!match)
            {
                return false;
            }

            if (Sources != null && Sources.Any())
            {
                match = Sources.Any(s => message.Source.Contains(s, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!match)
            {
                return false;
            }

            if (ExcludedSources != null && ExcludedSources.Any())
            {
                match = ExcludedSources.All(s => !message.Module.Contains(s, StringComparison.InvariantCultureIgnoreCase));
            }

            return match;
        }

    }

}
