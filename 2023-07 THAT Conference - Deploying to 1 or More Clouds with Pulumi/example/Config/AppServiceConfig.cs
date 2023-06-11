using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAl.OnetugExample.Config;

public record AppServiceConfig(string ServicePlanName, string WebAppName, string Tier, string TierName);
public class AppServiceConfigDto : ConfigDtoBase<AppServiceConfig>
{
    public string? ServicePlanName { get; set; }
    public string? WebAppName { get; set; }
    public string? Tier { get; set; }
    public string? TierName { get; set; }

    public override AppServiceConfig GenerateValidConfigObject()
    {
        if (!string.IsNullOrWhiteSpace(ServicePlanName)
            && !string.IsNullOrWhiteSpace(WebAppName) 
            && !string.IsNullOrWhiteSpace(Tier)
            && !string.IsNullOrWhiteSpace(TierName))
        {
            return new AppServiceConfig(ServicePlanName, WebAppName, Tier, TierName);
        }

        throw new Exception($"{GetType().Name} has invalid config");
    }
}
