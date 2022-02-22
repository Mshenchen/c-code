using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;
    public GameObject shadowPrefab;

    public int shadowCount;

    private Queue<GameObject> avaliableObjects = new Queue<GameObject>();
    private void Awake()
    {
        instance = this;
        //初始化对象池
        FillPool();
    }
    public void FillPool()
    {
        for(int i = 0; i < shadowCount; i++)
        {
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);//生成的实例放在对象池下
            //取消启用，返回对象池
            ReturnPool(newShadow);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        avaliableObjects.Enqueue(gameObject);
    }
    public GameObject GetFormPool()
    {
        if (avaliableObjects.Count==0)
        {
            FillPool();
        }
        var outShadow = avaliableObjects.Dequeue();
        outShadow.SetActive(true);
        return outShadow;
    }
}
