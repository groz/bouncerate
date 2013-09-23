using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI.WebControls.Expressions;
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
        private const int SampleSize = 100;
        private const double OutliersShare = 0.1;
        private readonly IBounceDB _ctx;

        private static volatile int _onlineUsersCount = 0;
        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, Stopwatch> CurrentCounters = new Dictionary<string, Stopwatch>();

        public LobbyHub(IBounceDB ctx)
        {
            _ctx = ctx;
        }

        private void PushDataToClients(dynamic clients)
        {
            var report = _ctx
                .RandomSample(SampleSize, OutliersShare)
                .Select(be => new
                {
                    BouncePeriod = be.BouncePeriod.TotalSeconds
                })
                .ToArray();

            clients.updatePlot(report);
        }

        public void GetCurrentData()
        {
            PushDataToClients(Clients.Caller);
        }

        public void ClientConnected()
        {
            int totalCount;

            lock (SyncRoot)
            {
                CurrentCounters[Context.ConnectionId] = Stopwatch.StartNew();
                totalCount = CurrentCounters.Count;
            }

            ++_onlineUsersCount;

            Clients.All.updateUserCount(_onlineUsersCount, totalCount);
        }

        public void ClientDisconnected()
        {
            bool found;
            Stopwatch stopwatch;

            lock (SyncRoot)
            {
                found = CurrentCounters.TryGetValue(Context.ConnectionId, out stopwatch);
            }

            if (found && stopwatch.IsRunning) // filter double disconnects
            {
                stopwatch.Stop();

                _ctx.BounceEntries.Add(new BounceEntry()
                {
                    BouncePeriod = stopwatch.Elapsed,
                    UtcExitTime = DateTime.UtcNow,
                });

                _ctx.SaveChanges();

                PushDataToClients(Clients.Others);
                --_onlineUsersCount;
            }
            
            Clients.Others.updateUserCount(_onlineUsersCount);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            ClientConnected();
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            // when connection times out/is lost and something prevented ClientDisconnected
            ClientDisconnected();
            return base.OnDisconnected();
        }
    }
}