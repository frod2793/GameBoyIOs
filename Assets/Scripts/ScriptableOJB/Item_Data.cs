using UnityEngine;

[CreateAssetMenu(fileName = "Item_Data", menuName = "GameData/Item_Data")]
public class Item_Data : ScriptableObject
{
 public string itemName;  // 아이템 이름
 public int itemCode;     // 아이템 코드
 public string itemtype;  // 아이템 타입
 public int itemCount;    // 아이템 개수
}
