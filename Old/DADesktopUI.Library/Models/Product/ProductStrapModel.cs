namespace DADesktopUI.Library.Models.Product
{
    public class ProductStrapModel : IPrototype<ProductStrapModel>
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Material { get; set; }

        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public int RAL { get; set; }
        public string ColorName { get; set; }
        public string Attachment { get; set; }

        public ProductStrapModel CreateDeepCopy()
        {
            return (ProductStrapModel)MemberwiseClone();
        }
    }
}