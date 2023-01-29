namespace Scripts.Items
{
    public class FirearmsItem : BaseItem
    {
        public enum FirearmsType
        {
            AssaultRifle,
            HandGun,
        }

        public FirearmsType CurrentFirearmsType;
        
        public string ArmsName;
    }
}