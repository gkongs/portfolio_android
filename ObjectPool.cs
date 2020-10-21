using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<string, Queue<GameObject>> dic_que = new Dictionary<string, Queue<GameObject>>();
    private Queue<GameObject> queue;
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    public List<GameObject> MakeObj(Transform parent, GameObject target, int size)
    {
        List<GameObject> _returnObj = new List<GameObject>();
        queue = new Queue<GameObject>();
        dic_que.Add(target.name, queue);
        for (int i = 0; i < size; i++)
        {
            target.SetActive(false);
            GameObject _obj = MonoBehaviour.Instantiate(target, parent);
            _obj.name = target.name;
            dic_que[target.name].Enqueue(_obj);
            _returnObj.Add(_obj);
        }
        return _returnObj;
    }
    public void Enque(GameObject obj)
    {
        obj.SetActive(false);
        dic_que[obj.name].Enqueue(obj);
    }
    public bool Deque(string obj_name, Vector3 spawnPos)
    {
        dic_que[obj_name].Peek().transform.position = spawnPos;
        dic_que[obj_name].Dequeue().SetActive(true);
        if (dic_que[obj_name].Count < 1) return false;
        return true;
    }
}
