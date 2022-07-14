using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPuzzleDial : MonoBehaviour
{
    private float selectedNumber;
    private int numberCount;
    private int selection;

    private float previousTouchAngle;
    private float touchAngle;
    private float touchAngleDifference;

    private float dialAngle; 
    private float rotationSpeed;

    private float snapTimer;
    private float snapSpeed;
    private bool isSnapping;

    private bool offsetFix;
    private MeshCollider dialCollision;

    private List<int> numbers = new List<int>();

    public TMP_FontAsset highlightFont;
    public TMP_FontAsset normalFont;
    private AudioSource turnClick;

    void Start()
    {
        rotationSpeed = 80f;
        dialAngle = 0;
        snapTimer = 0;
        isSnapping = false;
        offsetFix = false;
        turnClick = transform.GetComponent<AudioSource>();
        dialCollision = transform.GetComponent<MeshCollider>();      
    }

    void Update()
    {
        //once the user releases from dragging the dial will auto rotate to the closest number
        if(isSnapping)
        {
            SnapToPosition();
        }

        else
        {
            snapTimer = 0;
        }
    }


    //sets the mouse angle on first touch so the rotate function has a previous point to reference for movement initially
    private void OnMouseEnter()
    {
        setTouchAngle();
    }

    private void OnMouseDrag()
    {
        RotateDial();
    }

    private void OnMouseDown()
    {
        //sets all of the numbers to unhighlighted while the user has their finger held down on the dial
        changeHighlight(true);
    }

    //gets the 
    private void setTouchAngle()
    {
        //gets the position of the Puzzle in screen coordinates
        Vector2 PuzzlePos = Camera.main.WorldToScreenPoint(transform.position);

        //gets the touch positon in relation to the puzzle's position on the screen.
        Vector2 normalisedMousePosition = new Vector2(PuzzlePos.x - Input.mousePosition.x, Input.mousePosition.y - PuzzlePos.y);

        //gets the angle of the touch in relation to the centre of the screen
        float MouseAngle = Mathf.Atan2(normalisedMousePosition.y, normalisedMousePosition.x) * Mathf.Rad2Deg;

        //adjusts the touch angle so that the top centre is 0 instead of centre left. It also makes sure the values are within 0-360 degrees
        touchAngle = (MouseAngle + 270) % 360;
    }


    //moves the dial when the user is touching and dragging on the screen
    private void RotateDial()
    {
        //sets the initial touch angle on the prior frame
        previousTouchAngle = touchAngle;

        //gets the touch angle on the current frame
        setTouchAngle();
        
        //used to determine how much to rotate the dial
        touchAngleDifference =  touchAngle- previousTouchAngle;


        //checks if the difference between the previous touch isn't too large, it prevents sudden snapping on the initial touch
        if (Mathf.Abs(touchAngleDifference) < 10)
        {
            //changes the rotation
            transform.Rotate(Vector3.forward, touchAngleDifference, Space.Self);
            //stores the amount of rotation on the dial
            dialAngle += touchAngleDifference;
        }

        CheckSelection();

    }

    private void CheckSelection()
    {
        //makes sure the dial angle value is between 0-360
        dialAngle = dialAngle % 360;

        if (dialAngle < 0)
        {
            dialAngle = dialAngle + 360;
        }

        //based on the dial angle it gets a index number used for an array to get the selected number
        int clickSelection = (int)(dialAngle - 360) / (360 / numberCount);
        clickSelection = Mathf.Abs(clickSelection);

        if (clickSelection != selection)
        {
            //plays the click sound when rotating
            turnClick.Play();
            selection = clickSelection;
        }
        
    }

    //changes numbers to either glow or not depending on the rotation of the dial
    private void changeHighlight(bool unhighlightAll)
    {
        Canvas canvas = transform.GetComponentInChildren<Canvas>();
        List<TextMeshProUGUI> numbers = new List<TextMeshProUGUI>();

        //gets all the numbers on the dial and stores them into a list
        numbers.AddRange(canvas.transform.GetComponentsInChildren<TextMeshProUGUI>());

        //gets all the numbers and removes the glow
        if (unhighlightAll)
        {
            foreach (TextMeshProUGUI t in numbers)
            {
                t.font = normalFont;
            }
        }

        //gets all the numbers and checks which one is selected and makes it glow
        else
        {
            foreach (TextMeshProUGUI t in numbers)
            {
                if (t.text == selectedNumber.ToString())
                {
                    t.font = highlightFont;
                }

                //removes the glow on all the other numbers
                else
                {
                    t.font = normalFont;
                }
            }
        }
    }

    //executes once the touch has been released
    private void OnMouseUp()
    {
        dialAngle = dialAngle % 360;

        if (dialAngle < 0)
        {
            dialAngle = dialAngle + 360;
        }

        //offset used so that instead of the seletion changing right at the centre of the number is it at either side of it
        int selectionOffset = 360 / numberCount / 2;

        //based on the dial angle it gets a index number used for an array to get the selected number
        selection = (int)(dialAngle-360-selectionOffset) / (360 / numberCount);
        selection = Mathf.Abs(selection);

        //checks if the selection number is the initial value at the start of the puzzle
        if(selection == numberCount)
        {
            //makes adjustments based on the selection using 360 degrees as a number instead of 0
            selection = 0;
            offsetFix = true;
        }

        selectedNumber = numbers[selection];

        //starts snapping the rotation so the selected number is at the top centre of the dial
        isSnapping = true;

        //disables collision so the user can't rotate the dial while snapping, preventing errors and bugs
        dialCollision.enabled = false;

        //highlights the selected number
        changeHighlight(false);

        turnClick.Play();
    }


    public float getSelectedNumber()
    {
        return selectedNumber;
    }

    //gets the selected numbers text property
    public TextMeshProUGUI getDialNumber()
    {
        Canvas canvas = transform.GetComponentInChildren<Canvas>();
        List<TextMeshProUGUI> allNumbers = new List<TextMeshProUGUI>();
        TextMeshProUGUI highlightNumber = new TextMeshProUGUI();
        allNumbers.AddRange(canvas.transform.GetComponentsInChildren<TextMeshProUGUI>());
        foreach (TextMeshProUGUI t in allNumbers)
        {
            if (t.text == selectedNumber.ToString())
            {
                highlightNumber = t;
            }
        }

        return highlightNumber;
    }

    public void SetNumberCount(int n)
    {
        numberCount = n;
    }
    public void setNumbers(List<int> n)
    {
        numbers = n;
        selectedNumber = numbers[0];
        changeHighlight(false);
    }

    private float getSnapPosition()
    {
        float snapPos = (360 / numberCount) * selection;
        snapPos = Mathf.Abs( snapPos - 360);

        //used to change the snapPos to 0 instead of 360 to prevent the dial from rotating the opposite direction for nearly a whole rotation.
        if(offsetFix)
        {
            snapPos = 0;
        }

        return snapPos;
    }

    private void SnapToPosition()
    {
        //finds the difference between the current angle and where it needs to go
        float angleDiff = Mathf.Abs(dialAngle - getSnapPosition());

        //rotates the dial backwards to get to the snap position
        if (dialAngle > getSnapPosition())
        {
            
            snapTimer += Time.deltaTime * rotationSpeed;
            if (snapTimer < angleDiff)
            {
                snapSpeed = Time.deltaTime * rotationSpeed;
                transform.Rotate(Vector3.back, snapSpeed, Space.Self);
            }

            else
            {
                EndSnapping();           
            }
        }

        //rotates the dial forwards to get to the snap position
        else
        {
            snapTimer += Time.deltaTime * rotationSpeed;
            if (snapTimer < angleDiff)
            {
                snapSpeed = Time.deltaTime * rotationSpeed;
                transform.Rotate(Vector3.forward, snapSpeed, Space.Self);
            }

            else
            {
                EndSnapping();           
            }
        }      
    }

    //sets all afected variables to their appropriate values after the snapping has rotated to the right position
    private void EndSnapping()
    {
        //actually ends the snapping
        isSnapping = false;
        //turns ofset fix back to it's default
        offsetFix = false;
        //sets the dial angle to the exact position it should be to avoid errors if it is slightly innacurate
        dialAngle = getSnapPosition();
        //enables collision so the dial can be rotated again
        dialCollision.enabled = true;
        //sets the z rotation value to the exact position to be snapped to in order to avoid slight innacuracies causing issues over time
        transform.eulerAngles = Vector3.forward * getSnapPosition();
        //sets the x rotation value back to -90, setting the z rotation this way caused errors with negative and positive values
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
