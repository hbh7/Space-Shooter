using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public GameObject RollaBall;
    public GameObject Player;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float level;

	public Text scoreText;
    public Text levelText;
    public Text gameOverText;
    public Text winText;
    public GameObject restartButton;

    private bool gameOver;
    private bool restart;
    private bool won;
	private int score;
	
    void Start()
    {
        gameOver = false;
        restart = false;
        won = false;
        restartButton.SetActive(false);
        gameOverText.text = "";
        winText.text = "";
        score = 0;
		UpdateScore();
        StartCoroutine (SpawnWaves());
        Screen.SetResolution(600, 900, true);
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    IEnumerator SpawnWaves ()
    {
        yield return new WaitForSeconds(startWait);
        while (true) 
        { 
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0,hazards.Length)]; 
                if (hazard == RollaBall)
                {
                    if (Random.Range(1,10) == 1)
                    {
                        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        Quaternion spawnRotation = Quaternion.identity;
                        Instantiate(hazard, spawnPosition, spawnRotation);
                    }
                    else
                    {
                        hazard = hazards[Random.Range(0, hazards.Length)];
                        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                        Quaternion spawnRotation = Quaternion.identity;
                        Instantiate(hazard, spawnPosition, spawnRotation);
                        yield return new WaitForSeconds(spawnWait);
                    }

                }
                else
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                }
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (spawnWait > .2)
            {
                spawnWait = spawnWait - (spawnWait/4);
                Debug.Log(spawnWait);
            }
            UpdateLevel();
            if (won == true)
            {
                break;
            }
            hazardCount = hazardCount + (hazardCount/2);
            Debug.Log(hazardCount);

            if (gameOver)
            {
                //restartText.text = "Press 'R' for Restart";
                restartButton.SetActive(true);
                restart = true;
                break;
            }
            
        }
    }
	
	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore();
	}
	
	void UpdateScore() 
	{
		scoreText.text = "Score: " + score;
	}

    void UpdateLevel()
    {
        if (gameOver == false)
        {
            level = level + 1;
            Debug.Log("Updating Level");
            if (level == 11)
            {
                winText.text = "You've WON!!!";
                won = true;
                restartButton.SetActive(true);
                gameOver = true;
                restart = true; 
            }
            else
            {
                levelText.text = "Level: " + level;
            }
        }
    }

    public void GameOver ()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        restartButton.SetActive(true);
        restart = true;
    }

    public void RestartGame ()
    {
        Debug.Log("Trying to restart game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
