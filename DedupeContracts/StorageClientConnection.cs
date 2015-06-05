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
using MYCOMP.AIOB.DirectoryContracts;

namespace MYCOMP.AIOB.DedupeContracts
{
    public class StorageClientConnection : System.ServiceModel.ClientBase<IStorageClient>, IStorageClient, IDisposable
    {
        /// <summary>
        /// Thic helper function which creates a local StorageClientConnection connection object.
        /// A handle function for the serverside components.
        /// </summary>
        /// <returns></returns>
        public static StorageClientConnection GetLocalConnectionObject()
        {
            return new StorageClientConnection(ConfigUtils.GetLocalComputerFQDN(), Site.ServicePort);
        }
        public StorageClientConnection(string port)
            : this(BuildStorageClientServiceBinding(), GetStorageClientEPAddress(ConfigUtils.GetLocalComputerFQDN(), port))
        {
        }
        public StorageClientConnection(string server, string port)
            : this(BuildStorageClientServiceBinding(), GetStorageClientEPAddress(server, port))
        {

        }
        public StorageClientConnection(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public StorageClientConnection(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public static EndpointAddress GetStorageClientEPAddress(string serverName, string port)
        {
            return new EndpointAddress(new Uri(BuildStorageServiceUriString(serverName, port))
                , EndpointIdentity.CreateSpnIdentity(serverName));
        }

        public static string BuildStorageServiceUriString(string serverName, string port)
        {
            return String.Format("net.tcp://{0}:{1}/StorageClient", serverName, port);
        }

        public static NetTcpBinding BuildStorageClientServiceBinding()
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
            binding.OpenTimeout = new TimeSpan(1, 0, 0);
            binding.CloseTimeout = new TimeSpan(1, 0, 0);
            binding.SendTimeout = new TimeSpan(1, 0, 0);
            binding.ReceiveTimeout = new TimeSpan(1, 0, 0);

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
        public string GetDBConnectionString(string dedupeEngineId)
        {
            try
            {
                return base.Channel.GetDBConnectionString(dedupeEngineId);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }
        public void CreateObject(string dedupeEngineId, List<DedupeObjectName> inputObjectList, out List<DedupeObjectName> outputObjectList)
        {
            try
            {
                base.Channel.CreateObject(dedupeEngineId, inputObjectList, out outputObjectList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void CreateSegments(string dedupeEngineId, Int64 objectIdentity, List<SegmentEntry> inputSegmentList, out List<SegmentEntry> outputSegmentList)
        {
            try
            {
                base.Channel.CreateSegments(dedupeEngineId, objectIdentity, inputSegmentList, out outputSegmentList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void StageSegmentFingerPrint(string dedupeEngineId, string storageId, Int64 segmentIdentity, List<SegmentFingerPrintEntry> ObjectStageList, out List<SegmentFingerPrintEntry> UploadBolckList)
        {
            try
            {
                base.Channel.StageSegmentFingerPrint(dedupeEngineId, storageId, segmentIdentity, ObjectStageList, out UploadBolckList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        //public void UpdateFPBlocks(string dedupeEngineId, List<FingerPrintBlockEntry> fpBlockList, out List<string> blockAlreadyExistList)
        //{
        //    try
        //    {
        //        base.Channel.UpdateFPBlocks(dedupeEngineId, fpBlockList, out blockAlreadyExistList);
        //    }
        //    catch (FaultException<ExceptionDetail> ex)
        //    {
        //        DTrace.WriteLine(DTraceLevel.Error, ex);
        //        ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        DTrace.WriteLine(DTraceLevel.Error, ex);
        //        throw;
        //    }
        //}
        //public bool GetSegmentUploadBlockList(string dedupeEngineId, Int64 segmentIdentity, out List<SegmentFingerPrintEntry> UploadBlockList)
        //{
        //    try
        //    {
        //        return base.Channel.GetSegmentUploadBlockList(dedupeEngineId, segmentIdentity, out UploadBlockList);
        //    }
        //    catch (FaultException<ExceptionDetail> ex)
        //    {
        //        DTrace.WriteLine(DTraceLevel.Error, ex);
        //        ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        DTrace.WriteLine(DTraceLevel.Error, ex);
        //        throw;
        //    }
        //}
        public void CommitObject(string dedupeEngineId, Int64 objectIdentity)
        {
            try
            {
                base.Channel.CommitObject(dedupeEngineId, objectIdentity);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }
        public void RollbackObject(string dedupeEngineId, Int64 transactionId)
        {
            try
            {
                base.Channel.RollbackObject(dedupeEngineId, transactionId);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void GetAllObjectIdentityByContainer(string dedupeEngineId, string storageId, string containerName, out List<Int64> objectIdentityList)
        {
            try
            {
                base.Channel.GetAllObjectIdentityByContainer(dedupeEngineId, storageId, containerName, out objectIdentityList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void GetSegmentIdsByObjectIdentity(string dedupeEngineId, Int64 objectIdentity, out List<string> segmentIdList)
        {
            try
            {
                base.Channel.GetSegmentIdsByObjectIdentity(dedupeEngineId, objectIdentity, out segmentIdList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void UpdateBlockData(string dedupeEngineId, string storageId, string containerName, string objectName, Int64 size, DateTime creationDate, List<NewBlockEntry> blockList)
        {
            try
            {
                base.Channel.UpdateBlockData(dedupeEngineId, storageId, containerName, objectName, size, creationDate, blockList);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public void GetSegmentToBlockDataMapping(string dedupeEngineId, string segmentId, out List<SegmentBlkToDataBlkEntry> map)
        {
            try
            {
                base.Channel.GetSegmentToBlockDataMapping(dedupeEngineId, segmentId, out map);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public DownloadSegmentResult DownloadSegment(DownloadSegmentParam param)
        {
            try
            {
                return base.Channel.DownloadSegment(param);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public Dictionary<Int32, StorageProperty> GetDedupeStorageMap(string dedupeEngineId)
        {
            try
            {
                return base.Channel.GetDedupeStorageMap(dedupeEngineId);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }

        public List<NewBlockEntry> GetNewFPBlocksByStorageIdentity(string dedupeEngineId, Int64 storageIdentity)
        {
            try
            {
                return base.Channel.GetNewFPBlocksByStorageIdentity(dedupeEngineId, storageIdentity);
            }
            catch (FaultException<ExceptionDetail> ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                ExtractAndThrowWcfServiceException.ExtractAndReThrow(ex.Detail);
                throw;
            }
            catch (Exception ex)
            {
                DTrace.WriteLine(DTraceLevel.Error, ex);
                throw;
            }
        }
    }
}
