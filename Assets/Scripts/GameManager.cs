using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    private GroundPiece[] allGroundPieces;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;


        }

        else if (singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);


        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpNewLevel();


    }
    private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();

    }

    private void SetUpNewLevel()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();

    }

    public void CheckComplete()
    {
        if (allGroundPieces == null || allGroundPieces.Length == 0)
        {
            // Handle the case where there are no ground pieces
            return;
        }

        bool isFinished = true;
        for (int i = 0; i < allGroundPieces.Length; i++) 
        {
            if (allGroundPieces[i].isColored == false ) 
            {
                isFinished = false;
                break;
               
            
            }
        
        
        }
        if (isFinished) 
        { 
            //call NextLevel()
            NextLevel();
        
        }
    
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(0);

        }
        else 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
    }
}
