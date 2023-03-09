using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Lib.Model
{
    public class SW_AP_HEAD
    {
        public string Date { get; set; }

        public string Time { get; set; }

        public string[] Applicant { get; set; }

        public string REQForm { get; set; }
    }

}
