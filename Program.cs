using TinyCompiler;

const string lexerSource = "Let foo = 123456";
const string tokenSource = "+- \"This is a string\" # This is a comment!\n */";
const string numberSource = "+-123 9.8654*/";
const string keywordSource = "If+-123 foo*Then/";

Lexer lexer = new(lexerSource);
var token = lexer.GetToken();
while (token.Type != TokenType.Eof) {
    Console.WriteLine("TokenType : {0}", token.Type);
    token = lexer.GetToken();
}
