﻿namespace Scripts.Items
{
    public class FirearmsItem : BaseItem
    {
        public enum FirearmsType
        {
            AssaultRifle,
            HandGun,
            SMG,
            Sniper,
        }

        public FirearmsType CurrentFirearmsType;
        
        public string ArmsName;
    }
}