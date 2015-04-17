using System;
using System.ComponentModel;
using System.Configuration;
using Enyim.Caching.Memcached;

namespace Enyim.Caching.Configuration
{
    /// <summary>
    /// Configures the socket pool settings for Memcached servers.
    /// </summary>
    public sealed class SocketPoolElement : ConfigurationElement, ISocketPoolConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating the minimum amount of sockets per server in the socket pool.
        /// </summary>
        /// <returns>The minimum amount of sockets per server in the socket pool.</returns>
        [ConfigurationProperty("minPoolSize", IsRequired = false, DefaultValue = 10), IntegerValidator(MinValue = 0)]
        public int MinPoolSize
        {
            get { return (int)base["minPoolSize"]; }
            set { base["minPoolSize"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the maximum amount of sockets per server in the socket pool.
        /// </summary>
        /// <returns>The maximum amount of sockets per server in the socket pool. The default is 20.</returns>
        /// <remarks>It should be 0.75 * (number of threads) for optimal performance.</remarks>
        [ConfigurationProperty("maxPoolSize", IsRequired = false, DefaultValue = 20), IntegerValidator(MinValue = 0)]
        public int MaxPoolSize
        {
            get { return (int)base["maxPoolSize"]; }
            set { base["maxPoolSize"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which the connection attempt will fail.
        /// </summary>
        /// <returns>The value of the connection timeout. The default is 10 seconds.</returns>
        [ConfigurationProperty("connectionTimeout", IsRequired = false, DefaultValue = "00:00:10"), PositiveTimeSpanValidator, TypeConverter(typeof(InfiniteTimeSpanConverter))]
        public TimeSpan ConnectionTimeout
        {
            get { return (TimeSpan)base["connectionTimeout"]; }
            set { base["connectionTimeout"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which the getting a connection from the pool will fail. The default is 100 msec.
        /// </summary>
        /// <returns>The value of the queue timeout.</returns>
        [ConfigurationProperty("queueTimeout", IsRequired = false, DefaultValue = "00:00:02.500"), PositiveTimeSpanValidator, TypeConverter(typeof(InfiniteTimeSpanConverter))]
        public TimeSpan QueueTimeout
        {
            get { return (TimeSpan)base["queueTimeout"]; }
            set { base["queueTimeout"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which receiving data from the socket fails.
        /// </summary>
        /// <returns>The value of the receive timeout. The default is 10 seconds.</returns>
        [ConfigurationProperty("receiveTimeout", IsRequired = false, DefaultValue = "00:00:10"), PositiveTimeSpanValidator, TypeConverter(typeof(InfiniteTimeSpanConverter))]
        public TimeSpan ReceiveTimeout
        {
            get { return (TimeSpan)base["receiveTimeout"]; }
            set { base["receiveTimeout"] = value; }
        }

        /// <summary>
        /// Gets or sets a value that specifies the amount of time after which an unresponsive (dead) server will be checked if it is working.
        /// </summary>
        /// <returns>The value of the dead timeout. The default is 10 secs.</returns>
        [ConfigurationProperty("deadTimeout", IsRequired = false, DefaultValue = "00:00:02"), PositiveTimeSpanValidator, TypeConverter(typeof(InfiniteTimeSpanConverter))]
        public TimeSpan DeadTimeout
        {
            get { return (TimeSpan)base["deadTimeout"]; }
            set { base["deadTimeout"] = value; }
        }

        /// <summary>
        /// Called after deserialization.
        /// </summary>
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            if (this.MinPoolSize > this.MaxPoolSize)
                throw new ConfigurationErrorsException("maxPoolSize must be larger than minPoolSize.");
        }

        [ConfigurationProperty("failurePolicyFactory", IsRequired = false)]
        public ProviderElement<INodeFailurePolicyFactory> FailurePolicyFactory
        {
            get { return (ProviderElement<INodeFailurePolicyFactory>)base["failurePolicyFactory"]; }
            set { base["failurePolicyFactory"] = value; }
        }

        #region [ ISocketPoolConfiguration     ]

        int ISocketPoolConfiguration.MinPoolSize
        {
            get { return this.MinPoolSize; }
            set { this.MinPoolSize = value; }
        }

        int ISocketPoolConfiguration.MaxPoolSize
        {
            get { return this.MaxPoolSize; }
            set { this.MaxPoolSize = value; }
        }

        TimeSpan ISocketPoolConfiguration.ConnectionTimeout
        {
            get { return this.ConnectionTimeout; }
            set { this.ConnectionTimeout = value; }
        }

        TimeSpan ISocketPoolConfiguration.DeadTimeout
        {
            get { return this.DeadTimeout; }
            set { this.DeadTimeout = value; }
        }

        TimeSpan ISocketPoolConfiguration.QueueTimeout
        {
            get { return this.QueueTimeout; }
            set { this.QueueTimeout = value; }
        }

        TimeSpan ISocketPoolConfiguration.ReceiveTimeout
        {
            get { return this.ReceiveTimeout; }
            set { this.ReceiveTimeout = value; }
        }

        INodeFailurePolicyFactory ISocketPoolConfiguration.FailurePolicyFactory
        {
            get { return this.FailurePolicyFactory.CreateInstance() ?? new FailImmediatelyPolicyFactory(); }
            set { }
        }

        #endregion

        [ConfigurationProperty("lingerTime", IsRequired = false, DefaultValue = "00:00:10"), PositiveTimeSpanValidator, TypeConverter(typeof(InfiniteTimeSpanConverter))]
        public TimeSpan LingerTime
        {
            get { return (TimeSpan)base["lingerTime"]; }
            set { base["lingerTime"] = value; }
        }

        [ConfigurationProperty("lingerEnabled", IsRequired = false, DefaultValue =false)]
        public bool LingerEnabled
        {
            get { return (bool)base["lingerEnabled"]; }
            set { base["lingerEnabled"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable TCP keep alives.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable TCP keep alives; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>The default is true; TCP Keep Alives are enabled.</remarks>
        [ConfigurationProperty("enableTcpKeepAlives", DefaultValue = true, IsRequired = false)]
        public bool EnableTcpKeepAlives
        {
            get { return (bool)this["enableTcpKeepAlives"]; }
            set { this["enableTcpKeepAlives"] = value; }
        }

        /// <summary>
        /// Specifies the timeout, in milliseconds, with no activity until the first keep-alive packet is sent.
        /// </summary>
        /// <value>
        /// The TCP keep alive time in milliseconds.
        /// </value>
        /// <remarks>The default is 2hrs.</remarks>
        [ConfigurationProperty("tcpKeepAliveTime", DefaultValue = ((uint)2 * 60 * 60 * 1000), IsRequired = false)]
        public uint TcpKeepAliveTime
        {
            get { return (uint)this["tcpKeepAliveTime"]; }
            set { this["tcpKeepAliveTime"] = value; }
        }

        /// <summary>
        /// Specifies the interval, in milliseconds, between when successive keep-alive packets are sent if no acknowledgement is received.
        /// </summary>
        /// <value>
        /// The TCP keep alive interval in milliseconds..
        /// </value>
        /// <remarks>The default is 1 second.</remarks>
        [ConfigurationProperty("tcpKeepAliveInterval", DefaultValue = ((uint)1000), IsRequired = false)]
        public uint TcpKeepAliveInterval
        {
            get { return (uint)this["tcpKeepAliveInterval"]; }
            set { this["tcpKeepAliveInterval"] = value; }
        }
    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    Copyright (c) 2010 Attila Kisk�, enyim.com
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion