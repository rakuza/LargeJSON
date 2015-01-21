using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LargeJSON
{
    class JsonObjectReference
    {
        private long start;

        public long Start
        {
            get { return start; }
            set { start = value; }
        }
        private long end;

        public long End
        {
            get { return end; }
            set { end = value; }
        }
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public JsonObjectReference(long start, long end, int id)
        {
            this.id = id;
            this.start = start;
            this.end = end;
        }

        public JsonObjectReference(long start, long end)
        {
            this.start = start;
            this.end = end;
        }

    }
}
