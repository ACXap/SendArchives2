using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendArchives.ViewModel
{
    public class EmailSettings
    {
        public bool CanChangeTextSubjectNextEmail { get; set; }
        public bool CanUseSignature { get; set; }
        public bool CanUseSignatureOnlyFirstEmail { get; set; }
        public int TimeDelaySending { get; set; }
        public bool CanRequestDeliveryReport { get; set; }
        public bool CanRequestDeliveryReportOnlyFirstEmail { get; set; }
        public bool CanRequestReadReport { get; set; }
        public bool CanRequestReadReportOnlyFirstEmail { get; set; }
    }
}
