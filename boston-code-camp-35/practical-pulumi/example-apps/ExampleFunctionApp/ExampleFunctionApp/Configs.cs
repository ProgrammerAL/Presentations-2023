using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleFunctionApp;

//public record ServiceConfig(string Version)
//{
//    public static ServiceConfig LoadFromConfig(IServiceProvider provider)
//    {
//        var version = ConfigHelpers.LoadStringValueFromConfig(provider, nameof(ServiceConfig), nameof(Version));
//        return new ServiceConfig(version);
//    }
//}


public class ServiceConfig
{
    [Required, NotNull]
    public string? Version { get; set; }
}
