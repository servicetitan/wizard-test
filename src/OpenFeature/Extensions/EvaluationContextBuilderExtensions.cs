using OpenFeature.Model;

namespace WizardTest.OpenFeature;

public static class EvaluationContextBuilderExtensions
{
    private const string ContextKindKey = "kind";
    private const string ContextKindDefaultValue = "user";

    /// <summary>
    ///     Sets the context kind to the specified value.
    /// </summary>
    public static EvaluationContextBuilder SetKind(this EvaluationContextBuilder builder, string? value = null)
        => builder.Set(ContextKindKey, value ?? ContextKindDefaultValue);
}
