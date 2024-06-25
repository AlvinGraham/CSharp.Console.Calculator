using CalculatorLibrary;
using System.Text.RegularExpressions;



namespace CalculatorProgram;

class Program
{
	static void Main(string[] args)
	{
		bool endApp = false;
		string? userInput = "";
		//Display title as the C# Console calculator app.
		ClearScreen();

		Calculator calculator = new Calculator();
		while (!endApp)
		{

			bool validInput = false;
			Console.WriteLine("Please select an option from the following list:");
			Console.WriteLine("1 - Perform a mathematical operation");
			Console.WriteLine($"2 - Manage Operation History ({calculator.OpsConducted} operations coducted)");
			Console.WriteLine("3 - Exit Application");

			while (!validInput)
			{
				Console.Write("\nYour Selection: ");
				userInput = Console.ReadLine();

				switch (userInput)
				{
					case "1":
						validInput = true;
						ConductOperation(calculator);
						break;
					case "2":
						validInput = true;
						ManageOperations(calculator);
						break;
					case "3":
						Console.WriteLine("Goodbye!");
						Environment.Exit(0);
						break;
					default:
						Console.WriteLine("Invalid Input! Please select an option listed above");
						break;
				}
			}



			// Wait for the user to respond before closing

			Console.Write("Press Enter to continue.");
			Console.ReadLine();
			ClearScreen();
		}

		// Add call to close the JSON writer before return
		calculator.Finish();
		return;
	}

	internal static void ManageOperations(Calculator calculator)
	{
		string? userInput = "";
		bool validInput = false;
		int selectedOp = 0;
		double result = 0;
		ClearScreen();

		Console.WriteLine("Operations History:");
		calculator.PrintHistory();

		Console.WriteLine("\nSelect a history option:");
		Console.WriteLine("1 - Repeat an operation");
		Console.WriteLine("2 - Clear History");
		Console.WriteLine("3 - Return to Main Menu");

		while (!validInput)
		{

			Console.Write("\nYour Selection: ");
			userInput = Console.ReadLine();

			switch (userInput)
			{
				case "1":
					validInput = true;
					Console.Write("Which operation would you like to repeat: ");
					userInput = Console.ReadLine();
					while (!Int32.TryParse(userInput, out selectedOp) || Int32.Parse(userInput) < 0 || Int32.Parse(userInput) > calculator.operations.Count)
					{
						Console.WriteLine("Invalid Selection! Please select a valid operation.");
						Console.Write("Which operation would you like to repeat: ");
						userInput = Console.ReadLine();
					}
					result = calculator.DoOperation(calculator.operations[selectedOp - 1].operand1, calculator.operations[selectedOp - 1].operand2, calculator.operations[selectedOp - 1].op);
					Console.WriteLine("Your result: {0:0.##}\n", result);
					calculator.AddToHistory(calculator.operations[selectedOp - 1].operand1, calculator.operations[selectedOp - 1].operand2, calculator.operations[selectedOp - 1].op);
					break;
				case "2":
					validInput = true;
					calculator.operations.Clear();
					calculator.OpsConducted = 0;
					break;
				case "3":
					return;

				default:
					Console.WriteLine("Invalid Input! Please select an option listed above");
					break;

			}


		}
	}

	internal static void ConductOperation(Calculator calculator)
	{
		// Declare variables and set to empty
		// Use Nullable types (with ?) to match type od System.Console.Readline
		string? numInput1 = "";
		string? numInput2 = "";
		string? opInput = "";
		Operator op = Operator.Add;
		double result = 0;
		bool validInput = false;

		while (!validInput)
		{
			ClearScreen();
			Console.WriteLine("What operation would you like to conduct?");
			Console.WriteLine("\ta - Add");
			Console.WriteLine("\ts - Subtract");
			Console.WriteLine("\tm - Multiply");
			Console.WriteLine("\td - Divide");
			Console.WriteLine("\tq - Square Root");
			Console.WriteLine("\tp - Power x^y");
			Console.WriteLine("\tt - Power of 10");
			Console.WriteLine("\tc - Cosine");

			Console.Write("\nYour option? ");

			opInput = Console.ReadLine();

			// validate input is not null, and matches the pattern
			if (opInput == null || !Regex.IsMatch(opInput, "[a|s|m|d|q|p|t|c]"))
			{
				Console.WriteLine("Error: Unrecognized input.");
				Pause();
			}
			else
			{
				try
				{
					switch (opInput)
					{
						case "a":
							op = Operator.Add;
							break;
						case "s":
							op = Operator.Subtract;
							break;
						case "m":
							op = Operator.Multiply;
							break;
						case "d":
							op = Operator.Divide;
							break;
						case "q":
							op = Operator.SquareRoot;
							break;
						case "p":
							op = Operator.Power;
							break;
						case "t":
							op = Operator.TenX;
							break;
						case "c":
							op = Operator.Cos;
							break;
					}
					// Ask the user to type the first number
					Console.Write("Type a number, and then press Enter: ");
					numInput1 = Console.ReadLine();

					double cleanNum1 = 0;
					while (!double.TryParse(numInput1, out cleanNum1))
					{
						Console.Write("This is not a valid input. Please enter an integer value: ");
						numInput1 = Console.ReadLine();
					}

					// Ask the user to type the second number.
					if (op != Operator.SquareRoot && op != Operator.TenX && op != Operator.Cos)
					{
						Console.Write("Type another number, and then press Enter: ");
						numInput2 = Console.ReadLine();
					}
					else
					{
						numInput2 = "0";
					}
					double cleanNum2 = 0;
					while (!double.TryParse(numInput2, out cleanNum2))
					{
						Console.Write("This is not a valid input. Please enter an integer value: ");
						numInput2 = Console.ReadLine();
					}
					result = calculator.DoOperation(cleanNum1, cleanNum2, op);
					if (double.IsNaN(result))
					{
						Console.WriteLine("This operation will result in a mathematical error.\n");
						Pause();
					}
					else
					{
						Console.WriteLine("Your result: {0:0.##}\n", result);
						calculator.AddToHistory(cleanNum1, cleanNum2, op);
						validInput = true;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Oh No! An exception occurred trying to do the math.\n - Details: " + e.Message);
					Pause();
				}
			}
			Console.WriteLine("----------------------------\n");
		}
	}

	public static void ClearScreen()
	{
		Console.Clear();
		Console.WriteLine("Console Calculator in C#\r");
		Console.WriteLine("------------------------\n");
	}

	public static void Pause()
	{
		Console.WriteLine("Press Enter to continue");
		Console.ReadLine();
	}
}