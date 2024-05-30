using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesGenerator : MonoBehaviour
{
    [Header("Player Particles")]
    public GameObject touchGroundParticles;
    public GameObject highTouchGroundParticles;
    public GameObject jumpGroundParticles;

    [Header("Pollen Particles")]
    public GameObject pollenParticles;

    static ParticlesGenerator _instance;
    public static ParticlesGenerator Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayTouchGround(Vector3 position)
    {
        if (touchGroundParticles != null)
        {
            GameObject particles = Instantiate(touchGroundParticles);
            particles.transform.position = position;
        }
    }

    public void PlayHighTouchGround(Vector3 position)
    {
        if (highTouchGroundParticles != null)
        {
            GameObject particles = Instantiate(highTouchGroundParticles);
            particles.transform.position = position;
        }
    }

    public void PlayJumpGround(Vector3 position, Vector3 jumpDirection)
    {
        if (jumpGroundParticles != null)
        {
            GameObject particles = Instantiate(jumpGroundParticles);
            particles.transform.position = position;
            particles.transform.rotation = Quaternion.LookRotation(jumpDirection);
        }
    }

    public void PlayPollenSuccess(Vector3 position)
    {
        if (pollenParticles != null)
        {
            GameObject particles = Instantiate(pollenParticles);
            particles.transform.position = position;
        }
    }
}
