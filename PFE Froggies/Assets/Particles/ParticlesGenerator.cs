using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;
using UnityEngine.UIElements;

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
    [Header("Default Color")]
    public List<Color> defaultColor = new List<Color>(2){Color.gray, Color.gray};

    [Header("Player Particles")]
    public GameObject touchGroundParticles;
    public GameObject highTouchGroundParticles;
    public GameObject jumpGroundParticles;
    public GameObject highJumpGroundParticles;
    public GameObject slideGround;
    public GameObject deathPlayer;

    [Header("Insect Particles")]
    public GameObject insectHitParticles;

    [Header("Object Particles")]
    public GameObject objectHitParticles;
    public GameObject objectFallingParticles;

    [Header("Libellule Particles")]
    public GameObject libelluleDeathParticles;

    [Header("Pont Levis Particles")]
    public GameObject pontLevisParticles;

    static ParticlesGenerator _instance;
    public static ParticlesGenerator Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //AudioGenerator.Instance.PlayClipAt(this.transform.position, "GRE_Langue_Hit_Objet");
    }

    void SetParticlesEntityColor(GameObject particles, string tag)
    {
        ParticlesEntity pe = particles.GetComponent<ParticlesEntity>();
        ParticleSystem.MainModule main = pe.Particles.main;
        main.startColor = new ParticleSystem.MinMaxGradient(ColorByTag(tag)[0], ColorByTag(tag)[1]);
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
        else if (useTag == "Player0")
        {
            Color pCol = PlayerManager.Instance.Controllers[0].Player.playerColor;
            Color darkPCol = Color.Lerp(pCol, Color.black, 0.15f);
            return new List<Color>(2){pCol, darkPCol};
        }
        else if (useTag == "Player1")
        {
            Color pCol = PlayerManager.Instance.Controllers[1].Player.playerColor;
            Color darkPCol = Color.Lerp(pCol, Color.black, 0.15f);
            return new List<Color>(2){pCol, darkPCol};
        }
        else
            return defaultColor;
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

    public void PlayDeathPlayer(Vector3 position)
    {
        if (deathPlayer != null)
        {
            GameObject particles = Instantiate(deathPlayer);
            particles.transform.position = position;
        }
    }

    public void PlayInsectHit(Vector3 position)
    {
        if(insectHitParticles != null)
        {
            GameObject particles = Instantiate(insectHitParticles);
            particles.transform.position = position;
        }
    }

    public void PlayObjectHit(Vector3 position)
    {
        if (objectHitParticles != null)
        {
            GameObject particles = Instantiate(objectHitParticles);
            particles.transform.position = position;
        }
    }

    public void PlayFallingBlock(Vector3 position)
    {
        if (objectFallingParticles != null)
        {
            GameObject particles = Instantiate(objectFallingParticles);
            particles.transform.position = position;
        }
    }

    public void PlayLibelluleDeath(Vector3 position)
    {
        if (libelluleDeathParticles != null)
        {
            GameObject particles = Instantiate(libelluleDeathParticles);
            particles.transform.position = position;
        }
    }

    public void PlayPontLevisHitParticles(Vector3 position)
    {
        
        if (pontLevisParticles != null)
        {
            GameObject particles = Instantiate(pontLevisParticles);
            particles.transform.position = position;
        }
    }
}
