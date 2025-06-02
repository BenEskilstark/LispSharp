using Interpreter;



if (args.Length == 0 || args.Contains("--repl")) {
    Tree env = new(0);
    if (args.Count() > 1) {
        foreach (var arg in args) {
            if (arg != "--repl") {
                env = Parser.Parse(File.ReadAllText(arg));
                break;
            }
        }
    }
    REPL repl = new(env);
    repl.Start();
} else {
    string path = args[0];
    string file = File.ReadAllText(path);

    Console.WriteLine(Parser.Parse(file).Eval());
}


