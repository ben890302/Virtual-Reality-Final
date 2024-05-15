using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FPS_Camera : MonoBehaviour
{
    public string info;
    private const float maxDistance = 10;
    private Hand hand;
    // Start is called before the first frame update
    void Start()
    {
        info = null;
        hand = GetComponent<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        GameObject curCloseTreasureChest = GameObject.Find("chest_close(Clone)");
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if (hit.transform.gameObject == curCloseTreasureChest)
            {
                info = "find";
                bool state = SteamVR_Input.GetState("GrabPinch", SteamVR_Input_Sources.RightHand) || Input.GetMouseButtonDown(1);
                if (state)
                {
                    if (curCloseTreasureChest != null)
                    {
                        Destroy(curCloseTreasureChest);
                    }
                }
            }
            else
            {
                info = null;
            }
        }
    }
}
