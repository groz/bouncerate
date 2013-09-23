/// <reference path="../jquery-2.0.3.intellisense.js" />
/// <reference path="../jquery.signalR-1.1.3.js" />
/// <reference path="../nv.d3.js" />
/// <reference path="../knockout-2.3.0.debug.js" />
/// <reference path="../d3.v3.js" />

$(function () {
    
    var onlineUserCount = $("#onlineUserCountSpan"),
        totalUserCount = $("#totalUserCountSpan");
    
    var chart = nv.models.lineChart();

    nv.addGraph(function () {

        chart.xAxis
            .axisLabel('Time (s)')
            .tickFormat(d3.format('.2f'));

        chart.yAxis
            .axisLabel('Bounce probability')
            .tickFormat(d3.format('.02f'));

        nv.utils.windowResize(function () {
            d3.select('#chart').call(chart);
        });

        return chart;
    });

    function updateChart(bounceData) {
        d3.select('#chart')
            .datum(prepareBounceData(bounceData))
            .transition().duration(500)
            .call(chart);
    }

    function prepareBounceData(bounceData) {
        console.log("prepareBounceData called...");

        var msArray = $.map(bounceData, function(obj) {
            return obj.BouncePeriod;
        });

        msArray.sort(function(a, b) { return a - b; });
        
        console.log(msArray);
        
        var total = bounceData.length;

        var chartData = [{x: 0, y: 0}];
        
        for (var i = 0; i < total; i++) {

            var point = {
                x: msArray[i],
                y: (i+1) / total
            };
            
            chartData.push(point);
        }

        console.log(chartData);

        return [{
            values: chartData,
            key: 'Bounce rate',
            color: '#5555ff'
        }];
    }

    //  WIRING UP HUB CLIENT AND SERVER METHODS
    
    var hub = $.connection.lobby;

    hub.client.updateOnlineUserCount = function (n) {
        onlineUserCount.text(n);
    };

    hub.client.updateTotalUserCount = function (n) {
        totalUserCount.text(n);
    };

    hub.client.updatePlot = function (bounceData) {
        console.log("updatePlot called from server...");
        console.log(bounceData);

        updateChart(bounceData);
        
        console.log("updatePlot successful.");
    };
    
    $.connection.hub.start().done(function () {
        
        console.log("Connected to SignalR...");
        
        // safe to trigger server commands here or hook them to events
        hub.server.getCurrentData();
    });

});