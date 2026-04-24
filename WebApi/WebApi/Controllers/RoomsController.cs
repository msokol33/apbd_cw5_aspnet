using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private static IList<Room> _rooms = InMemoryDataStore.Rooms;
    private static IList<Reservation> _reservations = InMemoryDataStore.Reservations;
    [HttpGet]
    public IActionResult GetOpis()
    {
        return Ok(_rooms);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetOpisByRoomId(int id)
    {
        var room =  _rooms.FirstOrDefault(x => x.Id == id);
        if (room == null)
            return NotFound();
        
        return Ok(room);
    }
    
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetOpisByBuildingCode([FromRoute]string buildingCode)
    {
        var room =  _rooms.FirstOrDefault(x => x.BuildingCode == buildingCode);
        if (room == null)
            return NotFound();
        
        return Ok(room);
    }
    
    [HttpGet("filter")]
    public IActionResult GetFilteredRooms([FromQuery] int minCapacity, [FromQuery] bool hasProjector, [FromQuery] bool activeOnly)
    {
        var filteredRooms = _rooms.Where(x => x.Capacity >= minCapacity && x.HasProjector  == hasProjector && x.IsActive == activeOnly).ToList();
        if (filteredRooms.Count == 0)
            return NotFound();
        
        return Ok(filteredRooms);
    }
    
    [HttpPost]
    public IActionResult PostOpis([FromBody] Room room)
    {
        try
        {
            ValidateRoomInputs(room);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        
        var createdRoom = new Room(
                room.Id,
                room.Name,
                room.BuildingCode,
                room.Floor,
                room.Capacity,
                room.HasProjector,
                room.IsActive
            );

        _rooms.Add(createdRoom);
        return CreatedAtAction(nameof(PostOpis), new { id = createdRoom.Id }, createdRoom);
    }

    [HttpPut("{id}")]
    public IActionResult PutOpis(int id, [FromBody] Room room)
    {
        try
        {
            ValidateRoomInputs(room);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        var roomById =  _rooms.FirstOrDefault(x => x.Id == id);
        if (roomById == null)
            return NotFound();

        room.Id = id;
        
        _rooms.Remove(roomById);
        _rooms.Add(room);
        
        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteOpis(int id)
    {
        var roomToDelete = _rooms.FirstOrDefault(x => x.Id == id);
        if (roomToDelete == null)
            return NotFound();

        var hasLinkedReservations = _reservations.Any(x => x.RoomId == id);
        if (hasLinkedReservations)
            return Conflict("Cannot delete room with existing reservations.");
        
        _rooms.Remove(roomToDelete);
        return NoContent();
    }


    private void ValidateRoomInputs(Room room)
    {
        if (room == null)
            throw new ArgumentNullException(nameof(room));
        
        if(string.IsNullOrWhiteSpace(room.Name))
            throw new ArgumentException("Room name cannot be empty.");
            
        if(string.IsNullOrWhiteSpace(room.BuildingCode))
            throw new ArgumentException("Building code cannot be empty.");
     
        if(room.Capacity <= 0)
            throw new ArgumentException("Capacity cannot be less than 0.");
    }
}