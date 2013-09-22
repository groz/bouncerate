using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using LiveBounceChart.Web.DAL;
using LiveBounceChart.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;

namespace LiveBounceChart.Web.Infra
{
    [HubName("lobby")]
    public class LobbyHub : Hub
    {
        private readonly IBounceDB _ctx;
        private static readonly ConcurrentDictionary<string, Stopwatch> Data = new ConcurrentDictionary<string, Stopwatch>();

        public LobbyHub(IBounceDB ctx)
        {
            _ctx = ctx;
        }

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
            Stopwatch stopwatch;
            
            if (Data.TryGetValue(Context.ConnectionId, out stopwatch))
            {
                stopwatch.Stop();
                
                _ctx.BounceEntries.Add(new BounceEntry()
                {
                    BouncePeriod = stopwatch.Elapsed,
                    UtcExitTime = DateTime.UtcNow,
                });
            }

            return base.OnDisconnected();
        }
    }
}