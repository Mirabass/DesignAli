namespace DAWebERP1.Services
{
    public interface IColorProvider
    {
        string GetHexFromRal(string ral);
        string GetHexFromRal(int? ral);
    }
}