using Mahamudra.Core.Entity;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    public interface IHttpService
    {
        Task<int> Get(BaseEntity<int> number);
    }

    public interface IHttpErrorService : IHttpService { }
}