using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    public enum Type { Straight, Jump, Fly}

    public Type pattern;
    bool _isActive;

    public static float velocity = 2f;
    public int point = 1;
    public bool isActive {
        get {
            return _isActive;
        }
        set {
            _isActive = value;
            GetComponent<Collider2D>().enabled = value;
            if (_isActive)
            {
                //GetComponent<SpriteRenderer>().color = Random.ColorHSV(0.5f,1f,0.4f,0.7f,0.5f,0.7f);
                if (pattern == Type.Fly) transform.Translate(Vector3.up * Random.Range(1f,2f));
            }

        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _isActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            switch (pattern)
            {
                case Type.Fly:
                case Type.Straight:
                    transform.Translate(Vector3.left * Time.deltaTime * velocity);
                    break;
                case Type.Jump:
                    if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 1<<9))
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(-velocity, 5);
                    }
                    break;
                
                default:
                    break;
            }
        }
            
    }
}
