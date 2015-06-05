/* Copyright (C) 2015 Megha Ghaywat
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Megha Ghaywat <megha.ghaywat@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using MYCOMP.AIOB.Diagnostics;
using MYCOMP.AIOB.Common;

namespace MYCOMP.AIOB.DedupeContracts
{
    public class StorageServerConnection : System.ServiceModel.ClientBase<IStorageServer>, IStorageServer, IDisposable
    {
        public StorageServerConnection(string port)
            : this(BuildStorageServerServiceBinding(), GetStorageServerEPAddress(ConfigUtils.GetLocalComputerFQDN(), port))
        {
        }
        public StorageServerConnection(string server, string port)
            : this(BuildStorageServerServiceBinding(), GetStorageServerEPAddress(server, port))
        {

        }
        public StorageServerConnection(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public StorageServerConnection(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public static EndpointAddress GetStorageServerEPAddress(string serverName, string port)
        {
            return new EndpointAddress(new Uri(BuildStorageServiceUriString(serverName, port))
                , EndpointIdentity.CreateSpnIdentity(serverName));
        }

        public static string BuildStorageServiceUriString(string serverName, string port)
        {
            return String.Format("net.tcp://{0}:{1}/StorageServer", serverName, port);
        }

        public static NetTcpBinding BuildStorageServerServiceBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReaderQuotas.MaxDepth = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);

            return binding;
        }
        void IDisposable.Dispose()
        {
            if (this.State == CommunicationState.Faulted)
            {
                base.Abort();
            }
            else
            {
                base.Close();
            }
        }

        new public void Close()
        {
            if (this.State == CommunicationState.Faulted)
            {
                base.Abort();
            }
            else
            {
                base.Close();
            }
        }

    }
}
