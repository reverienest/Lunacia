using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class VelocityBarrier : MonoBehaviour
{

    private TilemapCollider2D barriers;

    // Start is called before the first frame update
    void Start()
    {
        barriers = GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!barriers.enabled)
        {
            StartCoroutine(TurnOnCollider());
        }
    }

    IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(1);
        barriers.enabled = true;
        yield break;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.relativeVelocity.magnitude > 2)
            {
                barriers.enabled = false;
            } else {
                barriers.enabled = true;
            }
        }
    }
}
