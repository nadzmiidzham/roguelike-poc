using UnityEngine;
using UnityEngine.UI;

public class InteractableController : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject dialogue;
    public Text dialogueText;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private bool isTouchingPlayer = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isTouchingPlayer = boxCollider.IsTouchingLayers(playerLayer);
        spriteRenderer.color = isTouchingPlayer ? Color.yellow : Color.green;
        dialogue.SetActive(isTouchingPlayer);

        if (!isTouchingPlayer)
        {
            dialogueText.text = "Press 'E' to interact.";
        }
        if (isTouchingPlayer && Input.GetKeyDown(KeyCode.E))
        {
            dialogueText.text = "Received potion.";
        }
    }
}
