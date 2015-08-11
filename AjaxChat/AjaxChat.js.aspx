<%@ Page Theme="" %>
<%@ Import namespace="AspNetDating.Classes"%>

var lastMessageId = null;
var fetchNewEventsLock = false;
var fetchOnlineUsersLock = false;
var updateOnlineUsersFlag = true;
var userAvatars = new Array();
var userProfileUrls = new Array();
var chatSession = null;
var initialEventsLoaded = false;
var onlineUsers = null;
var messagesTimestamps = new Array();
var chatRoomId = 0;
var chatRoom = null;

function disableInterface()
{
    document.getElementById("left").style.backgroundColor = "#cccccc";
    document.getElementById("right").style.backgroundColor = "#cccccc";
    document.getElementById("txtSendMessage").disabled = true;
    document.getElementById("btnSendMessage").disabled = true;
}

function OnFetchNewEventsSucceeded(result)
{
    try
    {
        if (result == null || result.Messages == null || result.Messages.length == 0) return;

        var objDiv = document.getElementById("messages");

        for (i = 0; i < result.Messages.length; i++)
        {
            if (result.Messages[i].SenderUserId != chatSession.ChatUserInstance.Id 
                || !initialEventsLoaded
                || result.Messages[i].MessageType == 2 || result.Messages[i].MessageType == 3
                || result.Messages[i].MessageType == 4 || result.Messages[i].MessageType == 5)
            {
                displayMessage(result.Messages[i]);
            }
            
            if ((result.Messages[i].MessageType == 4 || result.Messages[i].MessageType == 5)
                && result.Messages[i].TargetUserId == chatSession.ChatUserInstance.Id
                && initialEventsLoaded)
            {
                CloseChat();                        
            }
            
            if (result.Messages[i].Id > lastMessageId)
                lastMessageId = result.Messages[i].Id;
        }
        
        objDiv.scrollTop = objDiv.scrollHeight;
        objDiv.scrollTop = objDiv.scrollHeight;
    }
    finally
    {
        fetchNewEventsLock = false;
        initialEventsLoaded = true;
    }
}

function displayMessage(message)
{
    var objDiv = document.getElementById("messages");

    if (message.MessageType == 2 || message.MessageType == 3 || message.MessageType == 4 
        || message.MessageType == 5)
    {
        objDiv.innerHTML += 
            "<div class=\"srv_msg2\">" + message.TextHtml + "</div>";
        updateOnlineUsersFlag = true;    
        
        /*
        if (onlineUsers != null && initialEventsLoaded)
        {
            if (message.MessageType == 2)
            {
                updateOnlineUsersFlag = true;
            }
            if (message.MessageType == 3)
            {
                for (i = 0; i < onlineUsers.length; i++)
                {
                    if (message.SenderUserId == onlineUsers[i].Id)
                    {
                        onlineUsers.splice(i, 1);
                        RenderOnlineUsersList();
                        break;
                    }
                }
            }
        }
        */
    }
    else
    {
        var avatar = "images/avatar.jpg";
        if (userAvatars[message.SenderUserId] != null)
            avatar = userAvatars[message.SenderUserId];
        var profileUrl = "javascript: return void();";
        if (userProfileUrls[message.SenderUserId] != null)
        		profileUrl = userProfileUrls[message.SenderUserId];

        if (message.SenderUserId == chatSession.ChatUserInstance.Id)
        {
            objDiv.innerHTML += 
                "<div class=\"msgwrap\"><div class=\"useravatar\"><a href=\"" + profileUrl + "\" target=\"_blank\"><img src=\"" + avatar + "\" border=\"0\" /></a></div>" + 
                "<div class=\"user_id2\">" + message.SenderDisplayName + ":</div>" + 
                "<div class=\"user_msg\">" + message.TextHtml + "</div></div>";
        }
        else
        {
            objDiv.innerHTML += 
                "<div class=\"msgwrap\"><div class=\"useravatar\"><img src=\"" + avatar + "\" /></div>" + 
                "<div class=\"user_id1\">" + message.SenderDisplayName + ":</div>" + 
                "<div class=\"user_msg\">" + message.TextHtml + "</div></div>";
        }
    }
    
    objDiv.innerHTML += "<SEP>";
    
    while (objDiv.innerHTML.length > 20000 && objDiv.innerHTML.indexOf("<SEP>") != -1)
    {
    		objDiv.innerHTML = objDiv.innerHTML.substring(objDiv.innerHTML.indexOf("<SEP>") + 5);
    }
}

