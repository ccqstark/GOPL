using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Items;
using Scripts.Weapon;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public Firearms MainWeapon; // 主武器
    public Firearms SecondaryWeapon; // 副武器
    public Text AmmoCountTextLabel; // 子弹数显示UI
    
    public List<Firearms> Arms = new List<Firearms>(); // 用于存储不同武器模型(带手)
    public Transform WorldCameraTransform; // 枪械的Camera
    public float RaycastMaxDistance = 5; // 捡东西的最大检测距离
    public LayerMask CheckItemLayerMask; // 检查物品过滤用的层

    private Firearms carriedWeapon; // 当前手持的武器

    [SerializeField] private FPCharacterControllerMovement fpCharacterControllerMovement;
    private IEnumerator waitingForHolsterEndCoroutine;

    public Firearms GetCarriedWeapon() => carriedWeapon;

    // 更新武器信息 UI 的子弹数量
    private void UpdateAmmoInfo(int _ammo, int _remainingAmmo)
    {
        AmmoCountTextLabel.text = _ammo + "/" + _remainingAmmo;
    }
    
    private void Start()
    {
        // 主武器不为空
        if (MainWeapon)
        {
            carriedWeapon = MainWeapon;
            fpCharacterControllerMovement.SetAnimator(carriedWeapon.GunAnimator);
        }
    }

    private void Update()
    {
        if (PauseMenu.IsPaused) return;
        
        // 检查前方是否有物体
        CheckItem();

        if (!carriedWeapon) return;
        // 切换武器
        SwitchWeapon();

        // 按下鼠标左键开火
        if (Input.GetMouseButton(0))
        {
            carriedWeapon.HoldTrigger();
        }

        // 松开左键停止开火
        if (Input.GetMouseButtonUp(0))
        {
            carriedWeapon.ReleaseTrigger();
        }

        // 按下 R 换弹药
        if (Input.GetKeyDown(KeyCode.R))
        {
            carriedWeapon.ReloadAmmo();
        }

        // 按下鼠标右键进入机瞄状态
        if (Input.GetMouseButtonDown(1))
        {
            carriedWeapon.Aiming(true);
        }

        // 松开鼠标右键退出机瞄状态
        if (Input.GetMouseButtonUp(1))
        {
            carriedWeapon.Aiming(false);
        }
        
        // 更新子弹数
        UpdateAmmoInfo(carriedWeapon.GetCurrentAmmo(), 
            carriedWeapon.GetCurrentMaxAmmoCarried());
    }

    private void CheckItem()
    {
        // 用射线检测视野内可互动物体
        bool isItem = Physics.Raycast(WorldCameraTransform.position,
            WorldCameraTransform.forward,
            out RaycastHit raycastHitInfo,
            RaycastMaxDistance,
            CheckItemLayerMask);
        if (isItem)
        {
            // 按 F 拾取武器
            if (Input.GetKeyDown(KeyCode.F))
            {
                var hasItem = raycastHitInfo.collider.TryGetComponent(out BaseItem tmpBaseItem);
                if (hasItem)
                {
                    PickupWeapon(tmpBaseItem);
                    PickupAttachment(tmpBaseItem);
                }
            }
        }
    }
    
    // 拾起武器
    private void PickupWeapon(BaseItem tmpBaseItem)
    {
        if (tmpBaseItem is FirearmsItem tmpFirearmsItem)
        {
            foreach (Firearms tmpArm in Arms)
            {
                if (tmpFirearmsItem.ArmsName.CompareTo(tmpArm.name) != 0) continue;
                // 替换主武器
                if (MainWeapon == null || (carriedWeapon == MainWeapon && SecondaryWeapon != null))
                {
                    MainWeapon = tmpArm;
                }
                else
                {
                    SecondaryWeapon = tmpArm;
                }
                // 捡起武器后设置为当前手持武器
                SetCarriedWeapon(tmpArm);
            }
        }
    }

    // 拾起武器配件
    private void PickupAttachment(BaseItem _baseItem)
    {
        if (!(_baseItem is AttachmentItem tmpAttachmentItem)) return;
        switch (tmpAttachmentItem.CurrentAttachmentType)
        {
            case AttachmentItem.AttachmentType.Scope:
                foreach (Firearms.ScopeInfo tmpScopeInfo in carriedWeapon.ScopeInfos)
                {
                    if (tmpScopeInfo.ScopeName.CompareTo(tmpAttachmentItem.ItemName) != 0)
                    {
                        tmpScopeInfo.ScopeGameObject.SetActive(false);
                        continue;
                    }
                    tmpScopeInfo.ScopeGameObject.SetActive(true);
                    carriedWeapon.BaseIronSight.ScopeGameObject.SetActive(false);
                    carriedWeapon.SetCarriedScope(tmpScopeInfo);
                }
                break;
            case AttachmentItem.AttachmentType.Other:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // 切换武器
    private void SwitchWeapon()
    {
        // 数字键 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (MainWeapon == null) return;
            // 更换为主武器
            if (carriedWeapon == MainWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("Holster");
            }
            else
            {
                SetCarriedWeapon(MainWeapon);
            }
        }
        // 数字键 2
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SecondaryWeapon == null) return;
            // 更换为副武器
            if (carriedWeapon == SecondaryWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimator.SetTrigger("Holster");
            }
            else
            {
                SetCarriedWeapon(SecondaryWeapon);
            }
        }
        // 鼠标滚轮
        else if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (carriedWeapon == null || MainWeapon == null || SecondaryWeapon == null) return;
            StartWaitingForHolsterEndCoroutine();
            carriedWeapon.GunAnimator.SetTrigger("Holster");
        }
    }

    // 启动协程来判断收枪动画是否播放完成，然后进行枪支的切换
    private void StartWaitingForHolsterEndCoroutine()
    {
        if (waitingForHolsterEndCoroutine == null)
        {
            waitingForHolsterEndCoroutine = WaitingForHolsterEnd();
        }

        StartCoroutine(waitingForHolsterEndCoroutine);
    }

    private IEnumerator WaitingForHolsterEnd()
    {
        while (true)
        {
            AnimatorStateInfo animatorStateInfo = carriedWeapon.GunAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsTag("holster"))
            {
                if (animatorStateInfo.normalizedTime >= 0.9f)
                {
                    // 隐藏现在这把枪
                    // 替换目标枪支
                    var targetWeapon = carriedWeapon == MainWeapon ? SecondaryWeapon : MainWeapon;
                    SetCarriedWeapon(targetWeapon);
                    waitingForHolsterEndCoroutine = null;
                    yield break;
                }
            }

            yield return null;
        }
    }

    protected void SetCarriedWeapon(Firearms targetWeapon)
    {
        if (carriedWeapon)
        {
            carriedWeapon.gameObject.SetActive(false);
        }
        carriedWeapon = targetWeapon;
        carriedWeapon.gameObject.SetActive(true);
        fpCharacterControllerMovement.SetAnimator(carriedWeapon.GunAnimator);
    }
}