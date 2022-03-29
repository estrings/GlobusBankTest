using Customer.API.Data.Repository;
using Customer.API.Data.UnitOfWork;
using Customer.API.Service.Implementation;
using Customer.API.Service.Implementation.Mock;
using Customer.API.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.API
{
    public partial class Startup
    {
        public IServiceCollection ConfigureDIServices(IServiceCollection services)
        {
            services.AddTransient<IUnitofWork, UnitofWork>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IOTPService, OTPService>();
            services.AddTransient<ISMSService, MockSMSService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<ILgaService, LgaService>();
            return services;
        }
    }
}
