using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Trashman;
using Unity.VisualScripting;
using myGUI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialStages
{
    Movement = 0,
    HealthLost = 1,
    ItemsUse = 2,
    AttackBarrier = 3,
    GetStar =4,
}

public class GameController : MonoBehaviour
{
    public GameObject character;

    public UIManager _uiManager;

    public GameObject inventory;

    public int isTutorialOn = 1;

    public int tutorialStage = 0;
    //tutorialStageChange is a listener to tutorialStage
    public int tutorialStageChange {
        get {return tutorialStage; }
        set
        {
            if (value != tutorialStage)
            {
                TriggerNextTutorial(value);
            }

            tutorialStage = value;
        }
    }
    
    public int didSucceed = 0; // -1 means lose game, +1 means pass this level
    //didSucceedChange is a listener to didSucceed
    public int didSucceedChange {
        get {return didSucceed; }
        set
        {
            if (value != didSucceed)
            {
                if (value == 1)
                {
                    Succeed();
                }else if (value == -1)
                {
                    Fail();
                }
            }

            didSucceed = value;
        }
    }

    private int isUp = 0;
    
    private int isDown = 0;

    private int isLeft = 0;
    
    private int isRight = 0;
    // Start is called before the first frame update
    void Start()
    {
        //true game levels
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            isTutorialOn = 0;
        }

        //tutorial process
        if (isTutorialOn == 1)
        {
            tutorialStageChange = 0;
            TriggerNextTutorial(tutorialStage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //tutorial process
        if (isTutorialOn == 1)
        {
            MovedInAllDirections();
        }
    }

    void TriggerNextTutorial(int tutorialStage)
    {
        DisableWholeScene();
        switch(tutorialStage){
            case (int)TutorialStages.Movement:
                _uiManager.CreateMsgBox("About Movement","Welcome to Trashman! To get started, please try W,A,S,D to move.");
                break;
            
            case (int)TutorialStages.HealthLost:
                _uiManager.CreateMsgBox("About Health","As you may have found, any operations including movement will make you lose health. You will lose when the health is 0." +
                                                       " So this is a strategy game about fulfilling your goal before death. And food can help you gain some health." +
                                                       "Now go to fetch that burger!");
                break;
            
            case (int)TutorialStages.ItemsUse:
                _uiManager.CreateMsgBox("Use Item","Now you have the burger in your inventory! Press the key shows in the burger slot and you'll gain some health. Always remember " +
                                                       "that food can help you go farther and enable you to do more operations in the world since they give you more energy.");
                break;
            case (int)TutorialStages.AttackBarrier:
                _uiManager.CreateMsgBox("Barriers","You gained more health! Notice that your way to the destination is not that easy. There may be lots of barriers which hinder you from success." +
                                                       "But the good news is that you can destroy them by tools. Pick up that sword and then press the key shows in the sword slot to destroy the barrier " +
                                                       "when you are facing them");
                break;
            case (int)TutorialStages.GetStar:
                _uiManager.CreateMsgBox("Get the Star","Now nobody can stop you from success. Go to pick the star!");
                break;
            default:
                break;
        }
       
        
    }

    void Succeed()
    {
        // only go to the next level when the player confirm the message box
        StartCoroutine(successReload());

        IEnumerator successReload()
        {
            DisableWholeScene();
            MessageBox msg = _uiManager.CreateMsgBox("Congratulations!", "Congratulations! You've passed this level!");
            yield return new WaitUntil(() => msg == null);
            SceneManager.LoadScene(1); //TODO: load next game
        }
    }

    void Fail()
    {
        // only reload the entire game when the player confirm the message box
        StartCoroutine(failReload());

        IEnumerator failReload()
        {
            DisableWholeScene();
            MessageBox msg = _uiManager.CreateMsgBox("Failed!", "You lost the game!");
            yield return new WaitUntil(() => msg == null); 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    //when msgBox shows, all objects in the scene should be disabled(not controlled by the player)
    void DisableWholeScene()
    {
        character.GetComponent<PlayerController>().enabled = false;
    }
    
    public void EnableWholeScene()
    {
        character.GetComponent<PlayerController>().enabled = true;
    }
    
    
    void MovedInAllDirections()
    {
        UpMoved();
        DownMoved();
        LeftMoved();
        RightMoved();
            
        if (isUp == 1 && isDown == 1 && isLeft == 1 && isRight == 1 && tutorialStage == 0)
        {
            tutorialStageChange = (int)TutorialStages.HealthLost;
        }
    }

    void BurgerPickedUp()
    {
        
    }
    
    void UpMoved()
    {
        if ((character.activeInHierarchy) && Input.GetKeyUp(KeyCode.W))
        {
            isUp = 1;
        }
    }
    
    void DownMoved()
    {
        if ((character.activeInHierarchy) && Input.GetKeyUp(KeyCode.S))
        {
            isDown = 1;
        }
    }
    
    void LeftMoved()
    {
        if ((character.activeInHierarchy) && Input.GetKeyUp(KeyCode.A))
        {
            isLeft = 1;
        }
    }
    
    void RightMoved()
    {
        if ((character.activeInHierarchy) && Input.GetKeyUp(KeyCode.D))
        {
            isRight = 1;
        }
    }
}
