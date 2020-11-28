using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class StreetGen : MonoBehaviour
{
    public GameObject empty;
    public int segmentsAmount;
    public GameObject[] segmentPrefabs;
    public bool reverse;
    int next;
    
    private void Start()
    {
        Generate();
    }

    private void Generate() {
        Cleanup();
        for (int i = 0; i < segmentsAmount; i++)
        {
            int rs = Random.Range(0, segmentPrefabs.Length);
            GameObject bob = Instantiate(segmentPrefabs[rs], empty.transform.localPosition + new Vector3(0,0,next), transform.rotation);
            bob.transform.parent = empty.transform;
            bob.transform.localPosition = new Vector3(0,0, next);
            if (!reverse)
                next += 14;
            else
                next -= 14;
        }
    }

    private void Cleanup() {
        foreach (Transform child in empty.transform) {
            Destroy(child.gameObject);
        }
    }
}
