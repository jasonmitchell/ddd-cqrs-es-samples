CREATE TABLE reservation (
    id serial primary key,
    ticket_id integer,
    requested_quantity integer,
    reserved_quantity integer,
    status varchar(25),
    last_updated timestamp
);