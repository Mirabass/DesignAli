using DAERP.BL.Models.Movements;
using DAERP.DAL.DataAccess;
using DAERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAERP.Web.Helper
{
    public class DeliveryNoteSelectService : IDeliveryNoteSelectService
    {
        private readonly IDeliveryNoteData _deliveryNoteData;
        private ITempDataDictionary _tempData;
        private List<SelectedDeliveryNote> _selectedDeliveryNotes;
        public DeliveryNoteSelectService(IDeliveryNoteData deliveryNoteData)
        {
            _deliveryNoteData = deliveryNoteData;
        }
        public List<SelectedDeliveryNote> Get(ITempDataDictionary tempData)
        {
            _tempData = tempData;
            RetrieveSelectedDeliveryNotes();
            return _selectedDeliveryNotes;
        }
        public List<SelectedDeliveryNote> Get(int? addSelected,
            int? removeSelected,
            int? removeAllSelected, ITempDataDictionary tempData, bool checkRemains)
        {
            _tempData = tempData;
            RetrieveSelectedDeliveryNotes();
            if (addSelected is not null)
            {
                AddSelected(addSelected);
                if (checkRemains)
                {
                    CheckRemains();
                }
            }
            if (removeSelected is not null)
            {
                RemoveSelected(removeSelected);
            }
            if (removeAllSelected is not null)
            {
                RemoveAllSelected(removeAllSelected);
            }
            return _selectedDeliveryNotes;
        }

        private void CheckRemains()
        {
            _selectedDeliveryNotes.ForEach(sdn => sdn.IsPossibleAdd = sdn.DeliveryNote.Remains > 0);
        }

        private void RetrieveSelectedDeliveryNotes()
        {
            _selectedDeliveryNotes = new List<SelectedDeliveryNote>();
            var selectedDeliveryNotesIds = _tempData["SelectedDeliveryNoteIds"] as int[];
            var selectedDeliveryNoteAmounts = _tempData["SelectedDeliveryNoteAmounts"] as int[];
            if (selectedDeliveryNotesIds is not null)
            {
                for (int i = 0; i < selectedDeliveryNotesIds.Length; i++)
                {
                    int id = selectedDeliveryNotesIds[i];
                    int amount = selectedDeliveryNoteAmounts[i];
                    DeliveryNoteModel deliveryNote = _deliveryNoteData.GetDeliveryNoteBy(id);
                    SelectedDeliveryNote selectedDeliveryNote = new SelectedDeliveryNote()
                    {
                        DeliveryNote = deliveryNote,
                        Amount = amount
                    };
                    _selectedDeliveryNotes.Add(selectedDeliveryNote);
                }
            }
            _tempData.Clear();
        }

        private void AddSelected(int? addSelected)
        {
            if (_selectedDeliveryNotes.Select(sdn => sdn.DeliveryNote.Id).ToList().Contains((int)addSelected))
            {
                _selectedDeliveryNotes.Where(sdn => sdn.DeliveryNote.Id == (int)addSelected).FirstOrDefault().Amount += 1;
            }
            else
            {
                DeliveryNoteModel deliveryNote = _deliveryNoteData.GetDeliveryNoteBy((int)addSelected);
                _selectedDeliveryNotes.Add(new SelectedDeliveryNote()
                {
                    DeliveryNote = deliveryNote,
                    Amount = 1
                });
            }
        }
        private void RemoveSelected(int? removeSelected)
        {
            if (_selectedDeliveryNotes.Select(sdn => sdn.DeliveryNote.Id).ToList().Contains((int)removeSelected))
            {
                SelectedDeliveryNote selectedDeliveryNote = _selectedDeliveryNotes.Where(sp => sp.DeliveryNote.Id == (int)removeSelected).FirstOrDefault();
                if (_selectedDeliveryNotes.Where(sdn => sdn.DeliveryNote.Id == (int)removeSelected).FirstOrDefault().Amount == 1)
                {
                    _selectedDeliveryNotes.Remove(selectedDeliveryNote);
                }
                else
                {
                    _selectedDeliveryNotes.Where(sdn => sdn.DeliveryNote.Id == (int)removeSelected).FirstOrDefault().Amount -= 1;
                }
            }
            else
            {
                // TODO: add error
            }
        }
        private void RemoveAllSelected(int? removeAllSelected)
        {
            if (_selectedDeliveryNotes.Select(sp => sp.DeliveryNote.Id).ToList().Contains((int)removeAllSelected))
            {
                SelectedDeliveryNote selectedDeliveryNote = _selectedDeliveryNotes.Where(sp => sp.DeliveryNote.Id == (int)removeAllSelected).FirstOrDefault();
                _selectedDeliveryNotes.Remove(selectedDeliveryNote);
            }
            else
            {
                // TODO: add error
            }
        }
    }
}
