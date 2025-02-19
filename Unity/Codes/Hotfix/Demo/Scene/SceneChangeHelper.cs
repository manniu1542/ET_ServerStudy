using ET.Adventure;

namespace ET
{
    public static class SceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene zoneScene, string sceneName, long sceneInstanceId)
        {
            zoneScene.RemoveComponent<AIComponent>();

            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            currentScenesComponent.Scene?.Dispose(); // 删除之前的CurrentScene，创建新的

            Scene currentScene = SceneFactory.CreateCurrentScene(sceneInstanceId, zoneScene.Zone, sceneName, currentScenesComponent);
            //在当前场景上 添加Unit组件
            UnitComponent unitComponent = currentScene.AddComponent<UnitComponent>();
            //添加去冒险的组件
            currentScene.AddComponent<AdventureComponent>();
            // 可以订阅这个事件中（ 切换场景 和 创建Loading界面 ）
            Game.EventSystem.Publish(new EventType.SceneChangeStart() { ZoneScene = zoneScene });

            // 等待CreateMyUnit的消息
            WaitType.Wait_CreateMyUnit waitCreateMyUnit = await zoneScene.GetComponent<ObjectWait>().Wait<WaitType.Wait_CreateMyUnit>();
            M2C_CreateMyUnit m2CCreateMyUnit = waitCreateMyUnit.Message;
            Unit unit =  UnitFactory.Create(currentScene, m2CCreateMyUnit.Unit);
       

            zoneScene.RemoveComponent<AIComponent>();

            //避免太快看不到 ui （loading）界面的打开，便于理解
            await TimerComponent.Instance.WaitAsync(1000);

            //关闭ui的loading页面
            Game.EventSystem.PublishAsync(new EventType.SceneChangeFinish() { ZoneScene = zoneScene, CurrentScene = currentScene }).Coroutine();

            // 通知等待场景切换的协程
            zoneScene.GetComponent<ObjectWait>().Notify(new WaitType.Wait_SceneChangeFinish());
        }
    }
}