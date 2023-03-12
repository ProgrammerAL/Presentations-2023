using OpenTelemetry.Trace;

using System;
using System.Text.RegularExpressions;

namespace ProgrammerAl.Presentations.OTel.Shared;

public static partial class DatabaseOpenTelemetryHelpers
{
    public static void TraceSqlServerExecutedQueryInfo(string message)
    {
        _ = Tracer.CurrentSpan.SetAttribute("db.statement", message);
    }

    public static void TraceCosmosDbExecutedQueryInfo(string message)
    {
        _ = Tracer.CurrentSpan.SetAttribute("db.statement", message);

        //Try to parse out the Cosmos DB RU count, and also how long the query took
        //Ex: Executed ReadNext (1610.9455 ms, 3.02 RU) ActivityId='c62fafbd-de9e-4aeb-bbc7-36bae5c4d3fd', Container='ActiveGameTickets', Partition='?', Parameters=[]
        var ruMatch = CosmosDbResourceUnitStringRegex().Match(message);
        if (ruMatch.Success)
        {
            var startIndex = 0;
            var length = ruMatch.Value.Length - 3;
            var ruString = ruMatch.Value.Substring(startIndex, length);
            if (double.TryParse(ruString, out double ruValue))
            {
                _ = Tracer.CurrentSpan.SetAttribute("db.cosmosdb.ru", ruValue);
            }
            else
            {
                _ = Tracer.CurrentSpan.SetAttribute("db.cosmosdb.ru", ruString);
            }
        }

        var queryLengthMatch = QueryLengthRegex().Match(message);
        if (queryLengthMatch.Success)
        {
            var startIndex = 1;
            var length = queryLengthMatch.Value.Length - 4;
            var queryLengthString = queryLengthMatch.Value.Substring(startIndex, length);
            if (double.TryParse(queryLengthString, out double querylength))
            {
                _ = Tracer.CurrentSpan.SetAttribute("db.cosmosdb.time-in-ms", querylength);
            }
            else
            {
                _ = Tracer.CurrentSpan.SetAttribute("db.cosmosdb.time-in-ms", queryLengthString);
            }
        }
    }

    [GeneratedRegex("\\([0-9\\.]+ ms,")]
    private static partial Regex QueryLengthRegex();
    [GeneratedRegex("[0-9\\.]+ RU")]
    private static partial Regex CosmosDbResourceUnitStringRegex();
}