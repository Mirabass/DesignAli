namespace DADataManager.Library.Models.Product
{
    public class ProductDivisionModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string Comment { get; set; }
        public ProductKindModel ProductKind { get; set; }
        public ProductMaterialModel ProductMaterial { get; set; }
    }
}