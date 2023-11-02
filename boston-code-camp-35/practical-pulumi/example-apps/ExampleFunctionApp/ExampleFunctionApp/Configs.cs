using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleFunctionApp;

public class ServiceConfig
{
    [Required, NotNull]
    public string? Environment { get; set; }

    [Required, NotNull]
    public string? Version { get; set; }
}
