using Microsoft.VisualStudio.TestTools.UnitTesting;

using Calculator.Model;
using Calculator.Abstract;

namespace Calculator.Test
{
    [TestClass]
    public class StandardCalcTest
    {
        [TestMethod]
        public void When_add_2_to_2_and_multiply_to_2_return_6()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(6, calc.Calculate("2 + 2 * 2"));
        }

        [TestMethod]
        public void When_input_single_number_2_return_2()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(2, calc.Calculate("2"));
        }

        [TestMethod]
        public void When_add_2_to_2_in_brackets_return_4()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(4, calc.Calculate("( 2 + 2 )"));
        }

        [TestMethod]
        public void When_sum_in_brackets_multiply_by_2_return_8()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(8, calc.Calculate("( 2 + 2 ) * 2"));
        }

        [TestMethod]
        public void When_sqrt_of_single_number_4_return_2()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(2, calc.Calculate("√ ( 4 )"));
        }

        [TestMethod]
        public void When_sqrt_of_sum_of_2_and_2_return_2()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(2, calc.Calculate("√ ( 2 + 2 )"));
        }

        [TestMethod]
        public void When_few_included_sqrt_add_3_return_2()
        {
            ICalculator calc = new RTNCalculator();
            Assert.AreEqual(5, calc.Calculate("√ ( √ ( 16 ) ) + 3"));
        }
    }
}
