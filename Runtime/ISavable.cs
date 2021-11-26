namespace EXSOM.SaveSyste
{
    public interface ISavable
    {
        void RegisterInSaveSystem();
        void UnregisterInSaveSystem();
        void Save(SaveDataBase saveDataBase);
    }
}