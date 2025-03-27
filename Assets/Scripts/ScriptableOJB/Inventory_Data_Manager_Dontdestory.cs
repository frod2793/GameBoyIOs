using System.Collections.Generic;
using BackEnd.BackndNewtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;
using System;

namespace DogGuns_Games.Lobby
{
    public class Inventory_Data_Manager_Dontdestory : MonoBehaviour
    {
        [SerializeField] private TextAsset itemDataJsonFile;
        public Inventory_Data _scritpableobjInventoryData;
        public string inventorydata_string;

        // Cache fields
        private Item_Data _scritpableobjItemData;
        private Dictionary<int, Item_Data> _itemDataCache = new Dictionary<int, Item_Data>();
        private bool _isDataLoaded = false;
        
        #region 싱글톤 할당
        
        private static Inventory_Data_Manager_Dontdestory _instance;
        private static readonly object Lock = new object();

        public static Inventory_Data_Manager_Dontdestory Instance
        {
            get
            {
                lock (Lock) // 스레드 안전성 확보
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<Inventory_Data_Manager_Dontdestory>();

                        if (_instance == null)
                        {
                            GameObject obj = new GameObject("Inventory_Data_Manager_Dontdestory");
                            _instance = obj.AddComponent<Inventory_Data_Manager_Dontdestory>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                    return _instance;
                }
            }
        }

        private void Awake()
        {
            lock (Lock)
            {
                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(gameObject);
                }
                else if (_instance != this)
                {
                    Destroy(gameObject); // 중복 제거
                }
            }
        }
        #endregion

        private void Start()
        {
            InitializeInventory();
            LoadItemDataFromJson();
            
            if (!_isDataLoaded)
                Debug.LogWarning("Failed to load item data from JSON");
                
            Server_Manager.Instance.Get_Inventory_Data(Server_Manager.Instance.InventoryDataInsert);
        }
        
        private void InitializeInventory()
        {
            if (_scritpableobjInventoryData == null)
                _scritpableobjInventoryData = ScriptableObject.CreateInstance<Inventory_Data>();
        }

        private void LoadItemDataFromJson()
        {
            if (itemDataJsonFile == null)
            {
                Debug.LogError("Item data JSON file not assigned in inspector");
                return;
            }

            try
            {
                JsonItemData[] jsonItems = JsonConvert.DeserializeObject<JsonItemData[]>(itemDataJsonFile.text);
                
                if (jsonItems == null || jsonItems.Length == 0)
                {
                    Debug.LogWarning("No items found in JSON data");
                    return;
                }
                
                _itemDataCache.Clear();
                
                foreach (var jsonItem in jsonItems)
                {
                    Item_Data itemData = ScriptableObject.CreateInstance<Item_Data>();
                    itemData.itemName = jsonItem.itemName;
                    itemData.itemCode = jsonItem.itemCode;
                    itemData.itemtype = jsonItem.itemtype;
                    itemData.itemCount = jsonItem.itemCount;
                    itemData.itemcoinType = jsonItem.itemcoinType;
                    itemData.itemcoinCount = jsonItem.itemcoinCount;
                    
                    _itemDataCache[itemData.itemCode] = itemData;
                }
                
                _isDataLoaded = true;
                Debug.Log($"Successfully loaded {_itemDataCache.Count} items from JSON data");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON data: {e.Message}");
            }
        }
        
        public Item_Data GetItemByItemCode(int itemCode)
        {
            if (!_isDataLoaded)
            {
                Debug.LogWarning("Item data not loaded yet");
                return null;
            }
            
            if (_itemDataCache.TryGetValue(itemCode, out Item_Data item))
            {
                _scritpableobjItemData = item;
                return item;
            }
            
            Debug.LogWarning($"Item with code {itemCode} not found");
            return null;
        }

        public void Update_Inventory_Data(Inventory_Data inventoryData)
        {
            _scritpableobjInventoryData = inventoryData;
            
            #if UNITY_EDITOR
            // Only generate debugging JSON in editor builds
            inventorydata_string = JsonConvert.SerializeObject(_scritpableobjInventoryData, Formatting.Indented);
            Debug.Log("<color=green>" + inventorydata_string + "</color>");
            #endif
        }

        public void UpdateItemData(Item_Data itemData)
        {
            _scritpableobjItemData = itemData;
        }

        public void SaveInventoryData()
        {
            if (_scritpableobjInventoryData == null)
                return;
                
            try 
            {
                string savePath = System.IO.Path.Combine(Application.persistentDataPath, "inventoryData.json");
                string jsonData = JsonUtility.ToJson(_scritpableobjInventoryData, true);
                System.IO.File.WriteAllText(savePath, jsonData);
                Debug.Log($"Saved inventory data to {savePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save inventory data: {e.Message}");
            }
        }

        public void SaveItemData()
        {
            if (_scritpableobjItemData == null)
                return;
                
            try
            {
                string savePath = System.IO.Path.Combine(Application.persistentDataPath, "itemData.json");
                string jsonData = JsonUtility.ToJson(_scritpableobjItemData, true);
                System.IO.File.WriteAllText(savePath, jsonData);
                Debug.Log($"Saved item data to {savePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save item data: {e.Message}");
            }
        }

        [System.Serializable]
        private class JsonItemData
        {
            public string itemName;
            public int itemCode;
            public string itemtype;
            public int itemCount;
            public string itemcoinType;
            public int itemcoinCount;
        }
    }
}