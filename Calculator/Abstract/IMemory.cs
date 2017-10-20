using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Abstract
{
    public interface IMemory
    {
        IEnumerable<double> Numbers { get; }
        void AddToMemoryEntry(double number, int index = 0);
        void SubstractFromMemoryEntry(double number, int index = 0);
        void SaveToMemory(double number);
        void RemoveFromMemory(double number);
        double MemoryRecall();
        void CleanMemory();
    }
}
