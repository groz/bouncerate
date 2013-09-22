/// <reference path="../jquery-2.0.3.intellisense.js" />
/// <reference path="../jquery.signalR-1.1.3.js" />

$(function () {

    var onlineUserCount = $("#onlineUserCountSpan"),
        totalUserCount = $("#totalUserCountSpan");

    var hub = $.connection.lobby;

    hub.client.updateOnlineUserCount = function(n) {
        onlineUserCount.text(n);
    };
    
    hub.client.updateTotalUserCount = function (n) {
        totalUserCount.text(n);
    };
    
    $.connection.hub.start().done(function() {
        
        // safe to trigger server commands here or hook them to events

    });

});