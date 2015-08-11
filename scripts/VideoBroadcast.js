﻿/// <reference name="MicrosoftAjax.js"/>

var newEventsTimer;
var fetchNewEventsLock = false;   
     
function KeyHandler(e) 
{   
    if(window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if(e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }

    
    if (keynum == 13)
    {
        SendMessage();
		// cancel the default submit
		e.returnValue=false;
		e.cancel = true;      
        return false;
    }
}  
 
function FetchNewEventsForVideoBroadcast()
{
    if (fetchNewEventsLock) return;
    
    var hidVideoGuid = $get("hidVideoGuid");
    var hidCurrentUser = $get("hidCurrentUser");    

    fetchNewEventsLock = true;
    AspNetDating.Services.OnlineCheck.FetchNewEventsForVideoBroadcast(hidVideoGuid.value, hidCurrentUser.value, OnFetchNewEventsSucceeded, OnFetchNewEventsFailed);
}

function OnFetchNewEventsFailed(error, userContext, methodName)
{
    fetchNewEventsLock = false;
}

function OnFetchNewEventsSucceeded(result)
{
    try
    {
        if (result)
        {
            $get("UsersWatching").innerHTML = result.UsersWatchingString;
            
            if (!result.IsBroadcastAlive)
            {
                if ($get('tblVideoBroadcast') != null)
                    $get('tblVideoBroadcast').style.display = 'none';
                    
                if ($get('divBroadcastEndMessage') != null)
                    $get('divBroadcastEndMessage').style.display = '';
                    
                if (newEventsTimer)
                    window.clearInterval(newEventsTimer);
                return;
            }
                            
            if (result.Messages)
            {             
                var divMessages = $get("VideoBroadcastMessages");
                var messages = '';
                var sender;
                var viewedUser = $get('hidViewedUser') == null?$get('hidCurrentUser').value:$get('hidViewedUser').value;



                for (i=0; i < result.Messages.length; i++)
                {
                    isBroadcaster = result.Messages[i].Username == viewedUser;
                    
                    if (isBroadcaster)
                    {
                        sender = result.Messages[i].Username;
                    }
                    else
                    {
                        sender = '<a href="' + result.Messages[i].ProfileURL + '" target="_blank" onmouseover="showUserPreview(\'' + result.Messages[i].Username + '\')" onmouseout="hideUserPreview()">' +
                        result.Messages[i].Username + '</a>';
                    }                
                    messages = messages + (isBroadcaster?'<b>':'') +
                    '<p>' + sender + ':&nbsp;' + (isBroadcaster?'</b>':'') + result.Messages[i].Message + '<span class="pull-right text-muted">' + result.Messages[i].TimestampString + '</span></p>';
                }
                
                if (messages != '')
                {
                    divMessages.innerHTML = messages;

                    divMessages.scrollTop = divMessages.scrollHeight;
                    divMessages.scrollTop = divMessages.scrollHeight;
                }
            }
        }    
    }
    finally
    {
        fetchNewEventsLock = false;
    }
}
function SendMessage()
{
    var txtSendMessage = $get("txtSendMessage");
    var hidVideoGuid = $get("hidVideoGuid");
    var hidCurrentUser = $get("hidCurrentUser");
    if (txtSendMessage.value == "") return;
    
    AspNetDating.Services.OnlineCheck.SendMessageForVideoBroadcast(hidVideoGuid.value, txtSendMessage.value, hidCurrentUser.value, OnSendMessageForVideoBroadcastSucceeded, 
        OnSendMessageForVideoBroadcastFailed);
    
    txtSendMessage.value = '';
}  

function OnSendMessageForVideoBroadcastSucceeded(result)
{

}

function OnSendMessageForVideoBroadcastFailed(error, userContext, methodName)
{
    alert(error.get_message());
}  
