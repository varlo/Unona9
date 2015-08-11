/* ASPnetDating 
 * Copyright (C) 2003-2014 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Web;
using AspNetDating;
//using NSpring.Logging;
//using NSpring.Logging.Loggers;

namespace AspNetDating.Classes
{
    public class AnswerRequiredException : Exception
    {
        private string questionName;

        public AnswerRequiredException() : base()
        {
        }

        public AnswerRequiredException(string questionName) : base()
        {
            this.questionName = questionName;
        }

        public override string Message
        {
            get { return String.Format(Lang.Trans("The question '{0}' requires answer!"), questionName); }
        }
    }

    public class NoAttributeFoundException : Exception
    {
        public NoAttributeFoundException() : base()
        {
        }

        public NoAttributeFoundException(string message) : base(message)
        {
        }
    }

    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base()
        {
        }

        public AccessDeniedException(string message) : base(message)
        {
        }
    }

    public class SmsNotConfirmedException : Exception
    {
        public SmsNotConfirmedException()
        {
        }

        public SmsNotConfirmedException(string message)
            : base(message)
        {
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class ExceptionLogger
    {
        public static void Log(HttpRequest Request, Exception ex)
        {
            Log(Request.Url.AbsoluteUri, ex);
        }

        public static void Log(string source, Exception ex)
        {
            try
            {
                string exceptionString = ex.ToString();

                while (ex.InnerException != null)
                {
                    exceptionString = exceptionString + "--------------------------------"
                                      + "The following InnerException reported: "
                                      + ex.InnerException.ToString();
                    ex = ex.InnerException;
                }

                string errorMsg = Environment.NewLine +
                                  "Machine: " + Environment.MachineName + Environment.NewLine +
                                  "Framework Version: " + Environment.Version + Environment.NewLine +
                                  "Assembly Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                                  Environment.NewLine +
                                  "Source: " + source + Environment.NewLine +
                                  "Exception: " + exceptionString + Environment.NewLine;

                if (Config.ErrorLogging.LogErrorsToFile)
                {
                    try
                    {
                        Global.Logger.LogError(errorMsg);
                    }
                    catch (Exception filelogException)
                    {
                        //cant log to file, possible access denied exception
                        if (Config.ErrorLogging.SendErrorsToDevelopers)
                        {
                            EmailQueueItem qItem =
                                EmailQueueItem.Create("debug@aspnetdating.com", "debug", filelogException.Message);
                            qItem.Save(true);
                        }
                    }
                }

                if (Config.ErrorLogging.SendErrorsToDevelopers)
                {
                    EmailQueueItem qItem = EmailQueueItem.Create("debug@aspnetdating.com", "debug", errorMsg);
                    qItem.Save(true);
                }
            }
            catch
            {
            }
        }
    }

    [Serializable]
    public class AuthorizationRequiredException : Exception, ISerializable
    {
        public AuthorizationRequiredException() : base() { }
        public AuthorizationRequiredException(string message) : base(message) { }
        public AuthorizationRequiredException(string message, Exception inner) : base(message, inner) { }
        protected AuthorizationRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.AuthorizationUri = (Uri)info.GetValue("AuthorizationUri", typeof(Uri));
        } 
        public Uri AuthorizationUri 
        { get; set; } 
        
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context); info.AddValue("AuthorizationUri", this.AuthorizationUri, typeof(Uri));
        }
    } 
}