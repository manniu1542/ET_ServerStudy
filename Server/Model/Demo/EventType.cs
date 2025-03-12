using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
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

        public struct MoveStart
        {
            public Unit Unit;
        }

        public struct MoveStop
        {
            public Unit Unit;
        }

        public struct UnitEnterSightRange
        {
            public AOIEntity A;
            public AOIEntity B;
        }

        public struct UnitLeaveSightRange
        {
            public AOIEntity A;
            public AOIEntity B;
        }

        /// <summary>
        /// 角色因为某件装备的操作导致属性更变
        /// </summary>
        public struct NumCpt_RoleEquipChange
        {
            public Unit Unit;
            public Item item;
            public RoleItemOp op;
        }

        /// <summary>
        /// 角色因为某件装备的操作导致属性更变
        /// </summary>
        public struct AdventureWinGameTask
        {
            public long UnitInstanceId;
            /// <summary>
            /// 管卡id
            /// </summary>
            public int adventureConfigId;

        }
        /// <summary>
        /// 角色因为某件装备的操作导致属性更变
        /// </summary>
        public struct ForgeProductionGameTask
        {
            public long UnitInstanceId;
            /// <summary>
            /// 传品id
            /// </summary>
            public int ProductionID;

        }
    }
}