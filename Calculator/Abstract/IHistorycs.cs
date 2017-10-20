using System.Collections.Generic;

namespace Calculator.Abstract
{
    public interface IHistorycs
    {
        IEnumerable<string> Entries { get; }

        void AddToHistory(string entry);
        void Clear();
    }
}
