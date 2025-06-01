namespace Interpreter;

public class Builtins()
{
    public static void Eval(Tree tree)
    {
        Tree? fn = tree.ScopeLookup(tree.Children[0].Value)?.Copy(tree);
        if (fn != null && fn.Value == null) {
            fn.Parent = tree;
            fn.Value = null;
            Tree.PopulateScope(fn);
            fn.Value = fn.Eval();
        }
        switch (tree.Children[0].Value)
        {
            case "def":
                tree.Parent!.Scope.Add(tree.Children[1].Value!, tree.Children[2].Copy(tree.Parent));
                break;
            case "print":
                Console.WriteLine(tree.Children[1].Value);
                break;

            // functions
            case "fn":
            case "\\":
                tree.Value = tree.Children[2].Value!;
                tree.Parent?.Value = tree.Value; // also set the caller's value
                break;

            // arrays
            case "[":
            case "list":
                tree.IsArray = true;
                break;

            // conditionals
            case "if":
                if (bool.Parse(tree.Children[1].Value ?? "false"))
                {
                    tree.Value = tree.Children[2].Value;
                }
                else
                {
                    tree.Value = tree.Children[3].Value;
                }
                break;

            // equality
            case "=":
            case "==":
                tree.Value =
                    AreEqual(tree.Children[1].Value, tree.Children[2].Value)
                    .ToString();
                break;
            case "<":
                tree.Value = (
                        double.Parse(tree.Children[1].Value!)
                        < double.Parse(tree.Children[2].Value!)
                    ).ToString();
                break;

            case "<=":
                tree.Value = (
                        double.Parse(tree.Children[1].Value!)
                        <= double.Parse(tree.Children[2].Value!)
                    ).ToString();
                break;

            case ">":
                tree.Value = (
                        double.Parse(tree.Children[1].Value!)
                        > double.Parse(tree.Children[2].Value!)
                    ).ToString();
                break;

            case ">=":
                tree.Value = (
                        double.Parse(tree.Children[1].Value!)
                        >= double.Parse(tree.Children[2].Value!)
                    ).ToString();
                break;


            // arithmetic
            case "+":
                tree.Value = tree.Children[1..]
                    .Select(c => double.Parse(c.Value!))
                    .Sum().ToString();
                break;
            case "-":
                List<double> numbers = tree.Children[1..]
                    .Select(c => double.Parse(c.Value!))
                    .ToList();
                tree.Value = numbers.Skip(1)
                    .Aggregate(numbers.First(), (cur, n) => cur - n)
                    .ToString();
                break;
            case "*":
                Console.WriteLine(tree);
                tree.Value = tree.Children[1..]
                    .Select(c => double.Parse(c.Value!))
                    .Aggregate((total, next) => total * next)
                    .ToString();
                break;
            case "/":
                numbers = tree.Children[1..].Select(c => double.Parse(c.Value!)).ToList();
                tree.Value = numbers.Skip(1)
                    .Aggregate(numbers.First(), (current, n) => current / n)
                    .ToString();
                break;

            default:
                // else try to evaluate first position as a function
                // Tree? treeFn = tree.FunctionLookup(tree.Children[0].Value);
                // Console.WriteLine("evaling: " + tree.Children[0] + " as " + treeFn);
                break;
        }
    }

    private static bool AreEqual(string? value1, string? value2)
    {

        if (bool.TryParse(value1, out bool bool1) && bool.TryParse(value2, out bool bool2))
        {
            return bool1 == bool2;
        }

        if (int.TryParse(value1, out int int1) && int.TryParse(value2, out int int2))
        {
            return int1 == int2;
        }

        if (double.TryParse(value1, out double num1) && double.TryParse(value2, out double num2))
        {
            double tolerance = 1e-10;
            return Math.Abs(num1 - num2) < tolerance;
        }

        return false;
    }
}