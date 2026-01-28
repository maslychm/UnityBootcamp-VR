using UnityEngine;
using UnityEngine.InputSystem;

public class CustomXRInputs : MonoBehaviour
{
    [SerializeField] private InputActionReference customActionReference;
    [SerializeField] private InputActionReference toggleShield;

    [SerializeField] private GameObject shieldObject;

    private void OnEnable()
    {
        customActionReference.action.Enable();
        customActionReference.action.performed += OnCustomActionPerformed;
    }

    private void OnDisable()
    {
        customActionReference.action.performed -= OnCustomActionPerformed;
        customActionReference.action.Disable();
    }

    private void OnCustomActionPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Custom action performed!");
        }
    }

    private void Update()
    {
        ProcessControllerInput();
    }

    private void ProcessControllerInput()
    {
        if (toggleShield == null || toggleShield.action == null) return;

        if (toggleShield.action.WasPressedThisFrame())
        {
            //Debug.Log("Action button pressed!");
            if (shieldObject != null)
            {
                shieldObject.SetActive(!shieldObject.activeSelf);
            }
        }
    }

    //private void ProcessControllerInput()
    //{
    //    if (actionReference == null || actionReference.action == null) return;

    //    // True as long as the button is held down
    //    actionReference.action.IsPressed();

    //    // True only on the frame button state transitions from "not pressed" to "pressed"
    //    if (actionReference.action.WasPressedThisFrame();

    //    // True only on the frame button state transitions from "pressed" to "not pressed"
    //    actionReference.action.WasReleasedThisFrame();

    //    // True only on the frame the action is performed (for button, identical to WasPressedThisFrame())
    //    actionReference.action.WasPerformedThisFrame();
    //}

    //private void OnEnable()
    //{
    //    actionReference.action.Enable();
    //}

    //private void OnActionPressed(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        // Implement your logic here
    //        Debug.Log("Action button pressed!");
    //        throw new System.NotImplementedException();
    //    }
    //}
}