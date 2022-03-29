using Customer.API.Data.Entities;
using Customer.API.Data.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    //public interface IStateCityManagementService
    //{
    //    Task CreateStateAndLga();
    //}

    public class StateCityManagementService
    {
        private readonly IUnitofWork _unitOfWork;
        public StateCityManagementService(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateStateAndLga()
        {            
            var result = await _unitOfWork.Repository<State>().FindAsync(s => s.Name == "Lagos");
            if(result == null)
            {
                var state = new State()
                {
                    Name = "Lagos"
                };

                var response = await _unitOfWork.Repository<State>().AddAsync(state);
                if(response != null)
                {
                    await CreateCities(response.Id);
                }
            }
        }

        public async Task CreateCities(long Id)
        {
            var lgas = new List<LGA>()
            {
                new LGA
                {
                    Name = "Agege",
                    StateId = Id
                },
                new LGA
                {
                    Name = "Lagos Island",
                    StateId = Id
                },
            };

            foreach(var lga in lgas)
            {
                await _unitOfWork.Repository<LGA>().AddAsync(lga);
            }
        }
    }
}
