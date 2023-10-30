using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExampleFunctionApp.Exceptions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleFunctionApp;

public static class ConfigHelpers
{
    public static string LoadStringValueFromConfig(IServiceProvider provider, string propertyName)
    {
        IConfiguration requiredService = provider.GetRequiredService<IConfiguration>();
        return LoadStringValueFromConfig(requiredService, propertyName);
    }

    public static string LoadStringValueFromConfig(IConfiguration config, string propertyName)
    {
        string text = config[propertyName];
        return text ?? throw new InvalidConfigException(propertyName);
    }

    public static string LoadStringValueFromConfig(IServiceProvider provider, string sectionName, string propertyName)
    {
        IConfiguration requiredService = provider.GetRequiredService<IConfiguration>();
        return LoadStringValueFromConfig(requiredService, sectionName, propertyName);
    }

    public static string LoadStringValueFromConfig(IConfiguration config, string sectionName, string propertyName)
    {
        string text = sectionName + ":" + propertyName;
        string text2 = config[text];
        return text2 ?? throw new InvalidConfigException(text);
    }

    public static TimeSpan LoadTimeSpanValueFromConfig(IServiceProvider provider, string sectionName, string propertyName)
    {
        string s = LoadStringValueFromConfig(provider, sectionName, propertyName);
        return TimeSpan.Parse(s);
    }

    public static TimeSpan LoadTimeSpanValueFromConfig(IConfiguration config, string sectionName, string propertyName)
    {
        string s = LoadStringValueFromConfig(config, sectionName, propertyName);
        return TimeSpan.Parse(s);
    }

    public static int LoadNonZeroIntValueFromConfig(IServiceProvider provider, string sectionName, string propertyName)
    {
        IConfiguration requiredService = provider.GetRequiredService<IConfiguration>();
        return LoadNonZeroIntValueFromConfig(requiredService, sectionName, propertyName);
    }

    public static int LoadNonZeroIntValueFromConfig(IConfiguration config, string sectionName, string propertyName)
    {
        string text = sectionName + ":" + propertyName;
        string s = config[text];
        if (!int.TryParse(s, out var result) || result == 0)
        {
            throw new InvalidConfigException(text);
        }

        return result;
    }

    public static bool LoadBoolFromConfig(IServiceProvider provider, string sectionName, string propertyName)
    {
        IConfiguration requiredService = provider.GetRequiredService<IConfiguration>();
        return LoadBoolFromConfig(requiredService, sectionName, propertyName);
    }

    public static bool LoadBoolFromConfig(IConfiguration config, string sectionName, string propertyName)
    {
        string text = sectionName + ":" + propertyName;
        string value = config[text];
        if (!bool.TryParse(value, out var result))
        {
            throw new InvalidConfigException(text);
        }

        return result;
    }
}