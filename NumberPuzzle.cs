using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberPuzzle : MonoBehaviour
{

    public TextMeshProUGUI scoreGoalDisplay;

    [SerializeField] private NumberPuzzleDial innerCircle;
    [SerializeField] private NumberPuzzleDial middleCircle;
    [SerializeField] private NumberPuzzleDial outerCircle;

    public GameObject gameNumber;

    private int outerNumberCount;
    private int middleNumberCount;
    private int innerNumberCount;

    private List<int> outerNumbers = new List<int>();
    private List<int> middleNumbers = new List<int>();
    private List<int> innerNumbers = new List<int>();

    private float scoreTotal;
    private float scoreGoal;

    private TimeMode timeGameController;
    private GameMode GameController;
    private SurvivalMode survivalGameController;
    private string Mode = "";

    private int puzzleValue = 100;

    private float glowTime;
    private bool glowChange;
    private bool scoresMatch;
    private List<TextMeshProUGUI> highlightedNumbers = new List<TextMeshProUGUI>();
    [SerializeField] private AudioSource GlowController;

    private void Awake()
    {
        //randomises how many numbers will be in each dial
        outerNumberCount = Random.Range(5, 9);
        middleNumberCount = Random.Range(5, 9);
        innerNumberCount = Random.Range(5, 7);

        glowTime = 0;
        glowChange = false;
        scoresMatch = false;
        CreateValues();
    }

    //Randomly generates numbers for each of the dials
    private void CreateValues()
    {
        //makes sure the gap between each consecutive number is between 1 and 5
        int innerGap = Random.Range(1, 5);
        //makes sure the first number is between 1 and 10. 
        int innerStartingNumber = Random.Range(1, 10);

        //makes sure the gap between each consecutive number is between 5 and 25
        int middleGap = Random.Range(1, 5) * 5;
        //makes sure the first number is between 5 and 55 but is a factor of 5. 
        int middleStartingNumber = Random.Range(1, 11) * 5;

        //makes sure the gap between each consecutive number is between 20 and 100
        int outerGap = Random.Range(1, 5) * 20;
        //makes sure the first number is between 20 and 220 but is a factor of 20. 
        int outerStartingNumber = Random.Range(1, 11) * 20;

        //generates the numbers using the number count for each dial, the gap between numbers and the starting numbers and adds them to each array
        for (int i = 0; i < innerNumberCount; i++)
        {
            innerNumbers.Add(innerStartingNumber + (innerGap * i));
        }

        for (int i = 0; i < middleNumberCount; i++)
        {
            middleNumbers.Add(middleStartingNumber + (middleGap * i));
        }

        for (int i = 0; i < outerNumberCount; i++)
        {
            outerNumbers.Add(outerStartingNumber + (outerGap * i));
        }

        //sorts the numbers from smallest to largest
        innerNumbers.Sort(SortNumbers);
        middleNumbers.Sort(SortNumbers);
        outerNumbers.Sort(SortNumbers);
    }

    void Start()
    {
        //creates the graphical objects that show the numbers
        SpawnNumbers(outerNumberCount, outerCircle, outerNumbers, 2.4f);
        SpawnNumbers(middleNumberCount, middleCircle, middleNumbers, 1.53f);
        SpawnNumbers(innerNumberCount, innerCircle, innerNumbers, 0.85f);

        SendValues();
        CreateScoreGoal();
        checkGameMode();
        
    }

    //creates the numbers on each dial
    private void SpawnNumbers(int numberCount, NumberPuzzleDial Circle, List<int> numbers, float rad)
    {
        Canvas canvas = Circle.transform.GetComponentInChildren<Canvas>();

        for (int i = 0; i < numberCount; i++)
        {
            float angle = i * Mathf.PI * 2f / numberCount;
            Vector2 newPos = new Vector2(Mathf.Sin(angle) * rad, Mathf.Cos(angle) * rad);
            GameObject newHole = Instantiate(gameNumber);
            float rotation = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg; ;
            newHole.transform.parent = canvas.transform;
            newHole.transform.localPosition = newPos;
            newHole.transform.localEulerAngles = new Vector3(0, 0, rotation - 90);
            newHole.transform.localScale = Vector3.one;
            newHole.transform.GetComponent<TextMeshProUGUI>().text = numbers[i].ToString();
            newHole.name = "Number";
        }
    }

    //sends the numbers to each dial so they can be used when they are rotated and store them when needed to determine which one is selected
    //The number count is needed to calculate certain things like the angle determining which number is selected
    private void SendValues()
    {
        innerCircle.setNumbers(innerNumbers);
        innerCircle.SetNumberCount(innerNumberCount);

        middleCircle.setNumbers(middleNumbers);
        middleCircle.SetNumberCount(middleNumberCount);

        outerCircle.setNumbers(outerNumbers);
        outerCircle.SetNumberCount(outerNumberCount);
    }

    //Grabs three random numbers from each dial and adds them together to create the solution to the puzzle
    private void CreateScoreGoal()
    {

        int innerCicleGoal = Random.Range(0, innerNumberCount);
        int middleCircleGoal = Random.Range(0, middleNumberCount);
        int outerCircleGoal = Random.Range(0, outerNumberCount);

        scoreGoal = innerNumbers[innerCicleGoal] + middleNumbers[middleCircleGoal] + outerNumbers[outerCircleGoal];
        //sends the number to the centre text GUi to show the score
        scoreGoalDisplay.text = scoreGoal.ToString();
    }


    private void checkGameMode()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMode>().enabled == true)
        {
            Mode = "Zen";
            GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameMode>();
        }

        if(GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeMode>().enabled == true)
        {
            Mode = "Timed";
            timeGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeMode>();
        }

        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<SurvivalMode>().enabled == true)
        {
            Mode = "Survival";
            survivalGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SurvivalMode>();
        }
        
    }

    private void Update()
    {
        //executes when tap is released
        if(Input.GetMouseButtonUp(0))
        {
            //stops the function from executing when a puzzle has already been completed
            if (!scoresMatch)
            {
                CheckScore();
            }
        }

        //brightens the glow of the selected numbers
        if(glowChange)
        {
            GlowOverTime();
        }
    }

    //sorts number from smallest to largest that are sent to it
    private int SortNumbers(int n1,int n2)
    {
        return n1.CompareTo(n2);
    }

    //checks if the total of the three selected numbers matches the score goal for the puzzle
    private void CheckScore()
    {

        scoreTotal = innerCircle.getSelectedNumber() + middleCircle.getSelectedNumber() + outerCircle.getSelectedNumber();

        if(scoreTotal == scoreGoal)
        {
            //glows the three selected numbers once the puzzle is solved
            GlowNumbersOnComplete();

            //prevents dials from being rotated after the puzzle is complete
            switch (Mode)
            {
                case "Zen":
                    GameController.DisableDials();
                    break;
                case "Timed":
                    timeGameController.DisableDials();
                    break;
                case "Survival":
                    survivalGameController.DisableDials();
                    break;
            }

            //prevents repeated taps causing glow sound to play over and over again
            scoresMatch = true;

        }
        
    }

   
    //starts the glowing process by adding the numbers that need to glow and sets a bollean to true to start doing it over time
    private void GlowNumbersOnComplete()
    {
        highlightedNumbers.Add(innerCircle.getDialNumber());
        highlightedNumbers.Add(middleCircle.getDialNumber());
        highlightedNumbers.Add(outerCircle.getDialNumber());

        //starts the glowing in the update
        glowChange = true;

        //plays the glowing sound effect
        GlowController.Play();
    }

    //increases the glow then decreases it back to normal over time
    private void GlowOverTime()
    {
        //runs the function until the timevalue has been met
        if (glowTime < 1f)
        {
            //controls the speed of the glow
            glowTime += Time.deltaTime/0.8f;
            //uses a modified sine wave to increase and then return to normal the glow settings on the text material over time
            float glowSignValue = (Mathf.Sin(Mathf.PI*glowTime)/2)+0.5f;

            //uses the glow value to actually modify the material properties of each higlighted number
            foreach (TextMeshProUGUI t in highlightedNumbers)
            {
                t.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glowSignValue);
                t.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, glowSignValue);
            }
        }

        //stops the glowing and creates a new puzzle
        else
        {
            glowChange = false;
            glowTime = 0;
            newPuzzle(Mode);
        }
    }

    private void newPuzzle(string mode)
    {
        //checks which game mode is active and makes a new puzzle
        switch (mode)
        {
            case "Zen":
                GameController.NewDialGame();
                break;
            case "Timed":
                //in timed mode it will also add to the players score
                timeGameController.AddScore(puzzleValue, mode);
                timeGameController.NewDialGame();
                break;
            case "Survival":
                //in survival mode it will add to the score and also add a time extension
                survivalGameController.AddScore(puzzleValue, mode);
                survivalGameController.NewDialGame();
                survivalGameController.AddTime();
                break;
        }
    }
}
