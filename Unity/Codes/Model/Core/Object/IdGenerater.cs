using System;
using System.Runtime.InteropServices;

namespace ET
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IdStruct
    {
        public uint Time;    // 30bit
        public int Process;  // 18bit
        public ushort Value; // 16bit

        public long ToLong()
        {
            ulong result = 0;
            result |= this.Value;
            result |= (ulong) this.Process << 16;
            result |= (ulong) this.Time << 34;
            return (long) result;
        }

        public IdStruct(uint time, int process, ushort value)
        {
            this.Process = process;
            this.Time = time;
            this.Value = value;
        }

        public IdStruct(long id)
        {
            ulong result = (ulong) id; 
            this.Value = (ushort) (result & ushort.MaxValue);
            result >>= 16;
            this.Process = (int) (result & IdGenerater.Mask18bit);
            result >>= 18;
            this.Time = (uint) result;
        }

        public override string ToString()
        {
            return $"process: {this.Process}, time: {this.Time}, value: {this.Value}";
        }
    }
    /// <summary>
    /// 唯一id的结构体  （进程的唯一表示）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InstanceIdStruct
    {
        public uint Time;   // 当年开始的tick 28bit
        public int Process; // 18bit
        public uint Value;  // 18bit

        public long ToLong()
        {
            ulong result = 0;
            result |= this.Value;
            result |= (ulong)this.Process << 18;
            result |= (ulong) this.Time << 36;
            return (long) result;
        }

        public InstanceIdStruct(long id)
        {
            ulong result = (ulong) id;
            this.Value = (uint)(result & IdGenerater.Mask18bit);
            result >>= 18;
            this.Process = (int)(result & IdGenerater.Mask18bit);
            result >>= 18;
            this.Time = (uint)result;
        }

        public InstanceIdStruct(uint time, int process, uint value)
        {
            this.Time = time;
            this.Process = process;
            this.Value = value;
        }
        
        // 给SceneId使用
        public InstanceIdStruct(int process, uint value)
        {
            this.Time = 0;
            this.Process = process;
            this.Value = value;
        }

        public override string ToString()
        {
            return $"process: {this.Process}, value: {this.Value} time: {this.Time}";
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UnitIdStruct
    {
        public uint Time;        // 30bit 34年       从34位开始  共占34位（十进制最大的时间戳就是这个）
        public ushort Zone;      // 10bit 1024个区    从24位开始   共占10位
        public byte ProcessMode; // 8bit  Process % 256  一个区最多256个进程      从第16位开始，共占8位
        public ushort Value;     // 16bit 每秒每个进程最大16K个Unit   从第0位开始 攻占16位  

        /// <summary>
        /// long占 64位，从2020到目前的时间戳（秒） 应该不会占34位。 所以 用不完的 16+8+10+34（最大，大概率用不上34位的） = 68 位，
        /// </summary>
        /// <returns></returns>
        public long ToLong()
        {
            ulong result = 0;
            result |= this.Value;
            result |= (uint)this.ProcessMode << 16;
            result |= (ulong) this.Zone << 24;
            result |= (ulong) this.Time << 34;
            return (long) result;    //可能导致溢出或变负数 ,因为ulong表示的正整数比long多
        }
        
        public UnitIdStruct(int zone, int process, uint time, ushort value)
        {
            this.Time = time;
            this.ProcessMode = (byte)(process % 256);
            this.Value = value;
            this.Zone = (ushort)zone;
        }
        
        public UnitIdStruct(long id)
        {
            ulong result = (ulong) id;
            this.Value = (ushort)(result & ushort.MaxValue);
            result >>= 16;
            this.ProcessMode = (byte)(result & byte.MaxValue);
            result >>= 8;
            this.Zone = (ushort)(result & 0x03ff);
            result >>= 10;
            this.Time = (uint)result;
        }
                        
        public override string ToString()
        {
            return $"ProcessMode: {this.ProcessMode}, value: {this.Value} time: {this.Time}";
        }
        
        public static int GetUnitZone(long unitId)
        {
            int v = (int) ((unitId >> 24) & 0x03ff); // 取出10bit
            return v;
        }
    }

    public class IdGenerater: IDisposable
    {
        public const int Mask18bit = 0x03ffff;
        public static IdGenerater Instance = new IdGenerater();

        public const int MaxZone = 1024;
        
        private long epoch2020;
        private ushort value;
        private uint lastIdTime;

        
        private long epochThisYear;
        private uint instanceIdValue;
        private uint lastInstanceIdTime;
        
        
        private ushort unitIdValue;
        private uint lastUnitIdTime;

        public IdGenerater()
        {
            long epoch1970tick = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
            this.epoch2020 = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000 - epoch1970tick;
            this.epochThisYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000 - epoch1970tick;
            
            this.lastInstanceIdTime = TimeSinceThisYear();
            if (this.lastInstanceIdTime <= 0)
            {
                Log.Warning($"lastInstanceIdTime less than 0: {this.lastInstanceIdTime}");
                this.lastInstanceIdTime = 1;
            }
            this.lastIdTime = TimeSince2020();
            if (this.lastIdTime <= 0)
            {
                Log.Warning($"lastIdTime less than 0: {this.lastIdTime}");
                this.lastIdTime = 1;
            }
            this.lastUnitIdTime = TimeSince2020();
            if (this.lastUnitIdTime <= 0)
            {
                Log.Warning($"lastUnitIdTime less than 0: {this.lastUnitIdTime}");
                this.lastUnitIdTime = 1;
            }
        }

        public void Dispose()
        {
            this.epoch2020 = 0;
            this.epochThisYear = 0;
            this.value = 0;
        }

        private uint TimeSince2020()
        {
            uint a = (uint)((Game.TimeInfo.FrameTime - this.epoch2020) / 1000);
            return a;
        }
        
        private uint TimeSinceThisYear()
        {
            uint a = (uint)((Game.TimeInfo.FrameTime - this.epochThisYear) / 1000);
            return a;
        }
        
        public long GenerateInstanceId()
        {
            uint time = TimeSinceThisYear();

            if (time > this.lastInstanceIdTime)
            {
                this.lastInstanceIdTime = time;
                this.instanceIdValue = 0;
            }
            else
            {
                ++this.instanceIdValue;
                
                if (this.instanceIdValue > IdGenerater.Mask18bit - 1) // 18bit
                {
                    ++this.lastInstanceIdTime; // 借用下一秒
                    this.instanceIdValue = 0;

                    Log.Error($"instanceid count per sec overflow: {time} {this.lastInstanceIdTime}");
                }
            }

            InstanceIdStruct instanceIdStruct = new InstanceIdStruct(this.lastInstanceIdTime, Game.Options.Process, this.instanceIdValue);
            return instanceIdStruct.ToLong();
        }

        public long GenerateId()
        {
            uint time = TimeSince2020();

            if (time > this.lastIdTime)
            {
                this.lastIdTime = time;
                this.value = 0;
            }
            else
            {
                ++this.value;
                
                if (value > ushort.MaxValue - 1)
                {
                    this.value = 0;
                    ++this.lastIdTime; // 借用下一秒
                    Log.Error($"id count per sec overflow: {time} {this.lastIdTime}");
                }
            }
            
            IdStruct idStruct = new IdStruct(this.lastIdTime, Game.Options.Process, value);
            return idStruct.ToLong();
        }
        /// <summary>
        /// 传入区服，来创建 该unitid单位id
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public long GenerateUnitId(int zone)
        {
            if (zone > MaxZone)
            {
                throw new Exception($"zone > MaxZone: {zone}");
            }
            //从2020年到当前帧的时间 有多少秒 （大约是10位数）
            uint time = TimeSince2020();

            if (time > this.lastUnitIdTime)
            {
                this.lastUnitIdTime = time;
                this.unitIdValue = 0;
            }
            else
            {
                ++this.unitIdValue;
                
                if (this.unitIdValue > ushort.MaxValue - 1)
                {
                    this.unitIdValue = 0;
                    ++this.lastUnitIdTime; // 借用下一秒
                    Log.Error($"unitid count per sec overflow: {time} {this.lastUnitIdTime}");
                }
            }

            UnitIdStruct unitIdStruct = new UnitIdStruct(zone, Game.Options.Process, this.lastUnitIdTime, this.unitIdValue);
            return unitIdStruct.ToLong();
        }
    }
}