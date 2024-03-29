﻿using DAERP.BL.Models;
using DAERP.BL;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAERP.DAL.Services
{
    public class ColorProvider : IColorProvider
    {
        private readonly List<ColorModel> _colors = new List<ColorModel>();
        public ColorProvider()
        {
            using (StreamReader r = new StreamReader(@"..\DAERP.DAL\Data\RAL.json"))
            {
                string json = r.ReadToEnd();
                Dictionary<string,string> items = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
                foreach (var item in items)
                {
                    _colors.Add(new ColorModel()
                    {
                        RAL = item.Key.Remove(0,3),
                        HEX = item.Value
                    }) ;
                }
            }
        }
        public string GetHexFromRal(string ral)
        {
            return _colors.Where(color => color.RAL == ral).Select(color => color.HEX).FirstOrDefault();
        }
        public string GetHexFromRal(int? ral)
        {
            string ralString = CustomOperations.LeadingZeros(ral, 4);
            var colorHex = GetHexFromRal(ralString);
            return colorHex != null ? colorHex : "#FFFFFF";
        }
    }
}
