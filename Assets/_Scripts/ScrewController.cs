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
    private bool isInterable = true;
    public bool IsInterable { get => isInterable; set => isInterable = value; }
    private bool isMoving = false;
    public bool IsMoving => isMoving;
    private bool isRemoved = false;
    public bool IsRemoved => isRemoved;


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
        if (IsRemoved || isMoving || !IsInterable) return;

        animator?.SetTrigger("isClicked");
        AudioManager.Instance.PlaySFX(SoundId.ScrewClick);
        StartCoroutine(PlayBounceThenMove());
    }


    private IEnumerator PlayBounceThenMove()
    {
        yield return StartCoroutine(WaitForAnimationEnd("Bounce"));
        yield return StartCoroutine(AttemptMove());
    }

    private IEnumerator AttemptMove()
    {
        if (isMoving) yield break;
        isMoving = true;

        if (boxCollider == null) yield break;

        Vector3 currentPosition = transform.position;
        Vector3 extractionWorldDirection = screwModelTransform.up;
        Vector3 targetWorldPosition = currentPosition + extractionWorldDirection * moveDistance;

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
            isMoving = false;
            yield break;
        }

        MoveForward();

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

        transform.SetParent(null, true);

        yield return MoveSmooth(transform.position + dir * 2f, 10f);
        isRemoved = true;
        MMEventManager.TriggerEvent(new ReleaseScrew(this));
    }


    // Di chuyển mượt tái sử dụng
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
    public void ForceRelease()
    {
        if (isRemoved || isMoving) return;

        isInterable = false;
        isMoving = true;

        if (boxCollider != null)
            boxCollider.enabled = false;

        MoveForward();

        if (cube != null)
            cube.ScrewRemoved(this);
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
    
    public void PlayAnim(string trigger)
    {
            animator?.SetTrigger(trigger);
    }
}
