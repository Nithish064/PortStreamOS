namespace PortStream.Shared.Models;

public enum VesselStatus
{
    Approaching,
    InChannelQueue,
    Docked,
    Departing,
    AtSea
}

public enum VesselType
{
    Cargo,
    Tanker,
    Passenger,
    RoRo,
    Other
}

public enum BerthStatus
{
    Available,
    Occupied,
    Maintenance
}
