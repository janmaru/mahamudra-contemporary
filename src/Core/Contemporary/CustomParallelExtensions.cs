using System.Threading.Tasks;

namespace Mahamudra.Contemporary
{
    public static class CustomParallelExtensions
    { 
        public static M ToSync<M>(this Task<M> task)
        {
            return Task.Run(async () => await task.ConfigureAwait(false)).Result;
        }
    }
}
