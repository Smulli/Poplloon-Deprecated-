using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Utils : MonoBehaviour
{
    [ContextMenu("ReadData")]
    public void ReadData(){
        StartCoroutine(ReadDataCoroutine());
    }

    private IEnumerator ReadDataCoroutine(){
        UnityWebRequest web = UnityWebRequest.Get("https://smulli.github.io/HighScore/data.txt");

        yield return web.SendWebRequest();

        if(!web.isNetworkError && !web.isHttpError){
            Debug.Log(web.downloadHandler.text);
        }
        else{Debug.Log("Hubo un problema con la obeteción de la petición");}
    }

    [ContextMenu("WriteData")]
    public void WriteData(){
        StartCoroutine(WriteDataCoroutine());
    }

    private IEnumerator WriteDataCoroutine(){
        WWWForm form = new WWWForm();
        form.AddField("text", "This is a test Unity");

        UnityWebRequest web = UnityWebRequest.Post("https://smulli.github.io/HighScore/index.html", form);

        yield return web.SendWebRequest();

        if(!web.isNetworkError && !web.isHttpError){
            Debug.Log(web.downloadHandler.text);
        }
        else{Debug.Log("Hubo un problema con la obeteción de la petición");}
    }
}
