namespace ET
{

#if SERVER
    public class RankInfo: Entity, IAwake, IDestroy
#else
    [ChildType(typeof (RankInfoComponent))]
    public class RankInfo: Entity, IAwake, IDestroy
#endif
    {
        public long unitId;
  
        public string name;
        
        public long count;


    }
}