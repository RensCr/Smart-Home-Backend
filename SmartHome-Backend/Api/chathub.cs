using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Pipes;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartHome_Backend.Api
{
    public class ChatHub : Hub
    {
        private int GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");

            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public async Task SendToGroup(string token, string targetIdString, string message)
        {
            int userId = GetUserIdFromToken(token);
            if(targetIdString == "manager")
            {
                string groupIdentifier = $"manager-{userId}";

                await Clients.Group(groupIdentifier).SendAsync("ReceiveMessage", userId, message);
            }
            else
            {
                string groupIdentifier = $"manager-{targetIdString}";

                await Clients.Group(groupIdentifier).SendAsync("ReceiveMessage", userId, message);
            }
        }

        public async Task AddToGroup(string token, string targetIdString)
        {
            int userId = GetUserIdFromToken(token);

            if (targetIdString == "manager")
            {
                string groupIdentifier = $"manager-{userId}";

                await Groups.AddToGroupAsync(Context.ConnectionId, groupIdentifier);
            }
            else
            {
                string groupIdentifier = $"manager-{targetIdString}";

                await Groups.AddToGroupAsync(Context.ConnectionId, groupIdentifier);
            }
        }
        public async Task ConnectUser(string username, string userId)
        { 
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await Clients.All.SendAsync("SendToGroup", new { username, userId });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            await Clients.All.SendAsync("UserDisconnected", userId);
            await base.OnDisconnectedAsync(exception);
        }

    }
}