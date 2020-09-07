using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than one instance of DiaglogueManager found!");
            return;
        }
    }

    #endregion

    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    public GameObject crosshair;

    private Queue<string> sentences;

    public bool isInDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }


    private void Update()
    {
        if (isInDialogue && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        isInDialogue = true;

        animator.SetBool("IsOpen", true);
        crosshair.SetActive(false);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }


    void EndDialogue ()
    {
        StartCoroutine(SetNotInDialogue());

        animator.SetBool("IsOpen", false);
        crosshair.SetActive(true);
        Debug.Log("End of conversation.");
    }

    IEnumerator SetNotInDialogue ()
    {
        yield return null;
        isInDialogue = false;
    }
}
