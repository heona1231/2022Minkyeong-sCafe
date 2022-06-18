using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Mixer mixer;
    public string[] ingredientsList = { "물", "얼음", "우유", "카카오", "커피" };

    public GameObject cup;
    public GameObject mixList;

    void Start()
    {

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
                    if (id == -1)
                    {
                        if (cup.activeSelf)
                        {
                            Debug.Log("이미 켜있음");
                        }
                        else
                        {
                            cup.SetActive(true);
                            mixList.SetActive(true);
                        }
                    }
                    else
                    {
                        if (cup.activeSelf)
                        {
                            mixer.AddIngredients(id);
                            Debug.Log("AddIngredients" + id);
                        }
                        else
                        {
                            Debug.Log("컵켜");
                        }
                    }
                }
            }
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
