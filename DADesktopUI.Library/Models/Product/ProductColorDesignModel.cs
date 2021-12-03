namespace DADesktopUI.Library.Models.Product
{
    public class ProductColorDesignModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Orientation { get; set; }
        public int MainPartRAL { get; set; }
        public string MainPartColorName { get; set; }
        public int PocketRAL { get; set; }
        public string PocketColorName { get; set; }
    }
}