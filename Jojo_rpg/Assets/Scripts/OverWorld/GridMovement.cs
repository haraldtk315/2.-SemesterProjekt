using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GridMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;      
    private float gridSize = 1f;       
    public LayerMask obstacleMask;
    public bool isPlayer;
    public GameObject leader;
    public GameObject follower;
    public float lookDist;
    public SpriteRenderer partySprite;

    [Header("Optional")]
    private bool allowHoldToMove = true;
    //private float inputBuffer = 0.1f; ignoreret for nu

    private Vector2 input;
    private bool isMoving;
    private Rigidbody2D rb;
    private bool canMove;

    private float lastInputTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        canMove = true;
        if (isMoving) return;

       
   
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (isPlayer)
        {
            if (x > 0)
            {
                partySprite.flipX = false;
            }

            if (x < 0)
            {
                partySprite.flipX = true;
            }
        }


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
            input = raw.normalized;

            // opdater retning med det samme
            GetComponent<PlayerInteract>()?.SetFacing(input);
            if (isPlayer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, input, lookDist, obstacleMask);
                if (!hit)
                {
                    TryMove(input);
                }
                else
                {
                    canMove = false;
                }
                if (follower)
                {
                    follower.GetComponent<GridMovement2D>().SequencingMoves(canMove);
                } 
            }
        }
    }

    public void SequencingMoves(bool leaderCanMove)
    {
        if (leaderCanMove)
        {
            TryMove(input);
        }
        else
        {
            canMove = false;
        }

        if (follower)
        {
            follower.GetComponent<GridMovement2D>().SequencingMoves(leaderCanMove);
        }
    }

    private void TryMove(Vector2 dir)
    {
        Vector2 start = rb.position;
        Vector2 target;
        if (isPlayer)
        {
            GetComponent<PlayerInteract>()?.SetFacing(input);
            target = start + dir * gridSize;
        }
        else
        {
            if ((transform.position - leader.transform.position).x > 0)
            {
                partySprite.flipX = true;
            }

            if ((transform.position - leader.transform.position).x < 0)
            {
                partySprite.flipX = false;
            }
            target = leader.transform.position;
        }
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