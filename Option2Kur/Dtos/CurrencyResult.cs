using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Option2Kur.Dtos
{
    public class CurrencyResult
    {
        public Res Result { get; set; }
    }

    public class Res
    {
        public List<CurrencyRate> Data { get; set; }
    }

    public class CurrencyRate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Calculated { get; set; }
    }
}
