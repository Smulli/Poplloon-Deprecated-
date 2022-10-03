using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool pooler;
    public Queue<Balloon> _available = new Queue<Balloon>();
    public List<Balloon> _instantiate = new List<Balloon>();
    public Queue<Power> availablePower = new Queue<Power>();
    public List<Power> instantiatePower = new List<Power>();
    [SerializeField] private Balloon _prefab;
    [SerializeField] private Power powerPrefab;

    void Awake(){
        if(Pool.pooler == null){
            pooler = this;
        }
        else{Destroy(gameObject);}
    }

    void Start(){
        for(int i = 0; i < 15; i++){
            Balloon balloon = Instantiate(_prefab);
            balloon.Pool = this;
            Recycle(balloon);
        }

        for(int i = 0; i < 10; i++){
            Power power = Instantiate(powerPrefab);
            power.Pool = this;
            RecyclePower(power);
        }

        InvokeRepeating(nameof(Spawn), 0f, 0.7f);
        InvokeRepeating(nameof(SpawnPower), 0f, 15f);
    }

    private void Spawn(){
        Balloon _balloon = GetBalloon();
        _balloon.SetColor(ColorManagement.Instance.GetRandomColor());
        
        float xPos = Random.Range(-2f, 2f);

        _balloon.transform.position = new Vector3(xPos, -5f, -6.5f);
        _balloon.transform.localScale = Vector3.one * Random.Range(0.1f, 0.5f);
        _balloon.gameObject.SetActive(true);
    }

    public void SpawnPower(){
        Power _power = GetPower();
        _power.SetPower(ColorManagement.Instance.GetRandomPower());

        float xPos = Random.Range(-2f, 2f);

        _power.transform.position = new Vector3(xPos, -6f, -8.5f);
        _power.transform.localScale = Vector3.one * Random.Range(0.1f, 0.5f);
        _power.gameObject.SetActive(true);
    }

    public void Recycle(Balloon balloon){
        balloon.gameObject.SetActive(false);
        balloon.transform.position = Vector3.zero;
        balloon.transform.rotation = Quaternion.identity;
        _instantiate.Remove(balloon);
        _available.Enqueue(balloon);
    }

    public void RecyclePower(Power power){
        power.gameObject.SetActive(false);
        power.transform.position = Vector3.zero;
        power.transform.rotation = Quaternion.identity;
        instantiatePower.Remove(power);
        availablePower.Enqueue(power);
    }

    public Balloon GetBalloon(){
        Balloon balloon = _available.Dequeue();
        _instantiate.Add(balloon);

        return balloon;
    }

    public Power GetPower(){
        Power power = availablePower.Dequeue();
        instantiatePower.Add(power);

        return power;
    }

    public void RecycleAll(){
        while(_instantiate.Count > 0){
            Recycle(_instantiate[0]);
        }

        while(instantiatePower.Count > 0){
            RecyclePower(instantiatePower[0]);
        }
    }
}
        
