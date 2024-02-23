using TinyCompiler;

const string lexerSource = "LET foo = 123456";
const string tokenSource = "+-*/";

Lexer lexer = new(tokenSource);
// while (lexer.Peek() != char.MinValue) {
//     lexer.GetCurrentChar();
//     lexer.NextChar();
// }

var token = lexer.GetToken();
while (token.GetTokenType() != TokenType.Eof) {
    Console.WriteLine("TokenType : {0}", token.GetTokenType());
    token = lexer.GetToken();
}