function FetchNewEvents()
{
    if (fetchNewEventsLock == true) return;

    fetchNewEventsLock = true;
    AjaxChat.ChatEngine.FetchNewEvents(chatRoomId, lastMessageId, OnFetchNewEventsSucceeded);
}

function OnFetchOnlineUsersSucceeded(result)
{
    try
    {
        if (result == null || result.OnlineUsers == null || result.OnlineUsers.length == 0) return;
        
        onlineUsers = result.OnlineUsers;

        RenderOnlineUsersList();
    }
    finally
    {
        fetchOnlineUsersLock = false;
    }
}

function RenderOnlineUsersList()
{
    document.getElementById("usersonline").innerHTML = onlineUsers.length + "<%= Lang.Trans(" users online") %>";
    
    var objDiv = document.getElementById("usersright");
    var htmlList = "";

    for (i = 0; i < onlineUsers.length; i++)
    {
        if (userAvatars[onlineUsers[i].Id] == null)
        {
            if (onlineUsers[i].ThumbImage == null)
                userAvatars[onlineUsers[i].Id] = "images/avatar.jpg";
            else
                userAvatars[onlineUsers[i].Id] = onlineUsers[i].ThumbImage;
        }
        
        if (userProfileUrls[onlineUsers[i].Id] == null)
        {
            if (onlineUsers[i].ProfileUrl == null)
                userProfileUrls[onlineUsers[i].Id] = "javascript: return void();";
            else
                userProfileUrls[onlineUsers[i].Id] = onlineUsers[i].ProfileUrl;
        }
    
        htmlList += "<div class=\"msgwrap\"><div class=\"useravatar\"><a href=\"" + userProfileUrls[onlineUsers[i].Id] + "\" target=\"_blank\"><img src=\"" + 
        		userAvatars[onlineUsers[i].Id] + "\" border=\"0\" />" + "</a></div><div class=\"user_id1\"><a href=\"" + userProfileUrls[onlineUsers[i].Id] + "\" class=\"\" target=\"_blank\">" + onlineUsers[i].DisplayName + "</a></div></div>";
    }
    
    objDiv.innerHTML = htmlList;
}

function FetchOnlineUsers()
{
    if (fetchOnlineUsersLock == true || !updateOnlineUsersFlag) return;

    fetchOnlineUsersLock = true;
    AjaxChat.ChatEngine.GetOnlineUsers(chatRoomId, OnFetchOnlineUsersSucceeded);
    
    updateOnlineUsersFlag = false;
}

var timersStarted = false;
var newEventsTimer;
var onlineUsersTimer;
function StartTimers()
{
    if (!timersStarted)
    {
        newEventsTimer = setInterval("FetchNewEvents()", 5000);
        onlineUsersTimer = setInterval("FetchOnlineUsers()", 10000);
        timersStarted = true;

        updateOnlineUsersFlag = true;
        FetchOnlineUsers();
    }
}

function StopTimers()
{
    clearInterval(newEventsTimer);
    clearInterval(onlineUsersTimer);
}

function SendMessage()
{
    var objDiv = document.getElementById("txtSendMessage");
    if (objDiv.value == "") return;

    var now = new Date();
    if (messagesTimestamps.length >= 5)
    {
        var seconds = (now.getTime() - messagesTimestamps[0].getTime()) / 1000;
        if (seconds <= 10)
        {
           	alert("<%= Lang.Trans("You cannot send so many message in such a short time. Please wait a few seconds and try again.") %>");
            return;
        }
        messagesTimestamps.splice(0, 1);
    }
    messagesTimestamps.push(now);

    AjaxChat.ChatEngine.SendMessage(chatRoomId, null, objDiv.value);
    var messageHtml = objDiv.value;
    objDiv.value = "";
    
    objDiv = document.getElementById("messages");    

    messageHtml = htmlEncode(messageHtml);
    messageHtml = parseSmilies(messageHtml);

    var avatar = "images/avatar.jpg";
    if (userAvatars[chatSession.ChatUserInstance.Id] != null)
        avatar = userAvatars[chatSession.ChatUserInstance.Id];
    objDiv.innerHTML += 
        "<div class=\"msgwrap\"><div class=\"useravatar\"><img src=\"" + avatar + "\" /></div>" + 
        "<div class=\"user_id2\">" + chatSession.ChatUserInstance.DisplayName + ":</div>" + 
        "<div class=\"user_msg\">" + messageHtml + "</div></div>";
        
    objDiv.innerHTML += "<SEP>";
    
    while (objDiv.innerHTML.length > 20000 && objDiv.innerHTML.indexOf("<SEP>") != -1)
    {
    		objDiv.innerHTML = objDiv.innerHTML.substring(objDiv.innerHTML.indexOf("<SEP>") + 5);
    }

    objDiv.scrollTop = objDiv.scrollHeight;
    objDiv.scrollTop = objDiv.scrollHeight;
}

