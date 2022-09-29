using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MyAvana.Models.ViewModels;

namespace MyAvana.CRM.Api.Contract
{
    public interface IVideoService
    {
        string GetTwilioJwt(string identity);

        Task<IEnumerable<RoomDetails>> GetAllRoomsAsync();
    }
}
