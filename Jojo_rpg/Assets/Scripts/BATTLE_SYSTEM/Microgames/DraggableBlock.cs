using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableBlock : MonoBehaviour
{
    public static bool blockBeingHeld;

    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private InputActionReference pointerPosAction;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private RectTransform blockPosition;
    [SerializeField] private GameObject snapPoint;
    [SerializeField] private float snapDist;

    private Vector2 offset;
    private bool isDragging;
    public bool isSnapped;

    private void OnEnable()
    {
        clickAction.action.started += OnClickStarted;
        clickAction.action.canceled += OnClickCancel;
        pointerPosAction.action.performed += OnPointMoved;
    }

    private void OnDisable()
    {
        clickAction.action.started -= OnClickStarted;
        clickAction.action.canceled -= OnClickCancel;
        pointerPosAction.action.performed -= OnPointMoved;
        isDragging = false;
        isSnapped = false;
    }

    private bool IsPointerOverBlock(Vector2 pointerPos)
    {
        if (pointerPos.x > (transform.position.x - blockPosition.rect.width/2 * 1.2f) && pointerPos.x < (transform.position.x + blockPosition.rect.width/2 * 1.2f))
        {
            if (pointerPos.y > (transform.position.y - blockPosition.rect.height / 2 * 1.2f) && pointerPos.y < (transform.position.y + blockPosition.rect.height / 2 * 1.2f))
            {
                return true;
            }
        }
        return false;
    }

    private void OnClickStarted(InputAction.CallbackContext ctx)
    {
        Vector2 pointerPos = Input.mousePosition;
        if (IsPointerOverBlock(pointerPos))
        {
            if (!blockBeingHeld)
            {
                blockBeingHeld = true;
                isDragging = true;
                isSnapped = false;
                offset = (Vector2)transform.position - pointerPos;
                Debug.Log("Click started!!");
            }
        }
    }

    private void OnClickCancel(InputAction.CallbackContext ctx)
    {
        blockBeingHeld = false;
        isDragging = false;
        if ((transform.position - snapPoint.transform.position).magnitude < snapDist)
        {
            transform.position = snapPoint.transform.position;
            isSnapped = true;
        }
        Debug.Log("Click ended :((");
    }

    private void OnPointMoved(InputAction.CallbackContext ctx)
    {
        if (!isDragging)
        {
            return;
        }
        Vector2 pointerPos = Input.mousePosition;
        transform.position = pointerPos + offset;
        Debug.Log("you're moving the block!!");
    }
}
