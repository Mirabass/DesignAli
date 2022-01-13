using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models
{
    public class CustomerModel
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Číslo odběratele")]
        [Required]
        [Column(TypeName = "nvarchar(5)")]
        public string Designation { get; set; }
        [Display(Name = "Stav")]
        [Column(TypeName = "nvarchar(3)")]
        public string State { get; set; }
        [Display(Name = "Název prodejny")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string Name { get; set; }
        [Display(Name = "Franšíza")]
        [Column(TypeName = "nvarchar(128)")]
        public string Franchise { get; set; }


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
        [Column(TypeName = "nvarchar(2)")]
        public string SFCountry { get; set; }
        [Display(Name = "IČO - SF")]
        [Column(TypeName = "nvarchar(8)")]
        public string SFIN { get; set; }
        [Display(Name = "DIČ - SF")]
        [Column(TypeName = "nvarchar(12)")]
        public string SFTIN { get; set; }
        #endregion

        #region MainSettings
        [Display(Name = "Provize u 60 %")]
        [Required]
        [Column(TypeName = "decimal(3,1)")]
        public decimal ProvisionFor60PercentValue { get; set; }
        [Display(Name = "Sleva - FV")]
        [Required]
        [Column(TypeName = "decimal(3,1)")]
        public decimal FVDiscountPercentValue { get; set; }
        [Display(Name = "Splatnost")]
        [Required]
        [Column(TypeName = "numeric(2,0)")]
        public int Maturity { get; set; }
        [Display(Name = "Měna")]
        [Required]
        [Column(TypeName = "nvarchar(3)")]
        public string CurrencyCode { get; set; }
        [Required]
        [Display(Name = "Zaokrouhlovat cenu s DPH")]
        public bool RoundPriceWithVAT { get; set; }
        #endregion

        #region DF
        [Display(Name = "Název - DF")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string DFName { get; set; }
        [Display(Name = "Kontaktní osoba")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFContactPerson { get; set; }
        [Display(Name = "Ulice, č.p. - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFStreetAndNo { get; set; }
        [Display(Name = "PSČ - DF")]
        [Column(TypeName = "nvarchar(6)")]
        public string DFZIP { get; set; }
        [Display(Name = "Město - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFCity { get; set; }
        [Display(Name = "Stát - DF")]
        [Column(TypeName = "nvarchar(2)")]
        public string DFCountry { get; set; }
        [Display(Name = "Telefon - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFPhone { get; set; }
        [Display(Name = "Mobil - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFMobile { get; set; }
        [Display(Name = "E-mail - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFEmail { get; set; }
        [Display(Name = "IČO - DF")]
        [Column(TypeName = "nvarchar(8)")]
        public string DFIN { get; set; }
        [Display(Name = "DIČ - DF")]
        [Column(TypeName = "nvarchar(12)")]
        public string DFTIN { get; set; }
        [Display(Name = "Banka - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFBank { get; set; }
        [Display(Name = "Číslo účtu - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFAccountNumber { get; set; }
        [Display(Name = "BIC - DF")]
        [Column(TypeName = "nvarchar(256)")]
        public string DFBIC { get; set; }
        #endregion

        #region MD
        [Display(Name = "Název - MD")]
        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string MDName { get; set; }
        [Display(Name = "Kontaktní osoba")]
        [Column(TypeName = "nvarchar(256)")]
        public string MDContactPerson { get; set; }
        [Display(Name = "Ulice, č.p. - MD")]
        [Column(TypeName = "nvarchar(256)")]
        public string MDStreetAndNo { get; set; }
        [Display(Name = "PSČ - MD")]
        [Column(TypeName = "nvarchar(6)")]
        public string MDZIP { get; set; }
        [Display(Name = "Město - MD")]
        [Column(TypeName = "nvarchar(256)")]
        public string MDCity { get; set; }
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
        [Column(TypeName = "money")]
        public decimal? ContractRent { get; set; }
        [Display(Name = "Období")]
        [Column (TypeName = "nvarchar(256)")]
        public string ContractPeriod { get; set; }
        [Display(Name = "Provize")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal? ContractProvisionPercentValue { get; set; }
        #endregion

        [Display(Name = "Poznámka")]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Comment { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastModified { get; set; }

        public IList<CustomerProductModel> CustomerProducts { get; set; }
    }
}
