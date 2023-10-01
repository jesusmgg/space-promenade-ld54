using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem _hitSmallParticleSystem;
    [SerializeField] ParticleSystem _hitMediumParticleSystem;
    [SerializeField] ParticleSystem _hitLargeParticleSystem;
    [SerializeField] ParticleSystem _explosionLargeParticleSystem;

    public void EmitHitSmall(Vector3 position)
    {
        _hitSmallParticleSystem.transform.position = position;
        _hitSmallParticleSystem.Play();
    }
    
    public void EmitHitMedium(Vector3 position)
    {
        _hitMediumParticleSystem.transform.position = position;
        _hitMediumParticleSystem.Play();
    }
    public void EmitHitLarge(Vector3 position)
    {
        _hitLargeParticleSystem.transform.position = position;
        _hitLargeParticleSystem.Play();
    }
    public void EmitExplosionLarge(Vector3 position)
    {
        _explosionLargeParticleSystem.transform.position = position;
        _explosionLargeParticleSystem.Play();
    }
}