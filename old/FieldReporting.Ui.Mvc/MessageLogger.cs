using System.ComponentModel.Composition;
using System.IO;
using FieldReporting.Infrastructure.Messaging;

namespace FieldReporting.Ui.Mvc
{
    [Export(typeof(ILogMessages))]
    public class MessageLogger : ILogMessages
    {   
        public void Log(IMessage message)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var logMessage = serializer.Serialize(message);

            // TODO: hack
            logMessage = logMessage.Replace("\"", "\"\"");

            var type = message.GetType().AssemblyQualifiedName;
            var messageType = "Message";

            using (var streamWriter = File.AppendText(@"C:\Users\JGough\Documents\Visual Studio 2010\Projects\FieldReporting\FieldReporting.Ui.Mvc\bin\log.txt"))
            {
                if (message is IMessageResponse)
                {
                    messageType = "Response";
                }

                streamWriter.WriteLine(string.Format("<Message type='{0}' messageType='{1}'>", type, messageType) + logMessage + "</Message>");
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
    }
}