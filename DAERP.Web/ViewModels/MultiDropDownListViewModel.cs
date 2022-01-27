using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DAERP.Web.ViewModels
{
    public class MultiDropDownListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> ItemList { get; set; }
    }
}
