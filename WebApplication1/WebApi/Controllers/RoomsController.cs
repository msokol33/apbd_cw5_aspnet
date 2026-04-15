using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private static IList<Room> _rooms = new List<Room>();
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
        _rooms.Add(
            new Room(
                room.Id,
                room.Name,
                room.BuildingCode,
                room.Floor,
                room.Capacity,
                room.HasProjector,
                room.IsActive
            ));
           return Ok(room);
    }

    [HttpPut("{id}")]
    public IActionResult PutOpis(int id, [FromBody] Room room)
    {
        var roomById =  _rooms.FirstOrDefault(x => x.Id == id);
        if (roomById == null)
            return NotFound();
        
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
        
        _rooms.Remove(roomToDelete);
        return Ok();
    }
    
}