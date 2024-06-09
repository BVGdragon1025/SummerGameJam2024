using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public KeyCode pauseKey;

    public bool isPaused;

    public Animator HeaderAnimator;
    public Animator HandAnimator;
    public Animator PanelAnimator;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        HeaderAnimator.SetBool("Show", true);
        HandAnimator.SetBool("Show", true);
        PanelAnimator.SetBool("Show", true);
    }
    public void ResumeGame()
    {
        HeaderAnimator.SetBool("Show", false);
        HandAnimator.SetBool("Show", false);
        PanelAnimator.SetBool("Show", false);
        //yield return new WaitForSeconds(1.5f);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
 
