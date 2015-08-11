var updateOnlineTimer;
var updateOnlineLock = false;
var currentNotificationDivOffset = 0;
var scrollSpeed = 1;
var scrollSign = +1;
var scrollStarted = false;

function InitializeOnlineCheck(interval)
{
    updateOnlineTimer = setInterval("UpdateOnline()", interval);
    UpdateOnline();
}

function ShowNotification() {
    $("#divNotificationInner").scrollTop(0);
    $('#divNotificationClose').css('visibility', 'visible');
    $('#divNotificationInner').css('visibility', 'visible');
    $('#divNotification').css('visibility', 'visible');
}

function HideNotification()
{
    $('#divNotificationInner').html('');
    $('#divNotificationClose').css('visibility', 'hidden');
    $('#divNotificationInner').css('visibility', 'hidden');
    $('#divNotification').css('visibility', 'hidden');
}

function UpdateOnline()
{
    if (updateOnlineLock == true) return;
    updateOnlineLock = true;
    
    AspNetDating.Services.OnlineCheck.UpdateOnline(OnUpdateOnlineSucceeded, OnUpdateOnlineError);
}

function OnUpdateOnlineSucceeded(result)
{
    try
    {
        if (result == null) return;

        if (result.Notifications != null && result.Notifications.length)
        {
            for (var i = 0; i < result.Notifications.length; i++)
            {
                var notification = result.Notifications[i];

                var eventHtml = 
                "<div class='NotificationInactive' onmouseover='OnNotificationMouseOver(this);' onmouseout='OnNotificationMouseOut(this);'" + " onclick=\"window.location.href = '" + notification.RedirectUrl + "'\">" +
                    "<div class='media'>" +
                        "<img class='pull-left img-thumbnail media-object' src='" + notification.ThumbnailUrl + "' />" +
                        "<div class='media-body'>" + notification.Text + "</div>" +
                    "</div>" +
                "</div>";

                $('#divNotificationInner').append(eventHtml);
                ShowNotification();
            }

            if (!scrollStarted)
            {
                scrollStarted = true;
                ScrollNotificationDiv();
            }
        }
    }
    finally
    {
        updateOnlineLock = false;
    }
}

function OnNotificationMouseOver(element)
{
    element.className = 'NotificationHover';
}

function OnNotificationMouseOut(element)
{
    element.className = 'NotificationInactive';
}

function OnUpdateOnlineError()
{

}

function ScrollNotificationDiv()
{
    var div = document.getElementById("divNotificationInner");
    var timerDelay = 1000;
    if (div.scrollHeight > 40)
    {
        currentNotificationDivOffset = currentNotificationDivOffset + (scrollSpeed * scrollSign);
        div.scrollTop = currentNotificationDivOffset;
        if (scrollSign == 1 && currentNotificationDivOffset > div.scrollHeight - 40)
        {
            scrollSign = -1;
            timerDelay = 1000;
        }
        else if (scrollSign == -1 && currentNotificationDivOffset <= 0)
        {
            scrollSign = +1;
            timerDelay = 1000;
        }
        else
        {
            timerDelay = (currentNotificationDivOffset % 40 == 0) ? 1000 : 50;
        }
    }
    setTimeout("ScrollNotificationDiv()", timerDelay);
}