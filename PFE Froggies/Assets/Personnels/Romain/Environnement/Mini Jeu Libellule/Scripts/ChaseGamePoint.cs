using UnityEngine;

public class ChaseGamePoint : MonoBehaviour
{
    public GameObject pointHaut, pointGauche, pointBas, pointDroite;

    private void Start()
    {
        if(pointHaut == null)
        {
            pointHaut = gameObject;
        }
        if(pointGauche == null)
        {
            pointGauche = gameObject;
        }
        if(pointBas == null)
        {
            pointBas = gameObject;
        }
        if(pointDroite == null)
        {
            pointDroite = gameObject;
        }
    }
}