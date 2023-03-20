using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [CreateAssetMenu(menuName = "FPS/Weapon Icon Data")]
    public class WeaponIconData : ScriptableObject
    {
        public List<WeaponIconDataElement> WeaponLogoDataList;
    }

    [System.Serializable]
    public class WeaponIconDataElement
    {
        public string Name;
        public Sprite WeaponIconSprite;
    }
}
