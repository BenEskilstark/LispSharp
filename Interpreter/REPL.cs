namespace Interpreter;

public class REPL(Tree startingEnv) {
    public Tree Environment { get; } = startingEnv;
    public int LineNum { get; set; } = startingEnv.Children.Count;

    public void Start() {
        startingEnv.Eval();
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            if (input == "exit" || input == "(exit)") break;
            if (input == "") continue;

            try {
                Tree nextLine = Parser
                    .Parse(input, this.Environment)
                    .Children[this.LineNum];

                Console.WriteLine(nextLine.Eval());
                this.LineNum++;
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}