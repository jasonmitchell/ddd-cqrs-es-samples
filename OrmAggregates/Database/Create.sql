CREATE TABLE reservation_memento (
    id serial primary key,
    ticket_id integer,
    requested_quantity integer,
    reserved_quantity integer,
    status varchar(25),
    last_updated timestamp
);

CREATE TABLE ticket (
    id integer primary key,
    name varchar(150)
);

create view reservation as
select
  reservation_memento.id,
  ticket.name,
  reserved_quantity,
  status
from reservation_memento
join ticket on ticket.id = reservation_memento.ticket_id;

create view confirmed_reservation as
select
  ticket.name,
  sum(reserved_quantity) as total_reservations
from reservation_memento
join ticket on ticket.id = reservation_memento.ticket_id
where status = 'Confirmed'
group by ticket.name;

INSERT INTO ticket VALUES(1, 'Early Bird');
INSERT INTO ticket VALUES(2, 'Standard');
INSERT INTO ticket VALUES(3, 'VIP');