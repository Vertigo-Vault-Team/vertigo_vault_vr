using UnityEngine;
using UnityEngine.InputSystem;

public class DevTeleport : MonoBehaviour
{
    [Header("Where to go?")]
    public Transform destinationAnchor;

    [Header("Who is teleporting?")]
    public Transform playerRig;

    [Header("Which button triggers it?")]
    public InputActionReference teleportButton;

    private void OnEnable()
    {
        if (teleportButton != null)
        {
            teleportButton.action.Enable();
            teleportButton.action.performed += TriggerTeleport;
        }
    }

    private void OnDisable()
    {
        if (teleportButton != null)
        {
            teleportButton.action.performed -= TriggerTeleport;
            teleportButton.action.Disable();
        }
    }

    private void TriggerTeleport(InputAction.CallbackContext context)
    {
        if (playerRig != null && destinationAnchor != null)
        {
            // Instantly snap the player's root object to the anchor's position
            playerRig.position = destinationAnchor.position;
        }
    }
}