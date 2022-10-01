using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBomb : Droppable
{
    private bool isActivated = false;
    [SerializeField] private float detonateTime = 2f;
    private float detonateTimer;

    [SerializeField] private GameObject myExplosion;

    // Start is called before the first frame update
    void Start()
    {
        OnDrop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            detonateTimer += Time.deltaTime;
            if (detonateTimer > detonateTime)
            {
                // Fuse expired. Detonate!
                Detonate();
            }
        }
    }

    // THIS SHOULD OVERRIDE FROM A 'DROPPABLE' LATER
    public override void OnDrop()
    {
        isActivated = true;
    }

    private void Detonate()
    {

    }
}
