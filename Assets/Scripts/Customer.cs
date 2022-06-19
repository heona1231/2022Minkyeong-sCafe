using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer : MonoBehaviour
{
    public GameObject customer;
    public TextMeshProUGUI dialogue;
    public GameObject dialogueSprite;

    public bool alreadyExit = false;
    public int randomMenu;
    public Sprite[] customerSprites;
    public Dictionary<int, string[]> dialogues = new Dictionary<int, string[]>();

    void Start()
    {
        //0. 아무것도 아닌 것, 1. 물, 2. 얼음 물, 3. 우유, 4. 아메리카노, 5. 아이스 아메리카노
        dialogues.Add(0, new string[] { "아무것도 아닌 것 주세요" });
        dialogues.Add(1, new string[] { "물 주세요" });
        dialogues.Add(2, new string[] { "얼음 물 주세요" });
        dialogues.Add(3, new string[] { "우유 주세요" });
        dialogues.Add(4, new string[] { "아메리카노줘", "아이스 아메리카노 따뜻하게주세요" });
        dialogues.Add(5, new string[] { "아아줘", "아이스 아메리카노 주세요" });
    }

    public void ResetCustomer()
    {
        alreadyExit = false;
        dialogue.text = "";
        customer.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        dialogueSprite.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    public void CreateCustomer()
    {
        int randomPeople = Random.Range(0, customerSprites.Length);
        randomMenu = Random.Range(0, 5);
        int randomDialogue = Random.Range(0, dialogues[randomMenu].Length);

        customer.GetComponent<SpriteRenderer>().sprite = customerSprites[randomPeople];
        Debug.Log(dialogues[randomMenu][randomDialogue]);
        dialogue.text = dialogues[randomMenu][randomDialogue];

        customer.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        dialogueSprite.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }
}
