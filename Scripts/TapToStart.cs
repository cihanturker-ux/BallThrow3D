using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TapToStart : MonoBehaviour
{
    public static TapToStart instance;

    public List<GameObject> feedbacks;
    public GameObject canvas;

    [SerializeField] private GameObject tapToStart, tapToStart2, tapToStart3, gameOver, finish, crosshair;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (!PlayerController.instance.hasGameStarted)
        {
            StartTheGame();
        }
    }

    private void StartTheGame()
    {
        if (Input.GetMouseButton(0))
        {
            tapToStart.SetActive(false);
            tapToStart2.SetActive(false);
            tapToStart3.SetActive(false);
            PlayerController.instance.hasGameStarted = true;
        }
    }

    public void LevelFailed()
    {
        crosshair.GetComponent<Image>().enabled = false;
        gameOver.gameObject.SetActive(true);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LevelFinished()
    {
        PlayerController.instance.hasGameFinished = true;
        PlayerController.instance.hasRotationStarted = false;
        PlayerController.instance.StopAllCoroutines();
        PlayerAnimator.instance.animator.SetBool("isShooting", false);

        crosshair.GetComponent<Image>().enabled = false;
        finish.SetActive(true);
    }
}
