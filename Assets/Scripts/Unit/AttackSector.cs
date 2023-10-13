using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSector : MonoBehaviour
{
    public BeAim beAim;

    private void Start()
    {
        beAim = GetComponentInParent<BeAim>();
    }
}
