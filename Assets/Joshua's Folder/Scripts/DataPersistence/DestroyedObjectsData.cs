using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DestroyedObjectsTracker", menuName = "Persistent Data/DestroyedObjectsTracker")]
public class DestroyedObjectsTracker : ScriptableObject
{
    public List<string> destroyedObjectIDs = new List<string>();
}
