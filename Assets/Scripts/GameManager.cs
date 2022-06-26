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
    public GameObject stopPage;
    public GameObject gameoverPage;

    public float customerTimer;
    public float customerState;
    public float satisfy = 50.0f;
    public bool isCoroutineActive = false;

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
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopButton();

        }

        if(Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Ray ray1 = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit1;
            if(Physics.Raycast(ray1, out hit1))
            {
                if(hit1.transform.tag == "Object")
                {
                    int id = hit1.collider.gameObject.GetComponent<ObjectInfo>().objectId;

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
                    else if (id == 8)
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
                                satisfy += customerState;
                            }
                            else
                            {
                                satisfy -= 20.0f;
                            }
                            isCoroutineActive = false;
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
#endif
#if UNITY_EDITOR
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
                                satisfy += customerState;
                            }
                            else
                            {
                                satisfy -= 10.0f;
                            }
                            isCoroutineActive = false;
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
#endif

        if (satisfy > 100.0f) satisfy = 100.0f;
        if (satisfy <= 0.0f)
        {
            gameoverPage.SetActive(true);
            Time.timeScale = 0;
        }
        
        satisfyGauge.GetComponent<Image>().fillAmount = satisfy / 100.0f;

        if (customer.alreadyExit)
        {
            customerTimer += Time.deltaTime;
            if(customerTimer < 2.0f)
            {
                customerState = 15.0f;
            }
            else if(customerTimer > 2.0f && customerTimer < 8.0f)
            {
                customerState = 5.0f;
            }
            else if (customerTimer > 8.0f && customerTimer < 10.0f)
            {
                customerState = -15.0f;
            }
            else
            {
                satisfy -= 10.0f;
                isCoroutineActive = false;
                customer.ResetCustomer();
                resultBeverage.ResetBeverage();
            }
        }
        else if(!customer.alreadyExit && !isCoroutineActive)
        {
            StartCoroutine("CustomerGenerator", (100.0f - satisfy) / 50);
            isCoroutineActive = true;
            customerTimer = 0.0f;
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
        yield return new WaitForSecondsRealtime(5.0f + spantime);
        customer.CreateCustomer();
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_ANDROID
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        Application.Quit();
    }

    public void StopButton()
    {
        stopPage.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeButton()
    {
        stopPage.SetActive(false);
        Time.timeScale = 1;
    }
    public void ReRodeButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
