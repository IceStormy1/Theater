using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Contracts.FileStorage;

public abstract class FileStorageParameters
{
    /// <summary>
    /// Название бакета
    /// </summary>
    public string BucketName { get; set; }
}