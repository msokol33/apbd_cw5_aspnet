namespace WebApplication.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Status Status { get; set; }
}

public abstract class Status
{
    public static readonly string Planned =  "Planned";
    public static readonly string Confirmed =  "Confirmed";
    public static readonly string Cancelled =  "Cancelled";
}