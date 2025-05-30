string path = args.Any() ? args[0] : "lisp.ls";

string file = File.ReadAllText(path);

Console.WriteLine(Interpreter.Eval(Interpreter.Parse(file)));

public class Interpreter()
{
    public static string Eval(Tree tree)
    {
        if (tree.Value != null) return tree.Value;
        Console.WriteLine(tree);
        foreach (Tree child in tree.Children)
        {
            if (child.Value == null) child.Value = Eval(child);
        }

        switch (tree.Children[0].Value)
        {
            case "+":
                tree.Value = tree.Children[1..].Select(c => double.Parse(c.Value!)).Sum().ToString();
                break;
        }
        return tree.Value!;
    }


    public static Tree Parse(string input)
    {
        List<string> tokens = Tokenize(input);
        Tree curTree = new();
        Stack<Tree> consStack = [];
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "(":
                    Tree child = new();
                    curTree.Add(child);
                    consStack.Push(curTree);
                    curTree = child;
                    break;
                case ")":
                    curTree = consStack.Pop();
                    break;
                default:
                    curTree.Add(token);
                    break;
            }
        }
        return curTree;
    }

    public static List<string> Tokenize(string input)
    {
        List<string> tokens = [];
        string curToken = "";

        bool inQuote = false;

        foreach (char c in input)
        {
            if (inQuote)
            {
                curToken += c;
                if (c == '"') inQuote = false;
                continue;
            }
            if (c == ' ' || c == '\n' || c == '\t' || c == '\r')
            {
                if (curToken != "") tokens.Add(curToken);
                curToken = "";
            }
            else if (c == '"')
            {
                if (curToken != "") throw new Exception("unexpected open quote");
                inQuote = true;
                curToken = "\"";
            }
            else if (c == '(')
            {
                if (curToken != "") throw new Exception("unexpected open paren");
                tokens.Add("(");
            }
            else if (c == ')')
            {
                if (curToken != "") tokens.Add(curToken);
                tokens.Add(")");
                curToken = "";
            }
            else
            {
                curToken += c;
            }

        }
        return tokens;
    }
}


public class Tree()
{
    public string? Value { get; set; }
    public List<Tree> Children { get; set; } = [];

    public void Add(string item)
    {
        Children.Add(new Tree() { Value = item });
    }
    public void Add(Tree item)
    {
        Children.Add(item);
    }

    public override string ToString()
    {
        if (Value != null) return Value;
        return "(" + string.Join(" ", Children) + ")";
    }
}
