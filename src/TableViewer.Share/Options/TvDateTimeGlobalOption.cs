using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableViewer.Share.Options
{
    public class TvDateTimeGlobalOption
    {
        public string Calendar { get; set; } = "gregorian";
        public string Year { get; set; } = "numeric";
        public string Month { get; set; } = "numeric";
        public string Day { get; set; } = "numeric";
        public string Locale { get; set; } = "en";
        public string NumberingSystem { get; set; } = "latn";
        public string Timezone { get; set; } = "auto";
        public int Offset { get; set; } = 0;
    }
}
