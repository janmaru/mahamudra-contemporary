using System;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary
{
    public static class CustomParallelExtensions
    { 
        public static M ToSync<M>(this Task<M> task)
        {
            return Task.Run(async () => await task.ConfigureAwait(false)).Result;
        }
        public static double ToMilliseconds (this DateTime date)
        {
            return date.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        public static double ToMilliseconds()
        {
            return  DateTime.UtcNow
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }
}
