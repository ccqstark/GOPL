using UnityEngine;

namespace Scripts.Items
{
    public abstract class BaseItem : MonoBehaviour
    {
        public enum ItemType
        {
            Firearms,
            Attachment,
            Other
        }

        public ItemType CurrentItemType;

        public int ItemId;

        public string ItemName;
    }
    
}