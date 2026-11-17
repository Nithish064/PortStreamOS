using PortStream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VesselTrafficService.Repositories;

public interface IVesselRepository
{
    Task<IEnumerable<Vessel>> GetAllVesselsAsync();
    Task<Vessel?> GetVesselByIdAsync(Guid id);
    Task<Vessel> AddVesselAsync(Vessel vessel);
    Task<Vessel?> UpdateVesselAsync(Vessel vessel);
    Task<bool> DeleteVesselAsync(Guid id);
}
