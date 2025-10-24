using UnityEngine;
using System.Collections;
using System.Linq;
using MoreMountains.Tools;

public class ScrewController : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private float moveDistance = 1f;
    public float moveSpeed = 5.0f;
    public LayerMask obstacleLayer;
    public CubeController cube;

    private Transform screwModelTransform;
    private ScrewSetup screwSetup; // 🟢 Thêm để lấy màu từ đây

    private const float BoxCastOffset = 0.05f;
    private const float OverlapCheckPadding = 0.001f;

    private BoxCollider boxCollider;

    void Start()
    {
        // Tìm ScrewSetup trên đối tượng con
        screwSetup = GetComponentInChildren<ScrewSetup>();
        if (screwSetup == null)
        {
            Debug.LogError("Không tìm thấy ScrewSetup trong con của ScrewController!");
            enabled = false;
            return;
        }

        screwModelTransform = screwSetup.transform;
        initialLocalPosition = transform.localPosition;
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("Thiếu BoxCollider!");
            enabled = false;
            return;
        }

        boxCollider.enabled = true;
    }

    private void OnMouseDown()
    {
        StartCoroutine(AttemptMove());
    }

    IEnumerator AttemptMove()
    {
        if (boxCollider == null) yield break;

        Vector3 currentPosition = transform.position;
        Vector3 extractionWorldDirection = screwModelTransform.up;
        Vector3 targetWorldPosition = currentPosition + extractionWorldDirection * moveDistance;

        Vector3 origin = transform.TransformPoint(boxCollider.center);
        Vector3 halfExtents = boxCollider.size / 2f;
        Quaternion orientation = transform.rotation;

        // Kiểm tra overlap
        boxCollider.enabled = false;
        Collider[] overlaps = Physics.OverlapBox(origin, halfExtents - Vector3.one * OverlapCheckPadding, orientation, obstacleLayer);
        boxCollider.enabled = true;

        if (overlaps.Any(c => c.transform != transform.parent))
        {
            Debug.Log("Cannot move. Bị đè lên bởi vật thể khác.");
            yield break;
        }

        // Kiểm tra BoxCast
        float distance = Vector3.Distance(currentPosition, targetWorldPosition);
        float castDistance = distance + BoxCastOffset;

        if (Physics.BoxCast(origin, halfExtents, extractionWorldDirection, out RaycastHit hit, orientation, castDistance, obstacleLayer))
        {
            if (hit.collider.transform != transform && hit.collider.transform != transform.parent)
            {
                Debug.Log("Cannot move fully. Blocked by: " + hit.collider.gameObject.name);
                transform.localPosition = initialLocalPosition;
                yield break;
            }
        }

        Debug.Log("Moving up. No obstacles detected.");
        MMEventManager.TriggerEvent(new ReleaseScrew(this)); // 🔔 Phát sự kiện tháo vít

        float startTime = Time.time;
        float journeyLength = distance;
        Vector3 startPos = currentPosition;

        while (Vector3.Distance(transform.position, targetWorldPosition) > 0.001f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fraction = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos, targetWorldPosition, fraction);
            yield return null;
        }

        transform.position = targetWorldPosition;
        transform.localPosition = transform.parent.InverseTransformPoint(targetWorldPosition);

        Debug.Log("Screw successfully removed!");

        if (cube != null)
        {
            cube.ScrewRemoved(this);
        }

        Destroy(gameObject, 0.5f);
    }

    // 🟢 Hàm public để lấy màu của vít
    public ScrewColor GetColor()
    {
        return screwSetup != null ? screwSetup.screwColor : ScrewColor.Gray;
    }
}
