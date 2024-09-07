using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Mobinfo
{
    public string Name;
    public float Damage;
}
public class MobDamageManager : MonoBehaviour
{


  
    public List<Mobinfo> Mobinfo;

    [SerializeField]
    MobBase[] mobList;
  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mobinfo.Count; i++)
        {
            if (mobList[i].mobName == Mobinfo[i].Name)
            {
                mobList[i].mobDamage = Mobinfo[i].Damage;
                Debug.Log("damage setting"+i);
            }
            else
            {
                Debug.Log("damage setting false " + mobList[i].mobName);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
