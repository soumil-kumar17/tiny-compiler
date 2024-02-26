namespace TinyCompiler;

public sealed class Token {
    public string Text { get; }
    public TokenType Type { get; }
    public static readonly Token Empty = new();

    private Token() {}
    public Token(string tokenText, TokenType tokenType) {
        Text = tokenText;
        Type = tokenType;
    }

    public static TokenType? CheckForKeyword(string tokenText) {
        foreach (TokenType type in Enum.GetValues(typeof(TokenType))) {
            if (type.ToString() == tokenText && (int)type >= 101 && (int)type < 200) {
                return type;
            }
        }
        return TokenType.Ident;
    }
}