using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Sprite[] sprites = new Sprite[5];
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private ParticleSystem[] partsColor = new ParticleSystem[2];
    public Pool Pool{get; set;}
    private float _offset;
    [SerializeField] private bool isMoving;
    [SerializeField] private int _click = 1;
    [SerializeField] private string powerTag;
    [SerializeField] private string colourTag;
    private Vector3 _scale;
    private float _speed = 200f;
    [SerializeField] private bool _wave;

    public enum PowerType{
        Vulnerable,
        Multiplied,
        Bomb
    }

    private PowerType _power{get; set;}

    void Start()
    {
        _offset = Random.Range(0f, 50f);
        isMoving = true;
        _wave = false;

        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void SetPower(PowerType power){
        _power = power;

        switch(power){
            case
            PowerType.Vulnerable:{
                _sprite.sprite = sprites[0];
                powerTag = "vulnerable";
                colourTag = "yellow";
            }
            break;
            case
            PowerType.Multiplied:{
                _sprite.sprite = sprites[1];
                powerTag = "multiplied";
                colourTag = "yellow";
            }
            break;
            case
            PowerType.Bomb:{
                _sprite.sprite = sprites[2];
                powerTag = "bomb";
                colourTag = "yellow";
            }
            break;
            }
    }
    public void ParticleColor(){
        if(colourTag == "yellow"){
            for(int i = 0; i < partsColor.Length; i++){
                var main = partsColor[i].main;
                main.startColor = Color.yellow;
            }
        }
        else{
            for(int i = 0; i < partsColor.Length; i++){
                var main = partsColor[i].main;
                main.startColor = Color.black;
            }
        }
    }

    void Update(){
        if(isMoving){
            Vector3 dir = Vector3.up;
            float _speed = 4f * Time.deltaTime;

            transform.Translate(dir * _speed + Vector3.right * (Mathf.Sin(Time.time + _offset) / 100));
        }
        else{}

        if(transform.position.y > 12f){
            Pool.RecyclePower(this);
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

        if(_wave == true){
            _scale = transform.GetChild(3).localScale;

            _scale.x += _speed * Time.deltaTime;
            _scale.z += _speed * Time.deltaTime;
            
            transform.GetChild(3).localScale = _scale;
        }
    }

    void OnMouseDown(){
        ParticleColor();

        if(powerTag == "vulnerable" && _click == 1){
            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();

            if(_particle.isPlaying){
                SoundManagement._sfx.selection = 0;
                SoundManagement._sfx.PlaySound();

                ScoreManagement.scoreManager.powersExploded ++;
            }
        
            StartCoroutine("DisableBalloon");

            SystemManagment.SINGLETON.isVulnerable = true;
            SystemManagment.SINGLETON.StartCoroutine("DisablePower");
        }

        if(powerTag == "multiplied" && _click == 1){
            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();

            if(_particle.isPlaying){
                SoundManagement._sfx.selection = 0;
                SoundManagement._sfx.PlaySound();

                ScoreManagement.scoreManager.powersExploded ++;
            }
        
            StartCoroutine("DisableBalloon");

            SystemManagment.SINGLETON.multiplied = true;
            SystemManagment.SINGLETON.StartCoroutine("DisablePower");
        }

        if(powerTag == "bomb" && _click == 1){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        
            _wave = true;

            _explosion.Play();
            _particle.Play();

            if(_explosion.isPlaying){
                SoundManagement._sfx.selection = 1;
                SoundManagement._sfx.PlaySound();

                ScoreManagement.scoreManager.powersExploded ++;
            }
        
            StartCoroutine("DisableBalloon");

            SystemManagment.SINGLETON.StartCoroutine("DisablePower");
        }

        if(powerTag == "time" && _click == 1){
            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();

            if(_particle.isPlaying){
                SoundManagement._sfx.selection = 0;
                SoundManagement._sfx.PlaySound();
            }
        
            StartCoroutine("DisableBalloon");

            SystemManagment.SINGLETON.timer += 5f;
            SystemManagment.SINGLETON.StartCoroutine("DisablePower");
        }

        if(powerTag == "disTime" && _click == 1){
            transform.GetChild(0).gameObject.SetActive(false);
            isMoving = false;

            _particle.Play();

            if(_particle.isPlaying){
                SoundManagement._sfx.selection = 0;
                SoundManagement._sfx.PlaySound();
            }
        
            StartCoroutine("DisableBalloon");

            SystemManagment.SINGLETON.timer -= 5f;
            SystemManagment.SINGLETON.StartCoroutine("DisablePower");
        }

        _click = 0;
    }

    private IEnumerator DisableBalloon(){
        yield return new WaitForSeconds(1f);
        
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
        
        isMoving = true;
        _wave = false;
        _click = 1;

        Pool.RecyclePower(this);
        
        StopAllCoroutines();
    }
}
