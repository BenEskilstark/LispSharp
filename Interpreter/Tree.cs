namespace Interpreter;


public class Tree(int _depth)
{
    public int Depth { get; } = _depth;
    public string? Value {
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

    

    public string Eval()
    {
        if (this.Value != null) return this.Value;
        if (this.Children.Count == 0) return null;

        bool lazyEval = false;
        // Console.WriteLine(this);
        if (this.Children[0].Value == "if") {
            this.Children[1].Value ??= this.Children[1].Eval();
            if (bool.Parse(this.Children[1].Value ?? "false")) {
                this.Children[2].Value ??= this.Children[2].Eval();
            }
            else {
                this.Children[3].Value ??= this.Children[3].Eval();
            }
            lazyEval = true;
        }

        if (!lazyEval) {
            for (int i = 0; i < this.Children.Count; i++)
            {
                Tree child = this.Children[i];
                if (i == 0 && child.IsLambda)
                {
                    PopulateScope(child);
                }
                else if (child.IsLambda)
                {
                    continue; // don't eval fn that's not in fn position
                }
                else if (i == 1 && this.IsLambda)
                {
                    continue; // don't eval your own arguments
                }
                child.Value = child.Eval();

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
        // child.Value = null;
        Tree parent = child.Parent!;
        for (int j = 0; j < child.Children[1].Children.Count; j++)
        {
            // Console.WriteLine("X " + NoScopeValue(child.Children[1].Children[j])+ ": " + parent.Children[j + 1]);
            parent.Scope[NoScopeValue(child.Children[1].Children[j])!] = parent.Children[j + 1];
        }
    }

    public static void PrintScope(Tree tree) {
        foreach ((string key, Tree value) in tree.Scope) {
            Console.WriteLine(key + ": " + value);
        }
    }


    public Tree? ScopeLookup(string? value)
    {
        if (value == null) return null;
        // Console.WriteLine(value);
        if (Scope.TryGetValue(value, out var tree)) return tree;
        if (this.Parent != null) return this.Parent.ScopeLookup(value);
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

    public Tree Copy(Tree? parent) {
        Tree copy = new(this.Depth) { 
            Value = NoScopeValue(this),
            Parent = parent,
        };
        foreach (var p in this.Scope) {
            copy.Scope[p.Key] = p.Value.Copy(parent);
        }
        copy.Children = this.Children.Select(c => c.Copy(copy)).ToList();
        return copy;
    }

    public static string? NoScopeValue(Tree thisTree) {
        if (thisTree.Value == null) return thisTree.Value;
        
        Tree? p = thisTree;
        while (p != null) {  
            foreach ((string key, Tree tree) in p.Scope) {
                if (thisTree.Value == tree.Value) {
                    // Console.WriteLine("No scoped " + key);
                    return key;
                }
            }
            p = p.Parent;
        }
        return thisTree.Value;
    }

    public override string ToString()
    {
        if (this.Value != null) return this.Value;
        return "(" + string.Join(" ", Children) + ")";
    }
}
