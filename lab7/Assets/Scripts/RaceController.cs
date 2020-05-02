using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Vehicles.Car;

public enum RaceState
{
    START,
    RACING,
    FINISH
}
public class RaceController : MonoBehaviour
{
    public Text resultText;
    public Text timeText;

    public CarController[] cars;

    public ScoreManager scoreData;

    RaceState raceState;

    float startTime;

    float duration;

    Color defaultColor;

    private bool playerStarted = false;
    private bool enemyStarted = false;

    private bool updateTime = true;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = resultText.color;
        raceState = RaceState.START;

        foreach (CarController car in cars)
        {
            car.MaxSpeed = 0;
        }

        StartCoroutine(startCountdown());
    }

    Color[] colors =
    {
        Color.green,
        (Color.red + Color.yellow) / 2.0f,
        Color.red
    };

    IEnumerator startCountdown()
    {
        int count = 3;
        while (count > 0)
        {
            resultText.color = colors[count - 1];
            resultText.text = "" + count;
            count--;
            yield return new WaitForSeconds(1);
        }

        resultText.color = Color.red;
        raceState = RaceState.RACING;
        startTime = Time.time;
        resultText.text = "GO!";

        foreach (CarController car in cars)
        {
            car.MaxSpeed = 50;
        }

        yield return new WaitForSeconds(1);

        resultText.color = defaultColor;
        resultText.text = "";
        resultText.enabled = false;

        // what even is this line lmao
        resultText.gameObject.transform.parent.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (raceState == RaceState.RACING)
        {
            if (updateTime) duration = Time.time - startTime;
            timeText.text = duration.ToString("F2");
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (raceState != RaceState.RACING) return;

        if (other.gameObject.tag == "Player")
        {
            if (!playerStarted)
            {
                playerStarted = true;
                Debug.Log("Player has started");
            }
            else
            {
                Debug.Log("Player has finished");
                StartCoroutine(ShowResult("You Win!"));
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (!enemyStarted)
            {
                enemyStarted = true;
                Debug.Log("Enemy has started");
            }
            else
            {
                Debug.Log("Enemy has finished");
                StartCoroutine(ShowResult("You Lose!"));
            }
        }
    }

    IEnumerator ShowResult(string text)
    {
        
        resultText.gameObject.transform.parent.gameObject.SetActive(true);
        resultText.text = text;
        resultText.enabled = true;
        
        updateTime = false;

        scoreData.setTime(duration, 1);

        yield return new WaitForSeconds(5);
        // Exit Game
        SceneManager.LoadScene(0);
    }
}