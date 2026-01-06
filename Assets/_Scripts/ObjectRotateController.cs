using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ObjectRotateZoom_Mobile : MonoBehaviour,MMEventListener<StartGame>
{
    [Header("References")]
    public Transform target;
    public Slider zoomSlider; // slider UI (có thể null nếu không dùng)
    public float rotationSpeed = 0.3f;
    public float zoomSpeed = 0.05f;
    public float minDistance = 20f;
    public float maxDistance = 100f;

    private Vector2 lastTouchPos;
    private bool isRotating = false;
    private Camera cam;
    private float currentDistance;
    private float defaultDistance;
    private Vector3 defaultDirection;
    private Vector3 defaultTargetPosition;

    public float CurrentDistance => currentDistance;
    private void OnEnable()
    {
        this.MMEventStartListening<StartGame>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<StartGame>();
    }
    void Start()
    {
        cam = Camera.main;
        if (target != null && cam != null)
        {
            currentDistance = Vector3.Distance(cam.transform.position, target.position);

            // ===== SAVE DEFAULT STATE =====
            defaultDistance = currentDistance;
            defaultDirection = (target.position - cam.transform.position).normalized;
            defaultTargetPosition = target.position;
        }

        if (zoomSlider != null)
        {
            zoomSlider.minValue = minDistance;
            zoomSlider.maxValue = maxDistance;

            float normalized = (currentDistance - minDistance) / (maxDistance - minDistance);
            zoomSlider.value = Mathf.Lerp(maxDistance, minDistance, normalized);

            zoomSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }


    void Update()
    {
        if (GameManager.Instance.InputLocked || target == null || cam == null) return;

        // --- PC ---
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

        // --- Mobile ---
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
                RotateTarget(t.deltaPosition.x, t.deltaPosition.y);
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
            float currDist = (t0.position - t1.position).magnitude;

            float diff = currDist - prevDist;
            ZoomTarget(diff * zoomSpeed);
        }
    }

    void RotateTarget(float deltaX, float deltaY)
    {
        target.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.World);
        target.Rotate(Vector3.right, deltaY * rotationSpeed, Space.World);
    }

    public void ZoomTarget(float zoomAmount)
    {
        Vector3 direction = (target.position - cam.transform.position).normalized;
        currentDistance = Mathf.Clamp(currentDistance - zoomAmount, minDistance, maxDistance);
        target.position = cam.transform.position + direction * currentDistance;

        UpdateSlider();
    }

    public void SetZoom(float sliderValue)
    {
        // đảo ngược: slider lớn → vật đi xa
        float normalized = (sliderValue - minDistance) / (maxDistance - minDistance);
        float zoomDistance = Mathf.Lerp(maxDistance, minDistance, normalized);
        currentDistance = Mathf.Clamp(zoomDistance, minDistance, maxDistance);

        Vector3 direction = (target.position - cam.transform.position).normalized;
        target.position = cam.transform.position + direction * currentDistance;
    }

    private void OnSliderChanged(float value)
    {
        SetZoom(value);
    }

    private void UpdateSlider()
    {
        if(zoomSlider != null)
        {
            float normalized = (currentDistance - minDistance) / (maxDistance - minDistance);
            zoomSlider.value = Mathf.Lerp(maxDistance, minDistance, normalized);
        }
    }
    
    public void OnMMEvent(StartGame eventType)
    {
            if (target == null || cam == null) return;

            currentDistance = Mathf.Clamp(defaultDistance, minDistance, maxDistance);
            target.position = cam.transform.position + defaultDirection * currentDistance;

            UpdateSlider();
    }
}
