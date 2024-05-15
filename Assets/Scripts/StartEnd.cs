using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnd : MonoBehaviour
{
    public GameObject startText;
    public GameObject endText;
    public GameObject closeTreasureChest;
    public GameObject openTreasureChest;
    // Start is called before the first frame update
    void Start()
    {
        GameObject newStartText = Instantiate(startText);
        GameObject newTreasureChest = Instantiate(closeTreasureChest);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject curCloseTreasureChest = GameObject.Find("chest_close(Clone)");
        GameObject curOpenTreasureChest = GameObject.Find("chest_open(Clone)");
        if (curCloseTreasureChest == null && curOpenTreasureChest == null)
        {
            GameObject newTreasureChest = Instantiate(openTreasureChest);
            GameObject newEndText = Instantiate(endText);
        }
    }
}