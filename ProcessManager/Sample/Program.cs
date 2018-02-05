using System;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace Sample
{
    class Program
    {
        private static IEventStoreConnection _connection;
        
        static void Main(string[] args)
        {
            Console.ReadLine();
            
            _connection?.Dispose();
        }

        private static IEventStoreConnection GetEventStoreConnection()
        {
            return _connection ?? (_connection = CreateEventStoreConnection().ConfigureAwait(false).GetAwaiter().GetResult());
        }
        
        private static async Task<IEventStoreConnection> CreateEventStoreConnection()
        {
            var connectionSettings = ConnectionSettings.Create()
                .KeepRetrying()
                .KeepReconnecting();

            var connection = EventStoreConnection.Create(connectionSettings,
                new IPEndPoint(IPAddress.Loopback, 1123), 
                "Process Manager Sample");

            await connection.ConnectAsync().ConfigureAwait(false);

            return connection;
        }
    }
}
