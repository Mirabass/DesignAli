using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models
{
    public class EshopModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo")]
        [Required]
        [Column(TypeName = "nvarchar(5)")]
        public string Designation { get; set; }
        public enum CodeType
        {
            E, S
        }
        [Display(Name = "Kód (E - Eshop, S - Přímý prodej z výrobního skladu)")]
        [Required]
        public CodeType Code { get; set; } = CodeType.E;
        [Display(Name = "Stav")]
        [Column(TypeName = "nvarchar(3)")]
        public string State { get; set; } = "A";
        [Display(Name = "Název prodejny")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string Name { get; set; }
        [Display(Name = "Internetová adresa")]
        [Column(TypeName = "nvarchar(256)")]
        public string Web { get; set; }
        [Display(Name = "Kontaktní osoba")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContactPerson { get; set; }
        [Display(Name = "Telefon")]
        [Column(TypeName = "nvarchar(256)")]
        public string Phone { get; set; }
        [Display(Name = "Mobil")]
        [Column(TypeName = "nvarchar(256)")]
        public string Mobile { get; set; }
        [Display(Name = "E-mail")]
        [Column(TypeName = "nvarchar(256)")]
        public string Email { get; set; }


        #region SF
        [Display(Name = "Název - SF")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string SFName { get; set; }
        [Display(Name = "Ulice, č.p. - SF")]
        [Column(TypeName = "nvarchar(256)")]
        public string SFStreetAndNo { get; set; }
        [Display(Name = "PSČ - SF")]
        [Column(TypeName = "nvarchar(6)")]
        public string SFZIP { get; set; }
        [Display(Name = "Město - SF")]
        [Column(TypeName = "nvarchar(256)")]
        public string SFCity { get; set; }
        [Display(Name = "Stát - SF")]
        [Column(TypeName = "nvarchar(3)")]
        public string SFCountry { get; set; }
        /// <summary>
        /// IČO
        /// </summary>
        [Display(Name = "IČO - SF")]
        [Column(TypeName = "nvarchar(8)")]
        public string SFIN { get; set; }
        /// <summary>
        /// DIČ
        /// </summary>
        [Display(Name = "DIČ - SF")]
        [Column(TypeName = "nvarchar(12)")]
        public string SFTIN { get; set; }
        #endregion

        #region MainSettings
        [Display(Name = "Sleva - FV")]
        [Required]
        [Column(TypeName = "decimal(3,1)")]
        public decimal FVDiscountPercentValue { get; set; } = 0.0m;
        [Display(Name = "Splatnost")]
        [Column(TypeName = "numeric(2,0)")]
        public int? Maturity { get; set; }
        [Display(Name = "Měna")]
        [Column(TypeName = "nvarchar(3)")]
        public string CurrencyCode { get; set; }
        [Display(Name = "Zaokrouhlovat cenu s DPH")]
        public bool? RoundPriceWithVAT { get; set; }
        #endregion

        #region Contract
        [Display(Name = "Číslo DA")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractDANumber { get; set; }
        [Display(Name = "Číslo O")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractONumber { get; set; }
        [Display(Name = "Obsah")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractContent { get; set; }
        [Display(Name = "PoPro")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractPoPro { get; set; }
        [Display(Name = "PoUm")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractPoUm { get; set; }
        [Display(Name = "Podpis")]
        public DateTime? ContractDateSigned { get; set; }
        [Display(Name = "Od")]
        public DateTime? ContractDateFrom { get; set; }
        [Display(Name = "Do")]
        public DateTime? ContractDateTo { get; set; }
        [Display(Name = "Nájem")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal? ContractRent { get; set; }
        [Display(Name = "Období")]
        [Column(TypeName = "nvarchar(256)")]
        public string ContractPeriod { get; set; }
        [Display(Name = "Provize")]
        [Column(TypeName = "decimal(8,2)")]
        public decimal? ContractProvisionPercentValue { get; set; }
        #endregion

        [Display(Name = "Poznámka")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }

        public static async Task<List<EshopModel>> MapAsync(Dictionary<(int, int), string> eshopData, Dictionary<string, int> mapSettings, int startingRow)
        {
            int lastRow = eshopData.Select(_ => _.Key.Item1).Max() + 1;
            List<EshopModel> eshops = new List<EshopModel>();
            List<Task> tasks = new List<Task>();
            for (int row = startingRow; row < lastRow; row++)
            {
                Dictionary<int, string> data = eshopData
                                .Where(cd => cd.Key.Item1 == row)
                                .ToDictionary(cd => cd.Key.Item2, cd => cd.Value);
                EshopModel Eshop = Mapper<EshopModel>.Map(data, mapSettings);
                eshops.Add(Eshop);
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {

                throw;
            }
            return eshops;
        }

    }
}
