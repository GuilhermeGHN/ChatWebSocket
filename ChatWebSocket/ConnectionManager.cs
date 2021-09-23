using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebSocket
{
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, InfoUsuario> _sockets = new ConcurrentDictionary<string, InfoUsuario>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value.Socket;
        }

        public ConcurrentDictionary<string, InfoUsuario> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value.Socket == socket).Key;
        }

        public bool ApelidoExists(string apelido)
		{
            return _sockets.Any(p => p.Value.Apelido == apelido);
        }

        public void AddSocket(string apelido, WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), new InfoUsuario 
            {
                Apelido = apelido,
                Socket = socket
            });
        }

        public async Task RemoveSocket(string id)
        {
			_sockets.TryRemove(id, out InfoUsuario infoUsuario);

			await infoUsuario.Socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                statusDescription: "Closed by the ConnectionManager",
                cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public class InfoUsuario
	{
		public string Apelido { get; set; }

		public WebSocket Socket { get; set; }
	}
}
