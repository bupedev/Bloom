using System.Text;
using Bloom.Language.Iterators;

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
        using (var iterator = new PeekableIterator<char>(input)) {
            while (iterator.HasNext) {
                iterator.Next();
                var token = ScanToken(iterator);
                if (token is not null) yield return token;

                // TODO: Somehow handle characters not supported by Bloomish without ignoring...
            }

            yield return new Token(EndOfFile, string.Empty);
        }
    }

    private static Token? ScanToken(PeekableIterator<char> iterator) {
        return iterator.Current switch {
            var c when IsWhitespace(c) => ScanWhitespace(iterator),
            var c when IsIdentifierStart(c) => ScanIdentifier(iterator),
            '-' when iterator.Peek() == '>' => new Token(Transition, "->"),
            _ => null
        };
    }

    private static Token ScanWhitespace(PeekableIterator<char> iterator) {
        var lexeme = CollectLexeme(iterator, IsWhitespace);
        return new Token(Whitespace, lexeme);
    }

    private static bool IsWhitespace(char c) {
        return c is ' ' or '\t' or '\r' or '\n';
    }

    private static Token ScanIdentifier(PeekableIterator<char> iterator) {
        var lexeme = CollectLexeme(iterator, IsIdentifier);
        var type = lexeme switch {
            "axiom" => AxiomKeyword,
            "rule" => RuleKeyword,
            _ => Symbol
        };
        return new Token(type, lexeme);
    }

    private static bool IsIdentifierStart(char c) {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }

    private static bool IsIdentifier(char c) {
        return IsIdentifierStart(c) || c is >= '0' and <= '9';
    }

    private static string CollectLexeme(PeekableIterator<char> iterator, Predicate<char> included) {
        var lexeme = new StringBuilder();
        lexeme.Append(iterator.Current);
        while (iterator.HasNext && included(iterator.Peek())) lexeme.Append(iterator.Next());
        return lexeme.ToString();
    }
}