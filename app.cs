using Interpreter;

string path = args.Length > 1 ? args[1] : "lisp.ls";

string file = File.ReadAllText(path);

Console.WriteLine(Parser.Parse(file).Eval());
