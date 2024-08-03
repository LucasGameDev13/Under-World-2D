using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayMusic : MonoBehaviour
{
    private static GamePlayMusic instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip gamePlayMusic;
    private bool isPlayed;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = gamePlayMusic;
            //audioSource.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4 && !isPlayed)
        {
            audioSource.Stop();
            isPlayed = true;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 1 && isPlayed)
        {
            audioSource.Play();
            isPlayed = false;
        }
    }


}
