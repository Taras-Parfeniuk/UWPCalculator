using System.Collections.Generic;

using Calculator.Abstract;

namespace Calculator.Model
{
    public class History : IHistorycs
    {
        public IEnumerable<string> Entries => _entries;

        public History()
        {
           _entries = new List<string>();
        }

        public void AddToHistory(string entry)
        {
            _entries.Add(entry);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        private List<string> _entries;
    }
}
