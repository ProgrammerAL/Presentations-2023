using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgrammerAl.Presentations.OTel.Shared.EF;

public static class EfExtensions
{
    public static async Task<ImmutableArray<TSource>> ToImmutableArrayAsync<TSource>(
        this IQueryable<TSource> source,
        CancellationToken cancellationToken = default)
    {
        var builder = ImmutableArray.CreateBuilder<TSource>();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            builder.Add(element);
        }

        return builder.ToImmutable();
    }
}
