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
        public static async ETTask AddOrUpdateUnitChache<T>(this Entity unitChache) where T : Entity, IUnitChache
        {
            int zone = unitChache.DomainZone();
            var sceneUnitChache = StartSceneConfigCategory.Instance.UnitChaches[zone];
            var bytes = unitChache.ToBson();

            var list = new List<byte[]>() { bytes };
            U2G_AddOrUpdateUnitChache msg =
                    await MessageHelper.CallActor(sceneUnitChache.InstanceId, new G2U_AddOrUpdateUnitChache() { Unit = list }) as
                            U2G_AddOrUpdateUnitChache;
        }

        /// <summary>
        /// 获取UnitChache      
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static async ETTask<List<T>> GetUnitChache<T>(long UnitID) where T : Entity, IUnitChache
        {
            // int zone = unitChache.DomainZone();
            // var sceneUnitChache = StartSceneConfigCategory.Instance.UnitChaches[zone];
            // var bytes = unitChache.ToBson();
            //
            // var list = new List<byte[]>(){bytes};
            // U2G_AddOrUpdateUnitChache msg =
            //         await MessageHelper.CallActor(sceneUnitChache.InstanceId, new G2U_AddOrUpdateUnitChache() { Unit =list }) as
            //                 U2G_AddOrUpdateUnitChache;
            await ETTask.CompletedTask;
            return null;
        }

        /// <summary>
        /// 删除UnitChache 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DelateUnitChache<T>() where T : Entity, IUnitChache
        {
        }
    }
}