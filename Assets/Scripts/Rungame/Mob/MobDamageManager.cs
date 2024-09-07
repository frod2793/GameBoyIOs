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
    void Awake()
    {
        // mobList와 Mobinfo 리스트 크기가 다르면 경고 출력
        if (mobList.Length != Mobinfo.Count)
        {
            Debug.LogWarning("mobList와 Mobinfo의 크기가 다릅니다. 확인이 필요합니다.");
        }

        // mobList의 각 mob에 대해 Mobinfo에서 해당 mobName을 찾기
        for (int i = 0; i < mobList.Length; i++)
        {
            bool foundMatch = false;

            // Mobinfo에서 해당하는 mobName을 가진 정보를 찾음
            for (int j = 0; j < Mobinfo.Count; j++)
            {
                if (mobList[i].mobName == Mobinfo[j].Name)
                {
                    mobList[i].mobDamage = Mobinfo[j].Damage;
                    Debug.Log("Damage setting: " + mobList[i].mobName + " = " + mobList[i].mobDamage);
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                Debug.LogWarning("Damage setting failed: " + mobList[i].mobName + " 에 해당하는 Mobinfo를 찾을 수 없습니다.");
            }
        }
    }

 
}
