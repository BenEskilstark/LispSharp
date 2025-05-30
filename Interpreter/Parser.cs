namespace Interpreter;

public class Parser()
{
    public static Tree Parse(string input)
    {
        List<string> tokens = Tokenize(input);
        Tree curTree = new(0);
        Stack<Tree> consStack = [];
        foreach (string token in tokens)
        {
            switch (token)
            {
                case "(":
                    Tree child = new(curTree.Depth + 1) { Parent = curTree };
                    curTree.Add(child);
                    consStack.Push(curTree);
                    curTree = child;
                    break;
                case ")":
                    curTree = consStack.Pop();
                    break;
                case "\\":
                case "fn":
                    if (curTree.Children.Count == 0)
                    {
                        curTree.IsLambda = true;
                    }

                    curTree.Add(token);
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
        bool inComment = false;

        foreach (char c in input)
        {
            if (inComment)
            {
                if (c == '\n' || c == '\r') inComment = false;
                continue;
            }
            if (!inQuote && c == '#')
            {
                inComment = true;
                continue;
            }

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
                if (curToken != "") throw new Exception("unexpected open quote " + curToken);
                inQuote = true;
                curToken = "\"";
            }
            else if (c == '(')
            {
                if (curToken != "") throw new Exception("unexpected open paren " + curToken);
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