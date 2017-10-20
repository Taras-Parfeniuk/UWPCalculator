using System.Collections.Generic;
using System.Linq;

using GalaSoft.MvvmLight.Command;

using Calculator.Model;
using Calculator.Abstract;

namespace Calculator.ViewModel
{
    public class StandardCalculatorViewModel : BaseViewModel
    {
        public RelayCommand ResultCommand { get; set; }
        public RelayCommand CleenCommand { get; set; }
        public RelayCommand CleenEntryCommand { get; set; }
        public RelayCommand BackspaceCommand { get; set; }
        public RelayCommand TurnNumberCommand { get; set; }
        public RelayCommand ChangeSignCommand { get; set; }
        public RelayCommand<string> TypeNumberCommand { get; set; }
        public RelayCommand<string> TypeOperatorCommand { get; set; }
        public RelayCommand<string> TypeUnaryFuncCommand { get; set; }
        public RelayCommand<string> TypeBracketCommand { get; set; }
        public RelayCommand<string> ToDegreeByCommand { get; set; }

        public RelayCommand CleanHistoryCommand { get; set; }
        public RelayCommand SaveToMemoryCommand { get; set; }
        public RelayCommand<string> RemoveFromMemoryCommand { get; set; }
        public RelayCommand MemoryRecallCommand { get; set; }
        public RelayCommand MemoryAddCommand { get; set; }
        public RelayCommand MemorySubstractCommand { get; set; }


        public string ExpressionString
        {
            get
            {
                return $"{_expressionString.Trim()} {_inputTemp} {_operatorTemp}";
            }
        }

        public string Input
        {
            get
            {
                return _input;
            }
        }

        public List<string> History => _history.Entries.ToList();
        public List<double> Memory => _memory.Numbers.ToList();

        public StandardCalculatorViewModel()
        {
            _calculator = new RTNCalculator();
            _history = new History();
            _memory = new Memory();

            _input = string.Empty;
            _expressionString = string.Empty;
            _inputTemp = string.Empty;
            _operatorTemp = string.Empty;

            ResultCommand = new RelayCommand(GetResult);
            CleenCommand = new RelayCommand(Cleen);
            CleenEntryCommand = new RelayCommand(CleenEntry);
            BackspaceCommand = new RelayCommand(Backspace);
            TurnNumberCommand = new RelayCommand(TurnNumber);
            ChangeSignCommand = new RelayCommand(ChangeSign);
            TypeNumberCommand = new RelayCommand<string>(TypeNumber);
            TypeOperatorCommand = new RelayCommand<string>(TypeOperator);
            TypeUnaryFuncCommand = new RelayCommand<string>(TypeUnaryFunc);
            ToDegreeByCommand = new RelayCommand<string>(ToDegreeBy);

            CleanHistoryCommand = new RelayCommand(CleanHistory);
            SaveToMemoryCommand = new RelayCommand(SaveToMemory);
            RemoveFromMemoryCommand = new RelayCommand<string>(RemoveFromMemory);
            MemoryRecallCommand = new RelayCommand(MemoryRecall);
            MemoryAddCommand = new RelayCommand(MemoryAdd);
            MemorySubstractCommand = new RelayCommand(MemorySubstract);
            TypeBracketCommand = new RelayCommand<string>(TypeBracket);
        }

