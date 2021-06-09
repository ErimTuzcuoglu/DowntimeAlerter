using System.Security.Claims;
using System.Threading.Tasks;

namespace DowntimeAlerter.Infrastructure.Helper.Contract
{
    public interface IJwtTokenGenerator
    {
        public Task<string> Generate(Claim[] claims);
    }
}