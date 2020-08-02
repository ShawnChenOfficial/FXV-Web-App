using System;
using System.Collections.Generic;
using FXV.Models;

namespace FXV.ViewModels
{
    public class CombineCateWeightEdit
    {
        public string Category { get; set; }
        public double Weight { get; set; }
        public List<Test> Tests { get; set; }
    }
}
