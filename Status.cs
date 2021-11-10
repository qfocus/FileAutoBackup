using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBackup
{
    public enum Status
    {
        Copied = 0, // Source file is successfully copied.
        CopyFailed = 1, // Source file is not properly copied.
        ConvertFailed = 2, // Convert ncm to original failed.
        SourceMissing = 3, // Source file is deleted before copying.
        Deleted = 4 // Source file is deleted after copying.
    }
}
