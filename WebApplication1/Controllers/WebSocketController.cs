using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class WebSocketController : Controller {
        private readonly ILogger<WebSocketController> _logger;

        public WebSocketController(ILogger<WebSocketController> logger) {
            _logger = logger;
        }

        [HttpGet("/ws/")]
        public async Task ConectWebSocket() {
            try {
                if (HttpContext.WebSockets.IsWebSocketRequest) {
                    using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    await Echo(HttpContext,webSocket);

                } else {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            } catch (Exception) {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }

        private async Task Echo(HttpContext context,WebSocket webSocket) {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
            _logger.Log(LogLevel.Information,"Message received from Client");

            while (!result.CloseStatus.HasValue) {
                byte[] serverMsg = Encoding.UTF8.GetBytes($"El servidor: KP2 Prro. Tu buffer es: {Encoding.UTF8.GetString(buffer)}");
                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg,0,serverMsg.Length),result.MessageType,result.EndOfMessage,CancellationToken.None);
                _logger.Log(LogLevel.Information,"Message sent to Client");

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
                _logger.Log(LogLevel.Information,"Message received from Client");

            }
            await webSocket.CloseAsync(result.CloseStatus.Value,result.CloseStatusDescription,CancellationToken.None);
            _logger.Log(LogLevel.Information,"WebSocket connection closed");
        }
    }
}
