using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;

public class StaminaController : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;
    public float drainRate = 20f; 
    public float regenRate = 15f;

    [Header("Drag the Near-Far Interactor Here!")]
    public XRBaseInteractor interactor; 

    private XRInteractionManager interactionManager;
    private bool isGrabbing = false;

    void Start()
    {
        currentStamina = maxStamina;
        
        // Ensure the interactor was assigned in the Inspector
        if (interactor != null)
        {
            interactionManager = interactor.interactionManager;
            interactor.selectEntered.AddListener(OnGrab);
            interactor.selectExited.AddListener(OnRelease);
        }
        else
        {
            Debug.LogError("Stamina Script: You forgot to drag the Interactor into the slot!");
        }
    }

    void Update()
    {
        if (isGrabbing)
        {
            currentStamina -= drainRate * Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                ForceDrop();
            }
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactableObject is ClimbInteractable)
        {
            isGrabbing = true;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;
    }

    private void ForceDrop()
    {
        isGrabbing = false;
        interactionManager.CancelInteractorSelection((IXRSelectInteractor)interactor);
    }
}