public interface IPersistentObject
{
    void Save(SaveData saveData);
    void Load(SaveData saveData);
}
