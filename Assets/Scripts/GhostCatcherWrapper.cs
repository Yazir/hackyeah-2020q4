using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCatcherWrapper : MonoBehaviour
{
    public Action<Collider> onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }
}
