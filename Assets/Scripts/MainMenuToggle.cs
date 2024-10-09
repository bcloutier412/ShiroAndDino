using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuToggle : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
