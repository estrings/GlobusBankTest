using Customer.API.Data.UnitOfWork;
using Customer.API.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace Customer.API
{
    public static class SeedDataBaseWithStateAndLga
    {
        public static void SeedUsers(this IServiceCollection services)
        {
            var _unitOfWork = services.BuildServiceProvider().GetRequiredService<IUnitofWork>();
            var stateLgaService = new StateCityManagementService(_unitOfWork);
            stateLgaService.CreateStateAndLga().Wait(CancellationToken.None);
        }
    }
}
