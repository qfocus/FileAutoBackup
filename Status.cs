using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBackup
{
    public enum Status
    {
        Copied = 0, // Source file is successfully copied.
        Failed = 1, // Source file is not properly copied.
        Missing = 2, // Source file is deleted before copying.
        Deleted = 3 // Source file is deleted after copying.
    }
}
