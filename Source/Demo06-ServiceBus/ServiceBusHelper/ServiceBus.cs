using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;

namespace ServiceBusHelper
{
    public class ServiceBus
    {
        string _connectionString;
        string _topicName;
        TopicClient _client;
        NamespaceManager _namespaceManger;

        private ServiceBus()
        {
        }

        public static ServiceBus Setup(string connectionString,
            string topicName)
        {
            return new ServiceBus
            {
                _connectionString = connectionString,
                _topicName = topicName
            }
            .CreateNamespaceManager()
            .CreateTopicIfNotExists();
        }

        private ServiceBus CreateNamespaceManager()
        {
            if (_namespaceManger == null)
                _namespaceManger = NamespaceManager.CreateFromConnectionString(_connectionString);

            return this;
        }

        private ServiceBus CreateTopicIfNotExists()
        {
            if (!_namespaceManger.TopicExists(_topicName))
                _namespaceManger.CreateTopic(_topicName);

            return this;
        }

        public ServiceBus Publish<T>(T target)
        {
            if (_client == null)
                _client = TopicClient.CreateFromConnectionString(_connectionString,
                    _topicName);

            _client.Send(new BrokeredMessage(target));

            return this;
        }

        public void Subscribe<T>(Action<T> callback)
        {
            if (!_namespaceManger.SubscriptionExists(_topicName, typeof(T).Name))
                _namespaceManger.CreateSubscription(_topicName, typeof(T).Name);

            var subscriptionClient = SubscriptionClient.CreateFromConnectionString(
                _connectionString,
                _topicName,
                typeof(T).Name);

            while (true)
            {
                var message =
                    subscriptionClient.Receive(TimeSpan.FromSeconds(3));

                if (message != null)
                {
                    try
                    {
                        if (callback != null)
                        {
                            var body = message.GetBody<T>();
                            callback(body);
                        }

                        message.Complete();
                    }
                    catch
                    {
                        // This isn't really a good idea, it's just a demo
                        // TODO: add your own code to handle poison messages
                        message.Abandon();
                    }
                }
            }
        }
    }
}