using System;
using System.Collections.Generic;

using Calculator.Abstract;

namespace Calculator.Model
{
    public class RTNCalculator : ICalculator
    {
        public RTNCalculator()
        {
            InitUnaryFunctions();
        }

        public double Calculate(string input)
        {
            string output = GetExpression(input);
            double result = Counting(output);
            return result;
        }

        private string GetExpression(string input)
        {
            var expressionMembers = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string output = string.Empty;

            Stack<string> operations = new Stack<string>();

            for (int i = 0; i < expressionMembers.Length; i++)
            {
                if (IsDelimeter(expressionMembers[i]))
                    continue;

                if (double.TryParse(expressionMembers[i], out double buffer))
                {
                    output += buffer + " ";
                    continue;
                }

                if (expressionMembers[i] == "(")
                {
                    operations.Push(expressionMembers[i]);
                    continue;
                }
                else
                if (expressionMembers[i] == ")")
                {
                    string member = operations.Pop();

                    while (member != "(")
                    {
                        output += member.ToString() + " ";
                        member = operations.Pop();
                    }
                    continue;
                }

                if (IsOperation(expressionMembers[i]))
                {

                    if (operations.Count > 0 && !string.IsNullOrWhiteSpace(output))
                    {
                        while (operations.Count > 0)
                            if (operations.Peek() != "(" && GetPriority(expressionMembers[i]) <= GetPriority(operations.Peek()))
                                output += operations.Pop() + " ";
                            else
                                break;
                        operations.Push(expressionMembers[i]);
                    }
                    else
                        operations.Push(expressionMembers[i]);
                }
            }

            while (operations.Count > 0)
                output += operations.Pop() + " ";

            return output;
        }

        private double Counting(string input)
        {
            var expressionMembers = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Stack<double> temp = new Stack<double>();

            for (int i = 0; i < expressionMembers.Length; i++)
            {
                if (double.TryParse(expressionMembers[i], out double buffer))
                {
                    temp.Push(buffer);
                }
                else if (IsOperation(expressionMembers[i])) // need to catch empty stack exception
                {
                    var operation = _operations[expressionMembers[i]];

                    var args = new double[operation.Arity];

                    for (var j = 0; j < args.Length; j++)
                    {
                        args[j] = temp.Pop();
                    }

                    temp.Push(operation.Execute(args));
                }
            }
            return temp.Peek();
        }

        private bool IsDelimeter(string block)
        {
            return block == "=";
        }

        private bool IsOperation(string block)
        {
            return _operations.ContainsKey(block);
        }

        private int GetPriority(string @operator)
        {
            return _operations[@operator].Priority;
        }

        protected virtual void InitUnaryFunctions()
        {
            _operations = new Dictionary<string, Operation>();

            _operations.Add("+", new Operation(2, 2, args => args[1] + args[0]));
            _operations.Add("-", new Operation(2, 2, args => args[1] - args[0]));
            _operations.Add("*", new Operation(2, 3, args => args[1] * args[0]));
            _operations.Add("/", new Operation(2, 3, args => args[1] / args[0]));
            _operations.Add("%", new Operation(2, 3, args => args[1] % args[0]));
            _operations.Add("^", new Operation(2, 4, args => Math.Pow(args[1], args[0])));

            _operations.Add("ln", new Operation(1, 4, args => Math.Log(args[0])));
            _operations.Add("log", new Operation(1, 4, args => Math.Log10(args[0])));
            _operations.Add("exp", new Operation(1, 4, args => Math.Exp(args[0])));
            _operations.Add("√", new Operation(1, 4, args => Math.Sqrt(args[0])));
            _operations.Add("sin", new Operation(1, 4, args => Math.Sin(args[0])));
            _operations.Add("cos", new Operation(1, 4, args => Math.Cos(args[0])));
            _operations.Add("tg", new Operation(1, 4, args => Math.Tan(args[0])));
            _operations.Add("fact", new Operation(1, 4, args =>
            {
                double result = 1;
                while (args[0] != 1)
                {
                    result = result * args[0];
                    args[0] = args[0] - 1;
                }
                return result;
            }));
        }

        protected Dictionary<string, Operation> _operations;
    }
}
