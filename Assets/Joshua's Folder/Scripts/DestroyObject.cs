using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public TrackableObject trackableObject; // Reference to the TrackableObject

    public void DestroyObject()
    {
        if (trackableObject != null)
        {
            trackableObject.MarkAsDestroyed(); // Mark the object as destroyed in the tracker
            Destroy(trackableObject.gameObject); // Destroy the GameObject
        }
    }
}
