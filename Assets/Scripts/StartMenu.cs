using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject startBtn;
    private GameObject how_to_playBtn;
    private GameObject how_to_playMenu;
    private GameObject backBtn;
    void Start()
    {
        Time.timeScale = 0;
        startBtn = GameObject.Find("Start");
        how_to_playBtn = GameObject.Find("HowToPlay");
        how_to_playMenu = GameObject.Find("HowToPlayMenu");
        backBtn = GameObject.Find("Back");
        how_to_playMenu.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        StartCoroutine(PlayOpeningScenes());
    }
    private IEnumerator PlayOpeningScenes()
    {
        string[] scenes = { "OpenScene1", "OpenScene2", "OpenScene3"};

        foreach (string scene in scenes)
        {
            // Load the scene additively so coroutine continues
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            yield return new WaitForSeconds(2f);

            // Unload the previous scene (except for the last one)
            yield return new WaitForSeconds(2f);
            SceneManager.UnloadSceneAsync(scene);
        }

        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
    }

    public void HowToPlay()
    {
        startBtn.SetActive(false);
        how_to_playBtn.SetActive(false);
        how_to_playMenu.SetActive(true);
    }

    public void Back()
    {
        how_to_playMenu.SetActive(false);
        startBtn.SetActive(true);
        how_to_playBtn.SetActive(true);
    }
}
