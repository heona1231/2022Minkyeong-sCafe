using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Mixer mixer;
    public ResultBeverage resultBeverage;
    public Customer customer;

    public Dictionary<int, string> ingredientList = new Dictionary<int, string>();

    public GameObject cup;
    public GameObject recipeBook;
    public TextMeshProUGUI popupMessage;
    public GameObject satisfyGauge;

    public float timer;
    public float customerState;
    public float satisfy = 50.0f;

    void Start()
    {
        ingredientList.Add(0, "물");
        ingredientList.Add(1, "얼음");
        ingredientList.Add(2, "우유");
        ingredientList.Add(3, "카카오");
        ingredientList.Add(4, "커피");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Object")
                {
                    int id = hit.collider.gameObject.GetComponent<ObjectInfo>().objectId;

                    if (id == 9)
                    {
                        if (resultBeverage.alreadyBeverage)
                        {
                            StartCoroutine("PopupMessage", "이미 만들어진 음료가 있습니다\n(음료를 다시 만드려면 먼저 쓰레기통에 버려주세요)");
                        }
                        else if (cup.activeSelf)
                        {
                            StartCoroutine("PopupMessage", "재료를 선택해주세요");
                        }
                        else
                        {
                            mixer.CupActive(true);
                        }
                    }
                    else if(id == 8)
                    {
                        mixer.CupActive(false);
                        resultBeverage.ResetBeverage();
                        StartCoroutine("PopupMessage", "음료 버리기");
                    }
                    else if (id == 7)
                    {
                        recipeBook.SetActive(true);
                    }
                    else if (id == 6)
                    {
                        if (customer.alreadyExit)
                        {
                            if (customer.randomMenu == resultBeverage.resultBeverageId)
                            {
                                Debug.Log("c");
                                satisfy += customerState;
                            }
                            else
                            {
                                satisfy -= 10.0f;
                            }
                            customer.ResetCustomer();
                            resultBeverage.ResetBeverage();
                        }
                    }
                    else if (mixer.index > 2)
                    {
                        StartCoroutine("PopupMessage", "컵이 가득 찼습니다");
                    }
                    else
                    {
                        if (cup.activeSelf)
                        {
                            mixer.AddIngredients(id);
                        }
                        else
                        {
                            StartCoroutine("PopupMessage", "컵이 없습니다");
                        }
                    }
                }
            }
        }

        if (satisfy > 100.0f) satisfy = 100.0f;
        if (satisfy < 0.0f) satisfy = 0.0f;

        satisfyGauge.GetComponent<Image>().fillAmount = satisfy / 100.0f;

        if (customer.alreadyExit)
        {
            timer += Time.deltaTime;
            if(timer < 4.0f)
            {
                customerState = 20.0f;
            }
            else if(timer > 4.0f && timer < 8.0f)
            {
                customerState = 10.0f;
            }
            else if (timer > 8.0f && timer < 12.0f)
            {
                customerState = 0.0f;
            }
            else
            {
                customerState = -10.0f;
            }
        }
        else
        {
            StartCoroutine("CustomerGenerator", (100.0f - satisfy) / 50);
        }
    }

    IEnumerator PopupMessage(string message)
    {
        popupMessage.text = message;
        yield return new WaitForSecondsRealtime(2.5f);
        popupMessage.text = ""; 
    }

    IEnumerator CustomerGenerator(float spantime)
    {
        yield return new WaitForSecondsRealtime(10.0f + spantime);
        //customer.CreateCustomer();
        customer.alreadyExit = true;
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
