using UnityEngine;

namespace ET
{
    [FriendClass(typeof (GlobalComponent))]
    public class AfterUnitCreate_CreateUnitView: AEventAsync<EventType.AfterUnitCreate>
    {
        protected override async ETTask Run(EventType.AfterUnitCreate args)
        {
            // Unit View层
            // 这里可以改成异步加载，demo就不搞了
            await ResourcesComponent.Instance.LoadBundleAsync("knight.unity3d");
            GameObject bundleGameObject = ResourcesComponent.Instance.GetAsset("knight.unity3d", "Knight") as GameObject;

            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, GlobalComponent.Instance.Unit, true);

            go.transform.SetParent(GlobalComponent.Instance.Unit, false);

            go.transform.position = Vector3.zero;
            args.Unit.AddComponent<GameObjectComponent>().GameObject = go;
            args.Unit.AddComponent<AnimatorComponent>();

            await ETTask.CompletedTask;
        }
    }
}