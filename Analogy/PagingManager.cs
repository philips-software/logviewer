﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Philips.Analogy.Interfaces.DataTypes;

namespace Philips.Analogy
{
    public class PagingManager
    {
        private UCLogs owner;
        public event EventHandler<AnalogyClearedHistoryEventArgs> OnHistoryCleared;
        public event EventHandler<AnalogyPagingChanged> OnPageChanged;
        private UserSettingsManager Settings { get; }
        private List<DataTable> pages;
        private List<AnalogyLogMessage> allMessages;
        private int pageSize;
        private int currentPageStartRowIndex;
        private int currentPageLength;
        private int currentPageNumber;
        private object lockObject;
        private DataTable currentTable;
        private static int _totalMissedMessages;

        public int TotalPages
        {
            get
            {
                lock (lockObject)
                    return pages.Count;
            }
        }

        public static int TotalMissedMessages => _totalMissedMessages;

        public PagingManager(UCLogs owner)
        {
            this.owner = owner;
            lockObject = new object();
            Settings = UserSettingsManager.UserSettings;
            pageSize = Settings.PagingEnabled ? Settings.PagingSize : int.MaxValue;
            pages = new List<DataTable>();
            currentTable = Utils.DataTableConstructor();
            pages.Add(currentTable);
            currentPageNumber = 1;
            allMessages = new List<AnalogyLogMessage>();
        }


        public IEnumerable<List<T>> SplitList<T>(List<T> locations)
        {
            for (int i = 0; i < locations.Count; i += pageSize)
            {
                yield return locations.GetRange(i, Math.Min(pageSize, locations.Count - i));
            }
        }


        public void ClearLogs()
        {
            AnalogyPageInformation analogyPage;
            lock (lockObject)
            {
                var oldMessages = allMessages.ToList();
                pages = new List<DataTable>();
                currentPageLength = 0;
                currentPageStartRowIndex = 0;
                currentPageNumber = 1;
                var first = Utils.DataTableConstructor();
                currentTable = first;
                pages.Add(first);
                OnHistoryCleared?.Invoke(this, new AnalogyClearedHistoryEventArgs(oldMessages));
                analogyPage = new AnalogyPageInformation(currentTable, 1, currentPageStartRowIndex);
            }

            OnPageChanged?.Invoke(this, new AnalogyPagingChanged(analogyPage));
        }

        public void SetAuditColumnVisibility(bool value)
        {
            owner.SetAuditColumnVisibility(value);
        }

        public void SetCategoryColumnVisibility(bool value)
        {
            owner.SetCategoryColumnVisibility(value);
        }

        public DataRow AppendMessage(AnalogyLogMessage message, string dataSource)
        {
            var table = pages.Last();
            allMessages.Add(message);
            if (table.Rows.Count + 1 > pageSize)
            {
                var isTableInView = currentTable == table;
                if (!isTableInView)
                    owner.AcceptChanges(table, $"{nameof(PagingManager)}-{nameof(AppendMessage)}");
                table = Utils.DataTableConstructor();
                pages.Add(table);
                var pageStartRowIndex = (pages.Count - 1) * pageSize;
                OnPageChanged?.Invoke(this, new AnalogyPagingChanged(new AnalogyPageInformation(table, pages.Count, pageStartRowIndex)));
            }
            lock (table.Rows.SyncRoot)
            {
                DataRow dtr = table.NewRow();
                dtr["Date"] = message.Date;
                dtr["Text"] = message.Text ?? "";
                dtr["Source"] = message.Source ?? "";
                dtr["Level"] = message.Level.ToString();
                dtr["Class"] = message.Class.ToString();
                dtr["Category"] = message.Category ?? "";
                dtr["User"] = message.User ?? "";
                dtr["Module"] = message.Module ?? "";
                dtr["Audit"] = message.AuditLogType;
                dtr["Object"] = message;
                dtr["ProcessID"] = message.ProcessID;
                dtr["ThreadID"] = message.Thread;
                dtr["DataProvider"] = dataSource ?? string.Empty;
                table.Rows.Add(dtr);
                return dtr;
            }
        }

