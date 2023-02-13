using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapon
{
    public abstract class Firearms : MonoBehaviour, IWeapon
    {
        public Camera EyeCamera; // 相机
        public Camera GunCamera;

        public GameObject CrosshairUI; // 准心UI
        
        public Transform MuzzlePoint; // 枪口位置
        public Transform CasingPoint; // 抛壳位置

        public ParticleSystem MuzzleParticle; // 枪口粒子特效
        public ParticleSystem CasingParticle; // 抛壳粒子特效

        public AudioSource FirearmsShootingAudioSource; // 开枪声
        public AudioSource FirearmsReloadAudioSource; // 换弹声
        public FirearmsAudioData FirearmsAudioData; // 枪械声音音源
        public ImpactAudioData ImpactAudioData; // 子弹撞击物体的声音

        public float FireRate; // 射速 (1s发射的子弹数)

        public int AmmoInMag = 30; // 一个弹匣子弹数量
        public int MaxAmmoCarried = 120; // 最大子弹携带数量
        public GameObject BulletPrefab; // 子弹预制体
        public GameObject BulletImpactPrefab; // 子弹撞击效果预制体

        public float SpreadAngle; // 散射角度

        public List<ScopeInfo> ScopeInfos; // 可用瞄具列表
        public ScopeInfo BaseIronSight; // 基础瞄具
        protected ScopeInfo rigoutScopeInfo; // 当前装备的瞄具

        public int GetCurrentAmmo => CurrentAmmo;
        public int GetCurrentMaxAmmoCarried => CurrentMaxAmmoCarried;
            
        protected int CurrentAmmo; // 当前弹匣里的子弹数量
        protected int CurrentMaxAmmoCarried; // 当前携带最大子弹数

        protected float LastFireTime; // 上一次开火时间

        internal Animator GunAnimator; // 动画
        protected AnimatorStateInfo GunStateInfo; // 动画播放状态

        protected float EyeOriginFOV;
        protected float GunOriginFov;
        protected bool IsAiming; // 是否处于瞄准状态
        protected bool IsHoldingTrigger; // 是否孔扣动扳机状态

        private Vector3 originalEyePosition;
        protected Transform gunCameraTransform;
        
        protected virtual void Awake()
        {
            // 初始化子弹数量
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            // 初始化动作控制器
            GunAnimator = GetComponent<Animator>();
            // 初始化音源
            FirearmsShootingAudioSource.clip = FirearmsAudioData.ShootingAudio;
            // FOV
            EyeOriginFOV = EyeCamera.fieldOfView;
            GunOriginFov = GunCamera.fieldOfView;
            // 启动一个协程检查换弹动画是否完成
            StartCoroutine(CheckReloadAmmoAnimationEnd());
            // 启动一个协程检测到进入机瞄状态就放大视角
            StartCoroutine(DoAim());
            gunCameraTransform = GunCamera.transform;
            originalEyePosition = gunCameraTransform.localPosition;
            // 默认瞄具赋值
            rigoutScopeInfo = BaseIronSight;
        }

        public void DoAttack()
        {
            Shooting();
        }

        protected abstract void Shooting();
        protected abstract void Reload();

        // protected abstract void Aim();

        // 是否可开枪
        protected bool IsAllowShooting()
        {
            return Time.time - LastFireTime > 1 / FireRate;
        }

        // 计算散射偏移
        protected Vector3 CalculateSpreadOffset()
        {
            // todo: 有点bug，这样的效果应该是相反的
            // 瞄准时，FOV变小，子弹散射程度变小
            float tmp_SpreadPercent = SpreadAngle / EyeCamera.fieldOfView;
            // insideUnitCircle 返回一个单位圆范围内的随机点
            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }

        // 检查一个换弹动画是否播放完毕
        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        // 装弹成功
                        // 补满一个弹匣所需的弹药数
                        var suppleAmmo = AmmoInMag - CurrentAmmo;
                        // 需要判断备用弹药是否充足
                        if (CurrentMaxAmmoCarried >= suppleAmmo)
                        {
                            CurrentAmmo = AmmoInMag;
                            CurrentMaxAmmoCarried -= suppleAmmo;
                        }
                        else
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                            CurrentMaxAmmoCarried = 0;
                        }
                    }
                }
            }
        }

        // 瞄准时放大视角
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                float tmpEyeCurrentFOV = 0;
                EyeCamera.fieldOfView =
                    Mathf.SmoothDamp(EyeCamera.fieldOfView,
                    IsAiming ? rigoutScopeInfo.EyeFov : EyeOriginFOV,
                    ref tmpEyeCurrentFOV, 
                    Time.deltaTime * 2);
                float tmpGunCurrentFOV = 0;
                GunCamera.fieldOfView = 
                    Mathf.SmoothDamp(GunCamera.fieldOfView,
                    IsAiming ? rigoutScopeInfo.GunFov : GunOriginFov,
                    ref tmpGunCurrentFOV, 
                    Time.deltaTime * 2);

                // GunCamera 平滑过渡到对应瞄具的机瞄视野的位置
                Vector3 tmpRefPosition = Vector3.zero;
                gunCameraTransform.localPosition = Vector3.SmoothDamp(gunCameraTransform.localPosition,
                    IsAiming ? rigoutScopeInfo.GunCameraPosition : originalEyePosition,
                    ref tmpRefPosition,
                    Time.deltaTime * 2);
                
                // 瞄准时隐藏十字准心
                CrosshairUI.SetActive(!IsAiming);
            }
        }

        internal void Aiming(bool _isAiming)
        {
            IsAiming = _isAiming;
            GunAnimator.SetBool("Aim", IsAiming);
        }

        internal void SetCarriedScope(ScopeInfo _scopeInfo)
        {
            rigoutScopeInfo = _scopeInfo;
        }
        
        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }

        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }

        internal void ReloadAmmo()
        {
            Reload();
        }
        
        // 倍镜信息
        [System.Serializable]
        public class ScopeInfo
        {
            public string ScopeName; // 倍镜名称
            public GameObject ScopeGameObject; // 倍镜游戏对象
            public float EyeFov; // 眼部视野
            public float GunFov; // 举枪视野
            public Vector3 GunCameraPosition; // 相机的位置
        }
    }
}