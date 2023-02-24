using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolHandler: MonoBehaviour
{
    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
          var objectPool = new Queue<GameObject>();

            for (var i = 0; i < pool.Size; i++)
            {
                var obj = Instantiate(pool.Prefab, this.transform);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj.gameObject);
            }
            poolDictionary.Add(pool.Tag, objectPool);
            StartCoroutine(spawn(pool.Tag, transform.position, Quaternion.identity));
        }
    }


    private IEnumerator spawn(string tag, Vector3 pos, Quaternion rot)
    {
        SpawnFromPool(tag, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        var isAllActive = poolDictionary[tag].Count > 0 & poolDictionary[tag].Any(i => !i.activeSelf);
        if (isAllActive)
            StartCoroutine(spawn(tag, transform.position, Quaternion.identity));
    }

    private void SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        var objSpawn = poolDictionary[tag].Dequeue();
        objSpawn.SetActive(true);
        objSpawn.transform.position = pos;
        objSpawn.transform.rotation = rot;
        var size = Random.Range(0.1f, 0.8f);
       // objSpawn.transform.localScale = new Vector3(size, size, size);
        objSpawn.transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0));
    }
}
