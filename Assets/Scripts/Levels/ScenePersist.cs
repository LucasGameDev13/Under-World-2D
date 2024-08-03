using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numScenesPersists = FindObjectsOfType<ScenePersist>().Length;

        if (numScenesPersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
       
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
