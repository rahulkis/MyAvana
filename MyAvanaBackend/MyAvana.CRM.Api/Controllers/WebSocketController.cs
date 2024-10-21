using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using MyAvana.CRM.Api.Contract;
using System.Net.Http;

namespace MyAvana.CRM.Api.Controllers {
    
    [Route("api/[controller]")]
    //[ApiController]
    public class WebSocketController : ControllerBase
    {
        private RequestDelegate _next;
        private IWebSocket iwebsocket;
        private IBaseBusiness _baseBusiness;
        private readonly HttpClient _httpClient;
        public WebSocketController(RequestDelegate next)
        {
            _httpClient = new HttpClient();
            _next = next;
        }
        private  async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];          
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!receiveResult.CloseStatus.HasValue)
            {
                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count), receiveResult.MessageType, receiveResult.EndOfMessage, CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    // Convert the received data to a string and process it
                    string message = System.Text.Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    iwebsocket.UpdateUserLastPing(message);
                    receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

            }
            await webSocket.CloseAsync(receiveResult.CloseStatus.Value, receiveResult.CloseStatusDescription, CancellationToken.None);
        }
        [Route("/ws")]
        public async Task InvokeAsync(HttpContext context, IWebSocket _iwebsocket, IBaseBusiness baseBusiness)
        {
            iwebsocket = _iwebsocket;
            _baseBusiness = baseBusiness;           
            //if (context.Request.Path == "/ws")
            //{
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    try
                    {
                        await Echo(webSocket);
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync($"Error: {ex.Message}");
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            //}
            //else
            //{
            //await _next(context);
            //}
        }   
      
    }
}



