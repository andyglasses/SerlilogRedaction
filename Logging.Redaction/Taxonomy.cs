using Microsoft.Extensions.Compliance.Classification;

namespace Serilog_redaction.Logging.Redaction;

public static class Taxonomy
{
    public static string TaxonomyName => typeof(Taxonomy).FullName!;

    public static DataClassification Personal => new(TaxonomyName, nameof(Personal));
    public static DataClassification Secret => new(TaxonomyName, nameof(Secret));

    public class PersonalDataAttribute() : DataClassificationAttribute(Taxonomy.Personal);
    public class SecretDataAttribute() : DataClassificationAttribute(Taxonomy.Secret);
}