using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldToggle : MonoBehaviour
{
    public GameObject shieldObject;
    public bool startEnabled = false;

    [SerializeField] private InputActionReference toggleShield;

    private void Start()
    {
        if (shieldObject != null)
            shieldObject.SetActive(startEnabled);
    }

    private void OnEnable()
    {
        toggleShield.action.Enable();
        toggleShield.action.performed += OnShieldToggle;
    }

    private void OnDisable()
    {
        toggleShield.action.performed -= OnShieldToggle;
        toggleShield.action.Disable();
    }

    private void Update()
    {
        //// toggle on press
        //if (toggleShield != null && toggleShield.action.WasPerformedThisFrame())
        //{
        //    if (shieldObject != null)
        //        shieldObject.SetActive(!shieldObject.activeSelf);
        //}
    }

    private void OnShieldToggle(InputAction.CallbackContext _)
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (shieldObject != null)
            shieldObject.SetActive(!shieldObject.activeSelf);
    }
}