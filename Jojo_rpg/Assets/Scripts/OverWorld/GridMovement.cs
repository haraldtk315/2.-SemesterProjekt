using UnityEngine;

public class GridMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;      
    private float gridSize = 1f;       
    public LayerMask obstacleMask;    

    [Header("Optional")]
    private bool allowHoldToMove = false;
    private float inputBuffer = 0.1f;

    private Vector2 input;
    private bool isMoving;
    private Rigidbody2D rb;

    private float lastInputTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving) return;

       
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(x) > 0.01f) y = 0f; 

        Vector2 raw = new Vector2(x, y);

        if (!allowHoldToMove)
        {
           
            if (Input.GetKeyDown(KeyCode.A)) raw = Vector2.left;
            else if  (Input.GetKeyDown(KeyCode.D)) raw = Vector2.right;
            else if (Input.GetKeyDown(KeyCode.W)) raw = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.S)) raw = Vector2.down;
            else raw = Vector2.zero;
        }

        if (raw != Vector2.zero)
        {

            if (Time.time - lastInputTime > inputBuffer || allowHoldToMove)
            {
                GetComponent<PlayerInteract>()?.SetFacing(input);
                input = raw.normalized;
                lastInputTime = Time.time;
                TryMove(input);
            }
        }
    }

    private void TryMove(Vector2 dir)
    {
        Vector2 start = rb.position;
        Vector2 target = start + dir * gridSize;

        // Check om der er en collider på vejen

        float radius = 0.2f;
        RaycastHit2D hit = Physics2D.CircleCast(start, radius, dir, gridSize, obstacleMask);

        if (hit.collider != null)
            return; // blokeret

        StartCoroutine(MoveRoutine(target));
    }

    private System.Collections.IEnumerator MoveRoutine(Vector2 target)
    {
        isMoving = true;

        while ((target - rb.position).sqrMagnitude > 0.0001f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(target);
        isMoving = false;
    }
}