using UnityEngine;

public class ObjectRotateZoom_Mobile : MonoBehaviour
{
    [Header("References")]
    public Transform target; // Vật thể cần xoay
    public float rotationSpeed = 0.3f;  // tốc độ xoay cho mobile
    public float zoomSpeed = 0.01f;     // tốc độ zoom
    public float minScale = 0.5f;       // giới hạn nhỏ nhất
    public float maxScale = 2f;         // giới hạn lớn nhất

    private Vector2 lastTouchPos;       // vị trí chạm trước đó
    private bool isRotating = false;    // đang xoay?

    void Update()
    {
        if (target == null) return;

        // 🖱 PC test (chuột)
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
                ZoomTarget(-scroll * zoomSpeed * 20f); // cuộn chuột
            }
        }

        // 📱 Mobile touch
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
            // 🔎 Pinch zoom
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
        float newScale = Mathf.Clamp(target.localScale.x + zoomAmount, minScale, maxScale);
        target.localScale = Vector3.one * newScale;
    }
}
