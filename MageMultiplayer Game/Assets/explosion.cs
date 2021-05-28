using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
