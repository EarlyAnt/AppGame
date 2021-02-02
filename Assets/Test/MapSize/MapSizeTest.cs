using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSizeTest : MonoBehaviour
{
    [SerializeField]
    private GameObject map1;
    [SerializeField]
    private GameObject map2;

    public void ShowMap1()
    {
        this.map1.SetActive(true);
        this.map2.SetActive(false);
    }

    public void ShowMap2()
    {
        this.map1.SetActive(false);
        this.map2.SetActive(true);
    }
}
