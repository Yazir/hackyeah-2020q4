using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetGen : MonoBehaviour
{
    public GameObject empty;
    public int segmentsAmount;
    public GameObject[] segmentPrefabs;
    public bool reverse;
    int next;
    private void Start()
    {
        for (int i = 0; i < segmentsAmount; i++)
        {
            int rs = Random.Range(0, segmentPrefabs.Length);
            GameObject bob = Instantiate(segmentPrefabs[rs], empty.transform.localPosition + new Vector3(0,0,next), transform.rotation);
            bob.transform.parent = empty.transform;
            bob.transform.localPosition = new Vector3(0,0, next);
            if (!reverse)
                next += 12;
            else
                next -= 12;
        }
    }
}
