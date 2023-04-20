namespace Scripts.Items
{
    public class SuppliesItem : BaseItem
    {
        public enum SuppliesType
        {
            Ammo,
            MedicalKit
        }

        public SuppliesType CurrentSuppliesType;

        public int Value;
    }
}
