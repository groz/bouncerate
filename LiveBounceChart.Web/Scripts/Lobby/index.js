/// <reference path="../jquery-2.0.3.intellisense.js" />
/// <reference path="../jquery.signalR-1.1.3.js" />
/// <reference path="../nv.d3.js" />
/// <reference path="../knockout-2.3.0.debug.js" />
/// <reference path="../d3.v3.js" />

$(function () {
    
    var viewModel = { 
        onlineUsers: ko.observable("1"),
        totalUsers: ko.observable("1"),
    };
    
    ko.applyBindings(viewModel);
    
    var chart = nv.models.lineChart();
    chart.margin({ left: 80 }); // workaround to show y-axis label

    nv.addGraph(function () {

        chart.xAxis
            .axisLabel('Time (seconds)')
            .tickFormat(d3.format('.2f'));

        chart.yAxis
            .axisLabel('Probability')
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

    hub.client.updateUserCount = function (online, total) {
        viewModel.onlineUsers(online);
        if (total) {
            viewModel.totalUsers(total);
        }
    };

    hub.client.updatePlot = function (bounceData) {
        updateChart(bounceData);
    };
    
    $.connection.hub.logging = true;
    
    $.connection.hub.error(function(err) {
        console.log(err);
    });

    console.log("SignalR error logging enabled.");
    
    $.connection.hub.start().done(function () {
        
        console.log("Connected to SignalR...");
        
        // safe to trigger server commands here or hook them to events
        hub.server.getCurrentData();
        
        function handleDisconnect() {
            try {
                hub.server.clientDisconnected();
            } catch(err) {
            }
        }

        window.onbeforeunload = function (e) {
            console.log(e);
            handleDisconnect();
            
            // uncomment the line below to enable immediate propagation on tab close in IE
            // but it has to show the alert because IE is awesome like that
            // return "Unneeded alert";
        };

    });

});