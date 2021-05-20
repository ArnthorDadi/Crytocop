using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Envelope;

namespace Cryptocop.Software.API.Services.Interfaces
{
    public interface IExchangeService
    {
        //Task<IEnumerable<ExchangeDto>> GetExchanges(int pageNumber = 1);
        Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1);
    }
}