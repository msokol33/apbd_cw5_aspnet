using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private static IList<Reservation> _reservations = new List<Reservation>();
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_reservations);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var  reservation = _reservations.FirstOrDefault(x => x.Id == id);
        if (reservation == null)
            return NotFound();
            
        return Ok(reservation);
    }

    [HttpGet]
    public IActionResult Get([FromQuery] DateTime date, [FromQuery] Status status, [FromQuery] int roomId)
    {
        var  reservation = _reservations.FirstOrDefault(x => x.Date == date &&x.Status == status && x.RoomId == roomId);
        if (reservation == null)
            return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Reservation reservation)
    {
        _reservations.Add(
            new Reservation(
                reservation.Id,
                reservation.RoomId,
                reservation.OrganizerName,
                reservation.Topic,
                reservation.Date,
                reservation.StartTime,
                reservation.EndTime,
                reservation.Status)
            );
        return Ok(reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Reservation reservation)
    {
        var reservationToDelete = _reservations.FirstOrDefault(x => x.Id == id);
        if (reservationToDelete == null)
            return NotFound();
        
        _reservations.Remove(reservationToDelete);
        _reservations.Add(reservation);
        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservationToDelete = _reservations.FirstOrDefault(x => x.Id == id);
        if (reservationToDelete == null)
            return NotFound();
        _reservations.Remove(reservationToDelete);
        return Ok();
    }
}