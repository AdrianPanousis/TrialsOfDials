using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMode : MonoBehaviour
{
    //stores the prefab of a 3 dial puzzle
    [SerializeField] private GameObject DialGame;

    //an invisbile 3d object that when activiated overlays the dials so they can be touched and rotated
    [SerializeField] protected GameObject DialBlocker;

    //text proteries that show the current score and the high score
    [SerializeField] protected TextMeshProUGUI ScoreText;
    [SerializeField] protected TextMeshProUGUI HighScoreText;

    //empty container that holds all of the dials used for the game
    [SerializeField] private GameObject DialStorage;

    protected int Score = 0;
    protected float moveTimer;
    protected bool areDialsMoving = false;

    //stores all of the 3 dial games in the scene at any one time
    private GameObject CurrentGame;
    private GameObject NewGame;
    private GameObject CreatedGame;

    //positions where a dial game will be from when it is spawned off screen to when it is moved off screen when completed
    private Vector3 onScreenPos = new Vector3(6.5f, 4.6f, 6.5f);
    private Vector3 startScreenPos = new Vector3(17f, 4.6f, 6.5f);
    private Vector3 endScreenPoos = new Vector3(-3f, 4.6f, 6.5f);

    protected SoundEffects soundSource;
    [SerializeField] protected AudioClip WooshSound;

    protected virtual void Start()
    {
        moveTimer = 0;

        //starts a new game right away
        BrandNewGame();
        soundSource = transform.GetComponent<SoundEffects>();
    }

    public void BrandNewGame()
    {
        StartCoroutine(DelayTimer());
    }

    protected IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(2.0f);

        //empties the scene of any old dial games if they are still there
        foreach (Transform child in DialStorage.transform)
        {
            Destroy(child.gameObject);
        }

        //creates the first 3 dial puzzle and the next one that will be shown once it is complete
        SpawnInitialDials();
        ScoreText.text = Score.ToString();

    }

    public virtual void RestartGame()
    {
        //empties the scene of any old dial games if they are still there
        foreach (Transform child in DialStorage.transform)
        {
            Destroy(child.gameObject);
        }

        //creates the first 3 dial puzzle and the next one that will be shown once it is complete
        SpawnInitialDials();
        Score = 0;
        ScoreText.text = Score.ToString();
        
    }

    protected virtual void Update()
    {
        if(areDialsMoving)
        {
            MoveDials();
        }
    }

    //creates the first 3 dial puzzle and the next one that will be shown once it is complete
    private void SpawnInitialDials()
    {
        NewGame = CreateDial(startScreenPos);
        CurrentGame = CreateDial(onScreenPos);
    }

    //instantiates a new Dial Puzzle from a prefab
    private GameObject CreateDial(Vector3 spawnPos)
    {
        GameObject dial = Instantiate(DialGame);
        
        dial.transform.position = spawnPos;
        dial.transform.localEulerAngles = Vector3.zero;
        dial.transform.localScale = Vector3.one;
        dial.transform.parent = DialStorage.transform;
        return dial;
    }

    public void DisableDials()
    {
        DialBlocker.SetActive(true);
    }

    //creates a new game and starts moving the other two games in the scene
    public void NewDialGame()
    {
        //starts moving the dials
        areDialsMoving = true;
        //DialBlocker.SetActive(true);

        //creates the new dial
        CreatedGame = CreateDial(startScreenPos);
        soundSource.PlaySound(WooshSound);
    }

    protected void MoveDials()
    {
        //moves the compledted puzzle and the next one to the left of the screen over time
        moveTimer += Time.deltaTime * 2;
        if (moveTimer < 1)
        {
            CurrentGame.transform.position = Vector3.Lerp(onScreenPos, endScreenPoos, moveTimer);
            NewGame.transform.position = Vector3.Lerp(startScreenPos, onScreenPos, moveTimer);
        }

        //sets the positions once the time to move is complete
        else
        {
            CurrentGame.transform.position = Vector3.Lerp(onScreenPos, endScreenPoos, 1);
            NewGame.transform.position = Vector3.Lerp(startScreenPos, onScreenPos, 1);
            moveTimer = 0;
            DestroyOldGame(CurrentGame);

            //stops the dials from moving
            areDialsMoving = false;
            DialBlocker.SetActive(false);

        }
    }

    //destroys the completed puzzle and replaces the other variables with the other two dials in the scene
    private void DestroyOldGame(GameObject g)
    {
        Destroy(g);
        CurrentGame = NewGame;
        NewGame = CreatedGame;
    }
}
