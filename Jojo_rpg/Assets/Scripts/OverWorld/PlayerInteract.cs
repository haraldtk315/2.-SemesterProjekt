using System.Xml.Serialization;
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
        spacebarIcon.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryInteract();
        }
    }

    public void SetFacing(Vector2 dir)
    {
        if (dir != Vector2.zero) facing = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            spacebarIcon.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            spacebarIcon.enabled = false;
        }
    }

    void TryInteract()
    {
        Vector2 origin = transform.position;
        Vector2 targetPos = origin + facing * gridSize;

        // overlap et lille område foran spilleren
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.15f, interactMask);
        if (!hit) return;

        // kald Interact på objektet
        var interactable = hit.GetComponent<IInteractable>();
        if (interactable != null)
            interactable.Interact(this);
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.yellow;
        Vector3 p = transform.position + (Vector3)(facing.normalized * gridSize);
        Gizmos.DrawWireSphere(p, 0.15f);
    }
}