using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
            new AndroidJavaClass("com.unity3d.player.UnityPlayer").Call("quit");
#else
            Application.Quit();
#endif
    }
}