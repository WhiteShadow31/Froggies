using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesGenerator : MonoBehaviour
{
    [Header("Particles color by tag")]
    public string tag1 = "Grass";
    public List<Color> grassColor = new List<Color>(2);
    public string tag2 = "Dirt";
    public List<Color> dirtColor = new List<Color>(2);
    public string tag3 = "Water";
    public List<Color> waterColor = new List<Color>(2);
    public string tag4 = "Rock";
    public List<Color> rockColor = new List<Color>(2);
    public string tag5 = "Wood";
    public List<Color> woodColor = new List<Color>(2);

    [Header("Player Particles")]
    public GameObject touchGroundParticles;
    public GameObject highTouchGroundParticles;
    public GameObject jumpGroundParticles;
    public GameObject highJumpGroundParticles;
    public GameObject salivateInsectParticles;
    public GameObject slideGround;

    [Header("Pollen Particles")]
    public GameObject pollenParticles;

    static ParticlesGenerator _instance;
    public static ParticlesGenerator Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    void SetParticlesEntityColor(GameObject particles, string tag)
    {
        ParticlesEntity pe = particles.GetComponent<ParticlesEntity>();
        ParticleSystem.MainModule main = pe.Particles.main;
        main.startColor = new ParticleSystem.MinMaxGradient(ColorByTag(tag)[0], ColorByTag(tag)[1]);;
    }

    List<Color> ColorByTag(string useTag)
    {
        if(useTag == tag1)
            return grassColor;
        else if(useTag == tag2)
            return dirtColor;
        else if(useTag == tag3)
            return waterColor;
        else if(useTag == tag4)
            return rockColor;
        else if(useTag == tag5)
            return woodColor;
        else
            return new List<Color>(2){Color.magenta, Color.red};
    }

    public void PlayTouchGround(Vector3 position, string tag = "None")
    {
        if (touchGroundParticles != null)
        {
            GameObject particles = Instantiate(touchGroundParticles);
            particles.transform.position = position;

            SetParticlesEntityColor(particles, tag);
        }
    }

    public void PlayHighTouchGround(Vector3 position, string tag = "None")
    {
        if (highTouchGroundParticles != null)
        {
            GameObject particles = Instantiate(highTouchGroundParticles);
            particles.transform.position = position;

            SetParticlesEntityColor(particles, tag);
        }
    }

    public void PlayJumpGround(Vector3 position, Vector3 jumpDirection, string tag = "None")
    {
        if (jumpGroundParticles != null)
        {
            GameObject particles = Instantiate(jumpGroundParticles);
            particles.transform.position = position;
            particles.transform.rotation = Quaternion.LookRotation(jumpDirection);

            SetParticlesEntityColor(particles, tag);
        }
    }
    public void PlayHighJumpGround(Vector3 position, Vector3 jumpDirection, string tag = "None")
    {
        if (jumpGroundParticles != null)
        {
            GameObject particles = Instantiate(highJumpGroundParticles);
            particles.transform.position = position;
            particles.transform.rotation = Quaternion.LookRotation(jumpDirection);

            SetParticlesEntityColor(particles, tag);
        }
    }

    public void PlaySlideGround(GameObject player, string tag = "None")
    {
        if(slideGround != null)
        {
            GameObject particles = Instantiate(slideGround);
            ParticlesEntity pe = particles.GetComponent<ParticlesEntity>();
            pe.Follow(player);

            SetParticlesEntityColor(particles, tag);
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

    public void PlaySalivateInsect(GameObject insect)
    {
        if(salivateInsectParticles != null)
        {
            GameObject particles = Instantiate(salivateInsectParticles);
            ParticlesEntity pe = particles.GetComponent<ParticlesEntity>();
            pe.Follow(insect);
        }
    }
}
