using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammerAl.OnetugExample.Config;

public abstract class ConfigDtoBase<T>
{
    public abstract T GenerateValidConfigObject();
}
