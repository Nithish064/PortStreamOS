using System;

namespace PortStream.Shared.Models;

public class EnvironmentReading
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SensorLocation { get; set; } = string.Empty; // e.g., "North Gate", "Berth 1"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double AirQualityIndex { get; set; }
    public double CO2Emissions { get; set; } // in ppm
    public double NoiseLevelDb { get; set; } // in Decibels
    public bool IsCompliant => AirQualityIndex < 100 && CO2Emissions < 500 && NoiseLevelDb < 85;
}

public class WorkerShift
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string WorkerName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // e.g., "Crane Operator", "Stevedore"
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool SafetyGearChecked { get; set; }
    
    // Safety check: a shift shouldn't exceed 12 hours
    public bool IsSafetyCompliant => (EndTime - StartTime).TotalHours <= 12 && SafetyGearChecked;
}
