// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Bot.Connector
{
    using Microsoft.Rest;
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// The Bot Connector REST API allows your bot to send and receive messages
    /// to channels configured in the
    /// [Bot Framework Developer Portal](https://dev.botframework.com). The
    /// Connector service uses industry-standard REST
    /// and JSON over HTTPS.
    ///
    /// Client libraries for this REST API are available. See below for a list.
    ///
    /// Many bots will use both the Bot Connector REST API and the associated
    /// [Bot State REST API](/en-us/restapi/state). The
    /// Bot State REST API allows a bot to store and retrieve state associated
    /// with users and conversations.
    ///
    /// Authentication for both the Bot Connector and Bot State REST APIs is
    /// accomplished with JWT Bearer tokens, and is
    /// described in detail in the [Connector
    /// Authentication](/en-us/restapi/authentication) document.
    ///
    /// # Client Libraries for the Bot Connector REST API
    ///
    /// * [Bot Builder for C#](/en-us/csharp/builder/sdkreference/)
    /// * [Bot Builder for Node.js](/en-us/node/builder/overview/)
    /// * Generate your own from the [Connector API Swagger
    /// file](https://raw.githubusercontent.com/Microsoft/BotBuilder/master/CSharp/Library/Microsoft.Bot.Connector.Shared/Swagger/ConnectorAPI.json)
    ///
    /// © 2016 Microsoft
    /// </summary>
    public partial interface IConnectorClient : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }

        /// <summary>
        /// Subscription credentials which uniquely identify client
        /// subscription.
        /// </summary>
        ServiceClientCredentials Credentials { get; }


        /// <summary>
        /// Gets the IAttachments.
        /// </summary>
        IAttachments Attachments { get; }

        /// <summary>
        /// Gets the IConversations.
        /// </summary>
        IConversations Conversations { get; }

    }
}
