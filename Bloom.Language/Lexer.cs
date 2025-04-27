namespace Bloom.Language;

using static TokenType;

/// <summary>
/// Responsible for breaking down a sequence of characters into lexical tokens.
/// </summary>
public interface ILexer {
    /// <summary>
    /// Generate a series of lexical tokens from the provided input sequence of characters.
    /// </summary>
    /// <param name="input"> The sequence of characters to be processed into tokens. </param>
    /// <returns> An enumerable collection of tokens representing the input sequence. </returns>
    public IEnumerable<Token> GenerateTokens(IEnumerable<char> input);
}

/// <inheritdoc />
public sealed class Lexer : ILexer {
    /// <inheritdoc />
    public IEnumerable<Token> GenerateTokens(IEnumerable<char> input) {
        return input.Select(c => new Token(Whitespace, new string([c]))); // Stupid dummy implementation...
    }
}