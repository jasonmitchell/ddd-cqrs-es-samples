CREATE TABLE reservation (
    id uuid primary key,
    ticket_id integer,
    quantity integer,
    requested_at timestamp,
    status varchar(25)
);

CREATE TABLE reservation_summary (
    ticket_id integer primary key,
    ticket_name varchar(150),
    total integer
);

INSERT INTO reservation_summary VALUES(1, 'Early Bird', 0);
INSERT INTO reservation_summary VALUES(2, 'Standard', 0);
INSERT INTO reservation_summary VALUES(3, 'VIP', 0);