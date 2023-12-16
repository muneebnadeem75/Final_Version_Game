using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SnakeController : MonoBehaviour
 {

    // Settings
    
    public float snakeMoveSpeed = 5;
    public float snakeRotationSpeed = 180;
    public float snakeBodySpeed = 5;
    public int bodyGap = 10;

    public float spawnRadius = 15f;
    
    public static int score = 0;

    // References
    public GameObject BodyPrefab;

    public GameObject foodPrefab;


     private AudioSource snakeAudio; 
    public AudioClip snakeEatSound;
    public AudioClip snakeCrashSound;

    public ParticleSystem crashParticle;

    public TextMeshProUGUI scoreText;



    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

     private void StartingScoreForLevel() 
    {
        int Level = SceneManager.GetActiveScene().buildIndex;
        if (Level == 1) { // Level 2
            score = 0;
        } else if (Level == 2) { // Level 3
            score = 100;
        }
            else if (Level == 3) { // Level 3
            score = 200;    
        } 
        else {
            score = 0; // Reset for Level 1 or any other level
        }

        scoreText.text = "Score: " + score;
    }


    // Start is called before the first frame update
    void Start() 
    {
        snakeAudio = GetComponent<AudioSource>();
        StartingScoreForLevel();
       
    }
    
    // Update is called once per frame
    void Update() 
    {

        // here we are making our snake move forward
        transform.position += transform.forward * snakeMoveSpeed * Time.deltaTime;

        // here we are adding rotation  to our snake horizontally
        float snakeDirection = Input.GetAxis("Horizontal"); 
        transform.Rotate(Vector3.up * snakeDirection * snakeRotationSpeed * Time.deltaTime);

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts) {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * bodyGap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * snakeBodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;  
        }
    }

    private void SnakeGrowing() 
    {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }

     private void AddScore(int amount)
        {
             score += amount;
             scoreText.text = "Score: " + score;
              if (score == 0) 
                {
                    SceneManager.LoadScene("Level_1");
                }
            
              else if(score == 100) 
                {
                    SceneManager.LoadScene("Level_2");
                }
              else if (score == 200) 
                {
                    SceneManager.LoadScene("Level_3");
                }
            
        }



   // Spawn food with collision check
    private void SpawnFood()
    {
        // Calculate random position within spawn radius
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0.3f, Random.Range(-spawnRadius, spawnRadius));

        // Check if the spawn position is valid (not colliding with obstacles)
        if (!IsCollidingWithObstacle(spawnPosition))
        {
            // Instantiate food at the random position
            Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            // Retry spawning if the position is not valid
            SpawnFood();
        }
    }

    // Check if the position is colliding with obstacles
    private bool IsCollidingWithObstacle(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f); 

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Obstacles"))
            {
                return true; // Colliding with an obstacle
            }
        }

        return false; // Not colliding with any obstacles
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            SnakeGrowing();
            AddScore(10);
            SpawnFood();
            snakeAudio.PlayOneShot(snakeEatSound,1.0f);
        }

        else if (other.CompareTag("Obstacles")) // Check for obstacle collision
        {
            crashParticle.Play();
            snakeAudio.PlayOneShot(snakeCrashSound,1.0f);
            Invoke("LoadGameOverSceneDelayed", 0.5f);
        }
        
    }

    private void LoadGameOverSceneDelayed()
    {
        LoadGameOverScene();
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameLost");
    }
}


    
 
        
