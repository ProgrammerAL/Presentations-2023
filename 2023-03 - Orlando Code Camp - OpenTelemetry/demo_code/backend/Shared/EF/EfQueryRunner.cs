using Microsoft.EntityFrameworkCore;

using OpenTelemetry.Trace;

using System;
using System.Threading.Tasks;

namespace ProgrammerAl.Presentations.OTel.Shared.EF;

public class EfQueryRunner<TContext>
    where TContext : DbContext
{
    private readonly Tracer _tracer;
    private readonly IDbContextFactory<TContext> _contextFactory;

    public EfQueryRunner(IDbContextFactory<TContext> contextFactory, Tracer tracer)
    {
        _contextFactory = contextFactory;
        _tracer = tracer;
    }

    public async ValueTask<T> RunAsync<T>(string spanName, Func<TContext, TelemetrySpan, ValueTask<T>> task)
    {
        using var span = _tracer.StartActiveSpan(spanName);
        using var context = await _contextFactory.CreateDbContextAsync();

        return await task(context, span);
    }

    public async ValueTask<T> RunAsync<T>(string spanName, TContext context, Func<TContext, TelemetrySpan, ValueTask<T>> task)
    {
        using var span = _tracer.StartActiveSpan(spanName);
        return await task(context, span);
    }

    public async ValueTask RunAsync(string spanName, Func<TContext, TelemetrySpan, ValueTask> task)
    {
        using var span = _tracer.StartActiveSpan(spanName);
        using var context = await _contextFactory.CreateDbContextAsync();

        await task(context, span);
    }
}
