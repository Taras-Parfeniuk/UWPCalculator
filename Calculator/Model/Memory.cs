using System.Collections.Generic;

using Calculator.Abstract;

namespace Calculator.Model
{
    public class Memory : IMemory
    {
        public IEnumerable<double> Numbers
        {
            get
            {
                return _numbers;
            }
        }

        public Memory()
        {
            _numbers = new List<double>();
        }

        public void AddToMemoryEntry(double number, int index = 0)
        {
            if (_numbers.Count == 0)
                _numbers.Add(number);
            else
            {
                _numbers[index] += number;
            }
        }

        public void SubstractFromMemoryEntry(double number, int index = 0)
        {
            AddToMemoryEntry(-number, index);
        }

        public void SaveToMemory(double number)
        {
            _numbers.Insert(0, number);
        }

        public void RemoveFromMemory(double number)
        {
            _numbers.Remove(number);
        }

        public double MemoryRecall()
        {
            if (_numbers.Count > 0)
            {
                var value = _numbers[0];
                _numbers.Remove(value);
                return value;
            }
            else return 0;
        }

        public void CleanMemory()
        {
            _numbers.Clear();
        }

        private List<double> _numbers;
    }
}
