using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Locations:")]
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private Transform destinationTransform;

    [Header("Game Info:")]
    [SerializeField] private int numberofRounds;

    [SerializeField] private int numberofTurns;

    [Header("Player Prefab:")]
    [SerializeField] private GameObject[] PlayerPrefabs;

    public float[] playerscores;

    [Header("Game Components:")]
    [SerializeField] private followCamera cam;

    [SerializeField] private Slider slider;
    [SerializeField] private float timeBetweenturns = 5f;
    [SerializeField] private float timeBetweenRounds = 5f;

    [Header("UI Elements")]
    bool ActiveUI = false; 
    [SerializeField] private GameObject ScoreScreen;
    [SerializeField] private Text Player1MenuScore;
    [SerializeField] private Text Player2MenuScore;
    [SerializeField] private Text ScoreText;

   


    private playerBall activeBall;

    private float timeElapsed;
    private bool timerEnabled = false;
  
    private bool ballActive = false;
    private int activePlayer = 0;

    public float turn;
    public float round;

    bool gameOver = false;


    [SerializeField] private float ScoreRadiusMiddle = 2;
    [SerializeField] private float ScoreRadiusRed = 2;
    [SerializeField] private float ScoreRadiusOuterWhite = 2;
    [SerializeField] private float ScoreRadiusBlue = 2;

    // Use this for initialization
    private void Start()
    {
        turn = numberofTurns;
        round = numberofRounds;
        playerscores = new float[PlayerPrefabs.Length];
        InstantiatePlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameOver)
        {
            return;
        }


        //Activate ball if not already Active / Deactivate once it stops moving
        if (Input.GetKeyDown(KeyCode.Space) && !ballActive)
        {
            activeBall.Activate();
            activeBall.transform.parent = null;
            ballActive = true;
        }

        if (ballActive && activeBall.GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            activeBall.DisableBall();
            timerEnabled = true;
        }

        if (turn == 0)
        {
            round--;
         
                CalculateScore();
            
            
            turn = numberofTurns;

            //return;
        }

        //pause Between Turns
        if (timerEnabled)
        {
            timeElapsed += Time.deltaTime;
        }

        if (ActiveUI)
        {
            if (timerEnabled && timeElapsed >= timeBetweenRounds)
            {
                ActiveUI = false;
                InstantiatePlayer();
                ScoreScreen.SetActive(false);
                timerEnabled = false;
                timeElapsed = 0;
                

            }
        }
        else
        {
            if (timerEnabled && timeElapsed >= timeBetweenturns)
            {
                activePlayer++;

                if (activePlayer > 1)
                {
                    activePlayer = 0;
                    turn--;
                }

                InstantiatePlayer();
                ballActive = false;
                timerEnabled = false;
                timeElapsed = 0;
            }

        }

        
    }

    private void InstantiatePlayer()
    {
        activeBall = Instantiate(PlayerPrefabs[activePlayer], spawnPosition).GetComponent<playerBall>();
        cam.ChangeTarget(activeBall.transform);
        if (round > 0)
        {
            activeBall.EnableBall(slider, destinationTransform);

        }
    }

    private void CalculateScore()
    {
        if (round == 0)
        {
            gameOver = true;
        }
        for (var i = 0; i < PlayerPrefabs.Length; i++)
        {
            GameObject[] playerPucks = GameObject.FindGameObjectsWithTag(PlayerPrefabs[i].tag);
            for (var x = 0; x < playerPucks.Length; x++)
            {
                float distanceBetween = Vector3.Distance(playerPucks[x].transform.position, destinationTransform.position);
                if (distanceBetween < ScoreRadiusMiddle)
                {
                    playerscores[i] += 100;
                }
                else
                if (distanceBetween < ScoreRadiusRed)
                {
                    playerscores[i] += 50;
                }
                else
                if (distanceBetween < ScoreRadiusOuterWhite)
                {
                    playerscores[i] += 25;
                }
                else
                if (distanceBetween < ScoreRadiusBlue)
                {
                    playerscores[i] += 10;
                }
                Destroy(playerPucks[x]);
            }
            if (gameOver)
            {
                ScoreScreen.SetActive(true);
                if (playerscores[0] > playerscores[1])
                {
                    ScoreText.text = "Player 1 Wins!";
                }
                else if (playerscores[0] < playerscores[1])
                {
                    ScoreText.text = "Player 2 Wins!";

                }
                else
                {
                    ScoreText.text = "It's A a Tie!";
                }
                Player1MenuScore.text = playerscores[0].ToString();
                Player2MenuScore.text = playerscores[1].ToString();

                return;
            }
            ActiveUI = true;
            timerEnabled = true;
            ScoreScreen.SetActive(true);
            Player1MenuScore.text = playerscores[0].ToString();
            Player2MenuScore.text = playerscores[1].ToString();
            
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationTransform.position, ScoreRadiusMiddle);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(destinationTransform.position, ScoreRadiusRed);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(destinationTransform.position, ScoreRadiusOuterWhite);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(destinationTransform.position, ScoreRadiusBlue);
    }
}