function htmlEncode(text) 
{
    var ret = text.replace(/&/,"&amp;");
    ret = ret.replace(/</,"&lt;");
    ret = ret.replace(/>/,"&gt;");
    ret = ret.replace(/\r\n/,"<br>");
    ret = ret.replace(/\n/,"<br>");
    ret = ret.replace(/\r/,"<br>");
    return(ret);
}

function RedirectEnterKey(e) 
{
    if(window.event) // IE
    {
        keynum = e.keyCode
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }

    if (keynum == 13)
    {
        SendMessage();
        return false;
    }
    
    return true;
}

function OnFetchChatRoomSucceeded(result)
{
    if (result == null) return;
    
    chatRoom = result;
    
    var objDiv = document.getElementById("righthead");
    objDiv.innerHTML = chatRoom.Name;
}

function OnJoinChatRoomSucceeded(result)
{
    if (result == null) return;
    
    chatSession = result;
    
    if (result.Authorized)
    {
        var objDiv = document.getElementById("messages");
        objDiv.innerHTML += "<div class=\"srv_msg1\"><%= Lang.Trans("Connected!") %></div>";

        StartTimers();
        FetchNewEvents();
        AjaxChat.ChatEngine.FetchChatRoom(chatRoomId, OnFetchChatRoomSucceeded);
    }
    else
    {
        if (result.Banned)
        {
            alert("<%= Lang.Trans("You are banned!") %>");
            disableInterface();
        }
        else if (result.AuthorizeUrl != null)
        {
            window.location.href = result.AuthorizeUrl;
        }
        else
        {
            alert("<%= Lang.Trans("You must log in before in order to use the chat!") %>");
            disableInterface();
        }
    }
}

function JoinChatRoom()
{
    var objDiv = document.getElementById("messages");
    objDiv.innerHTML += "<div class=\"srv_msg1\"><%= Lang.Trans("Please wait...") %></div>";
    AjaxChat.ChatEngine.JoinChatRoom(chatRoomId, OnJoinChatRoomSucceeded);
}

function LeaveChatRoom()
{
    var objDiv = document.getElementById("messages");
    objDiv.innerHTML += "<div class=\"srv_msg1\"><%= Lang.Trans("Disconecting...") %></div>";
    AjaxChat.ChatEngine.LeaveChatRoom(chatRoomId);
    disableInterface();
}

function InitializeChat(initChatRoomId)
{
		chatRoomId = initChatRoomId;
    JoinChatRoom();
}

function CloseChat()
{
    LeaveChatRoom(chatRoomId);
    StopTimers();
}

function SetHeight()
{
	if (document.documentElement.clientHeight > 220)
	{
		document.getElementById("left").style.height = document.documentElement.clientHeight - 152 + "px";
		document.getElementById("right").style.height = document.documentElement.clientHeight - 138 + "px";       	
		document.getElementById("usersright").style.height = document.documentElement.clientHeight - 220 + "px";
		document.getElementById("messages").style.height = document.documentElement.clientHeight - 194 + "px";
		document.getElementById("messages").scrollTop = document.getElementById("messages").scrollHeight;
	}
	if (document.documentElement.clientWidth > 163)
	{
		document.getElementById("left").style.width = document.documentElement.clientWidth - 163 + "px";
	    document.getElementById("txtSendMessage").style.width = document.documentElement.clientWidth - 100 + "px";
	}
}