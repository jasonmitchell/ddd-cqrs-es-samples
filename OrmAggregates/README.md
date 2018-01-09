# Aggregate mementos saved with ORM

Mementos are saved using Entity Framework Core and reads are performed using Dapper against Postgres views

- RequestReservation => Create new aggregate and insert memento using Entity Framework
- QueryReservation => Retrieves newly created memento
- ConfirmReservation => Reconstructs aggregate from memento, calls `Confirm` on aggregate and generates updated memento
- QueryReservation => Retrieves updated state of memento
- QueryReport => Retrieves report of confirmed reservation counts