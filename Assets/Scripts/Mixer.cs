using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Mixer : MonoBehaviour
{
    public GameManager gameManager;

    public int index;
    public bool alreadyIn;
    public GameObject cup;

    public int resultBeverageId;
    public int[] mixIngredientsList;

    public TextMeshProUGUI[] ingredientsUi;

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if(mixIngredientsList[i] == 9)
            {
                ingredientsUi[i].text = "";
            }
            else
            {
                ingredientsUi[i].text = gameManager.ingredientsList[mixIngredientsList[i]];
            }
        }
            
    }

    public void CupActive(bool isActive)
    {
        index = 0;
        for (int i = 0; i < 4; i++)
        {
            mixIngredientsList[i] = 9;
            ingredientsUi[i].text = "";
        }
        cup.SetActive(isActive);
    }

    public void AddIngredients(int id)
    {
        alreadyIn = false;
        if (index < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if(mixIngredientsList[i] == id)
                {
                    alreadyIn = true;
                }
            }

            if (!alreadyIn)
            {
                mixIngredientsList[index++] = id;
                gameManager.StartCoroutine("PopupMessage", gameManager.ingredientsList[id]);
            }
            else
            {
                gameManager.StartCoroutine("PopupMessage", "이미 첨가된 재료입니다");
            }
        }
    }

    public void MixButton()
    {
        resultBeverageId = 0;
        string resultBeverageName = "";

        Array.Sort(mixIngredientsList);

        for (int i = 0; i < 4; i++)
        {
            resultBeverageId += mixIngredientsList[i] * (int)Mathf.Pow(10, i);
        }

        if(!gameManager.menuList.TryGetValue(resultBeverageId, out resultBeverageName))
        {
            resultBeverageName = "아무것도 아닌 것";
        }

        gameManager.StartCoroutine("PopupMessage", resultBeverageName);

        CupActive(false);
    }
}
