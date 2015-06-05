/* Copyright (C) 2015 Megha Ghaywat
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Megha Ghaywat <megha.ghaywat@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using MYCOMP.AIOB.DirectoryContracts;

namespace MYCOMP.AIOB.DedupeContracts
{
    [ServiceContract(Namespace = "DedupeStorageService")]
    public interface IStorageClient
    {
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void CreateObject(string dedupeEngineId, List<DedupeObjectName> inputObjectList, out List<DedupeObjectName> outputObjectList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void CreateSegments(string dedupeEngineId, Int64 objectIdentity, List<SegmentEntry> inputSegmentList, out List<SegmentEntry> outputSegmentList);   
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void StageSegmentFingerPrint(string dedupeEngineId, string storageId, Int64 segmentIdentity, List<SegmentFingerPrintEntry> ObjectStageList, out List<SegmentFingerPrintEntry> UploadBolckList);
        //[OperationContract]
        //[FaultContract(typeof(ExceptionDetail))]
        //void UpdateFPBlocks(string dedupeEngineId, List<FingerPrintBlockEntry> fpBlockList, out List<string> blockAlreadyExistList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void CommitObject(string dedupeEngineId, Int64 objectIdentity);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void RollbackObject(string dedupeEngineId, Int64 transactionId);
        //[OperationContract]
        //[FaultContract(typeof(ExceptionDetail))]
        //bool GetSegmentUploadBlockList(string dedupeEngineId, Int64 segmentIdentity, out List<SegmentFingerPrintEntry> UploadBlockList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void GetAllObjectIdentityByContainer(string dedupeEngineId, string storageId, string containerName, out List<Int64> objectIdentityList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void GetSegmentIdsByObjectIdentity(string dedupeEngineId, Int64 objectIdentity, out List<string> segmentIdList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void UpdateBlockData(string dedupeEngineId, string storageId, string containerName, string objectName, Int64 size, DateTime creationDate, List<NewBlockEntry> blockList);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        string GetDBConnectionString(string dedupeEngineId);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        void GetSegmentToBlockDataMapping(string dedupeEngineId, string segmentId, out List<SegmentBlkToDataBlkEntry> map);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        DownloadSegmentResult DownloadSegment(DownloadSegmentParam param);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        Dictionary<Int32, StorageProperty> GetDedupeStorageMap(string dedupeEngineId);
        [OperationContract]
        [FaultContract(typeof(ExceptionDetail))]
        List<NewBlockEntry> GetNewFPBlocksByStorageIdentity(string dedupeEngineId, Int64 storageIdentity);
    }
    [ServiceContract(Namespace = "DedupeStorageService")]
    public interface IStorageServer
    {
    }
}
