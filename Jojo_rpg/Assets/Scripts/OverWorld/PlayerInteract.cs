using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteract player);
}

public class PlayerInteract : MonoBehaviour
{
    [Header("Grid / Range")]
    public float gridSize = 1f;
    public LayerMask interactMask;
    public SpriteRenderer spacebarIcon;

    public Vector2 facing = Vector2.down;

    [Header("Debug")]
    public bool drawGizmos = true;

    public void Start()
    {
        if (spacebarIcon != null)
        {
            spacebarIcon.enabled = false;
        }
    }

    void Update()
    {
        UpdateSpacebarIcon();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryInteract();
        }
    }

    public void SetFacing(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            facing = dir.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            facing = dir.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    void UpdateSpacebarIcon()
    {
        if (spacebarIcon == null)
            return;

        Vector2 origin = transform.position;
        Vector2 targetPos = origin + facing * gridSize;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.15f, interactMask);

        spacebarIcon.enabled = hit != null;
    }

    void TryInteract()
    {
        Vector2 origin = transform.position;
        Vector2 targetPos = origin + facing * gridSize;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.15f, interactMask);

        if (!hit)
            return;

        IInteractable interactable = hit.GetComponent<IInteractable>();

        if (interactable != null)
        {
            interactable.Interact(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Vector3 p = transform.position + (Vector3)(facing.normalized * gridSize);
        Gizmos.DrawWireSphere(p, 0.15f);
    }
}