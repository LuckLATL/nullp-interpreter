using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullPInterpreter.Interpreter.CallStackManagement
{
    internal class ActivationRecord
    {
        public string Name { get; set; }
        public ActivationRecordType Type { get; set; }
        public int NestingLevel { get; set; }
        public Dictionary<string, object> Members { get; set; } = new();

        public ActivationRecord PreviousRecord { get; set; }

        public void SetMember(string memberName, object memberValue)
        {
            Members[memberName] = memberValue;
        }

        public object GetMember(string memberName)
        {
            if (!Members.ContainsKey(memberName))
            {
                if (PreviousRecord != null)
                    return PreviousRecord.GetMember(memberName);
                else
                    return null;
            }
            return Members[memberName];
        }

        public ActivationRecord(string name, ActivationRecordType type, int nestingLevel)
        {
            Name = name;
            Type = type;
            NestingLevel = nestingLevel;
        }
    }

    

}
