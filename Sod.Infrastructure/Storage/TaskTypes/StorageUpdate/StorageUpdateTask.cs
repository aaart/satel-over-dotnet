using System.Collections.Generic;

namespace Sod.Infrastructure.Storage.TaskTypes.StorageUpdate
{
    public class StorageUpdateTask : SatelTask
    {

        public StorageUpdateTask(string storageKey, bool[] values)
        {
            StorageKey = storageKey;
            Values = values;
        }
            
     
        public string StorageKey { get; }
        public bool[] Values { get; }    
    }
}