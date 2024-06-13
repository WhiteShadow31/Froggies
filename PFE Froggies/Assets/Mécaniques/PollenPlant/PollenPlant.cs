using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollenPlant : InteractableDuoEntity, IInteractableEntity
{
    [Header("Pollen Parameters")]
    public float pollenEmitTimerMAX = 120f;
    [SerializeField]float _pollenEmitTimer = 0;
    public float pollenSucceedMAX = 100f;
    [SerializeField] float _pollenSucceed = 0;
    float pollenEmitTimer = 120f;
    [Space]
    [SerializeField] bool _isEmitting = false;

    [Header("Gate Parameters")]
    public GatePlant gatePlant;


    [Header("Spawn Insects Parameters")]
    public float spawnRadius = 1f;
    public Vector3 offset;
    public int nbrInsectToSpawn = 3;
    [Space]
    public float insectSpawningTimeBetween = 0.8f;
    bool _isSpawning = false;

    [Header("Spawn Wave Parameters")]
    public float waitingTimeNextWave = 0.8f;
    float _waitingTime = 0;
    bool _canSpawn = true;

    public GameObject insectPrefab;

    bool canBeActivated = true;

    AudioEntity audioPlant;

    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        pollenEmitTimer = pollenEmitTimerMAX;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isEmitting && canBeActivated)
        {
            // TIME TO EMIT
            if (_pollenEmitTimer < pollenEmitTimer)
            {
                _pollenEmitTimer += Time.fixedDeltaTime;
                _pollenSucceed += Time.fixedDeltaTime;

                gatePlant.Open(_pollenSucceed / pollenSucceedMAX);
            }
            else // END OF EMITTING -> CLOSING
            {
                gatePlant.Close();
                pollenEmitTimer = pollenEmitTimerMAX;
                _pollenEmitTimer = 0;
                _pollenSucceed = 0;
                _isEmitting = false;

                if(audioPlant != null)
                    Destroy(audioPlant.gameObject);
            }

            // SUCCEED
            if(_pollenSucceed > pollenSucceedMAX)
            {
                gatePlant.TotalOpen();
                if(audioPlant != null)
                    Destroy(audioPlant.gameObject);
                canBeActivated = false;
            }

            // CAN IT SPAWNING ?
            if (_canSpawn)
            {
                // IS SPAWNING WAVE
                if (!_isSpawning)
                {
                    // COROUTINE SPAWN
                    StartCoroutine(SpawnInsect(nbrInsectToSpawn));
                }
            }
            // NOT SPAWNING 
            else
            {
                // Timer waiting for spawn wave
                if (_waitingTime < waitingTimeNextWave)
                    _waitingTime += Time.fixedDeltaTime;
                // Timer reached
                else
                {
                    _waitingTime = 0;
                    _canSpawn = true;
                }
            }
        }

        
    }

    IEnumerator SpawnInsect(int nbrInsects = 1)
    {
        _isSpawning = true;

        List<Vector3> result = CirclePositionsPizza(nbrInsects, 0, 360);


        for (int i = 0; i < nbrInsects; i++)
        {
            // SPAWN
            if(insectPrefab != null && _isEmitting)
            {
                GameObject go = Instantiate(insectPrefab);
                insectPrefab.transform.position = result[i];

                go.transform.GetComponent<Insect>().plantTarget = this;

                yield return new WaitForSeconds(insectSpawningTimeBetween);
            }
        }
        _isSpawning = false;
        _canSpawn = false;
    }

    public override void Push(Vector3 dir, float force, GameObject frog)
    {
        if (canBeActivated && !_isEmitting)
        {
            _isEmitting = true;

            if(audioPlant == null && AudioGenerator.Instance != null)
                audioPlant = AudioGenerator.Instance.PlayClipAt(this.transform.position, "GRE_Fleur_01", true);
        }
    }

    protected List<Vector3> CirclePositions(int nbrPoints = 5)
    {
        List<Vector3> result = new List<Vector3>();

        float offsetAngle = 360 / nbrPoints;

        for(int i = 0; i < nbrPoints; i++)
        {
            Vector3 pos = Vector3.zero;
            pos.x = Mathf.Sin((offsetAngle * i) * Mathf.Deg2Rad) * spawnRadius;
            pos.z = Mathf.Cos((offsetAngle * i) * Mathf.Deg2Rad) * spawnRadius;

            pos += this.transform.position + offset;
            result.Add(pos);
        }

        return result;
    }

    protected List<Vector3> CirclePositionsPizza(int nbrPoints = 5, float startAngle = 0, float endAngle = 360)
    {
        List<Vector3> result = new List<Vector3>();

        float lastAngle = 0;
        for (int i = 0; i < nbrPoints; i++)
        {
            float offsetAngle = Random.Range(startAngle, endAngle);
            while(offsetAngle < lastAngle+10 && offsetAngle > lastAngle - 10)
            {
                offsetAngle = Random.Range(startAngle, endAngle);
            }
            lastAngle = offsetAngle;

            Vector3 pos = Vector3.zero;
            pos.x = Mathf.Sin((offsetAngle * i) * Mathf.Deg2Rad) * spawnRadius;
            pos.z = Mathf.Cos((offsetAngle * i) * Mathf.Deg2Rad) * spawnRadius;

            pos += this.transform.position + offset;
            result.Add(pos);
        }

        return result;
    }

    public void HurtPlant(float timeToDecrease)
    {
        if(_pollenEmitTimer + timeToDecrease <= pollenEmitTimer)
        _pollenEmitTimer += timeToDecrease;
    }

    private void OnDrawGizmos()
    {
        List<Vector3> result = CirclePositions(10);

        for(int i = 0; i < result.Count; i++)
        {
            if (i + 1 < result.Count)
                Debug.DrawLine(result[i], result[i+1], Color.red);
            else
                Debug.DrawLine(result[i], result[0], Color.red);
        }
    }
}
