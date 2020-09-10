using System;
using System.Collections.Generic;
using System.Text;

namespace Artlist.Common.Models.DTO
{
    public class ProccesEvent
    {
        public ProcessRequestType Type { get; set; }
        public ProcessStatusType Status { get; set; }
        public string FileId { get; set; }
        public float Percent { get; set; }
        public string Massege { get; set; }
        public Object Data { get; set; }
    }
}
