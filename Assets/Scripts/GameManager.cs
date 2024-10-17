
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    bool gamehasEnded = false;
    public void EndGame()
    {
        if (gamehasEnded == false)
        {
            gamehasEnded = true;
            Debug.Log("Game Over");

            //Restart the game
            Restart();
        }

    }


    void Restart()
    {
        
        PauseMenu.GameIsPaused = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
