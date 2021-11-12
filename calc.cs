using System;

public class Program
{
	public static void Main()
	{
		Calculator.Test();
		var calc = new Calculator(new InputOutput());
		calc.Calc();
	}

	class Calculator
	{
		IInputOutput _io;
		public Calculator(IInputOutput io)
		{
			_io = io;
		}

		public void Calc()
		{
			var result = GetResult();
			_io.Write("Resultaat: " + result);
		}

		int GetNumber(string prompt)
		{
			var line = _io.ReadLine(prompt);
			var converted = IsValidNumber(line);
			return converted.Item1 ? converted.Item2 : GetNumber(prompt);
		}

		Func<int, int, int> GetOperator()
		{
			var line = _io.ReadLine("geef operator (+ of -) ");
			var key = line[0];
			return key == '-' ? (left, right) => left - right : key == '+' ? (left, right) => left + right : GetOperator();
		}

		int GetResult()
		{
			var left = GetNumber("Geef 1ste getal ");
			var op = GetOperator();
			var right = GetNumber("Geef 2de getal ");
			return op(left, right);
		}

		Tuple<bool, int> IsValidNumber(string line)
		{
			int parsedResult;
			return new Tuple<bool, int>(Int32.TryParse(line, out parsedResult), parsedResult);
		}

		public static void Test()
		{
			Tester.Test();
		}

		static class Tester
		{
			public static void Test()
			{
				var testIO = new TestInputOutput();
				var testCalc = new Calculator(testIO);
				Test(testIO, "1", "Call to GetNumber(1) should be 1", () => testCalc.GetNumber("") == 1);
				Test(testIO, "+", "Call to GetOperator(+, 1, 2) should be 3", () => testCalc.GetOperator()(1, 2) == 3);
				Test(testIO, "-", "Call to GetOperator(-, 1, 2) should be -1", () => testCalc.GetOperator()(1, 2) == -1);
				Test(testIO, "", "Call to IsValidNumber(1) should be (true, 1)", () => {
					var tuple = testCalc.IsValidNumber("1");
					return tuple.Item1 && tuple.Item2 == 1;
				});
			}
			static void Test(TestInputOutput io, string input, string errorMessage, Func<bool> assert) {
				io.SetLine(input);
				if (!assert()) throw new Exception(errorMessage);
			}
		}
	}

	public interface IInputOutput
	{
		string ReadLine(string prompt);
		void Write(string msg);
	}

	class InputOutput : IInputOutput
	{
		public string ReadLine(string prompt)
		{
			Console.Write(prompt);
			return Console.ReadLine();
		}

		public void Write(string msg) {
			Console.Write(msg);
		}
	}

	class TestInputOutput : IInputOutput
	{
		string _line;
		public void SetLine(string line)
		{
			_line = line;
		}

		public string ReadLine(string prompt)
		{
			return _line;
		}

		public void Write(string msg)
		{
		}
	}
}
