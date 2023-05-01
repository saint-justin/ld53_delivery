using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    // TextMesh components
    public TextMeshProUGUI speakerBadgeLComponent;
    private GameObject badgeLParent;
    public TextMeshProUGUI speakerBadgeRComponent;
    private GameObject badgeRParent;
    public TextMeshProUGUI dialogueTextComponent;

    [Tooltip("Script for the scene")]
    public TextAsset sceneScript;
    private List<DialogueEntry> lines;

    [Tooltip("Speed of text (chars p/ second)")]
    public float textSpeed = 0.05f;
    private int lineIndex;

    [Tooltip("Default sprite to pull in case character sprite is not found")]
    public Texture2D defaultCharacter;

    [Tooltip("Character sprites (order matters)")]
    public List<Sprite> characterSprites;

    public GameObject character1;
    public GameObject character2;

    void Start()
    {
        speakerBadgeRComponent.text = "";
        dialogueTextComponent.text = "";
        lines = new List<DialogueEntry>();

        badgeLParent = speakerBadgeLComponent.transform.parent.gameObject;
        badgeRParent = speakerBadgeRComponent.transform.parent.gameObject;

        ParseScript(sceneScript);
        StartDialogue();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (dialogueTextComponent.text == lines[lineIndex].lineText)
            {
                NextLine();
            } else {
                // Skip to end of text line
                StopAllCoroutines();
                dialogueTextComponent.text = lines[lineIndex].lineText;
            }
        }
    }

    void ParseScript(TextAsset script)
    {
        List<string> parsedLines = new List<string>(sceneScript.text.Split('\n'));

        // Updates sprites + badges to reflect speakers
        character1.GetComponent<SpriteRenderer>().sprite = characterSprites[0];
        character2.GetComponent<SpriteRenderer>().sprite = characterSprites[1];
        string speakers = parsedLines[0];
        speakerBadgeLComponent.text = speakers.Split('/')[0];
        speakerBadgeRComponent.text = speakers.Split('/')[1];
        parsedLines.RemoveAt(0); // remove line delineating who's speaking

        // Split off script into list of dialogue entries
        foreach (string line in parsedLines) {
            string entryCharacter = line.Split(':')[0].Trim();
            string entryText = line.Split(':')[1].Trim();
            DialogueEntry entry = new DialogueEntry(entryCharacter, entryText, GetIndexFromCharName(entryCharacter));
            lines.Add(entry);
        }
    }

    void StartDialogue()
    {
        lineIndex = 0;
        StartCoroutine(TypeLine());
        UpdateActiveSpeakerBadge();
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[lineIndex].lineText.ToCharArray())
        {
            dialogueTextComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (lineIndex < lines.Count - 1)
        {
            // Update dialogue
            lineIndex += 1;
            dialogueTextComponent.text = "";
            UpdateActiveSpeakerBadge();
            StartCoroutine(TypeLine());
        } 
        else
        {
            // Disable when all lines have been read
            gameObject.SetActive(false);

            SceneManager.LoadScene("InventoryTestScene");
        }
    }

    void UpdateActiveSpeakerBadge()
    {
        if (speakerBadgeLComponent.text == lines[lineIndex].nameText)
        {
            badgeLParent.SetActive(true);
            badgeRParent.SetActive(false);
        }
        else
        {
            badgeLParent.SetActive(false);
            badgeRParent.SetActive(true);
        }
    }

    // TODO: Swap to an enum
    int GetIndexFromCharName(string name)
    {
        switch(name.ToLower())
        {
            case "boss":
                return 0;
            case "kid":
                return 1;
            default:
                return -1;
        }
    }
}
