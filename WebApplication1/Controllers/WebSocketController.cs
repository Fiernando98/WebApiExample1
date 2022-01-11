using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using WebApplication1.Services;

namespace WebApplication1.Controllers {
    [Route("api/[controller]/")]
    [ApiController]
    public class WebSocketController : Controller {
        [HttpGet]
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
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
            while (!result.CloseStatus.HasValue) {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer,0,result.Count),result.MessageType,result.EndOfMessage,CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value,result.CloseStatusDescription,CancellationToken.None);
        }
    }
}
