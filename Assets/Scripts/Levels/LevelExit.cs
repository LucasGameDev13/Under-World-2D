using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float loadTimer;
    [SerializeField] private string levelName;
    private AudioSource audioSouce;
    [SerializeField] private AudioClip sceneAudio;

    private void Start()
    {
        audioSouce = GetComponent<AudioSource>();
        
    }

    IEnumerator LoadTheNextLevel()
    {
        yield return new WaitForSeconds(loadTimer);

        NextLevelName(levelName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioSouce.PlayOneShot(sceneAudio);
            StartCoroutine(LoadTheNextLevel());
        }
    }

    private void NextLevelName(string levelsNames)
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(levelsNames);
    }
}
