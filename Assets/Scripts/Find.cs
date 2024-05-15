using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find : MonoBehaviour
{
    private GameObject cmr;
    public GameObject Text;
    // Start is called before the first frame update
    void Start()
    {
        cmr = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        FPS_Camera fps = cmr.GetComponent<FPS_Camera>();
        string info_ = fps.info;
        GameObject curText = GameObject.Find("Text(Clone)");
        if(info_ == "find" && curText == null)
        {
            GameObject newText = Instantiate(Text);
        }
        else if(info_ != "find")
        {
            Destroy(curText);
        }
    }
}
