using UnityEngine;
using System.Collections; // Cần thiết để dùng Coroutine

public class ScrewController : MonoBehaviour
{
    private Vector3 initialPosition;
    public float moveDistance = 0.5f;     // Khoảng cách tối đa vít sẽ di chuyển
    public float moveSpeed = 5.0f;      
    public LayerMask obstacleLayer;
    public CubeController cube;

    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDown()
    {
        StartCoroutine(AttemptMove());
    }

    IEnumerator AttemptMove()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = initialPosition + Vector3.up * moveDistance;
        float startTime = Time.time;
        float journeyLength = moveDistance;
        
        RaycastHit hit;
        if (Physics.Raycast(currentPosition, Vector3.up, out hit, moveDistance, obstacleLayer))
        {
            Debug.Log("Cannot move fully. Blocked by: " + hit.collider.gameObject.name);
            // Có thể thêm rung lắc nhỏ ở đây
            
            transform.position = initialPosition;
            yield break;  
        }
        
        Debug.Log("Moving up. No obstacles detected.");
        
        while (Vector3.Distance(transform.position, targetPosition) > 0.001f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            
            transform.position = Vector3.Lerp(currentPosition, targetPosition, fractionOfJourney);
            if (cube != null)
            {
                cube.ScrewRemoved(this);
            }
            yield return null;
        }

        transform.position = targetPosition;  
        Debug.Log("Screw successfully removed!");
        
        // Kích hoạt logic tháo tấm gỗ ở đây (gọi PlankController)
        
        // Tùy chọn: Hủy đối tượng vít sau khi tháo
        // Destroy(gameObject, 0.5f);
    }
}