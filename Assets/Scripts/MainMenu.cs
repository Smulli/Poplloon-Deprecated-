using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public static MainMenu _menu;
    private float blinkingTime = 2f;
    public TextMeshProUGUI startText;
    public bool _start;
    public bool _stop;
    public GameObject[] setActive = new GameObject[6];
    [SerializeField] private TextMeshProUGUI saved;

    void Awake(){
        if(MainMenu._menu == null){
            _menu = this;
        }
        else{Destroy(gameObject);}
    }
    void Start()
    {
        StartCoroutine("BlinkingEnabled");

        for(int i = 0; i < setActive.Length; i++){
            setActive[i].SetActive(false);
        }

        _start = false;
        _stop = false;
    }

    void Update(){
        if(MainMenu._menu.enabled == true && _stop == false){
            _stop = true;

            SoundManagement._sfx.selection = 2;
            SoundManagement._sfx.PlaySound();

            for(int i = 0; i < 1; i++){
                startText.gameObject.SetActive(true);
                
                StartCoroutine("BlinkingEnabled");
            }
        }

        if(_start != true){
            foreach(GameObject i in setActive){
                i.SetActive(false);
            }
        }

        if(Input.GetMouseButtonDown(0)){
            RaycastHit _hit;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(_ray.origin, Vector3.forward, Color.red, 1f);

            if(Physics.Raycast(_ray.origin, Vector3.forward, out _hit, Mathf.Infinity)){
                foreach(GameObject i in setActive){
                    i.SetActive(true);
                }
                startText.gameObject.SetActive(false);
                gameObject.SetActive(false);
                
                _start = true;

                SoundManagement._sfx._source.Stop();

                SystemManagment.SINGLETON.DontDestroy();

                ScoreManagement.scoreManager.score = 0;

                SceneManager.LoadScene("Play");

                StopAllCoroutines();
            }
        }
    }

    private IEnumerator BlinkingEnabled(){
        yield return new WaitForSeconds(1f);

        startText.GetComponent<TextMeshProUGUI>().enabled = true;

        StartCoroutine("BlinkingDisabled");
    }

    private IEnumerator BlinkingDisabled(){
        yield return new WaitForSeconds(1f);

        startText.GetComponent<TextMeshProUGUI>().enabled = false;

        StartCoroutine("BlinkingEnabled");
    }

    void OnDisable(){
        saved.text = PlayerPrefs.GetInt("score").ToString();
        Debug.Log("Cargando");
    }
}
