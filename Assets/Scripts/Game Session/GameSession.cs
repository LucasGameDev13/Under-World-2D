using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class GameSession : MonoBehaviour
{
    //public static GameSession instance; 

    private AudioSource audioSource;

    [SerializeField] private int playerLives;
    [SerializeField] private int score;


    [SerializeField] private TextMeshProUGUI playerLivesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTotalText;

    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject BackgroundCanvas;
    [SerializeField] private GameObject winCanvas;

    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip gameOverSound;

 
    // Start is called before the first frame update
    void Awake()
    {
        //instance = this;

        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
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
        audioSource = GetComponent<AudioSource>();
        playerLivesText.text = " X " + playerLives.ToString();
        scoreText.text = score.ToString();
        scoreTotalText.text = "Score: " + score.ToString();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex > 3)
        {
            BackgroundCanvas.SetActive(false);
            winCanvas.SetActive(true);
        }
        else if(SceneManager.GetActiveScene().buildIndex >= 1)
        {
            BackgroundCanvas.SetActive(true);
            winCanvas.SetActive(false);
        }
    }

    public void GameScore(int _score)
    {
        score += _score;
        scoreText.text = score.ToString();
        scoreTotalText.text = "Score: " + score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 0)
        {
            TakeLife();
        }
        else
        {
            audioSource.PlayOneShot(gameOverSound);
            buttons.SetActive(true);     
        }
    }

    public void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        playerLivesText.text = " X " + playerLives.ToString();
    }


    public void ResetGameSession()
    {
        audioSource.PlayOneShot(startSound);
        StartCoroutine("ResetDelay");
    }

    public void GameExit()
    {
        Application.Quit();

        //UnityEditor.EditorApplication.isPlaying = false;
    }

    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(2f);

        ScenePersist scenePersist = FindObjectOfType<ScenePersist>();

        if (scenePersist != null)
        {
            scenePersist.ResetScenePersist();
        }

        SceneManager.LoadScene(1);

        Destroy(gameObject);

    }
}
