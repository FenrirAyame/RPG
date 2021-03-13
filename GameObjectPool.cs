using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏对象池
/// </summary>
public class GameObjectPool:MonoBehaviour
{
    public Transform uiCanvas;

    //1 创建池
    private Dictionary<string, List<GameObject>> cache =
        new Dictionary<string, List<GameObject>>();
    [HideInInspector]
    public static GameObjectPool instance;

    private void Awake()
    {
        instance = this;
    }

    //2 创建使用池的元素 一个对象并使用对象：
    public GameObject CreateObject(string key,GameObject go,
        Vector3 position,Quaternion quaternion)
    {
        //1 查找池中有无可用游戏对象
        GameObject tempGo = FindUsable(key);
        //2 池中有从池中返回
        if(tempGo != null)
        {
            tempGo.transform.position = position;
            tempGo.transform.rotation = quaternion;
            tempGo.SetActive(true);
        }
        else //3 池中没有加载，放入池中再返回
        {
            tempGo = Instantiate(go, position, quaternion);
            //放入池中
            Add(key, tempGo);
        }
        //作为池物体的子物体
        if (tempGo.layer == 5)
            tempGo.transform.SetParent(uiCanvas);
        else
            tempGo.transform.parent = this.transform;
        return tempGo;
    }

    private GameObject FindUsable(string key)
    {
        if(cache.ContainsKey(key))
        {
            //找列表中找出未激活的游戏物体
            return cache[key].Find(p => !p.activeSelf);
        }
        return null;
    }

    private void Add(string key,GameObject go)
    {
        //检查池中是否有需要的键
        if(!cache.ContainsKey(key))
        {
            cache.Add(key, new List<GameObject>());
        }
        //把游戏对象添加到池中
        cache[key].Add(go);
    }

    //3 释放资源：从池中删除对象
    //3.1 释放部分：按Key释放
    public void Clear(string key)
    {
        if(cache.ContainsKey(key))
        {
            //释放场景中的游戏物体
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i]);
            }
            //移除了对象的地址
            cache.Remove(key);
        }
    }

    //3.2 释放全部
    public void ClearAll()
    {

        List<string> keys = new List<string>(cache.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Clear(keys[i]);
        }

        //foreach (var key in cache.Keys)
        //{
        //    Clear(key);
        //}
    }

    //4 回收对象
    //4.1 即时回收对象
    public void CollectObject(GameObject go)
    {
        go.SetActive(false);
    }
    //4.2 延时回收对象
    public void CollectObject(GameObject go,float delay)
    {
        StartCoroutine(CollectDelay(go, delay));
    }

    private IEnumerator CollectDelay(GameObject go,float delay)
    {
        yield return new WaitForSeconds(delay);
        CollectObject(go);
    }
}
