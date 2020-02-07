
using System;

namespace STL_ACT_1
{
    class Process : IEquatable<Process>
    {
        public string ProcessId { get; set; }
        public string PrgmmrName { get; set; }
        public string Operation { get; set; }
        public int TME { get; set; }
        public string Result { get; set; }

        public Process()
        {
            ProcessId = ""; Operation = ""; Result = "";
        }
        public Process(string name, string ope, int tme, string id, string result)
        {
            this.PrgmmrName = name; this.Operation = ope; this.TME = tme; this.ProcessId = id; this.Result = result;
        }
        public bool Equals(Process other)
        {
            return this.ProcessId == other.ProcessId;
        }
    }
}
