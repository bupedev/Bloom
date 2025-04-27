namespace Bloom.CLI;

/// <summary>
/// Utilities for wrangling types via reflection.
/// </summary>
internal static class ReflectionUtils {
    /// <summary>
    /// Get all types that concretely implement <typeparamref name="TInterface" />.
    /// </summary>
    internal static IReadOnlyCollection<Type> GetImplementingTypes<TInterface>() {
        return typeof(TInterface)
            .Assembly
            .DefinedTypes
            .Where(t => t.IsAbstract is false && typeof(TInterface).IsAssignableFrom(t))
            .ToArray();
    }
}