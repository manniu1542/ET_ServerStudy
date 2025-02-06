using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Linq;

namespace ET
{
    public static class UnitChacheHelper
    {
        /// <summary>
        /// 添加或更新UnitChache ()   //找到 缓存服并给他推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask AddOrUpdateUnitChache<T>(this T unitChache) where T : Entity, IUnitChache
        {
            var message = new G2U_AddOrUpdateUnitChache() { UnitId = unitChache.Id };
            message.EntityType.Add(typeof (T).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unitChache));

            var sceneUnitChache = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unitChache.Id);

            U2G_AddOrUpdateUnitChache msg = await MessageHelper.CallActor(sceneUnitChache.InstanceId, message) as U2G_AddOrUpdateUnitChache;
        }

        /// <summary>
        /// 获取UnitChache      
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask<Unit> GetUnitChache<T>(Scene scene, long unitID) where T : Entity, IUnitChache
        {
            await ETTask.CompletedTask;
            return null;
        }

        /// <summary>
        /// 获取UnitChache      
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask<T> GetUnitComponentChache<T>(long unitID) where T : Entity, IUnitChache
        {
            G2U_GetUnitChache message = new G2U_GetUnitChache() { UnitId = unitID };
            message.listComponentName.Add(typeof (T).FullName);

            long sceneID = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unitID).InstanceId;
            U2G_GetUnitChache response = await MessageHelper.CallActor(sceneID, message) as U2G_GetUnitChache;

            if (response.Error == ErrorCode.ERR_Success && response.listComponentName.Count > 0)
            {
                return response.listComponentName[0] as T;
            }

            return null;
        }

        /// <summary>
        /// 删除UnitChache 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask DelateUnitChache(long unitID)
        {
            long sceneID = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unitID).InstanceId;
            await MessageHelper.CallActor(sceneID, new G2U_DeleteUnitChache() { UnitID = unitID });
        }
    }
}