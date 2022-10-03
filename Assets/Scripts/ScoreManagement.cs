using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagement : MonoBehaviour
{
    public static ScoreManagement scoreManager{get; set;}
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreResult;
    public int _clicks;
    public int balloonsExploded;
    public int powersExploded;
    public int _fails;
    public int result;
    
    void Awake(){
        if(ScoreManagement.scoreManager == null){
            scoreManager = this;
        }
        else{Destroy(gameObject);}
    }
    
    void Update(){
        scoreText.text = score.ToString();

        if(Input.GetMouseButtonDown(0)){
            _clicks ++;
        }
    }

    public void Result(){
        PlayerPrefs.SetInt("score", score / _fails);
        Debug.Log("guardando");
    }
}
