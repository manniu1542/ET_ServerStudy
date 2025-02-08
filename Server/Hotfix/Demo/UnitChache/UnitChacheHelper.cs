using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Linq;

namespace ET
{
    [FriendClass(typeof (Scene))]
    public static class UnitChacheHelper
    {
        /// <summary>
        /// 添加或更新UnitChache ()   //找到 缓存服并给他推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask AddOrUpdateAllUnitChache(Unit unit)
        {
            var message = new G2U_AddOrUpdateUnitChache() { UnitId = unit.Id };
            message.EntityType.Add(typeof (Unit).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unit));
            foreach (var cpt in unit.Components)
            {
                
                if (typeof (IUnitChache).IsAssignableFrom(cpt.Key)) ;
                {
                    message.EntityType.Add(cpt.Key.FullName);
                    message.EntityBytes.Add(MongoHelper.ToBson(cpt.Value));
                }
            }
          
            var sceneUnitChache = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unit.Id );

            U2G_AddOrUpdateUnitChache msg = await MessageHelper.CallActor(sceneUnitChache.InstanceId, message) as U2G_AddOrUpdateUnitChache;
        }

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
        public static async ETTask<Unit> GetUnitChache(Scene scene, long unitID)
        {
            G2U_GetUnitChache message = new G2U_GetUnitChache() { UnitId = unitID };
            long sceneID = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unitID).InstanceId;
            U2G_GetUnitChache response = await MessageHelper.CallActor(sceneID, message) as U2G_GetUnitChache;

            if (response.Error != ErrorCode.ERR_Success || response.EntityType.Count == 0)
            {
                return null;
            }

            //找到Unit组件 （没有 继承了IUnitChache怎么会被找到呢？）   获取到的UnitChache 必定有 Unit这个基类 存储 因为 UnitChacheComponent的Aawke里面存储了Unit的类型。
            string unitName = typeof (Unit).FullName;
            int unitIdx = response.listComponentName.FindIndex(str => str == unitName);
            if (unitIdx < 0) return null;

            Unit unit = response.EntityType[unitIdx] as Unit;
            scene.AddChild(unit);

            foreach (var cpt in response.EntityType)
            {
                if (cpt != null && cpt is Unit)
                {
                    unit.AddComponent(cpt);
                }
            }

            return unit;
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