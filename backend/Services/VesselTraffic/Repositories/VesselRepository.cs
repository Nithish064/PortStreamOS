using Microsoft.EntityFrameworkCore;
using PortStream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VesselTrafficService.Data;

namespace VesselTrafficService.Repositories;

public class VesselRepository : IVesselRepository
{
    private readonly VesselTrafficDbContext _context;

    public VesselRepository(VesselTrafficDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vessel>> GetAllVesselsAsync()
    {
        return await _context.Vessels.ToListAsync();
    }

    public async Task<Vessel?> GetVesselByIdAsync(Guid id)
    {
        return await _context.Vessels.FindAsync(id);
    }

    public async Task<Vessel> AddVesselAsync(Vessel vessel)
    {
        _context.Vessels.Add(vessel);
        await _context.SaveChangesAsync();
        return vessel;
    }

    public async Task<Vessel?> UpdateVesselAsync(Vessel vessel)
    {
        var existing = await _context.Vessels.FindAsync(vessel.Id);
        if (existing == null) return null;

        _context.Entry(existing).CurrentValues.SetValues(vessel);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteVesselAsync(Guid id)
    {
        var vessel = await _context.Vessels.FindAsync(id);
        if (vessel == null) return false;

        _context.Vessels.Remove(vessel);
        await _context.SaveChangesAsync();
        return true;
    }
}
