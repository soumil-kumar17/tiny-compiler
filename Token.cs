namespace TinyCompiler;

public sealed class Token {
    private readonly string _tokenText;
    private readonly TokenType _tokenType;
    public static readonly Token Empty = new();

    private Token() {}
    public Token(string tokenText, TokenType tokenType) {
        _tokenText = tokenText;
        _tokenType = tokenType;
    }
    
    public TokenType GetTokenType() => _tokenType;
}