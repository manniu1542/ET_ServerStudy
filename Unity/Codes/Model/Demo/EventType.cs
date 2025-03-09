using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct SceneChangeStart
        {
            public Scene ZoneScene;
        }
        
        
        public struct SceneChangeFinish
        {
            public Scene ZoneScene;
            public Scene CurrentScene;
        }

        public class ChangePosition: DisposeObject
        {
            public static readonly ChangePosition Instance = new ChangePosition();

            public Unit Unit;
            public WrapVector3 OldPos = new WrapVector3();

            // 因为是重复利用的，所以用完PublishClass会调用Dispose
            public override void Dispose()
            {
                this.Unit = null;
            }
        }
    

        public class ChangeRotation: DisposeObject
        {
            public static readonly ChangeRotation Instance = new ChangeRotation();
                   
            public Unit Unit;
                   
            // 因为是重复利用的，所以用完PublishClass会调用Dispose
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public struct PingChange
        {
            public Scene ZoneScene;
            public long Ping;
        }
        
        public struct AfterCreateZoneScene
        {
            public Scene ZoneScene;
        }
        
        public struct AfterCreateCurrentScene
        {
            public Scene CurrentScene;
        }
        
        public struct AfterCreateLoginScene
        {
            public Scene LoginScene;
        }

        public struct AppStartInitFinish
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            public Scene ZoneScene;
        }

        public struct LoadingBegin
        {
            public Scene Scene;
        }

        public struct LoadingFinish
        {
            public Scene Scene;
        }

        public struct EnterMapFinish
        {
            public Scene ZoneScene;
        }

        public struct AfterUnitCreate
        {
            public Unit Unit;
        }
        
        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }
        
        public struct NumericSpwanUI
        {
            public Scene ZoneScene;
        }
        public struct AdventureRefreshUI
        {
            public Scene ZoneScene;
        }
        public struct AdventureAlive
        {
            public Scene ZoneScene;
            public long unitId;
        }
        public struct AdventureStartReset
        {
            public Scene ZoneScene;
        }
        /// <summary>
        /// 角色跟角色之间的战斗 
        /// </summary>
        public struct AdventureUnit2UnitBattle
        {
            public Scene ZoneScene;
            /// <summary>
            /// 攻击者
            /// </summary>
            public long AttackerUnitID;
            //被攻击者
            public long TartgetUnitID;
            
        }
        /// <summary>
        /// 角色跟角色之间的战斗 
        /// </summary>
        public struct AdventureUnit2UnitBattleView
        {
            public Scene ZoneScene;
            /// <summary>
            /// 攻击者
            /// </summary>
            public long AttackerUnitID;
            //被攻击者
            public long TartgetUnitID;
            
        }
        public struct AdventureBattleRoundEnd
        {
            public Scene ZoneScene;
            public AdventureBattleRoundState state;
           
        }
        
        public struct SetUpdateHeadHp
        {
            public Scene ZoneScene;
            public long unitId;
            public bool isShow;
        }
        public struct CreateUnitDamageValue
        {
            public Scene ZoneScene;
            public long unitId;
            public long damgeValue;
        }

        public struct RefreshForgeRedPoint
        {
            public Scene ZoneScene;
        }
    }
}