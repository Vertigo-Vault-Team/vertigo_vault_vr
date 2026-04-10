using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;
using System.Collections;

public class BreakableHold : MonoBehaviour
{
    public float breakTime = 3f;

    private ClimbInteractable climbInteractable;
    private MeshRenderer meshRenderer;
    private Collider holdCollider;
    
    // THE FIX: We need a variable to keep track of the timer so we can stop it later
    private Coroutine breakCoroutine; 

    void Start()
    {
        climbInteractable = GetComponent<ClimbInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
        holdCollider = GetComponent<Collider>();

        if (climbInteractable != null)
        {
            // Listen for both GRAB and RELEASE
            climbInteractable.selectEntered.AddListener(OnGrabbed);
            climbInteractable.selectExited.AddListener(OnReleased); 
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Start the timer AND save a reference to it
        breakCoroutine = StartCoroutine(BreakSequence());
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // THE FIX: If the player lets go early, stop the runaway train!
        if (breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
            breakCoroutine = null; // Reset the timer
        }
    }

    private IEnumerator BreakSequence()
    {
        yield return new WaitForSeconds(breakTime);
        
        if (climbInteractable.isSelected)
        {
            climbInteractable.interactionManager.CancelInteractableSelection((UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)climbInteractable);
        }

        meshRenderer.enabled = false;
        holdCollider.enabled = false;

        Destroy(gameObject, 5f);
    }
}