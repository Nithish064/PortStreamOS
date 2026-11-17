using System;
using System.Collections.Generic;

namespace PortStream.Shared.Models;

public enum RiskLevel
{
    Low,
    Medium,
    High,
    Critical
}

public class RiskRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RuleName { get; set; } = string.Empty;
    public string TargetField { get; set; } = string.Empty; // e.g., "OriginCountry", "CargoType"
    public string Operator { get; set; } = string.Empty; // e.g., "Equals", "Contains"
    public string Value { get; set; } = string.Empty;
    public RiskLevel AssignedRiskLevel { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CargoItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public string HSCode { get; set; } = string.Empty; // Harmonized System Code
    public double WeightKg { get; set; }
    public string OriginCountry { get; set; } = string.Empty;
}

public class CustomsManifest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string DocumentNumber { get; set; } = string.Empty;
    public Guid VesselId { get; set; }
    public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;
    
    public List<CargoItem> CargoItems { get; set; } = new();
    
    // Calculated by RiskAssessmentService
    public RiskLevel OverallRiskScore { get; set; } = RiskLevel.Low;
    public bool RequiresManualAudit { get; set; }
}
