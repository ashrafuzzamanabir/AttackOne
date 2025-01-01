using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private LayerMask playerLayer; // Player layer to check
    audioManager audioManager;
    public Animator animator;

    //private bool checkpointTriggered = false;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<audioManager>();

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            //checkpointTriggered = true;
            //checkpointreached
            animator.SetTrigger("checkpointreached");
            Debug.Log("Checkpoint reached!");
            audioManager.PlaySFX(audioManager.checkpoint);
            gameManager.UpdateCheckpoint(transform.position);
            //animator.ResetTrigger("checkpointreached");
            StartCoroutine(StopCheckpointAnimation());

        }
    }
    private IEnumerator StopCheckpointAnimation()
    {
        // Wait for 2 seconds (or desired duration)
        yield return new WaitForSeconds(1f);

        // Reset the checkpoint animation trigger
        animator.ResetTrigger("checkpointreached");
        animator.SetTrigger("IDEL");
        Debug.Log("Checkpoint animation stopped.");
    }
}
