using System;
using System.Collections.Generic;

namespace ET
{
    [Timer(TimerType.UnitSaveDBTimerCheck)]
    public class UnitSaveDBTimeCheck: ATimer<UnitSaveDBComponent>
    {
        public override void Run(UnitSaveDBComponent self)
        {
            try
            {
                if (self == null || self.IsDisposed || self.Parent == null) return;
                //场景已经被销毁了
                if (self.DomainScene() == null) return;

                self.CheckSaveDB();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UnitSaveDBComponentAwakeSystem: AwakeSystem<UnitSaveDBComponent>
    {
        public override void Awake(UnitSaveDBComponent self)
        {
            self.hsNeedSaveDBCpt = new HashSet<Type>();
            self.TimerId = Game.Scene.GetComponent<TimerComponent>()
                    .NewRepeatedTimer(self.timeInterval, TimerType.UnitSaveDBTimerCheck, self);
        }
    }

    public class UnitSaveDBComponentDestroySystem: DestroySystem<UnitSaveDBComponent>
    {
        public override void Destroy(UnitSaveDBComponent self)
        {
            Game.Scene.GetComponent<TimerComponent>().Remove(ref self.TimerId);
            self.hsNeedSaveDBCpt.Clear();
            self.hsNeedSaveDBCpt = null;
        }
    }

    public class UnitAddComponentSystem: AddComponentSystem<Unit>
    {
        public override void AddComponent(Unit unit, Entity cpt)
        {
            Type t = cpt.GetType();
            if (typeof (IUnitChache).IsAssignableFrom(t))
            {
                unit.GetComponent<UnitSaveDBComponent>()?.AddSaveDBType(t);
            }
        }
    }

    public class UnitGetComponentSystem: GetComponentSystem<Unit>
    {
        public override void GetComponent(Unit unit, Entity cpt)
        {
            Type t = cpt.GetType();
            if (typeof (IUnitChache).IsAssignableFrom(t))
            {
                unit.GetComponent<UnitSaveDBComponent>()?.AddSaveDBType(t);
            }
        }
    }

    [FriendClass(typeof (UnitSaveDBComponent))]
    public static class UnitSaveDBComponentSystem
    {
        public static void AddSaveDBType(this UnitSaveDBComponent self, Type t)
        {
            self.hsNeedSaveDBCpt.Add(t);
        }

        public static void CheckSaveDB(this UnitSaveDBComponent self)
        {
            if (self.hsNeedSaveDBCpt.Count <= 0) return;

            Unit unit = self.Parent as Unit;

            var message = new G2U_AddOrUpdateUnitChache() { UnitId = unit.Id };
            message.EntityType.Add(typeof (Unit).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unit));
            foreach (var cpt in self.hsNeedSaveDBCpt)
            {
                if (typeof (IUnitChache).IsAssignableFrom(cpt))
                {
                    var classCpt = unit.GetComponent(cpt);
                    if (classCpt != null)
                    {
                        message.EntityType.Add(cpt.FullName);
                        message.EntityBytes.Add(MongoHelper.ToBson(classCpt));
                    }
                }
            }

            var sceneUnitChache = StartSceneConfigCategory.Instance.GetUnitChacheConfig(unit.Id);
            MessageHelper.CallActor(sceneUnitChache.InstanceId, message).Coroutine();
            self.hsNeedSaveDBCpt.Clear();
        }
    }
}