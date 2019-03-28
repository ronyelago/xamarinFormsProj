namespace AppEpi
{
    public interface IStorageController
    {
        void SaveText(string filename, string text);
        string LoadText(string filename);
    }
}
