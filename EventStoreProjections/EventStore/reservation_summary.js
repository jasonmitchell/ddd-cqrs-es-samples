function getTicketType(ticketId) {
    if (ticketId === 1) {
        return 'EarlyBird';
    }

    if (ticketId === 2) {
        return 'Standard';
    }

    if (ticketId === 3) {
        return 'VIP'
    }
}

fromCategory('reservation')
.when({
    $init: function() {
        return {
            EarlyBird: 0,
            Standard: 0,
            VIP: 0
        }
    },
    ReservationRequested: function (s, e) {
        var ticketType = getTicketType(e.data.TicketId);
        s[ticketType] = s[ticketType] + e.data.Quantity;
    }
})
.outputState();