using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject button;
    public Image fadeIn;
    public Color alpha;

    private float timer;

    void Start()
    {
        button.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3.5f)
            StartCoroutine(FadeImage(true));
    }

    IEnumerator FadeImage(bool fadeAway)
    {

        button.SetActive(true);
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fadeIn.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    public void StartGame()
    { 
        SceneManager.LoadScene(1);
    }

}
