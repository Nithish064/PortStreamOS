using PortStream.Shared.Models;

namespace CustomsRiskService.Services;

public class RiskAssessmentService
{
    private readonly ILogger<RiskAssessmentService> _logger;

    public RiskAssessmentService(ILogger<RiskAssessmentService> logger)
    {
        _logger = logger;
    }

    public CustomsManifest EvaluateManifest(CustomsManifest manifest, List<RiskRule> activeRules)
    {
        _logger.LogInformation("Starting risk evaluation for Manifest {DocumentNumber}", manifest.DocumentNumber);
        
        var highestRisk = RiskLevel.Low;

        foreach (var cargo in manifest.CargoItems)
        {
            foreach (var rule in activeRules)
            {
                if (RuleAppliesToCargo(rule, cargo))
                {
                    _logger.LogWarning("Manifest {Doc} triggered Rule '{RuleName}'. Escalating Risk to {Level}.", manifest.DocumentNumber, rule.RuleName, rule.AssignedRiskLevel);
                    if (rule.AssignedRiskLevel > highestRisk)
                    {
                        highestRisk = rule.AssignedRiskLevel;
                    }
                }
            }
        }

        manifest.OverallRiskScore = highestRisk;
        manifest.RequiresManualAudit = highestRisk == RiskLevel.High || highestRisk == RiskLevel.Critical;

        return manifest;
    }

    private bool RuleAppliesToCargo(RiskRule rule, CargoItem cargo)
    {
        // Simplistic rule engine implementation
        if (rule.TargetField == "OriginCountry" && rule.Operator == "Equals")
        {
            return cargo.OriginCountry.Equals(rule.Value, StringComparison.OrdinalIgnoreCase);
        }
        
        if (rule.TargetField == "WeightKg" && rule.Operator == "GreaterThan")
        {
            if (double.TryParse(rule.Value, out var threshold))
            {
                return cargo.WeightKg > threshold;
            }
        }

        return false;
    }
}
