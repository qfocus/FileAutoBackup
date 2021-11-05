using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBackup
{
    public class SourceFile
    {
        public long Id { get; set; }
        public String Name { get; set; }
        public Status Status { get; set; }
        public DateTime LastModifiedTime { get; set; }

    }
}
