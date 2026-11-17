using System;

namespace PortStream.Shared.Models;

public enum EventType
{
    Entry,
    Exit
}

public class GateEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TruckLicensePlate { get; set; } = string.Empty;
    public string ContainerNumber { get; set; } = string.Empty;
    public EventType Type { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string GateId { get; set; } = string.Empty; // e.g., "North-Gate-1"
}

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string InvoiceNumber { get; set; } = string.Empty;
    public string TruckLicensePlate { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public double TotalAmount { get; set; } // Tariff amount in USD
    public bool IsPaid { get; set; } = false;
    public string Reason { get; set; } = string.Empty; // e.g., "Port Stay Duration: 5 Hours"
}
