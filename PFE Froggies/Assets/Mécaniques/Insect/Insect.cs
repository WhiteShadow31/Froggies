using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Insect : MonoBehaviour, IInteractableEntity
{
    public PollenPlant plantTarget;
    public float durationToReach = 3f;

    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Start()
    {
        if(plantTarget != null)
            StartCoroutine(MoveInsect(durationToReach));
    }

    IEnumerator MoveInsect(float duration)
    {
        // Move timer
        float timer = 0;
        Vector3 startPosition = this.transform.position;
        Vector3 targetPosition = plantTarget != null ? plantTarget.transform.position : this.transform.position;

        // Calculate move with time
        while (timer < duration)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPosition, targetPosition, timer / duration);
            yield return null;
        }
        this.transform.position = targetPosition;
        plantTarget.HurtPlant(5);
        Destroy(this.gameObject);
    }

    public virtual void Push(Vector3 dir, float force, GameObject frog)
    {
        //this.gameObject.SetActive(false);

        if(AudioGenerator.Instance != null)
            AudioGenerator.Instance.PlayClipAt(this.transform.position, "GRE_Langue_Hit_Insecte");
        Destroy(this.gameObject);
    }
}
