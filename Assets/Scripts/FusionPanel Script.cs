using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class FusionPanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private GameObject fusion;
    private GameObject reactor;
    private GameObject fusionPanel;
    private GameObject selectionPanel;
    private GameObject leftPanel;
    private GameObject rightPanel;
    private GameObject topPanel;
    public Sprite[] elements;
    public Sprite[] mixings;
    private Image[] element_panels;
    AudioManager audioManager;
    private bool selected = false, fused = false;
    int sumIndex = 0;
    private void Awake()
    {
        player = GameObject.Find("Player");
        fusion = GameObject.Find("Fusion");
        reactor = GameObject.Find("Reactor");
        fusionPanel = GameObject.Find("FusionPanel");
        selectionPanel = GameObject.Find("SelectionPanel");
        leftPanel = GameObject.Find("LeftPanel");
        leftPanel.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        rightPanel = GameObject.Find("RightPanel");
        rightPanel.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        topPanel = GameObject.Find("MixPanel");
        topPanel.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        fusionPanel.SetActive(false);
        foreach (var item in Attack.ores)
        {
            if (item.Value)
            {
                selectionPanel.gameObject.transform.GetChild(item.Key).GetChild(0).GetComponent<Image>().sprite = elements[item.Key];
            }
            else
            {
                selectionPanel.gameObject.transform.GetChild(item.Key).GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float dist = Vector2.Distance(player.transform.position, fusion.transform.position);
            if (dist < 3f)
            {
                fusionPanel.SetActive(true);
                Time.timeScale = 0;
                audioManager.PlaySFX(audioManager.pressingControlPanel);

                //player.SetActive(false);
            }
        }
    }

    public void BackToGame()
    {
        fusionPanel.SetActive(false);
        Time.timeScale = 1;
        //player.SetActive(true);
    }

    public void Select(GameObject clickedButton)
    {
        if (!fused)
        {
            if (!selected)
            {
                leftPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
                leftPanel.transform.GetChild(0).GetComponent<Image>().sprite = clickedButton.transform.GetChild(0).GetComponent<Image>().sprite;
                selected = true;
                sumIndex += System.Array.IndexOf(elements, clickedButton.transform.GetChild(0).GetComponent<Image>().sprite);
            }
            else
            {
                rightPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
                rightPanel.transform.GetChild(0).GetComponent<Image>().sprite = clickedButton.transform.GetChild(0).GetComponent<Image>().sprite;
                topPanel.transform.GetChild(0).GetComponent<Image>().enabled = true;
                sumIndex += System.Array.IndexOf(elements, clickedButton.transform.GetChild(0).GetComponent<Image>().sprite);
                sumIndex -= sumIndex == 5 ? 3 : 1;
                topPanel.transform.GetChild(0).GetComponent<Image>().sprite = mixings[sumIndex];
                fused = true;

                // Start the end credits sequence
                StartCoroutine(HandleEndCredits(sumIndex));
            }
            clickedButton.GetComponent<Button>().enabled = false;
        }
    }
    private IEnumerator HandleEndCredits(int index)
    {
        // Load Win or Lose scene based on the index
        string sceneToLoad = index == 0 ? "Win" : "Lose";
        SceneManager.LoadScene(sceneToLoad);

        // Wait ? seconds on the end scene before exiting
        yield return new WaitForSeconds(0.5f);

        // Quit the application
        Application.Quit();
    }

}
