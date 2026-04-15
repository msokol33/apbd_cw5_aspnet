namespace WebApi.Models;

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

    public Reservation(int id, int roomId, string organizerName, string topic, DateTime date, DateTime startTime, DateTime endTime, Status status)
    {
        Id = id;
        RoomId = roomId;
        OrganizerName = organizerName;
        Topic = topic;
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
        Status = status;
    }
}


public abstract class Status
{
    public static readonly string Planned =  "planned";
    public static readonly string Confirmed =  "confirmed";
    public static readonly string Cancelled =  "cancelled";
}