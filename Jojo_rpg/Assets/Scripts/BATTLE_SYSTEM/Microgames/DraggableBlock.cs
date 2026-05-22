using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DraggableBlock : MonoBehaviour
{

    [SerializeField] private InputActionReference clickAction;
    [SerializeField] private InputActionReference pointerPosAction;
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private RectTransform blockPosition;
    [SerializeField] private Camera Camera;

    private Vector2 offset;
    private bool isDragging;

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
    }

    private bool IsPointerOverBlock(Vector2 pointerPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(blockPosition, pointerPos, Camera);
    }

    private void OnClickStarted(InputAction.CallbackContext ctx)
    {
        Vector2 pointerPos = Input.mousePosition;
        if (IsPointerOverBlock(pointerPos) || true)
        {
            isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pointerPos, Camera, out Vector2 localPos);
            offset = (Vector2)transform.position - localPos;
            Debug.Log("Click started!!");
        }
    }

    private void OnClickCancel(InputAction.CallbackContext ctx)
    {
        isDragging = false;
        Debug.Log("Click ended :((");
    }

    private void OnPointMoved(InputAction.CallbackContext ctx)
    {
        if (!isDragging)
        {
            return;
        }
        Vector2 pointerPos = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pointerPos, Camera, out Vector2 localPos);
        transform.position = pointerPos + offset;
        Debug.Log("you're moving the block!!");
    }
}
