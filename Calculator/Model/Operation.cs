using System;

namespace Calculator.Model
{
    public class Operation
    {
        public Operation(int arity, int priority, Func<double[], double> function)
        {
            Arity = arity;
            Priority = priority;
            _function = function;
        }

        public int Priority { get; private set; }
        public int Arity { get; private set; }

        public double Execute(double[] parameters)
        {
            if (parameters.Length != Arity)
                throw new ArgumentException();

            return _function(parameters);
        }

        private Func<double[], double> _function;
    }
}
