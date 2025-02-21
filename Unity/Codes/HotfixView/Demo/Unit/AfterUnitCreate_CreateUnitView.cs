using UnityEngine;

namespace ET
{
    [FriendClass(typeof (GlobalComponent))]
    [FriendClassAttribute(typeof (ET.Unit))]
    public class AfterUnitCreate_CreateUnitView: AEventAsync<EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventType.AfterUnitCreate args)
        {
            var config = UnitConfigCategory.Instance.Get(args.Unit.ConfigId);
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            await ResourcesComponent.Instance.LoadBundleAsync(config.PrefabName + ".unity3d");
            GameObject bundleGameObject = ResourcesComponent.Instance.GetAsset(config.PrefabName + ".unity3d", config.PrefabName) as GameObject;

            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, GlobalComponent.Instance.Unit, true);

            var goCpt = args.Unit.AddComponent<GameObjectComponent>();
            goCpt.GameObject = go;
            goCpt.SpriteRenderer = go.GetComponent<SpriteRenderer>();

            go.transform.SetParent(GlobalComponent.Instance.Unit, false);

            args.Unit.Position = args.Unit.Type == UnitType.Player? Vector3.left * 1.5f : Vector3.right * 1.5f;

            args.Unit.AddComponent<AnimatorComponent>();
            args.Unit.AddComponent<HeadHpViewComponent>();

            await ETTask.CompletedTask;
        }
    }
}