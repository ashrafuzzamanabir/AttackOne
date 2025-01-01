using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxDeaths = 3;
    
    [SerializeField] private string winningPointLayerName = "Winning Point";
    [SerializeField] private string finishSceneName = "Finish"; // Scene to load after finishing
    [SerializeField] public Text resultText;
    private Vector2 CheckpointPos;

    public Animator animator;
    audioManager audioManager;

    [SerializeField] public GameObject deadimage;

    //deathcount
    public int deathCount = 0;
    private bool isGameFinished = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();
    }

    private void Start()
    {
        CheckpointPos = transform.position;
        if (string.IsNullOrEmpty(finishSceneName))
        {
            Debug.LogError("Finish scene name is not assigned in the GameManager!");
        }
    }

    public void UpdateCheckpoint(Vector2 newCheckpoint)
    {
        CheckpointPos = newCheckpoint;
        Debug.Log($"Checkpoint updated to: {CheckpointPos}");
    }

    public void PlayerDied()
    {
        if (isGameFinished) return;

        deathCount++;
        Debug.Log($"Player died {deathCount}/{maxDeaths} times.");

        if (deathCount == maxDeaths)
        {
            audioManager.PlaySFX(audioManager.lost);
            animator.SetTrigger("lost");
            deadimage.SetActive(true);
            StartCoroutine(PlayLostAnimationAndEndGame());
            //EndGame(false);
        }
        else
        {
            RespawnPlayer();
        }
    }

    private IEnumerator PlayLostAnimationAndEndGame()
    {
        // Wait for the duration of the "lost" animation
        yield return new WaitForSeconds(1f);

        // End the game after the animation has played
        EndGame(false);
        deadimage.SetActive(false);

    }

    public void PlayerWon()
    {
        if (isGameFinished) { return; }

        EndGame(true);
    }

    public void EndGame(bool hasWon)
    {
        isGameFinished = true;

        // Pass information to the Finish Scene via PlayerPrefs
        PlayerPrefs.SetString("GameResult", hasWon ? "Win" : "Lose");

        

        if (resultText != null)
            {
                resultText.text = hasWon ? "You win the Game!" : "You Died!!";
            }

        
        // Load the finish scene
        SceneManager.LoadScene(finishSceneName);
    }

    public void RespawnPlayer()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && CheckpointPos != null)
        {
            player.Respawn(CheckpointPos);
        }
        else
        {
            Debug.LogError("Respawn point or player is missing!");
        }
        //deathCount++;
        if (deathCount >= maxDeaths) {
            // Load the finish scene
            SceneManager.LoadScene(finishSceneName);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(winningPointLayerName))
        {
            PlayerWon();
        }
    }

    // Restart the game by reloading the current scene
    public void RestartGame()
    {
        Debug.Log("Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit the application
    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}
