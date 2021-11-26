using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace EXSOM.SaveSyste
{
    public static class SaveSystem
    {
        public const string SAVE_NAME = "SaveData";
        public const string SAVE_EXTENSION = "sd";
        public static string GetSavePath => Application.persistentDataPath + "/";
        public static bool SaveLoaded => _saveLoaded;

        private static bool _saveLoaded = false;
        private static SaveDataBase _currentSaveData;
        private static readonly List<ISavable> _savables = new List<ISavable>();

        public static void RegisterSavable(ISavable savable)
        {
            _savables.Add(savable);
        }

        public static void UnregisterSavable(ISavable savable)
        {
            _savables.Remove(savable);
        }

        public static void SaveGame()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + SAVE_NAME + "." + SAVE_EXTENSION;
            FileStream stream = new FileStream(path, FileMode.Create);

            SaveDataBase emptyDataContainer = new SaveDataBase();
            for (int i = 0; i < _savables.Count; i++)
            {
                _savables[i].Save(emptyDataContainer);
            }

            formatter.Serialize(stream, emptyDataContainer);
            stream.Close();
            Debug.Log("Game saved! Save path: " + path);
        }

        public static void LoadData(string nameSaveData = SAVE_NAME)
        {
            string path = Application.persistentDataPath + "/" + nameSaveData + "." + SAVE_EXTENSION;

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                _currentSaveData = formatter.Deserialize(stream) as SaveDataBase;
                if (_currentSaveData == null)
                {
                    _saveLoaded = false;
                }
                else
                {
                    _saveLoaded = true;
                }

                stream.Close();
                Debug.LogWarning("Game loaded!");
            }
            else
            {
                Debug.LogWarning("Save file not found in " + path);
            }
        }

        public static bool GetData<T>(string key, out T result)
        {
            if (_saveLoaded)
            {
                if (_currentSaveData.Storage.ContainsKey(key))
                {
                    result = (T)_currentSaveData.Storage[key];
                    return true;
                }
            }

            result = default;
            return false;
        }

        public static void ClearData()
        {
            _saveLoaded = false;
            _currentSaveData = null;
        }
    }
}
