namespace Bloom.Language;

/// <summary>
/// Represents the different categories of tokens that can be identified and processed by the language system.
/// </summary>
public enum TokenType {
    /// <summary>
    /// The end of a statement.
    /// </summary>
    Terminator,

    /// <summary>
    /// The transition from a symbol to some sequence of symbols inside a rule statement.
    /// </summary>
    Transition,

    /// <summary>
    /// The start of an axiom statement.
    /// </summary>
    AxiomKeyword,

    /// <summary>
    /// The start of a rule statement.
    /// </summary>
    RuleKeyword,

    /// <summary>
    /// A symbol used in axiom and rule statements.
    /// </summary>
    Symbol,

    /// <summary>
    /// Whitespace which is ignored in all systems beyond the lexer.
    /// </summary>
    Whitespace,

    /// <summary>
    /// The end of the program.
    /// </summary>
    EndOfFile
}