using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Mixer mixer;
    public string[] ingredientsList = { "물", "얼음", "우유", "카카오", "커피" };
    public Dictionary<int, string> menuList = new Dictionary<int, string>();

    public GameObject cup;
    public TextMeshProUGUI popupMessage;

    void Start()
    {
        menuList.Add(9910, "얼음물");
        menuList.Add(9992, "우유");
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
                    int id = hit.collider.gameObject.GetComponent<ObjectInfo>().id;
                    
                    if (id == 9)
                    {
                        if (cup.activeSelf)
                        {
                            StartCoroutine("PopupMessage", "재료를 선택해주세요");
                        }
                        else
                        {
                            mixer.CupActive(true);
                        }
                    }
                    else if (mixer.index > 3)
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
    }

    IEnumerator PopupMessage(string message)
    {
        popupMessage.text = message;
        yield return new WaitForSeconds(2);
        popupMessage.text = "";
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
