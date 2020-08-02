using System;
using System.Collections.Generic;

namespace FXV.ViewModels.NewModels
{
    public class ViewModel_AthleteCombineRecord
    {
        public int C_ID { get; set; }
        public string CombineName { get; set; }
        public int BestAttemp { get; set; }
        public int Rank { get; set; }
        public int TopScore { get; set; }
        public List<string> TestNames { get; set; }
    }
}
