namespace TableViewer.Share.Options;

/// <summary>Common field configuration with a presentation contract supplied by a UI adapter.</summary>
public abstract class TeFieldOption<TValue, TProperty> : ITeFieldOption
    where TProperty : new()
{
    public string? Id { get; set; }
    public TProperty Property { get; set; } = new();
    public string TypeName => typeof(TValue).FullName!;
}

/// <summary>Ordered domain-value validation shared by the web and native field adapters.</summary>
public abstract class TeValidatedFieldOption<TValue, TProperty> : TeFieldOption<TValue, TProperty>
    where TProperty : new()
{
    public List<ITeValidation> Validations { get; set; } = [];
}
