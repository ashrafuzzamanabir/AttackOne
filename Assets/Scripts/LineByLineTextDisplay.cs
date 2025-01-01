using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LineByLineTextDisplay : MonoBehaviour
{
    public GameObject instructionCanvas; // Reference to the Canvas GameObject
    public Text instructionText; // UI Text component for displaying the dialogue
    [TextArea(3, 10)] public string[] instructions; // Array of strings for instructions
    public float typingSpeed = 0.05f; // Speed at which each character is displayed

    private bool isDisplaying = false; // To ensure only one interaction happens at a time

    private void Start()
    {
        // Ensure the Canvas is hidden at the start
        if (instructionCanvas != null)
        {
            instructionCanvas.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Instruction Canvas not assigned in the Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object colliding is the Player
        if (collision.CompareTag("Player") && !isDisplaying)
        {
            if (instructionCanvas != null)
            {
                instructionCanvas.SetActive(true); // Show the Canvas
                StartCoroutine(DisplayTextLineByLine()); // Start text display coroutine
            }
        }
    }

    private IEnumerator DisplayTextLineByLine()
    {
        isDisplaying = true; // Lock interaction
        instructionText.text = ""; // Clear existing text

        foreach (string line in instructions)
        {
            for (int i = 0; i < line.Length; i++)
            {
                instructionText.text += line[i]; // Add one character at a time
                yield return new WaitForSeconds(typingSpeed); // Wait for typing effect
            }
            yield return new WaitForSeconds(1f); // Wait before showing the next line
            instructionText.text = ""; // Clear text before next line
        }

        yield return new WaitForSeconds(1f); // Additional delay
        instructionCanvas.SetActive(false); // Hide the Canvas
        isDisplaying = false; // Unlock interaction

        // Destroy the object after displaying the message
        Destroy(gameObject);
    }
}
