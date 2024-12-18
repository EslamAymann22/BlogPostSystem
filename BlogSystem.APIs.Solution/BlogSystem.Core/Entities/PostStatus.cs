using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public enum PostStatus
    {
        //o Status (Published, Draft, Archived) 
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Draft")]
        Draft,
        [EnumMember(Value = "Archived")]
        Archived

    }
}
