using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraApp.Config
{
    public abstract class ConfigDtoBase<T>
    {
        public abstract T GenerateValidConfigObject();
    }
}
