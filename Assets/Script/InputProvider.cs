using UnityEngine;
using UnityEngine.InputSystem;

public class InputPrivider : MonoBehaviour
{
    private string gamePlayMapName = "Player";
    private string moveActionName = "Move";
    private string jumpActionName = "Jump";
    private string sprintActionName = "Sprint";
    private string lookPositionActionName = "Look";

    public Vector2 lookInputValue { get; private set; }

    [Header("Action Asset")]
    [SerializeField] private InputActionAsset actionAsset;

    private InputActionMap inputActionMap;

    public InputAction moveAction;
    public InputAction sprintAction;
    public InputAction jumpAction;

    private InputAction lookAction;    

    private void OnEnable()
    {
        if (!EnsureInputActionsInitialized()) return;
        inputActionMap.Enable();

        bindCallbacks();
    }
    private void OnDisable()
    {
        inputActionMap.Disable();
        UnbindCallbacks();
    }

    private void bindCallbacks() 
    { 
        lookAction.performed += LookInput;
        lookAction.canceled += LookInput;
    }
    private void UnbindCallbacks()
    { 

        lookAction.performed -= LookInput;
        lookAction.canceled -= LookInput;
    }

    private bool EnsureInputActionsInitialized()
    {
        inputActionMap = actionAsset.FindActionMap(gamePlayMapName, true);
        moveAction = inputActionMap.FindAction(moveActionName, true);
        jumpAction = inputActionMap.FindAction(jumpActionName, true);
        sprintAction = inputActionMap.FindAction(sprintActionName, true);
        lookAction = inputActionMap.FindAction(lookPositionActionName, true);

        return true;
    }
    private void LookInput(InputAction.CallbackContext context)
    {
        lookInputValue = context.ReadValue<Vector2>();
    }
}
