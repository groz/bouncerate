using System;
using System.Collections.Concurrent;
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
        private const double OutliersPercentage = 0.1;
        private readonly IBounceDB _ctx;
        private static readonly ConcurrentDictionary<string, Stopwatch> CurrentCounters = new ConcurrentDictionary<string, Stopwatch>();

        public LobbyHub(IBounceDB ctx)
        {
            _ctx = ctx;
        }

        public class JsResult
        {
            public double BouncePeriod { get; set; }
        }

        public JsResult[] PrepareData()
        {
            return _ctx.RandomSample(SampleSize, OutliersPercentage)
                .Select(be => new JsResult()
                {
                    BouncePeriod = be.BouncePeriod.TotalSeconds
                })
                .ToArray();
        }

        public void GetCurrentData()
        {
            var report = PrepareData();
            Clients.Caller.updatePlot(report);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            CurrentCounters[Context.ConnectionId] = Stopwatch.StartNew();

            Clients.All.updateTotalUserCount(CurrentCounters.Count);

            var onlineUserCount = CurrentCounters.Count(d => d.Value.IsRunning);

            Clients.All.updateOnlineUserCount(onlineUserCount);

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Stopwatch stopwatch;
            
            if (CurrentCounters.TryGetValue(Context.ConnectionId, out stopwatch))
            {
                stopwatch.Stop();
                
                _ctx.BounceEntries.Add(new BounceEntry()
                {
                    BouncePeriod = stopwatch.Elapsed,
                    UtcExitTime = DateTime.UtcNow,
                });

                _ctx.SaveChanges();

                var report = PrepareData();
                Clients.Others.updatePlot(report);
            }

            return base.OnDisconnected();
        }
    }
}