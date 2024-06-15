using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableEntity : MonoBehaviour, IInteractableEntity
{
    protected Rigidbody _rb;
    public string clipToPlayOnHit = "GRE_Langue_Hit_Objet";

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void Push(Vector3 dir, float force, GameObject frog)
    {
        _rb.AddForce(dir * force, ForceMode.VelocityChange);

        if(AudioGenerator.Instance != null)
            AudioGenerator.Instance.PlayClipAt(this.transform.position, clipToPlayOnHit);
    }
}

public interface IInteractableEntity
{
    public void Push(Vector3 dir, float force, GameObject frog);
}
