namespace Interpreter;


public class Tree(int _depth)
{
    public int Depth { get; } = _depth;
    public string? Value
    {
        get => ScopeLookup(field)?.Value ?? field;
        set;
    }
    public List<Tree> Children { get; set; } = [];

    // scope
    public Tree? Parent { get; set; }
    public Dictionary<string, Tree> Scope { get; set; } = [];

    // arrays
    public bool IsArray { get; set; } = false;

    // lambdas
    public bool IsLambda { get; set; } = false;
    public Dictionary<string, Tree> Functions { get; set; } = [];


    public string Eval()
    {
        if (this.Value != null) return this.Value;

        for (int i = 0; i < this.Children.Count; i++)
        {
            Tree child = this.Children[i];
            if (i == 0 && child.IsLambda)
            {
                PopulateScope(child);
            }
            else if (child.IsLambda)
            {
                child.Value = "fn_" + Guid.NewGuid().ToString();
                this.Parent.Functions[child.Value] = child;

                continue; // don't eval fn that's not in fn position
            }
            else if (i == 1 && this.IsLambda)
            {
                continue; // don't eval your own arguments
            }
            child.Value ??= child.Eval();

        }

        // Console.WriteLine(this);
        if (this.Children[0].Value?.StartsWith("fn_") == true)
        {
            Tree? fn = FunctionLookup(this.Children[0].Value);
            // Console.WriteLine("looked up: " + fn);
            if (fn != null)
            {
                fn.Value = null;
                fn.Parent = this;
                PopulateScope(fn);
                fn.Value = fn.Eval();
            }
        }

        Builtins.Eval(this);

        if (Depth == 0 && this.Value == null) this.Value = this.Children[^1].Value;

        return this.Value!;
    }


    // assumes that its parent is providing the arguments in the next N positions
    // where N is the number of arguments in this tree's first child
    public static void PopulateScope(Tree child)
    {
        Tree parent = child.Parent!;
        for (int j = 0; j < child.Children[1].Children.Count; j++)
        {
            // Console.WriteLine(child.Children[1].Children[j].Value + ": " + parent.Children[j + 1]);
            parent.Scope[child.Children[1].Children[j].Value!] = parent.Children[j + 1];
        }
    }


    public Tree? ScopeLookup(string? value)
    {
        if (value == null) return null;
        if (value.StartsWith("fn_"))
        {


        }
        if (Scope.TryGetValue(value, out var tree)) return tree;
        if (this.Parent != null) return this.Parent.ScopeLookup(value);
        return null;
    }


    public Tree? FunctionLookup(string? value)
    {
        if (value == null) return null;
        if (Functions.TryGetValue(value, out var tree)) return tree;
        if (this.Parent != null) return this.Parent.FunctionLookup(value);
        return null;
    }


    public void Add(string item)
    {
        Children.Add(new Tree(Depth + 1) { Value = item, Parent = this });
    }
    public void Add(Tree item)
    {
        item.Parent = this;
        Children.Add(item);
    }

    public override string ToString()
    {
        if (this.Value != null) return this.Value;
        return "(" + string.Join(" ", Children) + ")";
    }
}
