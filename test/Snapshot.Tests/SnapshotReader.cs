using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Snapshot.Tests; 

public class SnapshotReader {
    public async Task<string> Read(string snapshotFile) {
        var lines = new List<string>();
        var fileStream = new FileStream($"snapshots/{snapshotFile}", FileMode.Open);
     
        using (var streamReader = new StreamReader(fileStream)) {
            return await streamReader.ReadToEndAsync();
        }
    }
}