﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DADataManager.Library.Models.Product
{
    public class ProductModel
    {
        /// <summary>
        /// The unique identifier for product model
        /// </summary>
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(15)")]
        public string Designation { get; set; }
        [Required]
        [MaxLength(13)]
        public long EAN { get; set; }
        [Required]
        public ProductDivisionModel ProductDivision { get; set; }
        [Required]
        public ProductColorDesignModel ProductColorDesign { get; set; }
        [Required]
        public ProductStrapModel ProductStrap { get; set; }
        [Required]
        [Column(TypeName = "numeric(4)")]
        public int Design { get; set; }
        public string Motive { get; set; }
        public string Accessories { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateLastModified { get; set; }
    }
}