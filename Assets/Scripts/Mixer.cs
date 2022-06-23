using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Mixer : MonoBehaviour
{
    public GameManager gameManager;
    public ResultBeverage resultBeverage;

    public int index;
    public bool alreadyIn;
    public GameObject cup;

    public int[] mixIngredientsList;

    public TextMeshProUGUI[] ingredientsUi;

    public void CupActive(bool isActive)
    {
        index = 0;
        for (int i = 0; i < 3; i++)
        {
            mixIngredientsList[i] = 9;
            ingredientsUi[i].text = "";
        }
        cup.SetActive(isActive);
    }

    public void AddIngredients(int id)
    {
        alreadyIn = false;
        if (index < 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if(mixIngredientsList[i] == id)
                {
                    alreadyIn = true;
                }
            }

            if (!alreadyIn)
            {
                ingredientsUi[index].text = gameManager.ingredientList[id];
                mixIngredientsList[index++] = id;
            }
            else
            {
                gameManager.StartCoroutine("PopupMessage", "이미 첨가된 재료입니다");
            }
        }
    }

    public void MixButton()
    {
        Array.Sort(mixIngredientsList);

        for (int i = 0; i < 3; i++)
        {
            resultBeverage.resultBeverageCode += mixIngredientsList[i] * (int)Mathf.Pow(10, i);
        }

        resultBeverage.MakeBeverage();
        CupActive(false);
    }
}
