using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractableEntity : MonoBehaviour, IIntaractableEntity
{
    protected Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void Push(Vector3 dir, float force)
    {
        _rb.AddForce(dir * force, ForceMode.Impulse);
    }
}

public interface IIntaractableEntity
{
    public void Push(Vector3 dir, float force);
}
