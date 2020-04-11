using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoParticle : MonoBehaviour
{
    public static IsoParticle CreateParticle(string particleName, Vector3 position, float zAngle, float lifeTime = 1f)
    {
        GameObject particle = ResourceManager.GetResource<GameObject>("Particles/" + particleName);

        if(particle == null || particle.GetComponent<IsoParticle>() == null)
        {
            return null;
        }

        IsoParticle iparticle = Instantiate(particle, position, Quaternion.identity).GetComponent<IsoParticle>();
        iparticle.Angle = zAngle;
        iparticle.LifeTime = lifeTime;
        iparticle.RenderChild.TranslateIsometricToWorldCoordination();
        return iparticle;
    }

    public RenderTransform RenderChild { get; set; }
    public float Angle { get => _angle;
        set
        {
            RenderChild.rotation = Quaternion.Euler(new Vector3(0f, 0f, value));
            _angle = value;
        }
    }
    public float LifeTime { get; set; }

    float _angle = 0f;
    float _currentLifeTime = 0f;

    private void Awake()
    {
        RenderChild = GetComponentInChildren<RenderTransform>();
    }

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;

        if(_currentLifeTime >= LifeTime)
        {
            Destroy(gameObject);
        }
    }
}
