using System.Collections.Generic;

namespace EXSOM.SaveSyste
{
    [System.Serializable]
    public class SaveDataBase
    {
        public readonly Dictionary<string, object> Storage = new Dictionary<string, object>();
    }
}