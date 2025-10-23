using UnityEngine;
using System.Collections;
using System.Linq;

public class ScrewController : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private float moveDistance = 1f;
    public float moveSpeed = 5.0f;
    public LayerMask obstacleLayer;
    public CubeController cube;
    
   private Transform screwModelTransform;
   
    private const float BoxCastOffset = 0.05f;
    private const float OverlapCheckPadding = 0.001f;

    private BoxCollider boxCollider;

    void Start()
    {
        // 1. TỰ ĐỘNG TÌM KIẾM SCREW MODEL TRANSFORM
        // Tìm ScrewSetup trên đối tượng con (Model) và lấy Transform của nó
        ScrewSetup setupComponent = GetComponentInChildren<ScrewSetup>();
        if (setupComponent != null)
        {
             screwModelTransform = setupComponent.transform;
             // Lấy cờ IsReversing từ ScrewSetup (nếu bạn vẫn cần nó)
             // Bạn có thể giữ lại logic IsReversing trong ScrewController nếu nó là nơi cuối cùng quyết định hướng.
        }

        initialLocalPosition = transform.localPosition;
        boxCollider = GetComponent<BoxCollider>();
        
        // KIỂM TRA AN TOÀN
        if (boxCollider == null || screwModelTransform == null)
        {
            Debug.LogError("Thiếu BoxCollider hoặc ScrewModelTransform (kiểm tra lại cấu trúc Cha-Con và ScrewSetup).");
            enabled = false;
            return;
        }
        
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        StartCoroutine(AttemptMove());
    }

    IEnumerator AttemptMove()
    {
        if (boxCollider == null) yield break;

        Vector3 currentPosition = transform.position;
        
        // TÍNH HƯỚNG THÁO TRỰC TIẾP (World Space) TỪ MODEL
        // Sử dụng screwModelTransform.up đã được xoay để lấy hướng tháo chính xác
        Vector3 extractionWorldDirection = screwModelTransform.up;
       
        
        Vector3 targetWorldPosition = currentPosition + extractionWorldDirection * moveDistance;
        Vector3 origin = transform.TransformPoint(boxCollider.center);
        Vector3 halfExtents = boxCollider.size / 2f; 
        Quaternion orientation = transform.rotation;

        // 1. KIỂM TRA OVERLAP
        Vector3 checkSize = halfExtents - (Vector3.one * OverlapCheckPadding);
        
        boxCollider.enabled = false;
        Collider[] overlaps = Physics.OverlapBox(origin, checkSize, orientation, obstacleLayer);
        boxCollider.enabled = true;

        if (overlaps.Any(c => c.transform != transform.parent))
        {
            Debug.Log("Cannot move. Vít đang bị ĐÈ lên bởi vật thể khác.");
            yield break;
        }

        // 2. BOXCAST
        float distance = Vector3.Distance(currentPosition, targetWorldPosition);
        float castDistance = distance + BoxCastOffset;

        RaycastHit hit;
        if (Physics.BoxCast(origin, halfExtents, extractionWorldDirection, out hit, orientation, castDistance, obstacleLayer))
        {
            if (hit.collider.transform != transform && hit.collider.transform != transform.parent)
            {
                Debug.Log("Cannot move fully. Blocked by: " + hit.collider.gameObject.name);
                transform.localPosition = initialLocalPosition;
                yield break;
            }
        }

        // Bắt đầu di chuyển mượt mà
        Debug.Log("Moving up. No obstacles detected.");

        float startTime = Time.time;
        float journeyLength = distance; 
        Vector3 startPos = currentPosition;

        while (Vector3.Distance(transform.position, targetWorldPosition) > 0.001f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPos, targetWorldPosition, fractionOfJourney);

            yield return null;
        }

        // Hoàn tất tháo vít
        transform.position = targetWorldPosition;
        transform.localPosition = transform.parent.InverseTransformPoint(targetWorldPosition); 
        
        Debug.Log("Screw successfully removed!");

        if (cube != null)
        {
            cube.ScrewRemoved(this);
        }

        Destroy(gameObject, 0.5f);
    }
}