using UnityEngine;

namespace Scripts.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        public GameObject BleedingEffectPrefab;
        
        public ImpactAudioData ImpactAudioData;

        private Transform bulletTransform;
        private Vector3 prePosition;

        private void Start()
        {
            bulletTransform = transform;
            prePosition = bulletTransform.position;
        }

        private void Update()
        {
            // 更新上一帧的位置
            prePosition = bulletTransform.position;
            
            // 子弹位移 = 速度 × 时间
            bulletTransform.Translate(0, 0, BulletSpeed * Time.deltaTime);
            // 判断两帧之间是否发生碰撞
            if (!Physics.Raycast(prePosition, (bulletTransform.position - prePosition).normalized,
                    out RaycastHit hitInfo, (bulletTransform.position - prePosition).magnitude)) return;
            // 如果有碰撞
            // 击中敌人造成伤害
            if (hitInfo.collider.tag.Equals("Enemy"))
            {
                Debug.Log("击中敌人");
                var enemy = hitInfo.collider.gameObject.GetComponent<EnemyFeature>();
                enemy.TakeDamage(20);
                
                // 出血特效
                var bleedingEffect = 
                    Instantiate(BleedingEffectPrefab,
                        hitInfo.point,
                        Quaternion.LookRotation(hitInfo.normal, Vector3.up));
                // 3s后特效自动消失
                Destroy(bleedingEffect, 0.5f);
            }
            else
            {
                // 非击中丧尸，创建子弹穿透、弹孔特效
                var bulletEffect = 
                    Instantiate(ImpactPrefab,
                        hitInfo.point,
                        Quaternion.LookRotation(hitInfo.normal, Vector3.up));
                // 3s后特效自动消失
                Destroy(bulletEffect, 3);
            }
            
            
            // 播放子弹撞击物体声音
            var audioWithTags =
                ImpactAudioData.ImpactAudioWithTags.Find(audioData => audioData.Tag.Equals(hitInfo.collider.tag));
            if (audioWithTags == null) return; // 受击物体如果没带tag就不播放声音
            int audioWithTagLength = audioWithTags.ImpactAudioClips.Count;
            AudioClip audioClip = audioWithTags.ImpactAudioClips[Random.Range(0, audioWithTagLength)];
            AudioSource.PlayClipAtPoint(audioClip, hitInfo.point);
        }
    }
}