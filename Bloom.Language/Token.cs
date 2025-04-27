namespace Bloom.Language;

/// <summary>
/// Represents a lexical token in the Bloomish language, characterized by its type and associated lexeme.
/// </summary>
/// <remarks>
/// A token is a fundamental unit in the process of lexical analysis, where the input string is broken down into components
/// that can be further analyzed or processed. Each token is identified by its specific type and the actual string
/// representation (lexeme).
/// </remarks>
/// <param name="Type"> The type of the token, which determines its role or purpose in the language system. </param>
/// <param name="Lexeme"> The actual string or sequence of characters that matches the token type. </param>
public sealed record Token(TokenType Type, string Lexeme);