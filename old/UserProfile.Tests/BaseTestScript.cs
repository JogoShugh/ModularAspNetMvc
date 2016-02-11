using System;
using System.Collections.Generic;
using System.Xml;
using FieldReporting.Infrastructure.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UserProfile.Tests
{
    [TestClass]
    public abstract class BaseTestScript
    {
        protected BaseTestScript()
        {
            var wiring = new MessageHandlerWiring();
            wiring.WireMessageHandlers(MessageProcessor.Instance);
            Setup();
        }

        public abstract void Setup();

        public string MessagesScriptAsXml { get; set; }

        public IEnumerable<IMessage> GetMessages()
        {
            var xml = "<Messages>" + MessagesScriptAsXml + "</Messages>";

            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xml);

            var messages = new List<IMessage>();

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                var type = node.Attributes["type"].Value;
                var messageType = node.Attributes["messageType"].Value;
                var serializedMessage = node.InnerXml;


                var actualType = Type.GetType(type);

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                var message = serializer.Deserialize(serializedMessage, actualType) as IMessage;

                messages.Add(message);
            }

            return messages;
        }

        [TestMethod]
        public void ExecuteScript()
        {
            var messages = new List<IMessage>(GetMessages());

            for (var i = 0; i < messages.Count / 2; i++)
            {
                var messageIndex = i*2;
                var responseIndex = i*2 + 1;

                var originalMessage = messages[messageIndex];
                var expectedResponse = messages[responseIndex];

                var actualResponse = MessageProcessor.Instance.Execute(originalMessage);

                SerializedAssert.AreEqual(expectedResponse, actualResponse);
            }
        }
    }
}
