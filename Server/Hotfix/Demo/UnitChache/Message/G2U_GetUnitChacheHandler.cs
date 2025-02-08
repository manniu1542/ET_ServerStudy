using System;
using System.Collections.Generic;

namespace ET
{
    [FriendClass(typeof(UnitChacheComponent))]
    [ActorMessageHandler]
    public class G2U_GetUnitChacheHandler: AMActorRpcHandler<Scene, G2U_GetUnitChache, U2G_GetUnitChache>
    {
        protected override async ETTask Run(Scene scene, G2U_GetUnitChache request, U2G_GetUnitChache response, Action reply)
        {
            //请求的服务器类型
            SceneType st = scene.SceneType;
            if (st != SceneType.UnitChache)
            {   
                response.Error = ErrorCode.ERR_SwitchSceneSever;
                reply();

                Log.Error("请求的场景服务器错误！" + st);
                return;
            }

            UnitChacheComponent ucc = scene.GetComponent<UnitChacheComponent>();
    
            Dictionary<string, Entity> dicCpt = MonoPool.Instance.Fetch<Dictionary<string, Entity>>();
            try
            {
                //从 UnitChacheComponent 中获取组件 ，判断请求的 组件列表是否有值，没有的话 默认获取 该UnitID的 所有继承IUnitChache的组件
                if (request.listComponentName.Count == 0)
                {
                    foreach (var type in ucc.listUnitChacheKey)
                    {
                        dicCpt.Add(type,null);
                    }
                }
                else //有的话 获取指定的 组件类型
                {
                    foreach (var type in request.listComponentName)
                    {
                        dicCpt.Add(type,null);
                    }
                }

                foreach (var item in dicCpt)
                {
                    var entity = await ucc.Get(request.UnitId, item.Key);
                    dicCpt.Add(item.Key,entity);
                    
                }
                
                response.listComponentName.AddRange(dicCpt.Keys);
                response.EntityType.AddRange(dicCpt.Values);
                
                
            }
            finally
            {
                dicCpt.Clear();
                MonoPool.Instance.Recycle(dicCpt);
            }
            
            reply();
        }
        
    
    }
}