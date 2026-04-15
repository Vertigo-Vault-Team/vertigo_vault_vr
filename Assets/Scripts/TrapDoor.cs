using UnityEngine;
using System.Collections;

public class TrapDoor : MonoBehaviour
{
    public float breakTime = 1.0f; 
    private bool isTriggered = false;

    // THE FIX: Changed from OnCollisionEnter to OnTriggerEnter
    private void OnTriggerEnter(Collider other) 
    {
        // THE FIX: Changed 'collision.gameObject' to 'other'
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            StartCoroutine(BreakSequence());
        }
    }

    private IEnumerator BreakSequence()
    {
        yield return new WaitForSeconds(breakTime);
        Destroy(gameObject);
    }
}