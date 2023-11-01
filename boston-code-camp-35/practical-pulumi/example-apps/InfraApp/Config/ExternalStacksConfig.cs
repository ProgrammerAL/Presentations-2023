using InfraApp.Config;

using System;

namespace PurpleSpikeProductions.Iaac.ArcadeServices.Config;
public record ExternalStacksConfig(
    string SharedStackName);

public class ExternalStacksConfigDto : ConfigDtoBase<ExternalStacksConfig>
{
    public string? SharedStackName { get; set; }

    public override ExternalStacksConfig GenerateValidConfigObject()
    {
        if (!string.IsNullOrWhiteSpace(SharedStackName))
        {
            return new ExternalStacksConfig(SharedStackName);
        }

        throw new Exception($"{GetType().Name} has invalid config");
    }
}
