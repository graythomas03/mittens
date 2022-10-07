using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : Droppable
{
    // Priming timer
    [SerializeField] private float detonateTime = 2f;
    private float timer;

    // Explosion duration timer
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionTime = 2f;
    [SerializeField] private float explosionRadius = 3f;

    [SerializeField] private GameObject myExplosion;

    private enum BombState
    {
        PRIMING,
        EXPLODING
    }
    private BombState state;

    // Start is called before the first frame update
    void Start()
    {
        Drop();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (state == BombState.PRIMING && timer > detonateTime)
        {
            // Fuse expired. Detonate!
            Debug.Log("Detonate!");
            Detonate();
        }
        else if (state == BombState.EXPLODING && timer > explosionTime)
        {
            // Explosion has been on screen for long enough. Remove drop bomb
            Debug.Log("Destroy!");
            Destroy(gameObject);
        }
    }

    // THIS SHOULD OVERRIDE FROM A 'DROPPABLE' LATER
    public override void Drop()
    {
        state = BombState.PRIMING;
    }

    private void Detonate()
    {
        // Visual; for debugging
        float explosionDiameter = explosionRadius * 2;
        myExplosion.transform.localScale = new Vector3(explosionDiameter, explosionDiameter, explosionDiameter);
        myExplosion.SetActive(true);

        Collider[] affectedEnemies = Physics.OverlapSphere(myExplosion.transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider c in affectedEnemies)
        {
            ImpactActor(c);
        }

        state = BombState.EXPLODING;
        timer = 0;
    }

    private void ImpactActor(Collider target)
    {
        // "Damage Enemy" call would go here
        Rigidbody body = target.GetComponent<Rigidbody>();
        body.AddExplosionForce(explosionForce, myExplosion.transform.position, explosionRadius, 0, ForceMode.VelocityChange);
    }
}