        public IEnumerable<(DataRow, AnalogyLogMessage)> AppendMessages(List<AnalogyLogMessage> messages, string dataSource)
        {
            var table = pages.Last();
            var countInsideTable = table.Rows.Count;
            foreach (var message in messages)
            {
                allMessages.Add(message);
                if (countInsideTable + 1 > pageSize)
                {
                    var isTableInView = currentTable == table;
                    if (!isTableInView)
                        owner.AcceptChanges(table, $"{nameof(PagingManager)}-{nameof(AppendMessages)}");
                    table = Utils.DataTableConstructor();
                    pages.Add(table);
                    countInsideTable = 0;
                    var pageStartRowIndex = (pages.Count - 1) * pageSize;
                    OnPageChanged?.Invoke(this, new AnalogyPagingChanged(new AnalogyPageInformation(table, pages.Count, pageStartRowIndex)));
                }
                lock (table.Rows.SyncRoot)
                {
                    countInsideTable++;
                    DataRow dtr = table.NewRow();
                    dtr["Date"] = message.Date;
                    dtr["Text"] = message.Text ?? "";
                    dtr["Source"] = message.Source ?? "";
                    dtr["Level"] = message.Level.ToString();
                    dtr["Class"] = message.Class.ToString();
                    dtr["Category"] = message.Category ?? "";
                    dtr["User"] = message.User ?? "";
                    dtr["Module"] = message.Module ?? "";
                    dtr["Audit"] = message.AuditLogType;
                    dtr["Object"] = message;
                    dtr["ProcessID"] = message.ProcessID;
                    dtr["ThreadID"] = message.Thread;
                    dtr["DataProvider"] = dataSource ?? string.Empty;
                    table.Rows.Add(dtr);
                    yield return (dtr, message);
                }
            }
            //if (currentTable != table)
            //    owner.AcceptChanges(table, $"{nameof(PagingManager)}-{nameof(AppendMessage)}(2)");
        }


        public AnalogyPageInformation NextPage()
        {
            lock (lockObject)
            {
                if (pages.Last() != currentTable)
                {
                    currentPageNumber++;
                    currentTable = pages[currentPageNumber - 1];
                    currentPageStartRowIndex = pages.IndexOf(currentTable) * pageSize;
                }
                return new AnalogyPageInformation(currentTable, currentPageNumber, currentPageStartRowIndex);
            }
        }

        public AnalogyPageInformation PrevPage()
        {
            lock (lockObject)
            {
                if (pages.First() != currentTable)
                {
                    currentPageNumber--;
                    currentTable = pages[currentPageNumber - 1];
                    currentPageStartRowIndex = pages.IndexOf(currentTable) * pageSize;
                }

                return new AnalogyPageInformation(currentTable, currentPageNumber, currentPageStartRowIndex);
            }
        }

        public DataTable FirstPage()
        {
            lock (lockObject)
            {
                currentTable = pages.First();
                currentPageNumber = 1;
                return currentTable;
            }
        }
        public DataTable LastPage()
        {
            lock (lockObject)
            {
                currentTable = pages.Last();
                currentPageNumber = pages.Count;
                return currentTable;
            }
        }

        public List<AnalogyLogMessage> GetAllMessages()
        {
            lock (lockObject)
                return allMessages.ToList();
        }

        public DataTable CurrentPage()
        {
            lock (lockObject)
                return currentTable;
        }

        public bool IsCurrentPageInView(DataTable currentView)
        {
            lock (lockObject)
                return currentTable == currentView;
        }

        public void IncrementTotalMissedMessages()
        {
            Interlocked.Increment(ref _totalMissedMessages);
        }
    }

}
