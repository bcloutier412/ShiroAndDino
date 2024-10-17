using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

    private GameData gameData;
   public static DataPersistenceManager instance {get; private set;}

   private void Awake()
   {
    if (instance != null)
    {
        Debug.LogError("Found more than one Data Persistence Manager in the scene");
    }
    instance = this;
   }

   public void NewGame()
   {
    this.gameData = new GameData();
   }

   public void LoadGame()
   {
        //todo load using save data
        if(this.gameData == null)
        {
            Debug.Log("No data was found using defaults");
            NewGame();
        }
        //todo push load daata
   }

   public void SaveGame()
   {
    //todo pass the data to other scripts

    // save the data
   }

   private void OnGameQuit()
   {
    SaveGame();
   }
}
