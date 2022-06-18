using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mixer : MonoBehaviour
{
    public GameManager gameManager;

    public int index;
    public bool alreadyIn;

    public int[] mixList;
    public string[] ingredients;

    public TextMeshProUGUI[] ingredientsUi;

    void Start()
    {
        index = 0;
        for (int i = 0; i < index + 1; i++)
        {
            mixList[i] = -1;
            ingredientsUi[i].text = "";
        }
    }

    void Update()
    {
        for (int i = 0; i < index + 1; i++)
        {
            if(mixList[i] == -1)
            {
                ingredientsUi[i].text = "";
            }
            else
            {
                ingredientsUi[i].text = gameManager.ingredientsList[mixList[i]];
            }
        }
            
    }

    public void AddIngredients(int id)
    {
        alreadyIn = false;
        if (index < 4)
        {
            for (int i = 0; i < 4; i++)
            {
                if(mixList[i] == id)
                {
                    alreadyIn = true;
                }
            }

            if (!alreadyIn)
            {
                mixList[index++] = id;
            }
        }
        
        Debug.Log('s');
    }
}
