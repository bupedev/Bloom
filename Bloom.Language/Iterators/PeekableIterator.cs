namespace Bloom.Language.Iterators;

/// <summary>
/// Forward‑only reader that lets you preview the upcoming element without consuming it.
/// <para>
/// Typical workflow:
/// <list type="number">
///     <item>
///         <description> Create <see cref="PeekableIterator{T}" /> from any <see cref="IEnumerable{T}" />. </description>
///     </item>
///     <item>
///         <description> Call <see cref="HasNext" /> to check for more data. </description>
///     </item>
///     <item>
///         <description> Use <see cref="Peek" /> to look ahead, or <see cref="Next" /> to consume. </description>
///     </item>
///     <item>
///         <description>
///         <see cref="Current" /> always holds the element returned by the most recent <see cref="Next" />
///         call.
///         </description>
///     </item>
/// </list>
/// </para>
/// <remarks>
/// Designed for single‑pass processing—no reset, no <c> foreach </c> support, and no thread‑safety guarantees.
/// </remarks>
/// </summary>
/// <typeparam name="T"> The type of element being iterated. </typeparam>
internal sealed class PeekableIterator<T> : IDisposable {
    private readonly IEnumerator<T> _source;
    private T _next;

    public PeekableIterator(IEnumerable<T> source) {
        _source = source.GetEnumerator();
        HasNext = _source.MoveNext();
        _next = HasNext ? _source.Current : default!;
    }

    /// <summary>
    /// The element that was last returned by <see cref="Next" />. Undefined (default) before the first call.
    /// </summary>
    public T Current { get; private set; } = default!;

    /// <summary>
    /// True if there’s another element available (for <see cref="Peek" /> or <see cref="Next" />).
    /// </summary>
    public bool HasNext { get; private set; }

    /// <inheritdoc />
    public void Dispose() {
        _source.Dispose();
    }

    /// <summary>
    /// Look at the next element without consuming it.
    /// </summary>
    public T Peek() {
        if (!HasNext) throw new InvalidOperationException("Nothing left to peek.");
        return _next;
    }

    /// <summary>
    /// Consume and return the next element.
    /// </summary>
    public T Next() {
        if (!HasNext) throw new InvalidOperationException("No more elements.");

        Current = _next;
        HasNext = _source.MoveNext();
        _next = HasNext ? _source.Current : default!;

        return Current;
    }
}