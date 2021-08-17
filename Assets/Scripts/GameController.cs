using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Singleton game controller
public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    [SerializeField]
    GameObject messagePanel, loadingPanel, gamePanel, introPanel, winPanel;

    [SerializeField]
    GameObject leftContent;

 

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void StartApplication() 
    {
        var textfield = introPanel.transform.Find("Textfield");
        var path = textfield.transform.Find("InputText").GetComponent<Text>().text;

        //Check if file exists
        if (System.IO.File.Exists(path))
        {
            introPanel.SetActive(false);
            ColumnFiller columnFiller = gamePanel.GetComponent<ColumnFiller>();
            columnFiller.StartProccess(path);
        }
        else
        {
            SpawnMessage("File doesn't exist", 2);
        }
    }

    //Check if an image gameobject matches a name text gameobject
    public void CheckPair(GameObject first, GameObject second)
    {
        if (first != second && first.name == second.name)
        {
            SpawnMessage("Correct", 1);
            
            Destroy(first);
            Destroy(second);
            StartCoroutine(CheckForWin());
        }
        else
        {
            SpawnMessage("Wrong", 1);
        }
    }

    //Check for win condition
    IEnumerator CheckForWin() 
    {
        yield return new WaitForEndOfFrame();
        if (leftContent.transform.childCount < 1)
        {
            winPanel.SetActive(true);
        }
    }

    //Spawn message after a delay
    public void SpawnMessage(string message, int time) 
    {
        messagePanel.SetActive(true);
        Text text = messagePanel.GetComponentInChildren<Text>();
        text.text = message;
        StartCoroutine(ExecuteAfterTime(time));
    }

    public void SpawnLoading(bool active) 
    {
        loadingPanel.SetActive(active);
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        messagePanel.SetActive(false);
    }

    public void ToIntroScreen()
    {
        introPanel.SetActive(true);
    }

    public void ReloadApplication() 
    {
        SceneManager.LoadScene("SampleScene");
    }
}
