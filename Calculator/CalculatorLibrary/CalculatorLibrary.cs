using Newtonsoft.Json;

namespace CalculatorLibrary
{
	public class Calculator
	{
		JsonWriter writer;
		public List<Operation> operations = new List<Operation>();
		public int OpsConducted { get; set; }
		public Calculator()
		{
			StreamWriter logfile = File.CreateText("calculatorlog.json");
			logfile.AutoFlush = true;
			writer = new JsonTextWriter(logfile);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartObject();
			writer.WritePropertyName("Operations");
			writer.WriteStartArray();

		}
		public double DoOperation(double num1, double num2, Operator op)
		{
			double result = double.NaN; // Default value is "not a number" if an operation, such as division, could result in an error.
			writer.WriteStartObject();
			writer.WritePropertyName("Operand1");
			writer.WriteValue(num1);
			writer.WritePropertyName("Operand2");
			writer.WriteValue(num2);
			writer.WritePropertyName("Operation");

			// Use a switch statement to do the math.
			switch (op)
			{
				case Operator.Add:
					result = num1 + num2;
					writer.WriteValue("Add");
					break;
				case Operator.Subtract:
					result = num1 - num2;
					writer.WriteValue("Subtract");
					break;
				case Operator.Multiply:
					result = num1 * num2;
					writer.WriteValue("Multiply");
					break;
				case Operator.Divide:
					// Ask user to enter a non-zero divisor
					if (num2 != 0)
					{
						result = num1 / num2;
						writer.WriteValue("Divide");
					}
					break;
				case Operator.SquareRoot:
					if (num1 > 0)
					{
						result = Math.Sqrt(num1);
						writer.WriteValue("SquareRoot");
					}
					break;
				case Operator.Power:
					result = Math.Pow(num1, num2);
					writer.WriteValue("Exponent");
					break;
				case Operator.TenX:
					result = Math.Pow(num1, 10);
					writer.WriteValue("10 to the X");
					break;
				case Operator.Cos:
					result = Math.Cos(num1);
					writer.WriteValue("Cosine");
					break;
				//return text for an incorrect option  entry
				default:
					break;
			}
			writer.WritePropertyName("Result");
			writer.WriteValue(result);
			writer.WriteEndObject();

			this.OpsConducted++;
			return result;
		}


		public void Finish()
		{
			writer.WriteEndArray();
			writer.WriteEndObject();
			writer.Close();
		}

		public void AddToHistory(double operand1, double operand2, Operator op)
		{
			this.operations.Add(new Operation
			{
				operand1 = operand1,
				operand2 = operand2,
				op = op

			});
		}

		public void PrintHistory()
		{

			Console.WriteLine("Operation History");
			for (int i = 0; i < operations.Count; i++)
			{
				switch (operations[i].op)
				{
					case Operator.Add:
						Console.WriteLine($"{i + 1}) {operations[i].operand1} + {operations[i].operand2} = ");
						break;
					case Operator.Subtract:
						Console.WriteLine($"{i + 1}) {operations[i].operand1} - {operations[i].operand2} = ");
						break;
					case Operator.Multiply:
						Console.WriteLine($"{i + 1}) {operations[i].operand1} * {operations[i].operand2} = ");
						break;
					case Operator.Divide:
						Console.WriteLine($"{i + 1}) {operations[i].operand1} / {operations[i].operand2} = ");
						break;
					case Operator.SquareRoot:
						Console.WriteLine($"{i + 1}) Square Root of {operations[i].operand1} = ");
						break;
					case Operator.Power:
						Console.WriteLine($"{i + 1}) {operations[i].operand1} ^ {operations[i].operand2} = ");
						break;
					case Operator.TenX:
						Console.WriteLine($"{i + 1}) 10 ^ {operations[i].operand1} = ");
						break;
					case Operator.Cos:
						Console.WriteLine($"{i + 1}) Cosine ({operations[i].operand1})  = ");
						break;
				}

			}
		}
	}


	public class Operation
	{
		public double operand1 { get; set; }
		public double operand2 { get; set; }
		public Operator op { get; set; }

	}

	public enum Operator
	{
		Add,
		Subtract,
		Multiply,
		Divide,
		SquareRoot,
		Power,
		TenX,
		Cos
	}
}