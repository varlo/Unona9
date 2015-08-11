#if !AJAXCHAT_INTEGRATION
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace AjaxChat.Classes
{
    public interface IHttpApplicationConnectionStringProvider
    {
        string GetConnectionString();
    }

    public interface IHttpApplicationUserAdapter
    {
        string GetCurrentlyLoggedUsername();

        bool IsRoomAdmin(string username, int chatRoomId);

        bool HasChatAccess(string username, int chatRoomId);

        string GetUserDisplayName(string username);

        bool IsAdministrator(string username);
    }

    public interface IHttpApplicationSupportLogin
    {
        string GetLoginUrl();

        //TODO: Support login via chat
    }

    public interface IHttpApplicationSupportAvatars
    {
        string GetUserAvatar(string username);
    }

    public interface IHttpApplicationSupportProfiles
    {
        string GetUserProfileUrl(string username);
    }

    public interface IHttpApplicationSupportSmilies
    {
        string GetSmiliesDirectory();
        string GetSmiliesUrl();
    }

    public interface IHttpApplicationSupportTranslations
    {
        string Translate(string text);
    }

    public interface IHttpApplicationChatRoomProvider
    {
        ChatRoom GetChatRoom(int chatRoomId);
    }
}
#endif