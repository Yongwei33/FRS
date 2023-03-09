using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FRS.Lib.Model.AP
{
    [XmlRoot("APGridItem")]
    public class APGridItem
    {
        [XmlAttribute("Seq")]
        public int Seq { get; set; }
        [XmlAttribute("AcctCode")]
        public string AcctCode { get; set; }
        [XmlAttribute("AcctName")]
        public string AcctName { get; set; }
        [XmlAttribute("Price")]
        public string Price { get; set; } //不含千分號
        public void Update(APGridItem obj)
        {
            Seq = obj.Seq;
            AcctCode = obj.AcctCode;
            AcctName = obj.AcctName;
            Price = obj.Price;
        }
    }
}
