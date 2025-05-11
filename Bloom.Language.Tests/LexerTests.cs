using FluentAssertions;

namespace Bloom.Language.Tests;

using static TokenType;

public sealed class LexerTests {
    private readonly Lexer _lexer = new();

    [Fact]
    public void GenerateTokens_EmptyInput_ReturnsOnlyEndOfFile() {
        // Act
        var tokens = _lexer.GenerateTokens(string.Empty);

        // Assert
        tokens
            .Should()
            .SatisfyRespectively(token => {
                    token.Type.Should().Be(EndOfFile);
                    token.Lexeme.Should().BeEmpty();
                }
            );
    }

    [Fact]
    public void GenerateTokens_WhitespaceOnly_ReturnsWhitespaceThenEndOfFile() {
        // Arrange
        const string source = " \t\r\n";

        // Act
        var tokens = _lexer.GenerateTokens(source).ToArray();

        // Assert
        tokens
            .Should()
            .SatisfyRespectively(
                token => {
                    token.Type.Should().Be(Whitespace);
                    token.Lexeme.Should().Be(source);
                },
                token => {
                    token.Type.Should().Be(EndOfFile);
                    token.Lexeme.Should().BeEmpty();
                }
            );
    }

    [Theory]
    [InlineData("axiom", AxiomKeyword)]
    [InlineData("rule", RuleKeyword)]
    [InlineData("foo99", Symbol)]
    public void GenerateTokens_SingleIdentifier_ReturnsCorrectTokenType(
        string source,
        TokenType expectedType
    ) {
        // Act
        var tokens = _lexer.GenerateTokens(source);

        // Assert
        tokens
            .Should()
            .SatisfyRespectively(
                token => {
                    token.Type.Should().Be(expectedType);
                    token.Lexeme.Should().Be(source);
                },
                token => {
                    token.Type.Should().Be(EndOfFile);
                    token.Lexeme.Should().BeEmpty();
                }
            );
    }

    [Fact]
    public void GenerateTokens_AxiomStatement_ReturnsExpectedStream() {
        // Arrange
        const string source = "axiom A B";

        // Act
        var tokens = _lexer.GenerateTokens(source).ToArray();

        // Assert
        tokens
            .Select(t => (t.Type, t.Lexeme))
            .Should()
            .BeEquivalentTo(
                [
                    (AxiomKeyword, "axiom"),
                    (Whitespace, " "),
                    (Symbol, "A"),
                    (Whitespace, " "),
                    (Symbol, "B"),
                    (EndOfFile, string.Empty)
                ]
            );
    }

    [Fact]
    public void GenerateTokens_RuleStatement_ReturnsExpectedStream() {
        // Arrange
        const string source = "rule A -> B A";

        // Act
        var tokens = _lexer.GenerateTokens(source).ToArray();

        // Assert
        tokens
            .Select(t => (t.Type, t.Lexeme))
            .Should()
            .BeEquivalentTo(
                [
                    (RuleKeyword, "rule"),
                    (Whitespace, " "),
                    (Symbol, "A"),
                    (Whitespace, " "),
                    (Transition, "->"),
                    (Whitespace, " "),
                    (Symbol, "B"),
                    (Whitespace, " "),
                    (Symbol, "A"),
                    (EndOfFile, string.Empty)
                ]
            );
    }

    [Fact]
    public void GenerateTokens_UnknownCharacter_IgnoresAndStillProducesEof() {
        // Arrange
        const string source = "#";

        // Act
        var tokens = _lexer.GenerateTokens(source).ToArray();

        // Assert
        tokens
            .Should()
            .SatisfyRespectively(token => {
                    token.Type.Should().Be(EndOfFile);
                    token.Lexeme.Should().BeEmpty();
                }
            );
    }
}