using UnityEngine;
using System.Collections;

public class ScrewController : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    public float moveDistance = 0.5f;
    public float moveSpeed = 5.0f;
    public LayerMask obstacleLayer;
    public CubeController cube;
    public Vector3 extractionLocalAxis;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        StartCoroutine(AttemptMove());
    }

    IEnumerator AttemptMove()
    {
        Vector3 currentPosition = transform.position;
        Vector3 extractionWorldDirection = transform.TransformDirection(extractionLocalAxis).normalized;
        
        Vector3 targetLocalPosition = initialLocalPosition + extractionLocalAxis * moveDistance;
        Vector3 targetWorldPosition = transform.parent.TransformPoint(targetLocalPosition);

        float checkRayDistance = Vector3.Distance(currentPosition, targetWorldPosition);

        RaycastHit hit;
        if (Physics.Raycast(currentPosition, extractionWorldDirection, out hit, checkRayDistance, obstacleLayer))
        {
            Debug.Log("Cannot move fully. Blocked by: " + hit.collider.gameObject.name);
            transform.localPosition = initialLocalPosition;
            yield break;
        }

        Debug.Log("Moving up. No obstacles detected.");

        float startTime = Time.time;
        float journeyLength = checkRayDistance;
        Vector3 startPos = currentPosition;

        while (Vector3.Distance(transform.position, targetWorldPosition) > 0.001f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector3.Lerp(startPos, targetWorldPosition, fractionOfJourney);

            yield return null;
        }

        transform.localPosition = targetLocalPosition;
        Debug.Log("Screw successfully removed!");

        if (cube != null)
        {
            cube.ScrewRemoved(this);
        }

        Destroy(gameObject, 0.5f);
    }
}