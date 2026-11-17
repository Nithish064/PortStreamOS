using System;

namespace PortStream.Shared.Models;

public class Berth
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public double MaxVesselLength { get; set; } // meters
    public double MaxDraft { get; set; } // meters
    public BerthStatus Status { get; set; } = BerthStatus.Available;
    
    public Guid? CurrentVesselId { get; set; }
}
