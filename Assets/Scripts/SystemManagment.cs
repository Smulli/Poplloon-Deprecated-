using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class SystemManagment : MonoBehaviour
{
    public static SystemManagment SINGLETON;
    public float timer = 0f;
    [SerializeField] private float timeToSave = 100f;
    public bool isVulnerable;
    public bool multiplied;
    public TextMeshProUGUI timeText;
    public Pool Pool{get; set;}
    [SerializeField] private GameObject[] unDestroy = new GameObject[13];
    [SerializeField] private GameObject[] _destroy = new GameObject[8];
    [SerializeField] private GameObject activeMenu; 
    [SerializeField] private SpriteRenderer _icon;
    
    void Awake() {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;

        if(SystemManagment.SINGLETON == null){
            SINGLETON = this;
        }
        else{
            Destroy(gameObject);

            foreach(GameObject i in _destroy){
                Destroy(i);
            }
        }

        /*if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Screen.SetResolution(256, 512, false);
            Screen.orientation = ScreenOrientation.Portrait;
        }*/
    }

    void Start(){
        isVulnerable = false;
        multiplied = false;

        _icon.enabled = false;
    }

    void Update(){
        if(MainMenu._menu._start != false){
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F0");

            timeToSave -= Time.deltaTime;
        }

        if(timeToSave <= 0f){
            AutoSave();
        }

        if(multiplied == true){
            _icon.enabled = true;
        }
        else{_icon.enabled = false;}
    }

    void AutoSave(){
        timeToSave += 100f;

        ScoreManagement.scoreManager.Result();
    }

    private IEnumerator DisablePower(){
        yield return new WaitForSeconds(5f);

        isVulnerable = false;
        multiplied = false;
    }

    public void DontDestroy(){
        foreach(GameObject i in unDestroy){
            DontDestroyOnLoad(i);
        }

        DontDestroyOnLoad(_icon);
    }
}
