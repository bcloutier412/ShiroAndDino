using UnityEngine;

public class TrackableObject : MonoBehaviour
{
    public string objectID; // Unique ID for this object
    public DestroyedObjectsTracker tracker; // Reference to the ScriptableObject

    private void Start()
    {
        // Check if this object is marked as destroyed
        if (tracker != null && tracker.destroyedObjectIDs.Contains(objectID))
        {
            Destroy(gameObject); // Destroy the object if marked
        }
        else if (tracker == null)
        {
            Debug.LogError("DestroyedObjectsTracker is not assigned.");
        }
    }

    public void MarkAsDestroyed()
    {
        if (tracker != null && !tracker.destroyedObjectIDs.Contains(objectID))
        {
            tracker.destroyedObjectIDs.Add(objectID); // Add ID to the list
        }
        else if (tracker == null)
        {
            Debug.LogError("DestroyedObjectsTracker is not assigned.");
        }
    }
}