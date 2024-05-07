using System.Collections;
using UnityEngine;

public class Bascule : MonoBehaviour
{
    int _nbrPlayers = 0;
    bool _isRotating = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            _nbrPlayers++;
            if(_nbrPlayers >= 2)
            {
                ActivateRotation();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<PlayerEntity>(out PlayerEntity player))
        {
            _nbrPlayers--;
            if (_nbrPlayers < 2)
            {
                DeactivateRotation();
            }
        }
    }

    

    IEnumerator RotateBascule()
    {
        float time = 0;
        Vector3 eulers = this.transform.localEulerAngles;
        float startXEuler = eulers.x;

        while (time < 0.3f)
        {
            float xEuler = Mathf.Lerp(startXEuler, 0, time / 0.3f);
            this.transform.localRotation = Quaternion.Euler(new Vector3(eulers.x, eulers.y, xEuler));
            time += Time.fixedDeltaTime;

            yield return null;
        }
        this.transform.localRotation = Quaternion.Euler(eulers.x, eulers.y, 0);

    }

    public void ActivateRotation()
    {
        if(!_isRotating)
        {
            _isRotating = true;
            this.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(RotateBascule());
        }
    }

    public void DeactivateRotation()
    {
        StopCoroutine(RotateBascule());
        this.GetComponent<Rigidbody>().isKinematic = false;
        _isRotating = false;
    }
}
