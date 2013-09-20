using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiveBounceChart.Web.Models
{
    public class FileRepository : MemoryRepository
    {
        private readonly string _filename;

        public FileRepository(string filename) : base( LoadData(filename).ToList() )
        {
            _filename = filename;
        }

        private static IEnumerable<TimeSpan> LoadData(string filename)
        {
            return File.ReadAllLines(filename).Select(TimeSpan.Parse);
        }

        public override void Commit()
        {
            var data = BounceTimes.Select(t => t.ToString());
            File.WriteAllLines(_filename, data);
        }
    }
}