using DAERP.BL.Models.Movements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAERP.BL.Models.Files
{
    public class DeliveryNoteFileModel
    {
        public int Id { get; set; }
        public string DeliveryNoteNumber { get; set; }
        public string FileName { get; set; }
        public byte[] ExcelFile { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
        public IEnumerable<DeliveryNoteModel> DeliveryNotes { get; set;}
        public DeliveryNoteFileModel()
        {
            
        }
        public DeliveryNoteFileModel(string deliveryNoteNumber, CustomerModel customer, IEnumerable<DeliveryNoteModel> deliveryNotes)
        {
            Customer = customer;
            DeliveryNoteNumber = deliveryNoteNumber;
            DeliveryNotes = deliveryNotes;
            CustomerId = Customer.Id;
        }
        public void Create()
        {
            // TODO: Create MS Excel file
        }
    }
}
