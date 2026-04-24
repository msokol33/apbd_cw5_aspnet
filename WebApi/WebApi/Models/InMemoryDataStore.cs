namespace WebApi.Models;

public static class InMemoryDataStore
{
    public static IList<Room> Rooms { get; } = new List<Room>();
    public static IList<Reservation> Reservations { get; } = new List<Reservation>();
}

