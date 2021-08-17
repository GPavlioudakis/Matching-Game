using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ColumnFiller : MonoBehaviour
{
    [SerializeField]
    GameObject imagePrefab;

    [SerializeField]
    GameObject textPrefab;

    LineDrawer lineDrawer;

    FileReader fileReader;

    Dictionary<string, string> animal_url_dict;

    GameObject leftColumn, leftContent, rightColumn, rightContent;

    public void StartProccess(string filepath)
    {
        fileReader = new FileReader();
        lineDrawer = GetComponentInParent<LineDrawer>();
        animal_url_dict = fileReader.ReadDataFromFile(filepath);
        if (animal_url_dict == null)
        {
            GameController.instance.SpawnMessage("File is empty", 2);
            GameController.instance.ToIntroScreen();
        }
        else
            FillGrid(animal_url_dict);

        //Left content for animal images and right content for animal names
        leftColumn = GameObject.Find("Left Column");
        leftContent = leftColumn.transform.GetChild(0).gameObject;
        rightColumn = GameObject.Find("Right Column");
        rightContent = rightColumn.transform.GetChild(0).gameObject;
    }


    
    void FillGrid(Dictionary<string, string> dictionary)
    {
        List<GameObject> leftGameObjects = new List<GameObject>();
        List<GameObject> rightGameObjects = new List<GameObject>();

        int counter = 0;

        GameController.instance.SpawnLoading(true);

        //Pass through the dictionary to instatiate the UI gameobjects and fill every column
        foreach (KeyValuePair<string, string> p in dictionary) 
        {
            //Downloading images
            StartCoroutine(DownloadImage(p.Value, (texture)=> {
                counter++;

                GameObject imageGameObject = Instantiate(imagePrefab, leftContent.transform);
                RawImage image = imageGameObject.GetComponentInChildren<RawImage>();
                image.texture = texture;
                //p.key is each animal's name
                imageGameObject.name = p.Key;

                leftGameObjects.Add(imageGameObject);

                GameObject textGameObject = Instantiate(textPrefab, rightContent.transform);
                Text text = textGameObject.GetComponentInChildren<Text>();
                textGameObject.name = p.Key;
                text.text = p.Key;

                rightGameObjects.Add(textGameObject);

                //When a button is pressed, we start drawing a line or end the drawing
                Button buttonLeft = imageGameObject.GetComponentInChildren<Button>();
                buttonLeft.onClick.AddListener(() => lineDrawer.StartDrawingLine(imageGameObject));

                Button buttonRight = textGameObject.GetComponentInChildren<Button>();
                buttonRight.onClick.AddListener(() => lineDrawer.StartDrawingLine(textGameObject));

                //When downloading is done we suffle the two columns
                if (counter == dictionary.Count)
                    Shuffle(leftGameObjects, rightGameObjects);
            }));
        }
    }

    void Shuffle(List<GameObject> leftList, List<GameObject> rightList) 
    {
        //Based on random numbers, we change the order of the two columns
        for (int i = 0; i < leftList.Count; i++) 
        {
            int rand1 = Random.Range(0, leftList.Count);
            int rand2 = Random.Range(0, leftList.Count);

            leftList[i].transform.SetSiblingIndex(rand1);
            rightList[i].transform.SetSiblingIndex(rand2);
        }
        GameController.instance.SpawnLoading(false);
    }

    IEnumerator DownloadImage(string imageUrl, System.Action<Texture2D> callback)
    {
        //Getting a texture from the image url
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            callback(((DownloadHandlerTexture)request.downloadHandler).texture);
       
            
    }
}
