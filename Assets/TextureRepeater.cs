using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRepeater : MonoBehaviour
{
    Material material;
    Vector2 offset = Vector2.zero;

    public float xVelocity;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = new Vector2(xVelocity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.isRunning)
            material.mainTextureOffset += offset * Time.deltaTime;
    }
}
