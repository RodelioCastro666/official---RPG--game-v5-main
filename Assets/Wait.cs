using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    private float waitTime = 25;

    private void Start()
    {
        StartCoroutine(WaitF());
    }

    IEnumerator WaitF()
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(1);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}

