namespace TinyCompiler;

public sealed class Parser : IParser {
    private readonly ILexer _lexer;
    private Token CurrentToken { get; set; }
    private Token PeekToken { get; set; }
    private static HashSet<string> Symbols { get; set; }
    private static HashSet<string> DeclaredLabels { get; set; }
    private static HashSet<string> LabelsGotoed { get; set; }

    public Parser(ILexer lexer) {
        _lexer = lexer;
        Symbols = new HashSet<string>();
        DeclaredLabels = new HashSet<string>();
        LabelsGotoed = new HashSet<string>();
        CurrentToken = Token.Empty;
        PeekToken = Token.Empty;
        NextToken();
        NextToken();
    }
    
    public bool CheckToken(TokenType type) {
        return type == CurrentToken.Type;
    }

    public bool CheckPeek(TokenType type) {
        return type == PeekToken.Type;
    }

    public void Match(TokenType type) {
        if (!CheckToken(type)) {
            Abort($"Expected {type.ToString()}, got {CurrentToken.Type.ToString()}");
        }
        NextToken();
    }

    public void NextToken() {
        CurrentToken = PeekToken;
        PeekToken = _lexer.GetToken();
    }

    public void Abort(string message) {
        throw new Exception($"Error : {message}");
    }

    public void Statement() {
        if (CheckToken(TokenType.Print)) {
            Console.WriteLine("Statement : Print");
            NextToken();
            if (CheckToken(TokenType.String)) {
                NextToken();
            }
            else {
                Expression();
            }
        }
        else if (CheckToken(TokenType.If)) {
            Console.WriteLine("Statement : If");
            NextToken();
            Comparison();
            Match(TokenType.Then);
            Newline();
            while (!CheckToken(TokenType.Endif)) {
                Statement();
            }
            Match(TokenType.Endif);
        }
        else if (CheckToken(TokenType.While)) {
            Console.WriteLine("Statement : While");
            NextToken();
            Comparison();
            Match(TokenType.Repeat);
            Newline();
            while (!CheckToken(TokenType.EndWhile)) {
                Statement();
            }
            Match(TokenType.EndWhile);
        }
        else if (CheckToken(TokenType.Label)) {
            NextToken();
            if (DeclaredLabels.Contains(CurrentToken.Text)) {
                Abort($"Label already exists : {CurrentToken.Text}");
            }
            DeclaredLabels.Add(CurrentToken.Text);
            Match(TokenType.Ident);
        }
        else if (CheckToken(TokenType.Goto)) {
            Console.WriteLine("Statement : Goto");
            NextToken();
            LabelsGotoed.Add(CurrentToken.Text);
            Match(TokenType.Ident);
        }
        else if (CheckToken(TokenType.Let)) {
            Console.WriteLine("Statement : Let");
            NextToken();
            Symbols.Add(CurrentToken.Text);
            Match(TokenType.Ident);
            Match(TokenType.Eq);
        }
    }

    public bool IsComparisonOperator() {
        return CheckToken(TokenType.Lt) || CheckToken(TokenType.LtEq) || CheckToken(TokenType.Gt) || CheckToken
            (TokenType.GtEq) || CheckToken(TokenType.NotEq) || CheckToken(TokenType.EqEq);
    }

    public void Program() {
        Console.WriteLine("Program");
        while (CheckToken(TokenType.Newline)) {
            NextToken();
        }
        while (!CheckToken(TokenType.Eof)) {
            Statement();
        }
        foreach (var label in LabelsGotoed.Where(label => !DeclaredLabels.Contains(label))) {
            Abort($"Attempting to go to undeclared label {label}");
        }
    }

    public void Comparison() {
        Console.WriteLine("Comparison");
        Expression();
        if (IsComparisonOperator()) {
            NextToken();
            Expression();
        }
        else {
            Abort($"Expected comparison operator : {CurrentToken.Text}");
        }
        while (IsComparisonOperator()) {
            NextToken();
            Expression();
        }
    }

    public void Expression() {
        Console.WriteLine("Expression");
        Term();
        while (CheckToken(TokenType.Plus) || CheckToken(TokenType.Minus)) {
            NextToken();
            Term();
        }
    }

    public void Term() {
        Console.WriteLine("Term");
        Unary();
        while (CheckToken(TokenType.Slash) || CheckToken(TokenType.Asterisk)) {
            NextToken();
            Unary();
        }
    }

    public void Unary() {
        Console.WriteLine("Unary");
        if (CheckToken(TokenType.Plus) || CheckToken(TokenType.Minus)) {
            NextToken();
        }
        Primary();
    }

    public void Primary() {
        Console.WriteLine($"Primary : {CurrentToken.Text}");
        if (CheckToken(TokenType.Number)) {
            NextToken();
        }
        else if (CheckToken(TokenType.Ident)) {
            if (!Symbols.Contains(CurrentToken.Text)) {
                Abort($"Attempting to reference variable {CurrentToken.Text} before assignment");
            }
            NextToken();
        }
        else {
            Abort($"Unexpected token : {CurrentToken.Text}");
        }
    }

    public void Newline() {
        Console.WriteLine("Newline");
        Match(TokenType.Newline);
        while (CheckToken(TokenType.Newline)) {
            NextToken();
        }
    }
}