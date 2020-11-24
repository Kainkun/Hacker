using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropper : MonoBehaviour
{
    public GameObject[] blocks;
    void Start()
    {
        StartCoroutine(loop());
    }

    IEnumerator loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            GameObject block = blocks[Random.Range(0, blocks.Length)];
            Instantiate(block, transform.position, Quaternion.identity);
            yield return null;
        }
    }
}
