using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private ParticleSystem _free;
    [SerializeField] private ParticleSystem[] partsColor = new ParticleSystem[2];
    public Pool Pool{get; set;}
    private float _offset;
    [SerializeField] private bool isMoving;
    [SerializeField] private int _click = 1;
    [SerializeField] private string colourTag;
  
    public enum BalloonColor{
        Red,
        Blue,
        Green,
        Yellow
    }

    private BalloonColor _color{get; set;}
    
    void Start(){
        _offset = Random.Range(0f, 50f);
        isMoving = true;
    }

    public void SetColor(BalloonColor color){
        _color = color;

        switch(color){
            case 
            BalloonColor.Red:{
                _sprite.color = new Color(0.7075472f, 0.1773377f, 0.06274471f, 1); 
                colourTag = "red";
            } 
            break;
            case 
            BalloonColor.Blue:{
                _sprite.color = new Color(0f, 80f, 50f, 1);
                colourTag = "blue";
            } 
            break;
            case 
            BalloonColor.Green:{
                _sprite.color = Color.green;
                colourTag = "green";
            } 
            break;
            case 
            BalloonColor.Yellow:{
                _sprite.color = Color.yellow;
                colourTag = "yellow";
            } 
            break;
            }  
    }

    public void ParticleColor(){ 
        switch(colourTag){
            case "red":
                for(int i = 0; i < partsColor.Length; i++){
                    var main = partsColor[i].main;
                    main.startColor = Color.red;
                }
            break;
            case "blue":
                for(int i = 0; i < partsColor.Length; i++){
                    var main = partsColor[i].main;
                    main.startColor = new Color(0f, 80f, 50f);
                }
            break;
            case "green":
                for(int i = 0; i < partsColor.Length; i++){
                    var main = partsColor[i].main;
                    main.startColor = Color.green;
                }
            break;
            case "yellow":
                for(int i = 0; i < partsColor.Length; i++){
                    var main = partsColor[i].main;
                    main.startColor = Color.yellow;
                }
            break;
        }
    }

    void Update(){
        if(isMoving){
            Vector3 dir = Vector3.up;
            float speed = 4f * Time.deltaTime;

            transform.Translate(dir * speed + Vector3.right * (Mathf.Sin(Time.time + _offset) / 100));
        }
        else{}

        if(transform.position.y > 12f){
            Pool.Recycle(this);
        }
        
        if(transform.localScale.x > 0.3f && transform.localScale.y > 0.3f){
            var main = _particle.main;
            main.startSize = 5f;

            for(int i = 0; i < partsColor.Length; i++){
                main = partsColor[i].main;
                main.startSize = 1f;
            }
        }

        if(transform.localScale.x < 0.3f && transform.localScale.y < 0.3f){
            var main = _particle.main;
            main.startSize = 2f;

            for(int i = 0; i < partsColor.Length; i++){
                main = partsColor[i].main;
                main.startSize = 0.8f;
            }
        }

        if(transform.localScale.x < 0.2f && transform.localScale.y < 0.2f){
            var main = _particle.main;
            main.startSize = 1f;

            for(int i = 0; i < partsColor.Length; i++){
                main = partsColor[i].main;
                main.startSize = 0.3f;
            }
        }

        if(SystemManagment.SINGLETON.isVulnerable != false){
            _free.Play();
        }
        else{_free.Stop();}
    }

    void OnMouseDown(){
        ParticleColor();

        SoundManagement._sfx.selection = 0;
        SoundManagement._sfx.PlaySound();

        if(_click == 1){
            if(SystemManagment.SINGLETON.isVulnerable == false){
                ScoreManagement.scoreManager.score += ColorManagement.Instance.IsValid(_color)?500:-500;
                ScoreManagement.scoreManager._fails += ColorManagement.Instance.IsValid(_color)?0:1;
            }
            else{ScoreManagement.scoreManager.score += 500;}

            if(SystemManagment.SINGLETON.multiplied == true){
                if(SystemManagment.SINGLETON.isVulnerable == false){
                    ScoreManagement.scoreManager.score += ColorManagement.Instance.IsValid(_color)?500 * 2:-500;
                    ScoreManagement.scoreManager._fails += ColorManagement.Instance.IsValid(_color)?0:1;
                }
                else{ScoreManagement.scoreManager.score += 500 * 2;}
            }

            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();

            if(_particle.isPlaying){
                SoundManagement._sfx.selection = 0;
                SoundManagement._sfx.PlaySound();

                ScoreManagement.scoreManager.balloonsExploded ++;
            }
        
            StartCoroutine("DisableBalloon");
        }
        
        _click = 0;
    }

    private IEnumerator DisableBalloon(){
        yield return new WaitForSeconds(0.5f);
        
        transform.GetChild(0).gameObject.SetActive(true);
        isMoving = true;
        _click = 1;

        Pool.Recycle(this);
        
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider _collider){
        if(_collider.tag.Equals("Wave")){
            ParticleColor();

            SoundManagement._sfx.selection = 0;
            SoundManagement._sfx.PlaySound();

            ScoreManagement.scoreManager.score += 500;

            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();
        
            StartCoroutine("DisableBalloon");
        }
    }
}
            
        
        
