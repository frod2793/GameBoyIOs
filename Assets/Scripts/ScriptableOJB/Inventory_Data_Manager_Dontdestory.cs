
using BackEnd.BackndNewtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

public class Inventory_Data_Manager_Dontdestory : MonoBehaviour
{
    Item_Data _scritpableobjItemData;
    public Inventory_Data _scritpableobjInventoryData;
    
    
    public string inventorydata_string;
    
    #region DontDestroyOnLoad

    private static Inventory_Data_Manager_Dontdestory _instance;

    public static Inventory_Data_Manager_Dontdestory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<Inventory_Data_Manager_Dontdestory>();
                if (_instance == null)
                {
                    GameObject container = new GameObject(nameof(Inventory_Data_Manager_Dontdestory));
                    _instance = container.AddComponent<Inventory_Data_Manager_Dontdestory>();
                }
            }
            return _instance;
        }
    }
    
    
    #endregion
    
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Demo();

        if (_scritpableobjInventoryData == null)
        {
            _scritpableobjInventoryData = ScriptableObject.CreateInstance<Inventory_Data>();
        }

        Server_Manager.Instance.Get_Inventory_Data(Server_Manager.Instance.InventoryDataInsert);

        // Log the inventory data to verify its contents
     }
    
    public void UpdateInventoryData(Inventory_Data inventoryData)
    {
        _scritpableobjInventoryData = inventoryData;
        // List를 JSON으로 변환
        inventorydata_string = JsonConvert.SerializeObject(_scritpableobjInventoryData, Formatting.Indented);
    
        // JSON 데이터를 로그에 출력
        Debug.Log("<color=green>" + inventorydata_string + "</color>");
    }
    
    public void UpdateItemData(Item_Data itemData)
    {
        _scritpableobjItemData = itemData;
    }
    
    public void SaveInventoryData()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "inventoryData.json");
        string jsonData = JsonUtility.ToJson(_scritpableobjInventoryData, true);
        System.IO.File.WriteAllText(savePath, jsonData);
    }
    
    public void SaveItemData()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "itemData.json");
        string jsonData = JsonUtility.ToJson(_scritpableobjItemData, true);
        System.IO.File.WriteAllText(savePath, jsonData);
    }
    
    

    private static bool IsNullOrEmpty(Object value)
    {
        return ReferenceEquals(value, null);
    }
    
    // Inventory_Data_Manager_Dontdestory.cs
public void Demo()
{
    // 새로운 Inventory_Data 객체 생성
    _scritpableobjInventoryData = ScriptableObject.CreateInstance<Inventory_Data>();

    // 새로운 Item_Data 객체 생성
    Item_Data itemData = ScriptableObject.CreateInstance<Item_Data>();
    itemData.itemName = "Sword";
    itemData.itemCode = 1;
    itemData.itemtype = "Weapon";
    itemData.itemCount = 1;

    // 아이템을 인벤토리에 추가
    _scritpableobjInventoryData.AddItem(itemData);

    // 또 다른 Item_Data 객체 생성
    Item_Data itemData2 = ScriptableObject.CreateInstance<Item_Data>();
    itemData2.itemName = "Shield";
    itemData2.itemCode = 2;
    itemData2.itemtype = "Armor";
    itemData2.itemCount = 1;

    // 아이템을 인벤토리에 추가
    _scritpableobjInventoryData.AddItem(itemData2);

    // List를 JSON으로 변환
    inventorydata_string = JsonConvert.SerializeObject(_scritpableobjInventoryData, Formatting.Indented);
    
    // JSON 데이터를 로그에 출력
    Debug.Log("<color=green>" + inventorydata_string + "</color>");
}
}

