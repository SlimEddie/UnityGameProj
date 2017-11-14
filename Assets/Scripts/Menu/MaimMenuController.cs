using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MaimMenuController : MonoBehaviour {

    public Animator MainMenuAnimator;
    public Animator LevelMenuAnimator;
    public Animator SubMenuAnimator;
    private bool InMainMenu = true;
    public DataManager Data_M;
    

    void FixedUpdate()
    {

    }

    public void SwitchMenu(int ButtonIndex)
    {
        switch (ButtonIndex)
        {
            case 0: // Start button...
                {
                    InMainMenu = false;
                    LevelMenuAnimator.Play("DropDownLevelMenu");
                    MainMenuAnimator.Play("SwitchMenu");
                    SubMenuAnimator.Play("ReturnMainMenu");
                    break;
                }
            case 1:
                {

                    break;
                }
            case 2: //EXIT BUTTON HERE
                {
                    Application.Quit();
                    break;
                }
            case 3: //Return to main menu
                {
                    InMainMenu = true;
                    LevelMenuAnimator.Play("RollUpLevelMenu");
                    MainMenuAnimator.Play("ReturnMainMenu");
                    SubMenuAnimator.Play("SwitchMenu");
                    break;
                }
            case 4: //Next level buttons
                {
                    Data_M.D_Base.UlockedLevel++;// Current scene
                    Data_M.SaveTheGame();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                    break;
                }
            case 5: //Restart level buttons
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
                    break;
                }
            case 6: //Rate buttons
                {



                    break;
                }

        }
    }




}
