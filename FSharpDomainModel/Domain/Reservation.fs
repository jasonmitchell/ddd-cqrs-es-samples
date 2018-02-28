module Reservation

open System

type Reservation = { 
    id: Guid; 
    ticketId: Guid; 
    quantity: int; 
    confirmed: bool
}
with 
    static member empty = { id = Guid.Empty; ticketId = Guid.Empty; quantity = 0; confirmed = false }

module Events = 
    type ReservationRequested = { id: Guid; ticketId: Guid; quantity: int; }
    type ReservationConfirmed = { id: Guid }

    type Event = 
        | ReservationRequested of ReservationRequested
        | ReservationConfirmed of ReservationConfirmed

    let private apply reservation = function
        | ReservationRequested e -> { reservation with Reservation.id = e.id; Reservation.ticketId = e.ticketId; Reservation.quantity = e.quantity }
        | ReservationConfirmed e -> { reservation with Reservation.confirmed = true }
            
    let given events = events |> Seq.fold (fun s e -> apply s e) Reservation.empty

module Commands =
    open Events

    type RequestTickets = { id: Guid; ticketId: Guid; quantity: int; }
    type ConfirmReservation = { id: Guid; }
    
    type Command = 
        | RequestTickets of RequestTickets
        | ConfirmReservation of ConfirmReservation
        
    let handle (reservation:Reservation) = function
        | RequestTickets c -> [ReservationRequested({ id = c.id; ticketId = c.ticketId; quantity = c.quantity })]
        | ConfirmReservation c -> [ReservationConfirmed({ id = reservation.id })]