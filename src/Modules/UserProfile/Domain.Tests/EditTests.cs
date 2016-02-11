using Core.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Modules.UserProfile.Core.Tests
{
    [TestClass]
    public class EditTests : BaseTestScript
    {
        public override void Setup()
        {
            MessagesScriptAsXml =
                @"<Message type='Core.Modules.Authentication.Messages.Commands.LoginSubmit, Core.Modules.Authentication.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""UserName"":""admin"",""Password"":""lockheed101#""}</Message>
<Message type='Core.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Core.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Thanks for logging in admin"",""Success"":true}</Message>
<Message type='Core.Modules.UserProfile.Messages.Queries.UserProfileEdit, Core.Modules.UserProfile.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""UserName"":""Adminllll""}</Message>
<Message type='Core.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Core.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""You are not authorized for this action"",""Success"":true}</Message>
<Message type='Core.Modules.UserProfile.Messages.Queries.UserProfileEdit, Core.Modules.UserProfile.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""UserName"":""Admin""}</Message>
<Message type='Core.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Core.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Welcome admin"",""Success"":true}</Message>";
        }
    }
}





//            MessagesScriptAsXml =
//@"<Message type='Core.Modules.Authentication.Messages.Commands.LoginSubmit, Core.Modules.Authentication.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""UserName"":""admin"",""Password"":""lockheed101#""}</Message>
//<Message type='Core.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Core.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Thanks for logging in admin"",""Success"":true}</Message>
//<Message type='Core.Modules.UserProfile.Messages.Queries.UserProfileEdit, Core.Modules.UserProfile.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""UserName"":""Admin""}</Message>
//<Message type='Core.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], Core.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Welcome admin"",""Success"":true}</Message>";

//            var message = new LoginSubmit {Password = "admin", UserName = "username"};
//            var response = new TypedMessageResponse<string> {Data = "Hello admin", Success = true};

//            var array = new Object[] {message, response};

//            var code = array.CreateCode();

//            var list = new List<System.Object>();
//var loginSubmit39 = new LoginSubmit();
//loginSubmit39.UserName = "username";
//loginSubmit39.Password = "admin";
//list.Add(loginSubmit39);
//var typedMessageResponse`1178 = new TypedMessageResponse`1();
//typedMessageResponse`1178.Data = "Hello admin";
//typedMessageResponse`1178.Success = true;
//list.Add(typedMessageResponse`1178);
//var convertedObject = list.ToArray();
