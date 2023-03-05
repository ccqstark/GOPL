﻿using System.Collections;
using UnityEngine;

namespace Scripts.Weapon
{
    public class AssaultRifle : Firearms
    {
        private FPMouseLook mouseLook;

        protected override void Awake()
        {
            base.Awake();
            /*// 启动一个协程检查换弹动画是否完成
            StartCoroutine(CheckReloadAmmoAnimationEnd());
            // 启动一个协程检测到进入机瞄状态就放大视角
            StartCoroutine(DoAim());*/
            mouseLook = FindObjectOfType<FPMouseLook>();
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            CurrentAmmo--;

            MuzzleParticle.Play(); // 枪焰动画
            GunAnimator.Play("Fire", IsAiming ? 1 : 0, 0); // 枪械开火后坐力动画

            CreateBullet(); // 发射子弹
            FirearmsShootingAudioSource.Play();

            mouseLook.FiringForTest();

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
            // 设置换弹动画权重（半换弹、全换弹）
            GunAnimator.SetLayerWeight(2, 1);
            // 设置换弹动画触发器和声音
            if (tmpCurrentAmmo > 0)
            {
                GunAnimator.SetTrigger("ReloadLeft");
                FirearmsReloadAudioSource.clip = FirearmsAudioData.ReloadLeft;
            }
            else
            {
                GunAnimator.SetTrigger("ReloadOutOf");
                FirearmsReloadAudioSource.clip = FirearmsAudioData.ReloadOutOf;
            }

            // 播放换弹声音
            FirearmsReloadAudioSource.Play();
        }

        /*protected override void Aim()
        {
            GunAnimator.SetBool("Aim", IsAiming);
        }*/

        /*private void Update()
        {
            // 按下鼠标左键开火
            if (Input.GetMouseButton(0))
            {
                DoAttack();
            }

            // 按下 R 换弹药
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            // 按下鼠标右键进入机瞄状态
            if (Input.GetMouseButtonDown(1))
            {
                IsAiming = true;
                Aim();
            }

            // 松开鼠标右键退出机瞄状态
            if (Input.GetMouseButtonUp(1))
            {
                IsAiming = false;
                Aim();
            }
        }*/

        protected void CreateBullet()
        {
            // Instantiate方法用于创建一个新的游戏对象，并将它添加到场景中
            GameObject tmpBullet = Instantiate(BulletPrefab, MuzzlePoint.position, transform.rotation);
            // 添加tag
            tmpBullet.tag = "Bullet";
            // 子弹散射（改变子弹射出的角度）
            tmpBullet.transform.eulerAngles += CalculateSpreadOffset();
            var bulletScript = tmpBullet.AddComponent<Bullet>();
            bulletScript.ImpactPrefab = BulletImpactPrefab;
            bulletScript.ImpactAudioData = ImpactAudioData;
            bulletScript.BulletSpeed = 100;
        }

    }
}