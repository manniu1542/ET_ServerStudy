using UnityEngine;

namespace ET
{
    [FriendClass(typeof (Item))]
    public static class ItemViewHelper
    {
        public static Color ItemQualityColor(this Item item)
        {
            ItemQulityType quality = (ItemQulityType)item.quality;
            switch (quality)
            {
                case ItemQulityType.Normal:
                    return Color.white;
                case ItemQulityType.Good:
                    return Color.green;
                case ItemQulityType.Excellent:
                    return Color.blue;
                case ItemQulityType.Epic:
                    return Color.magenta;
                case ItemQulityType.Legendary:
                    return Color.red;
            }

            return Color.black;
        }
    }
}