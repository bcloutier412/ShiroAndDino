using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int totalCoinsCollected;

    public void ResetCoins()
    {
        totalCoinsCollected = 0;
    }
}
