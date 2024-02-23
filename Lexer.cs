namespace TinyCompiler;

public sealed class Lexer : ILexer {
    private readonly string _source;
    private string _currentChar;
    private int _currentPos;
    
    public Lexer(string source) {
        _source = source + '\n';
        _currentChar = string.Empty;
        _currentPos = 0;
    }
    
    public void NextChar() {
        if (_currentPos >= _source.Length) {
            _currentChar = "\0";
        }
        else {
            _currentChar = _source[_currentPos].ToString();
            _currentPos += 1;
        }
    }

    public char Peek() {
        return _currentPos >= _source.Length ? char.MinValue : _source[_currentPos];
    }

    public void Abort(string message) {
        throw new Exception($"Lexing error : {message}");
    }

    public void SkipWhiteSpace() {
        throw new NotImplementedException();
    }

    public void SkipComment() {
        throw new NotImplementedException();
    }

    public Token GetToken() {
        Token token = _currentChar switch {
            "+" => new Token(_currentChar, tokenType: TokenType.Plus),
            "-" => new Token(_currentChar, tokenType: TokenType.Minus),
            "*" => new Token(_currentChar, tokenType: TokenType.Asterisk),
            "/" => new Token(_currentChar, tokenType: TokenType.Slash),
            "\n" => new Token(_currentChar, tokenType: TokenType.Newline),
            "\0" => new Token("", tokenType: TokenType.Eof),
            _ => Token.Empty
        };

        NextChar();
        return token;
    }

    public void GetCurrentChar() => Console.WriteLine("Index : {0}, Character : {1}", _currentPos, _currentChar);
}