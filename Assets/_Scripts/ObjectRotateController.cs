using UnityEngine;

public class ObjectRotateZoom_Mobile : MonoBehaviour
{
    [Header("References")]
    public Transform target; // Vật thể cần xoay
    public float rotationSpeed = 0.3f;
    public float zoomSpeed = 0.05f;
    public float minDistance = 20f;   // khoảng cách gần nhất đến camera
    public float maxDistance = 100f;  // khoảng cách xa nhất đến camera

    private Vector2 lastTouchPos;
    private bool isRotating = false;

    private Camera cam;
    private float currentDistance;

    void Start()
    {
        cam = Camera.main;
        if (target != null)
            currentDistance = Vector3.Distance(cam.transform.position, target.position);
    }

    void Update()
    {
        if (target == null || cam == null) return;

        // --- PC TEST ---
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isRotating = true;
                lastTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && isRotating)
            {
                Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
                RotateTarget(delta.x, delta.y);
                lastTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isRotating = false;
            }

            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                ZoomTarget(-scroll * zoomSpeed * 20f);
            }
        }

        // --- MOBILE ---
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                lastTouchPos = t.position;
                isRotating = true;
            }
            else if (t.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 delta = t.deltaPosition;
                RotateTarget(delta.x, delta.y);
            }
            else if (t.phase == TouchPhase.Ended)
            {
                isRotating = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prev0 = t0.position - t0.deltaPosition;
            Vector2 prev1 = t1.position - t1.deltaPosition;

            float prevDist = (prev0 - prev1).magnitude;
            float currentDist = (t0.position - t1.position).magnitude;

            float diff = currentDist - prevDist;
            ZoomTarget(diff * zoomSpeed);
        }
    }

    void RotateTarget(float deltaX, float deltaY)
    {
        target.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.World);
        target.Rotate(Vector3.right, deltaY * rotationSpeed, Space.World);
    }

    void ZoomTarget(float zoomAmount)
    {
        // Tính hướng nhìn từ camera đến vật thể
        Vector3 direction = (target.position - cam.transform.position).normalized;

        // Cập nhật khoảng cách mới
        currentDistance = Mathf.Clamp(currentDistance - zoomAmount, minDistance, maxDistance);

        // Đặt lại vị trí của vật thể theo hướng nhìn camera
        target.position = cam.transform.position + direction * currentDistance;
    }
}
