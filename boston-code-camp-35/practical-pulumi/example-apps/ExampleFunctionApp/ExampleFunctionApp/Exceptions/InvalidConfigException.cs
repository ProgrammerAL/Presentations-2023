using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExampleFunctionApp.Exceptions;

public class InvalidConfigException : Exception
{
    public InvalidConfigException(string propertyName)
        : base("Invalid Config value for property " + propertyName)
    {
    }
}
