namespace TinyCompiler;

public interface ILexer {
    public void NextChar();
    public string Peek();
    public void Abort(string message);
    public void SkipWhiteSpace();
    public void SkipComment();
    public Token GetToken();
    public void GetCurrentChar();
}