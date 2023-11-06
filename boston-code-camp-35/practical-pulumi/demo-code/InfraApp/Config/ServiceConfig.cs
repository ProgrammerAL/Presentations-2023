using System;

namespace PulumiDemo.Config;
public record ServiceConfig(
    string Version,
    string Environment);

public class ServiceConfigDto : ConfigDtoBase<ServiceConfig>
{
    public string? Version { get; set; }
    public string? Environment { get; set; }

    public override ServiceConfig GenerateValidConfigObject()
    {
        if (!string.IsNullOrWhiteSpace(Version)
            && !string.IsNullOrWhiteSpace(Environment))
        {
            return new ServiceConfig(Version, Environment);
        }

        throw new Exception($"{GetType().Name} has invalid config");
    }
}
