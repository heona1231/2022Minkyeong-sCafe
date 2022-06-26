using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBeverage : MonoBehaviour
{
    public GameManager gameManager;

    public Sprite[] beverageSprites;
    public string[] menuList;
    public Dictionary<int, int> beverageList = new Dictionary<int, int>();
    public GameObject resultBeverageObject;

    public bool alreadyBeverage = false;
    public int resultBeverageCode = 0;
    public int resultBeverageId;

    private void Start()
    {
        //0.물, 1.얼음, 2.우유, 3.카카오, 4.커피
        //0. 아무것도 아닌 것, 1. 물, 2. 얼음 물, 3. 우유, 4. 아메리카노, 5. 아이스 아메리카노, 6.초코우유, 7.아이스초코우유
        beverageList.Add(990, 1);
        beverageList.Add(910, 2);
        beverageList.Add(992, 3);
        beverageList.Add(940, 4);
        beverageList.Add(410, 5);
        beverageList.Add(932, 6);
        beverageList.Add(321, 7);
    }

    public void ResetBeverage()
    {
        resultBeverageCode = 0;
        alreadyBeverage = false;
        resultBeverageId = 0;
        resultBeverageObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        resultBeverageObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void MakeBeverage()
    {
        resultBeverageObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        if (!beverageList.TryGetValue(resultBeverageCode, out resultBeverageId))
        {
            resultBeverageId = 0;
            resultBeverageObject.GetComponent<SpriteRenderer>().sprite = beverageSprites[0];
        }
        else
        {
            resultBeverageObject.GetComponent<SpriteRenderer>().sprite = beverageSprites[resultBeverageId];
        }
        alreadyBeverage = true;
        resultBeverageObject.GetComponent<BoxCollider>().enabled = true;
        gameManager.StartCoroutine("PopupMessage", menuList[resultBeverageId]);
    }
}
