using System;

namespace PortStream.Shared.Models;

public class Vessel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string IMONumber { get; set; } = string.Empty;
    public VesselType Type { get; set; }
    public VesselStatus Status { get; set; }
    
    public double Length { get; set; } // meters
    public double Draft { get; set; }  // meters
    
    public DateTime EstimatedTimeOfArrival { get; set; }
    public DateTime? ActualTimeOfArrival { get; set; }
    
    public Guid? AssignedBerthId { get; set; }
}
