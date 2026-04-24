using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;
[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private static IList<Reservation> _reservations = InMemoryDataStore.Reservations;
    private static IList<Room> _rooms = InMemoryDataStore.Rooms;
    
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
        try
        {
            ValidateReservationInputs(reservation);
            ValidateReservationBusinessRules(reservation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        
        var createdReservation = new Reservation(
                reservation.Id,
                reservation.RoomId,
                reservation.OrganizerName,
                reservation.Topic,
                reservation.Date,
                reservation.StartTime,
                reservation.EndTime,
                reservation.Status);

        _reservations.Add(createdReservation);
        return CreatedAtAction(nameof(Post), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Reservation reservation)
    {
        try
        {
            ValidateReservationInputs(reservation);
            ValidateReservationBusinessRules(reservation, id);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        
        var reservationToDelete = _reservations.FirstOrDefault(x => x.Id == id);
        if (reservationToDelete == null)
            return NotFound();

        reservation.Id = id;
        
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
        return NoContent();
    }

    private void ValidateReservationInputs(Reservation reservation)
    {
        if(reservation == null)
            throw new ArgumentNullException(nameof(reservation));
        
        if(string.IsNullOrWhiteSpace(reservation.OrganizerName))
            throw new ArgumentException("Organizer name cannot be empty");
        
        if(string.IsNullOrWhiteSpace(reservation.Topic))
            throw new ArgumentException("Topic cannot be empty");
        
        if(reservation.EndTime <= reservation.StartTime)
            throw new ArgumentException("Endtime must be after starttime.");
    }

    private void ValidateReservationBusinessRules(Reservation reservation, int? reservationIdToSkip = null)
    {
        var room = _rooms.FirstOrDefault(x => x.Id == reservation.RoomId);
        if (room == null)
            throw new ArgumentException("Cannot create reservation for a room that does not exist.");

        if (!room.IsActive)
            throw new ArgumentException("Cannot create reservation for an inactive room.");

        var overlappingReservationExists = _reservations.Any(x =>
            (!reservationIdToSkip.HasValue || x.Id != reservationIdToSkip.Value)
            && x.RoomId == reservation.RoomId
            && x.Date.Date == reservation.Date.Date
            && x.StartTime < reservation.EndTime
            && reservation.StartTime < x.EndTime);

        if (overlappingReservationExists)
            throw new InvalidOperationException("Reservation time overlaps with another reservation for this room.");
    }
    
}