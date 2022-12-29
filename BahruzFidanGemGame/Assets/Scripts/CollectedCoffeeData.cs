using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCoffeeData : MonoBehaviour
{
    public static CollectedCoffeeData instance;
    public List<Transform> CoffeeList;
    public bool changingScale = false;
    public Material diamondMaterial;
    public Transform player;
    public ParticleSystem diamondParticle;
    public int finishCount = 0;
    public List<Transform> finishPoses;
    public Transform finishPosUncompleted;
    public float damageSpeed = 1;
    public float damageRate = 1;
    public float nextDamageTime;
    public List<Material> randomColors;
    public float colorChangeSpeed = 10;
    public float colorChangeRate = 1;
    public float nextColorChangeTime;
    public Material randomColor;
    public List<GameObject> pieces;
    public List<GameObject> piecesRed;
    public float pitch;
    public float pitchSpeed;
    public float pitchRate;
    public float nextPitchTime;
    public ParticleSystem water;
    public bool inPool = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        if (finishCount == 6)
        {
            finishCount = 0;
        }

        if (Time.time > nextPitchTime)
        {
            pitch = 1;
        }
    }
}