        public void TypeBracket(string bracket)
        {

            _expressionString += $" {_operatorTemp} ";
            _operatorTemp = string.Empty;

            if (!string.IsNullOrWhiteSpace(_inputTemp))
            {
                _inputTemp = string.Empty;
                _input = string.Empty;
            }

            if (_isResultShown)
                _input = string.Empty;

            _isResultShown = false;
            _input += $" {bracket} ";

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void TypeUnaryFunc(string funcToken)
        {
            var input = string.Empty;

            if (!string.IsNullOrWhiteSpace(_inputTemp))
                input = _inputTemp;
            else
                input = string.IsNullOrWhiteSpace(_input) ? 0.ToString() : _input;

            _inputTemp = $" {funcToken} ( {input} ) ";

            _input = _calculator.Calculate(_inputTemp).ToString();

            _isResultShown = true;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void CleanHistory()
        {
            _history.Clear();

            RaisePropertyChanged(() => History);
        }

        public void SaveToMemory()
        {
            if (double.TryParse(_input, out double buffer))
                _memory.SaveToMemory(buffer);

            RaisePropertyChanged(() => Memory);
        }

        public void RemoveFromMemory(string value)
        {
            _memory.RemoveFromMemory(double.Parse(value));

            RaisePropertyChanged(() => Memory);
        }

        public void MemoryRecall()
        {
            _inputTemp = string.Empty;
            _input = _memory.MemoryRecall().ToString();

            RaisePropertyChanged(() => Memory);
            RaisePropertyChanged(() => Input);
        }

        public void MemoryAdd()
        {
            if (double.TryParse(_input, out double buffer))
                _memory.AddToMemoryEntry(buffer);

            RaisePropertyChanged(() => Memory);
        }

        public void MemorySubstract()
        {
            if (double.TryParse(_input, out double buffer))
                _memory.SubstractFromMemoryEntry(buffer);

            RaisePropertyChanged(() => Memory);
        }

        public void TypeNumber(string number)
        {
            if (_isResultShown || _inputTemp != string.Empty)
            {
                _input = string.Empty;
                _inputTemp = string.Empty;
                _isResultShown = false;
            }

            if (number == "." && _input == string.Empty)
                _input = "0";

            if (_operatorTemp != string.Empty)
            {
                _expressionString += $" {_operatorTemp} ";
                _operatorTemp = string.Empty;
            }

            _input += number;
            RaisePropertyChanged(() => Input);
        }

        public void TypeOperator(string @operator)
        {
            if (string.IsNullOrWhiteSpace(_input) && string.IsNullOrWhiteSpace(_inputTemp) && !string.IsNullOrWhiteSpace(_operatorTemp))
            {
                _operatorTemp = $" {@operator} ";
                RaisePropertyChanged(() => ExpressionString);
                return;
            }

            if (!string.IsNullOrWhiteSpace(_inputTemp))
            {
                _expressionString += $" {_inputTemp} {_operatorTemp} ";
                _inputTemp = string.Empty;
            }
            else
            {
                _expressionString += $" {_operatorTemp} {_input} ";
            }

            _operatorTemp = $" {@operator} ";
            _input = _calculator.Calculate(_expressionString).ToString();

            _isResultShown = true;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void GetResult()
        {
            if (_inputTemp != string.Empty)
            {
                _expressionString += _inputTemp;
            }
            else
            {
                _expressionString += $" {_input} ";
            }

            _input = _calculator.Calculate(_expressionString).ToString();

            _history.AddToHistory($"{_expressionString} = {_input}");
            RaisePropertyChanged(() => History);

            _expressionString = string.Empty;
            _inputTemp = string.Empty;

            _isResultShown = true;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void Cleen()
        {
            _input = string.Empty;
            _expressionString = string.Empty;
            _inputTemp = string.Empty;
            _operatorTemp = string.Empty;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void CleenEntry()
        {
            _input = string.Empty;

            RaisePropertyChanged(() => Input);
        }

        public void ToDegreeBy(string number)
        {
            _inputTemp = $" {_input} ^ {number} ";
            _input = _calculator.Calculate(_inputTemp).ToString();

            _isResultShown = true;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void Backspace()
        {
            _input = _input.TrimEnd();
            if (!string.IsNullOrWhiteSpace(_input))
                _input = _input.Remove(_input.Length - 1);

            RaisePropertyChanged(() => Input);
        }

        public void TurnNumber()
        {
            _inputTemp = $" ( 1 / {_input} ) ";

            _input = _calculator.Calculate(_inputTemp).ToString();

            _isResultShown = true;

            RaisePropertyChanged(() => Input);
            RaisePropertyChanged(() => ExpressionString);
        }

        public void ChangeSign()
        {
            _input = (double.Parse(_input) * (-1)).ToString();

            RaisePropertyChanged(() => Input);
        }

        private bool _isResultShown = false;

        private readonly ICalculator _calculator;
        private readonly IHistorycs _history;
        private readonly IMemory _memory;

        private string _input;
        private string _expressionString;

        private string _inputTemp;
        private string _operatorTemp;
    }
}
