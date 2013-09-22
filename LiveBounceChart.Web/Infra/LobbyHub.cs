using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;

namespace LiveBounceChart.Web.Infra
{
    [HubName("lobby")]
    public class LobbyHub : Hub
    {
        private static readonly ConcurrentDictionary<string, Stopwatch> Data = new ConcurrentDictionary<string, Stopwatch>();
        
        public override System.Threading.Tasks.Task OnConnected()
        {
            Data[Context.ConnectionId] = Stopwatch.StartNew();

            Clients.All.updateTotalUserCount(Data.Count);

            var onlineUserCount = Data.Count(d => d.Value.IsRunning);

            Clients.All.updateOnlineUserCount(onlineUserCount);

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Data[Context.ConnectionId].Stop();
            return base.OnDisconnected();
        }
    }
}