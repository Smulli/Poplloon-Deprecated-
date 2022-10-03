using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ColorManagement : MonoBehaviour
{
    public static ColorManagement color;
    public string[] colours => System.Enum.GetNames(typeof(Balloon.BalloonColor));
    public int index;
    private float speed = 0.29f;
    public TextMeshProUGUI colorText;
    private float changeColour = 5f;
    public TextMeshProUGUI changeText;
    public string currentColor;
    public SpriteRenderer frame;
    public static ColorManagement Instance{get; private set;}

    void Awake(){
        if(ColorManagement.Instance == null){
            Instance = this;
        }
        else{Destroy(gameObject);}
    }
    
    void Start(){
        Result();
    }

    void Update(){ 
        CurrentColour();

        changeColour -= Time.deltaTime;
        changeText.text = changeColour.ToString("F0");

        if(changeColour <= 0f){
            Result();
            changeColour += 5f;
        }

        //Debug.Log(colours[index]);
    }

    public void CurrentColour(){
        switch (index){
            case 0:
            frame.color = Color.red;
            colorText.color = Color.red;
            break;
            case 1:
            frame.color = new Color(0f, 80f, 50f);
            colorText.color = new Color(0, 80f, 50f);
            break;
            case 2:
            frame.color = Color.green;
            colorText.color = Color.green;
            break;
            case 3:
            frame.color = Color.yellow;
            colorText.color = Color.yellow;
            break;
        }
        colorText.text = colours[index].First().ToString().ToUpper() + colours[index].Substring(1);
    }

    public void Result(){
        index = Random.Range(0, colours.Length);
        currentColor = colours[index];
    }

    public Balloon.BalloonColor GetCurrentColor(){
        return(Balloon.BalloonColor)index;
    }

    public Balloon.BalloonColor GetRandomColor(){
        return(Balloon.BalloonColor)Random.Range(0, System.Enum.GetNames(typeof(Balloon.BalloonColor)).Length);
    }

    public Power.PowerType GetRandomPower(){
        return(Power.PowerType)Random.Range(0, System.Enum.GetNames(typeof(Power.PowerType)).Length);
    }

    public bool IsValid(Balloon.BalloonColor balloonColor){
        return currentColor == balloonColor.ToString();
    }
}
                        
                
                        

        
    
        
