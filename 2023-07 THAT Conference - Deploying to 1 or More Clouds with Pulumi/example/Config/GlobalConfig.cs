using Pulumi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAl.OnetugExample.Config;

public record GlobalConfig(
    string Location,
    string ResourceGroupName,
    string Environment,
    string StorageAccountName);
