using Serilog_redaction.Logging.Redaction;

namespace Serilog_redaction;

public class Payload
{
    public required string Name { get; set; }

    [Taxonomy.SecretData]
    public required string Secure { get; set; }

    [Taxonomy.PersonalData]
    public required string Personal { get; set; }

    public required Dictionary<string, string> Dictionary { get; set; }
}
