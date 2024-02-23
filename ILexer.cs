namespace TinyCompiler;

public interface ILexer {
    public void NextChar();
    public char Peek();
    public void Abort(string message);
    public void SkipWhiteSpace();
    public void SkipComment();
    public Token GetToken();
    public void GetCurrentChar();
}