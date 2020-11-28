using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clothRandom : MonoBehaviour
{
    public GameObject[] up;
    public GameObject[] down;
    public GameObject[] hair;
    public GameObject[] foot;

    private void Start()
    {
        int upr = Random.Range(0, up.Length);
        up[upr].SetActive(true);
        int downr = Random.Range(0, down.Length);
        down[downr].SetActive(true);
        int hairr = Random.Range(0, hair.Length);
        hair[hairr].SetActive(true);
        int footr = Random.Range(0, foot.Length);
        foot[footr].SetActive(true);
    }
}
