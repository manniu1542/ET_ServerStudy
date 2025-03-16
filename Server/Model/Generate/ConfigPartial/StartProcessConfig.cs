using System.Net;

namespace ET
{
    public partial class StartProcessConfig
    {
        private IPEndPoint innerIPPort;

        //改进程的唯一id，最多256个进程。（18位-24位之间，2进制的左移，  一个正数在long（64位）中最多32位）
        public long SceneId;

        /// <summary>
        /// 内网的ip和端口,用来给服务器内部通信使用
        /// </summary>
        public IPEndPoint InnerIPPort
        {
            get
            {
                if (this.innerIPPort == null)
                {
                    this.innerIPPort = NetworkHelper.ToIPEndPoint($"{this.InnerIP}:{this.InnerPort}");
                }

                return this.innerIPPort;
            }
        }

        /// <summary>
        /// 这个进程的内网ip （是 该进程 所在的机器 当前的内网 ip）
        /// </summary>
        public string InnerIP => this.StartMachineConfig.InnerIP;

        /// <summary>
        /// 这个进程的外网ip （是 该进程 所在的机器 当前的外网 ip）
        /// </summary>
        public string OuterIP => this.StartMachineConfig.OuterIP;

        public StartMachineConfig StartMachineConfig => StartMachineConfigCategory.Instance.Get(this.MachineId);

        public override void AfterEndInit()
        {
            InstanceIdStruct instanceIdStruct = new InstanceIdStruct((int)this.Id, 0);
            this.SceneId = instanceIdStruct.ToLong();
            Log.Info($"StartProcess info: {this.MachineId} {this.Id} {this.SceneId}");
        }
    }
}