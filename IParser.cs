namespace TinyCompiler;

public interface IParser {
    public bool CheckToken(TokenType type);
    public bool CheckPeek(TokenType type);
    public void Match(TokenType type);
    public void NextToken();
    public void Abort(string message);
    public void Statement();
    public bool IsComparisonOperator();
    public void Program();
    public void Comparison();
    public void Expression();
    public void Term();
    public void Unary();
    public void Primary();
    public void Newline();
}