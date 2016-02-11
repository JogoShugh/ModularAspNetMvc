using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Infrastructure.Composition;
//using Microsoft.Practices.Unity;

namespace Core.Infrastructure.Messaging
{
    public class MessageProcessor
    {
        private PartsList<ILogMessages> Loggers { get; set; }

        // NOTE: I know, static singletons are bad...but this is for demonstration 
        // of basic concepts, not optimization
        private static readonly MessageProcessor _instance = new MessageProcessor();

        public static MessageProcessor Instance
        {
            get { return _instance; }
        }

        public MessageProcessor()
        {
            Loggers = new PartsList<ILogMessages>();
        }

        private readonly IDictionary<Type, Type> _handlers = new Dictionary<Type, Type>();

        public bool RegisterMessageHandler<TMessageType, TMessageHandlerType>()
            where TMessageType : IMessage
        {
            var messageType = typeof(TMessageType);
            var messageHandlerType = typeof(TMessageHandlerType);

            if (_handlers.ContainsKey(messageType))
            {
                throw new InvalidOperationException(string.Format("Cannot add a handler for message type '{0}' since it is already mapped to a handler of type '{1}'", messageType, messageHandlerType));
            }

            _handlers.Add(messageType, messageHandlerType);

            return true;
        }

        //private readonly IDictionary<Type, IUnityContainer> _containersForType =
        //    new Dictionary<Type, IUnityContainer>();

        //public bool RegisterMessageHandler<TMessageType, TMessageHandlerType>(IUnityContainer container) 
        //public bool RegisterMessageHandler<TMessageType, TMessageHandlerType>() 
        //    where TMessageType : IMessage
        //{
        //    var messageType = typeof(TMessageType);
        //    var messageHandlerType = typeof(TMessageHandlerType);

        //    if (_handlers.ContainsKey(messageType))
        //    {
        //        throw new InvalidOperationException(string.Format("Cannot add a handler for message type '{0}' since it is already mapped to a handler of type '{1}'", messageType, messageHandlerType));
        //    }

        //    _handlers.Add(messageType, messageHandlerType);
        //    //_containersForType.Add(messageHandlerType, container);

        //    return true;
        //}

        public TResponseType Execute<TResponseType>(IMessage message)
            where TResponseType : class
        {
            var response = Execute(message);

            if (response == null)
            {
                return null;
            }

            var typedResponse = response as TResponseType;

            if (typedResponse == null)
            {
                throw new InvalidOperationException(
                    string.Format("Received an invalid response type of '{0}' when expecting type of '{1}'",
                                  response.GetType().Name, typeof(TResponseType).Name));
            }

            return typedResponse;
        }

        public IMessage Execute(IMessage message)
        {
            LogMessage(message); // Log the requesting message

            var response = GetResponseFor(message);

            if (response != null)
            {
                LogMessage(response); // Log the response now

                return response;
            }

            return null;
        }

        private IMessageResponse GetResponseFor(IMessage message)
        {
            var messageType = message.GetType();

            if (_handlers.ContainsKey(messageType))
            {
                var handlerType = _handlers[messageType];

                var instance = CreateInstanceFromType(handlerType);

                var handlerMethod = GetHandlerMethod(handlerType, messageType);

                if (handlerMethod != null)
                {
                    var response = CallHandlerMethod(message, handlerMethod, instance);
                    return response;
                }

                throw new InvalidOperationException(string.Format("Found a handler of type '{0}', but could not find appropriate method named Handle accepting argument of type '{1}'", handlerType, messageType));

            }

            throw new InvalidOperationException(string.Format("Could not find handler for message type '{0}'", messageType));
        }

        private static MethodInfo GetHandlerMethod(Type handlerType, Type messageType)
        {
            var handlerMethod = (from method in handlerType.GetMethods()
                                 where method.Name.Equals("Handle", StringComparison.OrdinalIgnoreCase)
                                       && method.GetParameters()[0].ParameterType == messageType
                                 select method).FirstOrDefault();
            return handlerMethod;
        }

        private object CreateInstanceFromType(Type handlerType)
        {
            object instance;

            //if (_containersForType.ContainsKey(handlerType))
            //{
            //    var container = _containersForType[handlerType];
            //    instance = container.Resolve(handlerType);
            //}
            //else
            {
                instance = handlerType.GetConstructors()[0].Invoke(new object[] {});
            }

            return instance;
        }

        private static IMessageResponse CallHandlerMethod(IMessage message, MethodInfo handlerMethod, object instance)
        {
            var response = (IMessageResponse)handlerMethod.Invoke(instance, new object[] { message });
            return response;
        }

        private void LogMessage(IMessage message)
        {
            if (Loggers.Items == null || !Loggers.Items.Any()) return;

            foreach (var logger in Loggers.Items)
            {
                logger.Log(message);
            }
        }
    }
}
