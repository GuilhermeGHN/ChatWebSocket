using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebSocket.Handlers
{
    public class ChatMessageHandler : WebSocketHandler
    {
        public ChatMessageHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override async Task OnConnected(string apelido, WebSocket socket)
        {
            await base.OnConnected(apelido, socket);

            var socketId = WebSocketConnectionManager.GetId(socket);

            var userConnectMessage = new Message
            {
                SenderId = socketId,
                Type = "UserConnect",
                Value = socketId
            };

            await SendMessageAsync(socketId, JsonConvert.SerializeObject(userConnectMessage));

            var otherUserConnectMessage = new Message
            {
                SenderId = socketId,
                Type = "OtherUserConnect",
                Value = "Usuário X entrou!"
            };

            await SendMessageToAllAsync(JsonConvert.SerializeObject(otherUserConnectMessage));
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);

            var message = new Message
            {
                SenderId = socketId,
                Type = "Message",
                Value = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}"
            };

            await SendMessageToAllAsync(JsonConvert.SerializeObject(message));
        }
    }

    public class Message
	{
		public string SenderId { get; set; }

		public string Type { get; set; }

		public string Value { get; set; }
	}
}
