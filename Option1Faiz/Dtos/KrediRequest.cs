using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Option1Faiz.Dtos
{
    public class KrediRequest
    {
        [Range(1,double.MaxValue)]
        public double Tutar { get; set; }

        [Range(1,int.MaxValue)]
        public int Vade { get; set; }
    }
}
