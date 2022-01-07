namespace DAERPLibrary.Services
{
    public interface IColorProvider
    {
        string GetHexFromRal(string ral);
        string GetHexFromRal(int? ral);
    }
}