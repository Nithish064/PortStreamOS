using System;

namespace PortStream.Shared.Models;

public class YardBin
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string BlockName { get; set; } = string.Empty; // e.g., "A", "B", "C"
    public int MaxCapacity { get; set; }
    public int CurrentCount { get; set; }
    
    // Calculated property for the heatmap
    public double UtilizationPercentage => MaxCapacity == 0 ? 0 : (double)CurrentCount / MaxCapacity * 100;
}

public class Container
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ContainerNumber { get; set; } = string.Empty;
    public bool IsReefer { get; set; }
    public double? CurrentTemperature { get; set; } // Celsius, only applicable if IsReefer is true
    public double? TargetTemperature { get; set; }
    public bool PowerConnected { get; set; }
    public Guid YardBinId { get; set; }
}

public class EquipmentTelemetry
{
    public Guid EquipmentId { get; set; }
    public string EquipmentType { get; set; } = string.Empty; // e.g., "GantryCrane", "StraddleCarrier"
    public DateTime Timestamp { get; set; }
    public double EngineTemperature { get; set; }
    public double VibrationLevel { get; set; }
    public int OperatingHours { get; set; }
}
