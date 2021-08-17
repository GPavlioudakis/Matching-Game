using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    // Starting position of the created line
    Vector3 startPosition;

    //Gameobject from which the line starts and ends
    GameObject startAnimal, endAnimal;

    // GameObject of the created line
    GameObject currentLineObject;

    // lineRenderer of the created line
    LineRenderer currentLineRenderer;

    public Material lineMaterial;

    public float lineThickness;


    // Canvas that we want to draw on
    public Canvas parentCanvas;

    void Update()
    {
        //If we start drawing a line, we want it to be visible
        if (currentLineObject != null)
            PreviewLine();
    }

    // Returns the current mouseposition relative to the canvas.
    // Modifies the z-value slightly so that stuff will be rendered in front of UI elements.
    Vector3 GetMousePosition()
    {
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            Input.mousePosition, parentCanvas.worldCamera,
            out movePos);
        Vector3 positionToReturn = parentCanvas.transform.TransformPoint(movePos);
        positionToReturn.z = parentCanvas.transform.position.z - 0.01f;
         
        return positionToReturn;
    }

    //Start drawing the line and keep the gameobject from which the line starts
    public void StartDrawingLine(GameObject animalGameObject)
    {
        //If a line exests, stop drawing
        if (currentLineObject == null)
        {
            startAnimal = animalGameObject;
            startPosition = GetMousePosition();
            currentLineObject = new GameObject();
            currentLineObject.transform.parent = transform;
            currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
            currentLineRenderer.material = lineMaterial;
            currentLineRenderer.startWidth = lineThickness;
            currentLineRenderer.endWidth = lineThickness;
            currentLineRenderer.useWorldSpace = false;
        }
        else 
        {
            EndDrawingLine(animalGameObject);
        }
        
    }

    void PreviewLine()
    {
        currentLineRenderer.SetPositions(new Vector3[] { startPosition, GetMousePosition() });
    }

    //Stop drawing the specific line, and erase it. 
    void EndDrawingLine(GameObject animalGameObject)
    {
        endAnimal = animalGameObject;
        GameController.instance.CheckPair(startAnimal, endAnimal);
        ClearLine();
    }

    public void ClearLine() 
    {
        Destroy(currentLineObject);
        startAnimal = null;
        endAnimal = null;
        startPosition = Vector3.zero;
        currentLineObject = null;
        currentLineRenderer = null;
    }
}