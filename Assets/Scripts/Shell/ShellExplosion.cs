using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 100f;
    public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_ExplosionRadius = 5f;

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
                continue;
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if (!targetHealth)
                continue;
            targetHealth.TakeDamage(CalculateDamage(targetRigidbody.position));
        }
        // 将Particle从这个shell中解耦，这样shell被Destroy之后还可以继续播放
        m_ExplosionParticles.transform.parent = null;
        m_ExplosionAudio.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        float dis = Vector3.Distance(transform.position, targetPosition);
        float damage = m_MaxDamage * (m_ExplosionRadius - dis) / m_ExplosionRadius;
        return Mathf.Clamp(damage, 0f, m_MaxDamage);
    }
}