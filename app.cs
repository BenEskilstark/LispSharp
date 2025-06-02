using Interpreter;

string path = args.Length > 0 ? args[0] : "lisp.ls";

string file = File.ReadAllText(path);

Console.WriteLine(Parser.Parse(file).Eval());
