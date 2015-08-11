var Messenger = (function ($) {
    var module = {};
    var timerId;
    var commandUrl;
    var dialog = null;

    function createDialogContent(request) {

        //        var dialogContent = $("<div />").addClass("notification-dialog");
        //        var avatar = $("<img />").attr("src", request.FromThumbnailUrl).appendTo(dialogContent);
        //        var message = $("<span>" + request.ChatRequestMessage + "</span>").addClass("notification-message").appendTo(dialogContent);
        //        var linkContainer = $("<div />").addClass("notification-viewprofile").appendTo(dialogContent);
        //        var link = $("<a href='" + request.FromProfileUrl + "' target='_blank'>view user profile</a>").appendTo(linkContainer);

        var dialogContent = "<div class='notification-wrap'>" +
                        "<img align='middle' src='" + request.FromThumbnailUrl + "' />" +
                        "<span class='notification-message'>" + request.ChatRequestMessage + "</span>" +
                        "<div class='notification-viewprofile'>" +
                           "<a href='" + request.FromProfileUrl + "' target='_blank'>view user profile</a>" +
                        "</div>" +
                      "</div>";

        return $(dialogContent);
    }

    function showDialog() {
    }

    function hideDialog() {
    }

    function UpdateOnline() {
        $.getJSON(commandUrl, function (result) {
            if (result != null && dialog == null) {
                //show dialog
                dialog = $('<div />').html(createDialogContent(result).html()).dialog(
                {
                    title: "Incoming Chat Request",
                    draggable: true,
                    buttons: {
                        "Accept": function () {
                            $.getJSON(commandUrl + "&reqInitiator=" + result.FromUserId + "&reject=false");
                            $(this).dialog("destroy");
                            dialog = null;
                            window.open(result.MessengerUrl, result.FromUserId, 'width=640,height=500,resizable=0,menubar=0,status=0,toolbar=0');
                        },
                        "Reject": function () {
                            $(this).dialog("close");
                        }
                    },
                    close: function (ev, ui) {
                        $.getJSON(commandUrl + "&reqInitiator=" + result.FromUserId + "&reject=true");
                        $(this).dialog("destroy");
                        dialog = null;
                    }
                });
            }
        });
    }


    module.initialize = function (chatHomeUrl, userId, timestamp, hash, updateOnlineFrequency) {
        commandUrl = chatHomeUrl + "/MessengerCommand.ashx?id=" + userId + "&timestamp=" + timestamp + "&hash=" + hash + "&callback=?";
        timerId = setInterval(UpdateOnline, updateOnlineFrequency);
        UpdateOnline();
    }

    return module;
} (jQuery));