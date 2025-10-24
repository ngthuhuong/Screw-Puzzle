using UnityEngine;

public class ObjectRotateController : MonoBehaviour
{
    public Transform target;          // Vật thể sẽ xoay
    public Camera mainCamera;         // Camera nhìn vật thể
    public float rotationSpeed = 3f;  // Tốc độ xoay
    public float zoomSpeed = 0.5f;    // Tốc độ zoom
    private float yaw;
    private float pitch;

    // Các biến zoom
    private float minZoom;
    private float maxZoom;
    private float mediumZoom;
    private float currentDistance;

    void Start()
    {
        if (target == null || mainCamera == null)
        {
            Debug.LogError("Target hoặc Camera chưa được gán!");
            enabled = false;
            return;
        }

        // 📏 Tính khoảng cách ban đầu giữa camera và target
        mediumZoom = Vector3.Distance(mainCamera.transform.position, target.position);

        // 🧮 Thiết lập min, max dựa theo medium ± 5f
        minZoom = mediumZoom - 5f;
        maxZoom = mediumZoom + 5f;

        // Khoảng cách hiện tại = medium
        currentDistance = mediumZoom;

        // Lưu góc xoay ban đầu của target
        yaw = target.eulerAngles.y;
        pitch = target.eulerAngles.x;

        // Đảm bảo camera nhìn vào vật thể
        mainCamera.transform.LookAt(target.position);
    }

    void Update()
    {
        // Xoay vật thể
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            RotateTarget(touch.deltaPosition.x, touch.deltaPosition.y);
        }
        else if (Input.GetMouseButton(0))
        {
            RotateTarget(Input.GetAxis("Mouse X") * 10f, Input.GetAxis("Mouse Y") * 10f);
        }

        // Zoom bằng pinch hoặc cuộn chuột
        if (Input.touchCount == 2)
        {
            HandleTouchZoom();
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            HandleScrollZoom(Input.mouseScrollDelta.y);
        }
    }

    void LateUpdate()
    {
        // Cập nhật vị trí camera dựa trên khoảng cách hiện tại
        Vector3 dir = (mainCamera.transform.position - target.position).normalized;
        mainCamera.transform.position = target.position + dir * currentDistance;
        mainCamera.transform.LookAt(target.position);
    }

    void RotateTarget(float deltaX, float deltaY)
    {
        yaw -= deltaX * rotationSpeed;
        pitch -= deltaY * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        target.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleTouchZoom()
    {
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        Vector2 prev0 = t0.position - t0.deltaPosition;
        Vector2 prev1 = t1.position - t1.deltaPosition;

        float prevMag = (prev0 - prev1).magnitude;
        float currentMag = (t0.position - t1.position).magnitude;

        float diff = prevMag - currentMag;
        currentDistance += diff * zoomSpeed * Time.deltaTime;
        currentDistance = Mathf.Clamp(currentDistance, minZoom, maxZoom);
    }

    void HandleScrollZoom(float scrollDelta)
    {
        currentDistance -= scrollDelta * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minZoom, maxZoom);
    }
}
