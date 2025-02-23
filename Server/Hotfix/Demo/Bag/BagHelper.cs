
namespace ET
{
    public static class BagHelper
    {
        public static bool AddItemByConfig(this Unit unit, int itemConfigID)
        {
            var bagCpt = unit.GetComponent<BagComponent>();
            
            return bagCpt.AddItemByConfigID(itemConfigID);
        }
    }
}