using UnityEngine;
using System.Collections;
using System.Linq;
using MoreMountains.Tools;

public class ScrewController : MonoBehaviour
{
    private Vector3 initialLocalPosition;
    private float moveDistance = 1f;
    public LayerMask obstacleLayer;
    private CubeController cube;

    private Transform screwModelTransform;
    private ScrewSetup screwSetup;
    private Animator animator;

    private BoxCollider boxCollider;

    void Start()
    {
        screwSetup = GetComponentInChildren<ScrewSetup>();
        if (screwSetup == null)
        {
            Debug.LogError("Không tìm thấy ScrewSetup!");
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
        animator = GetComponentInChildren<Animator>();
        cube = GetComponentInParent<CubeController>();
    }

    private void OnMouseDown()
    {
        animator?.SetTrigger("isClicked");
        StartCoroutine(PlayBounceThenMove());
    }

    private IEnumerator PlayBounceThenMove()
    {
        yield return StartCoroutine(WaitForAnimationEnd("Bounce"));
        yield return StartCoroutine(AttemptMove());
    }

    private IEnumerator AttemptMove()
    {
        if (boxCollider == null) yield break;

        Vector3 currentPosition = transform.position;
        Vector3 extractionWorldDirection = screwModelTransform.up;
        Vector3 targetWorldPosition = currentPosition + extractionWorldDirection * moveDistance;

        // overlap check
        boxCollider.enabled = false;
        Collider[] overlaps = Physics.OverlapBox(
            transform.TransformPoint(boxCollider.center),
            boxCollider.size / 2f,
            transform.rotation,
            obstacleLayer
        );
        boxCollider.enabled = true;

        if (overlaps.Any(c => c.transform != transform.parent))
        {
            yield break;
        }

        MoveForward(); // chỉ gọi MoveForward, không xử lý event ở đây

        // Báo cube
        if (cube != null)
            cube.ScrewRemoved(this);
    }

    // 🟢 Gọi từ AttemptMove — đi ra 2f rồi phát event
    public void MoveForward()
    {
        StartCoroutine(MoveOutThenRelease());
    }

    private IEnumerator MoveOutThenRelease()
    {
        Vector3 dir = screwModelTransform.up;
        yield return MoveSmooth(transform.position + dir * 2f, 10f);

        // 🔔 Sau khi ra ngoài mới trigger event
        MMEventManager.TriggerEvent(new ReleaseScrew(this));
    }

    // 🧩 Di chuyển mượt tái sử dụng
    private IEnumerator MoveSmooth(Vector3 targetPos, float speed)
    {
        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, targetPos);
        float duration = distance / speed;
        float t = 0;

        transform.SetParent(null);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }

    public ScrewColor GetColor() => screwSetup != null ? screwSetup.screwColor : ScrewColor.Gray;

    private IEnumerator WaitForAnimationEnd(string stateName)
    {
        if (animator == null)
            yield break;

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
    }

    public IEnumerator MoveTo(Vector3 targetPos, Transform newParent, float moveDuration = 1.5f)
    {
        if (this == null || gameObject == null)
            yield break;

        Vector3 startPos = transform.position;
        float elapsed = 0f;

        if (newParent != null)
            transform.SetParent(newParent);

        while (elapsed < moveDuration)
        {
            if (this == null || gameObject == null) yield break;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (this != null)
            transform.position = targetPos;
    }
    public void PlayAnim(string animName)
    {
            animator?.SetTrigger(animName);
    }
}
