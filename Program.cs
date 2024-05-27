List<string> decisions = new List<string>();
Dictionary<string, int> pointDecisions = new Dictionary<string, int>();

App();
void App()
{
    Console.WriteLine("Welcome. Select a decision mode:");
    Console.WriteLine();
    Console.WriteLine("1: Cup. Decisions fight a 1v1 battle until the ultimate decision is chosen");
    Console.WriteLine("2: Point battle. Highest point total wins");
    Console.WriteLine("3: Exit application");
    var input = Console.ReadLine();
    if (input == "1")
    {
        Console.Clear();
        Cup();
    }
    else if (input == "2")
    {
        Console.Clear();
        PointBattle();
    }
    else if (input == "3")
    {
        Console.WriteLine("Shutting down..");
    }
    else
    {
        Console.WriteLine("Invalid input. Enter a number 1-3");
        App();
    }
}

void Cup()
{
    Console.WriteLine("1: Add decision");
    Console.WriteLine("2: Cup start");
    Console.WriteLine("3: Reset");
    Console.WriteLine("4: Mode selection");
    Console.WriteLine("5: Show decisions");
    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            AddDecision(false);
            Cup();
            break;

        case "2":
            DecisionBattle();
            Cup();
            break;

        case "3":
            Reset();
            Cup();
            break;

        case "4":
            Reset();
            Console.Clear();
            App();
            break;

        case "5":
            DisplayDecisions();
            Cup();
            break;

        default:
            Console.WriteLine("Enter a number 1-5");
            Cup();
            break;
    }
}

void PointBattle()
{
    Console.WriteLine("1: Add decision");
    Console.WriteLine("2: See result");
    Console.WriteLine("3: Reset");
    Console.WriteLine("4: Mode selection");
    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            DecidePoints();
            PointBattle();
            break;

        case "2":
            DisplayPoints();
            PointBattle();
            break;

        case "3":
            Reset();
            PointBattle();
            break;

        case "4":
            Reset();
            Console.Clear();
            App();
            break;

        default:
            Console.WriteLine("Enter a number 1-4");
            PointBattle();
            break;
    }
}

string AddDecision(bool usesPoints)
{
    Console.Clear();
    Console.WriteLine("Enter decision name:");
    var decisionName = Console.ReadLine();
    while (string.IsNullOrWhiteSpace(decisionName))
    {
        Console.WriteLine("Please enter a valid decision name");
        decisionName = Console.ReadLine();
    }
    if (!usesPoints) { decisions.Add(decisionName); }
    return decisionName;
}
string DecidePoints()
{
    var decisionName = AddDecision(true);
    int decisionUpside = GetValidatedInput("Rate the upsides of this decision with a number from 1-10:");
    int decisionDownside = GetValidatedInput("Rate the negative effects or risk with a number from 1-10:");
    int decisionGutFeeling = GetValidatedInput("Enter your gut feeling about this decision with a number from 1-10:");

    var points = decisionUpside + decisionGutFeeling - decisionDownside;

    pointDecisions[decisionName] = points;

    string result = $"Added decision: {decisionName}";
    return result;
}
void DisplayDecisions()
{
    if (decisions.Count == 0) { Console.WriteLine("No decisions have been added"); }
    else
    {
        Console.WriteLine("\nDecisions:");
        foreach (var decision in decisions)
        {
            Console.WriteLine($"{decision}");
        }
    }
    Console.WriteLine();
}

void DisplayPoints()
{
    if (pointDecisions.Count == 0)
    {
        Console.WriteLine("No decisions have been added");
    }
    else
    {
     //   Console.WriteLine("\nDecisions and their points:");

        int maxValue = pointDecisions.Values.Max();
        int minValue = pointDecisions.Values.Min();

        var sortedDecisions = pointDecisions.OrderByDescending(d => d.Value);

        foreach (var decision in sortedDecisions)
        {
            if (decision.Value == maxValue)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (decision.Value == minValue)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine($"{decision.Key}: {decision.Value} points");

            Console.ResetColor();
        }
    }
    Console.WriteLine();
}


void DecisionBattle()
{
    if (decisions.Count < 2) { Console.WriteLine("A minimum of two decisions is required"); }
    else
    {
        Console.WriteLine("Make your decision");
        Console.WriteLine($"1: {decisions[0]}");
        Console.WriteLine($"2: {decisions[1]}");
        var input = Console.ReadLine();
        if (input == "1")
        {
            Console.WriteLine($"Decision {decisions[0]} wins this battle and proceedes to the next round!");
            decisions.Remove(decisions[1]);
        }
        else if (input == "2")
        {
            Console.WriteLine($"Decision {decisions[1]} wins this battle and proceedes to the next round!");
            decisions.Remove(decisions[0]);
        }
        else { Console.WriteLine("Invalid input. Enter a number 1 or 2"); } 
        Winner();
    }
}

void Winner()
{
    if (decisions.Count == 1) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"decision {decisions[0]} is the winner!");
        Console.ForegroundColor = ConsoleColor.Gray;
        Cup();
    }
    else { DecisionBattle(); }
    Console.WriteLine();
}

void Reset()
{
    pointDecisions.Clear();
    decisions.Clear();
    Console.Clear();
    Console.WriteLine("Decisions have been reset");
    Console.WriteLine();
}

int GetValidatedInput(string input)
{
    int result;
    bool isValid = false;

    while (!isValid)
    {
        Console.WriteLine(input);
        try
        {
            result = int.Parse(Console.ReadLine());
            if (result < 1 || result > 10)
            {
                Console.WriteLine("Please enter a number 1-10");
            }
            else
            {
                isValid = true;
                return result;
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input format");
        }
    }

    throw new InvalidOperationException("Unexpected end of input loop");
}