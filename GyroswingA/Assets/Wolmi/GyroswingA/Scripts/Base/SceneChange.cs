using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    static SceneChange instance;
    public static SceneChange Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SceneChange>();
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ChangeSceneToInGame()
    {
        SceneManager.LoadScene("InGame");

        Invoke("GameManager.Instance.ChangeSceneToInGame", 1.0f);
    }
}
