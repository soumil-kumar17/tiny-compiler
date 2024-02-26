namespace TinyCompiler;

public sealed class Lexer : ILexer {
    private readonly string _source;
    private string _currentChar;
    private int _currentPos;
    
    public Lexer(string source) {
        _source = source + '\n';
        _currentChar = string.Empty;
        _currentPos = -1;
        NextChar();
    }
    
    public void NextChar() {
        _currentPos += 1;
        _currentChar = _currentPos >= _source.Length ? "\0" : _source[_currentPos].ToString();
    }

    public string Peek() {
        return _currentPos + 1 >= _source.Length ? char.MinValue.ToString() : _source[_currentPos + 1].ToString();
    }

    public void Abort(string message) {
        throw new Exception($"Lexing error : {message}");
    }

    public void SkipWhiteSpace() {
        while (_currentChar is " " or "\t" or "\r") {
            NextChar();
        }
    }

    public void SkipComment() {
        if (_currentChar == "#") {
            while (_currentChar != "\n") {
                NextChar();
            }
        }
    }

    public Token GetToken() {
        SkipWhiteSpace();
        SkipComment();
        Token token = Token.Empty;

        switch (_currentChar) {
            case "+":
                token = new Token(_currentChar, TokenType.Plus);
                break;
            case "-":
                token = new Token(_currentChar, TokenType.Minus);
                break;
            case "*":
                token = new Token(_currentChar, TokenType.Asterisk);
                break;
            case "/":
                token = new Token(_currentChar, TokenType.Slash);
                break;
            case "=":
                if (Peek() == "=") {
                    string lastChar = _currentChar;
                    NextChar();
                    token = new Token(lastChar + _currentChar, TokenType.EqEq);
                }
                else {
                    token = new Token(_currentChar, TokenType.Eq);
                }
                break;
            case ">":
                if (Peek() == "=") {
                    string lastChar = _currentChar;
                    NextChar();
                    token = new Token(lastChar + _currentChar, TokenType.GtEq);
                }
                else {
                    token = new Token(_currentChar, TokenType.Gt);
                }
                break;
            case "<":
                if (Peek() == "=") {
                    string lastChar = _currentChar;
                    NextChar();
                    token = new Token(lastChar + _currentChar, TokenType.LtEq);
                }
                else {
                    token = new Token(_currentChar, TokenType.Lt);
                }
                break;
            case "!":
                if (Peek() == "=") {
                    string lastChar = _currentChar;
                    NextChar();
                    token = new Token(lastChar + _currentChar, TokenType.NotEq);
                }
                else {
                    Abort("Expected !=, got !" + Peek());
                }
                break;
            case "\"":
                NextChar();
                int startPos = _currentPos;

                while (_currentChar != "\"") {
                    if (_currentChar is "\r" or "\n" or "\t" or "\\" or "%") {
                        Abort("Illegal character in string.");
                    }
                    NextChar();
                }

                string tokText = _source.Substring(startPos, _currentPos - startPos);
                token = new Token(tokText, TokenType.String);
                break;
            default:
                if (char.IsDigit(_currentChar[0])) {
                    startPos = _currentPos;
                    while (char.IsDigit(Peek()[0])) {
                        NextChar();
                    }
                    if (Peek()[0] == '.') {
                        NextChar();
                        if (!char.IsDigit(Peek()[0])) {
                            Abort("Illegal character in number.");
                        }
                        while (char.IsDigit(Peek()[0])) {
                            NextChar();
                        }
                    }

                    tokText = _source.Substring(startPos, _currentPos - startPos + 1);
                    token = new Token(tokText, TokenType.Number);
                }
                else if (char.IsLetter(_currentChar[0])) {
                    startPos = _currentPos;
                    while (char.IsLetterOrDigit(Peek()[0])) {
                        NextChar();
                    }

                    tokText = _source.Substring(startPos, _currentPos - startPos + 1);
                    TokenType? keyword = Token.CheckForKeyword(tokText);
                    token = keyword == null ? new Token(tokText, TokenType.Ident) : new Token(tokText, keyword.Value);
                }
                else if (_currentChar == "\n")
                {
                    token = new Token(_currentChar, TokenType.Newline);
                }
                else if (_currentChar == "\0") {
                    token = new Token("", TokenType.Eof);
                }
                else {
                    Abort("Unknown token: " + _currentChar);
                }
                break;
        }
        NextChar();
        return token;
    }

    public void GetCurrentChar() => Console.WriteLine("Index : {0}, Character : {1}", _currentPos, _currentChar);
}