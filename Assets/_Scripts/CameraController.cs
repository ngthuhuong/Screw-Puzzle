using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Mục tiêu mà camera sẽ xoay quanh (Cube/Plank)
    public Transform target; 
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 3f; // Tốc độ xoay (dễ chịu)
    public float pitchMin = -80f;    // Giới hạn góc nhìn lên
    public float pitchMax = 80f;     // Giới hạn góc nhìn xuống
    private Quaternion initialRotation; // Rotation ban đầu của mục tiêu
    
    [Header("Zoom Settings")]
    public float zoomSpeed = 0.01f; // Tốc độ zoom (nhỏ cho di động)
    public float minDistance = 3f;  // minzoom
    public float maxDistance = 10f; // max zoom
    
    // Trạng thái xoay
    private float yaw;
    private float pitch;
    
    // Trạng thái zoom (khoảng cách hiện tại)
    private float currentDistance;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera Target is not assigned. Disabling controller.");
            enabled = false;
            return;
        }
        
        currentDistance = Vector3.Distance(transform.position, target.position);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        if (target != null)
        {
            // LƯU TRỮ VÀ KHỞI TẠO TỪ MỤC TIÊU
            initialRotation = target.rotation;
            Vector3 angles = target.eulerAngles; // Lấy góc xoay từ mục tiêu
            yaw = angles.y;
            pitch = angles.x;
        }
    }

    void Update()
    {
        // Xử lý Input dựa trên số lượng chạm hoặc trạng thái chuột
        if (Input.touchCount == 1 || Input.GetMouseButton(0))
        {
            HandleRotation();
        }
        else if (Input.touchCount >= 2)
        {
            HandleZoom(); //ưu tiên zoom khi có 2` chạm
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            HandleScrollZoom(Input.mouseScrollDelta.y);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;
        
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        target.rotation = initialRotation * targetRotation;
        
        // Hướng từ camera đến mục tiêu (Vector lùi)
        Vector3 direction = -transform.forward; 
        transform.position = target.position + direction * currentDistance;
        transform.LookAt(target.position);
    }

   
    void HandleRotation()
    {
        float inputX = 0f;
        float inputY = 0f;

        if (Input.touchCount == 1)
        {
            // Lấy độ dời của chạm đầu tiên
            Touch touch = Input.GetTouch(0);
            inputX = touch.deltaPosition.x;
            inputY = touch.deltaPosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            // Lấy độ dời của chuột (nhân với một giá trị để đồng bộ cảm giác)
            inputX = Input.GetAxis("Mouse X");
            inputY = Input.GetAxis("Mouse Y");
        }

        yaw -= inputX * rotationSpeed;
        pitch += inputY * rotationSpeed;
        
        // Giới hạn góc Pitch (dọc)
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    // Xử lý Zoom camera bằng cử chỉ chụm/tách (hai ngón tay)
    void HandleZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Lấy vị trí trước đó của hai chạm
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Tính khoảng cách giữa hai chạm
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Tính sự thay đổi khoảng cách
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        // Cập nhật khoảng cách hiện tại
        // Delta dương -> chụm vào -> Zoom in (giảm khoảng cách)
        // Delta âm -> tách ra -> Zoom out (tăng khoảng cách)
        currentDistance += deltaMagnitudeDiff * zoomSpeed;

        // Giới hạn khoảng cách
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }
    
    // Xử lý Zoom bằng con lăn chuột
    void HandleScrollZoom(float scrollDelta)
    {
        // scrollDelta dương khi cuộn lên (zoom in), âm khi cuộn xuống (zoom out)
        float mouseZoomSpeed = 0.5f; 
        
        currentDistance -= scrollDelta * mouseZoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }
}