using System;
using UnityEngine;

namespace ET
{
    [ActorMessageHandler]
    [FriendClassAttribute(typeof(ET.BagComponent))]
    public class M2M_UnitTransferRequestHandler : AMActorRpcHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
    {
        protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response, Action reply)
        {
            await ETTask.CompletedTask;
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = request.Unit;

            unitComponent.Add(unit);
            //里面应该有 属性组件NumCpt的 
            foreach (Entity entity in request.Entitys)
            {
                unit.AddComponent(entity);
            }

            // unit.AddComponent<MoveComponent>();
            // unit.AddComponent<PathfindingComponent, string>(scene.Name);
            // unit.Position = new Vector3(-10, 0, -10);
            //
            unit.AddComponent<MailBoxComponent>();
            //帮助unit 发送属性改变的消息组件
            unit.AddComponent<NumericNoticeComponent>();
            //数据库定时 检查/保存unit的属性
            unit.AddComponent<UnitSaveDBComponent>();
            //战斗检查
            unit.AddComponent<AdventureCheckComponent>();

            // 通知客户端创建My Unit
            M2C_CreateMyUnit m2CCreateUnits = new M2C_CreateMyUnit();
            m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
            MessageHelper.SendToClient(unit, m2CCreateUnits);

            ItemHelper.AsyncAllBagItemData(unit);

            ItemHelper.AsyncAllRoleEqpItemData(unit);
            ForgeHelper.AsyncForgeProducion(unit);
           
            
            
            // 加入aoi
            // unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);

            response.NewInstanceId = unit.InstanceId;

            reply();
        }
    }
}