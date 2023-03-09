using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model
{
    public class SW_ACCEPT_HEAD
    {
        public string Date { get; set; }

        public string[] Dept { get; set; }

        public string PODept { get; set; }

        public string[] ACCEPTDept { get; set; }

        public string[] Applicant { get; set; }

        public string REQForm { get; set; }

        public string POForm { get; set; }

        public string IsApplicant { get; set; }

        public string IsSignOff { get; set; }

        public string Vendor { get; set; }

        public string TotalPrice { get; set; }
    }

}
