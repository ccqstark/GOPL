using UnityEngine;

/*
 * 第一人称视角移动
 */
public class FPMouseLook : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Transform CharacterTransform; // 传入角色整体组件对象
    private Vector3 cameraRotation; // 缓存上一帧的值

    public float MouseSensitivity; // 鼠标灵敏度
    public Vector2 MaximumAngle; // 最大角度限制

    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;

    public float RecoilFadeOutTime = 0.3f;
    private float currentRecoilTime;
    private Vector2 currentRecoil;
    private CameraSpring cameraSpring;

    private void Start()
    {
        // 隐藏鼠标
        Cursor.visible = false;
        cameraTransform = transform;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    private void Update()
    {
        // 获取鼠标两个方向的运动增量
        var tmpMouseX = Input.GetAxis("Mouse X");
        var tmpMouseY = Input.GetAxis("Mouse Y");

        // 修改转动角度（乘以鼠标灵敏度）
        cameraRotation.y += tmpMouseX * MouseSensitivity;
        cameraRotation.x -= tmpMouseY * MouseSensitivity;

        // 枪的后坐力
        CalculateRecoilOffset();
        cameraRotation.y += currentRecoil.y;
        cameraRotation.x -= currentRecoil.x;
        
        // 限制转动角度
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, MaximumAngle.x, MaximumAngle.y);

        // 此方法用于返回欧拉角Vector3(x,y,z)对应的四元数Quaternion
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
        // 角色身体也随着视角转动
        CharacterTransform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
        // 获得指定时间点的曲线值
        float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
        // 计算插值
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }

    public void FiringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}