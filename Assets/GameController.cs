using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController instance = null;
    public static GameController Instance{
        get {return instance;}
    }
    public Text txtScore;
    public Text txtScoreGOver;

    public bool isRunning = false;
    public int point = 0;
    public bool gameOver = false;
    public GameObject panelGameOver;
    public Button btnRetry;
    Animator panelGameOverAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        btnRetry.onClick.AddListener(Retry);
        panelGameOverAnimator = panelGameOver.GetComponent<Animator>();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void AddPoint()
    {
        point += 1;
        txtScore.text = "Score " + point + "  ";
    }
    public void AddPoint(int amount)
    {
        point += amount;
        txtScore.text = "Score " + point + "  ";
    }
    bool aniFinished = false;
    void Update()
    {

        Debug.Log(gameOver+"   "+aniFinished);
        if (gameOver&& !aniFinished) {

            Debug.Log(panelGameOverAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finished"));
            if (panelGameOverAnimator.GetCurrentAnimatorStateInfo(0).IsName("Finished")) {
                Debug.Log("activated");
                Animator[] animators = panelGameOver.GetComponentsInChildren<Animator>();
                foreach (Animator a in animators)
                {
                    if(a!=panelGameOverAnimator)
                        a.SetTrigger("Play");
                }
                aniFinished = true;
            }
            
        }
    }
    public void GameOver()
    {
        gameOver = true;
        panelGameOver.SetActive(true);
        aniFinished = false;
        txtScore.enabled = false;
        txtScoreGOver.text = point + "";
        Time.timeScale = 0;
    }

    
}
