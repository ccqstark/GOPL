﻿using System.Collections;
using UnityEngine;

namespace Scripts.Weapon
{
    public class HandGun : Firearms
    {
        private FPMouseLook mouseLook;
        private FirearmsMagReloadAudioData firearmsMagReloadAudioData;

        protected override void Awake()
        {
            base.Awake();
            mouseLook = FindObjectOfType<FPMouseLook>();
            firearmsMagReloadAudioData = (FirearmsMagReloadAudioData)FirearmsAudioData;
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            CurrentAmmo--;

            MuzzleParticle.Play(); // 枪焰动画
            GunAnimator.Play("Fire", IsAiming ? 1 : 0, 0); // 枪械开火后坐力动画

            CreateBullet(); // 发射子弹
            FirearmsShootingAudioSource.clip = firearmsMagReloadAudioData.ShootingAudio;
            FirearmsShootingAudioSource.Play();

            // 震动视角
            mouseLook.VibratingPerspective();

            CasingParticle.Play(); // 抛壳动画
            LastFireTime = Time.time;
        }

        protected override void Reload()
        {
            // 弹匣满时、没有备用弹药时不进行换弹
            if (CurrentAmmo == AmmoInMag || CurrentMaxAmmoCarried == 0) return;
            // 避免换弹时可以开枪，先把子弹退回备用弹药
            var tmpCurrentAmmo = CurrentAmmo;
            CurrentAmmo = 0;
            CurrentMaxAmmoCarried += tmpCurrentAmmo;
            // 设置换弹动画触发器和声音
            if (tmpCurrentAmmo > 0)
            {
                GunAnimator.SetTrigger("ReloadLeft");
                FirearmsReloadAudioSource.clip = firearmsMagReloadAudioData.ReloadLeft;
            }
            else
            {
                GunAnimator.SetTrigger("ReloadOutOf");
                FirearmsReloadAudioSource.clip = firearmsMagReloadAudioData.ReloadOutOf;
            }

            // 播放换弹声音
            FirearmsReloadAudioSource.Play();
        }
        
        protected void CreateBullet()
        {
            // Instantiate方法用于创建一个新的游戏对象，并将它添加到场景中
            GameObject tmpBullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, transform.rotation);
            // 添加tag
            tmpBullet.tag = "Bullet";
            // 子弹散射（改变子弹射出的角度）
            tmpBullet.transform.eulerAngles += CalculateSpreadOffset();
            var bulletScript = tmpBullet.AddComponent<Bullet>();
            bulletScript.ImpactPrefab = BulletImpactPrefab;
            bulletScript.ImpactAudioData = ImpactAudioData;
            bulletScript.BleedingEffectPrefab = BleedingEffectPrefab;
            bulletScript.BulletSpeed = 100;
        }

    }
}