namespace WebApi.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BuildingCode { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjector  { get; set; }
    public bool IsActive  { get; set; }

    public Room(int id, string name, string buildingCode, int floor, int capacity, bool hasProjector, bool isActive)
    {
        Id = id;
        Name = name;
        BuildingCode = buildingCode;
        Floor = floor;
        Capacity = capacity;
        HasProjector = hasProjector;
        IsActive = isActive;
    }
}