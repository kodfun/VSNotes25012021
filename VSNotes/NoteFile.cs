using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSNotes
{
    public class NoteFile
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
