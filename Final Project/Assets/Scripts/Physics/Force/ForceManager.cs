﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForceManager : MonoBehaviour
{
    private static ForceManager instance;

    public static ForceManager Instance { get { return instance; } }

    List<ForceGenerator2D> mForceGenerators = new List<ForceGenerator2D>();
    List<ForceGenerator2D> mDeadGenerators = new List<ForceGenerator2D>();


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateBungeeForceGenerator(GameObject.Find("BungeePlatform"), GameObject.Find("BungeeAnchor").transform.position, 3, 4);
        CreateSpringForceGenerator(GameObject.Find("RodAnchorSpring"), GameObject.Find("RodPlatform"), 1, 5);

        //CreatePointForceGenerator(new Vector2(0, 0), 1);
        CreateBuoyancyForceGenerator(25, -10, 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach (ForceGenerator2D forceGenerator in mForceGenerators)
        {
            if (forceGenerator == null)
            {
                mDeadGenerators.Add(forceGenerator);
            }
            else
            {
                if (forceGenerator.mShouldEffectAll == true)
                {
                    foreach (Particle2D particle in ParticleManager.Instance.mParticles)
                    {
                        if (particle.gameObject == null)
                        {
                            return;
                        }
                        forceGenerator.UpdateForce(particle.gameObject);
                    }
                }
                else
                {
                    forceGenerator.UpdateForce(null);
                }
            }
        }
        foreach (ForceGenerator2D forceGenerator in mDeadGenerators)
        {
            DeleteForceGenerator(forceGenerator);
        }
        mDeadGenerators.Clear();
    }

    void AddForceGenerator(ForceGenerator2D forceGenerator)
    {
        mForceGenerators.Add(forceGenerator);
    }
    public void DeleteForceGenerator(ForceGenerator2D forceGenerator)
    {
        mForceGenerators.Remove(forceGenerator);
    }

    public ForceGenerator2D CreatePointForceGenerator(Vector2 point, float magnitude)
    {
        GameObject forceGenerator = new GameObject("PointForceGenerator");
        PointForceGenerator newGenerator = forceGenerator.AddComponent<PointForceGenerator>();
        newGenerator.Initialize(point, magnitude);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }

    public ForceGenerator2D CreateSpringForceGenerator(GameObject object1, GameObject object2, float springConstant, float restLength)
    {
        GameObject forceGenerator = new GameObject("SpringForceGenerator");
        SpringForceGenerator newGenerator = forceGenerator.AddComponent<SpringForceGenerator>();
        newGenerator.Initialize(object1, object2, springConstant, restLength);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }

    public ForceGenerator2D CreateBuoyancyForceGenerator(float liquidDensity, float surfaceLevel, float maxDepth)
    {
        GameObject forceGenerator = new GameObject("BuoyancyForceGenerator");
        BuoyancyForceGenerator newGenerator = forceGenerator.AddComponent<BuoyancyForceGenerator>();
        newGenerator.Initialize(liquidDensity, surfaceLevel, maxDepth);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }

    public ForceGenerator2D CreateBungeeForceGenerator(GameObject object1, Vector2 anchorPoint, float springConstant, float restLength)
    {
        GameObject forceGenerator = new GameObject("BungeeForceGenerator");
        BungeeForceGenerator newGenerator = forceGenerator.AddComponent<BungeeForceGenerator>();
        newGenerator.Initialize(object1, anchorPoint, springConstant, restLength);
        AddForceGenerator(newGenerator);
        return forceGenerator.GetComponent<ForceGenerator2D>();
    }
}
