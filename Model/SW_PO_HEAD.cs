using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model
{
    public class SW_PO_HEAD
    {
        public string Date { get; set; }

        public string[] Dept { get; set; }

        public string PODept { get; set; }

        public string[] Applicant { get; set; }

        public string[] POSignOff { get; set; }

        public string REQForm { get; set; }

        public string TotalPrice { get; set; }
    }

}
