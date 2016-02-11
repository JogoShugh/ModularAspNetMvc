using System.Collections.Generic;
using FieldReporting.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FieldReporting.Modules.UserProfile.Core.Tests
{
    [TestClass]
    public class UserProfileTest : BaseTestScript
    {
        public override void Setup()
        {
            MessagesScriptAsXml =
                @"<Message type='FieldReporting.Modules.UserProfile.Messages.Commands.UserProfileUpdate, FieldReporting.Modules.UserProfile.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""Name"":""Josh"",""Intro"":""Dev at Lockheed""}</Message>
<Message type='FieldReporting.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], FieldReporting.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Thank you Josh Dev at Lockheed"",""Success"":true}</Message>
<Message type='FieldReporting.Modules.UserProfile.Messages.Commands.UserProfileUpdate, FieldReporting.Modules.UserProfile.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Message'>{""Name"":""Josh"",""Intro"":""Dev at Lockheed""}</Message>
<Message type='FieldReporting.Infrastructure.Messaging.TypedMessageResponse`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], FieldReporting.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' messageType='Response'>{""Data"":""Thank you Josh Dev at Lockheed"",""Success"":true}</Message>";
        }


        public class Container
        {
            public string Label { get; set; }

            public IList<Element<string>> Elements
            {
                get;
                set;
            }
        }

        public class Element<T>
        {
            public T Data { get; set; }
        }

        public void TestNoResharper()
        {
            var container = new Container();
            container.Label = "Label";
            container.Elements = new List<Element<string>>();
            var element = new Element<string>();
            element.Data = "one";
            container.Elements.Add(element);
            element = new Element<string>();
            element.Data = "two";
            container.Elements.Add(element);
        }

        public void TestAfterResharper()
        {
            var container = new Container { Label = "Label", Elements = new List<Element<string>>() };
            var element = new Element<string> { Data = "one" };
            container.Elements.Add(element);
            element = new Element<string> { Data = "two" };
            container.Elements.Add(element);
        }

        public void TestManualIntelligence()
        {
            var container = new Container
            {
                Label = "Label",
                Elements = new List<Element<string>>()
                {
                    new Element<string> { Data = "one" },
                    new Element<string> { Data = "two" }
                }
            };
        }
    }
}
