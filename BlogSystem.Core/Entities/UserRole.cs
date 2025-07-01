using System.Runtime.Serialization;

namespace BlogSystem.Core.Entities
{
    //[Flags]
    public enum UserRole
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "Editor")]
        Editor,
        [EnumMember(Value = "Reader")]
        Reader,
        Blocked,
    }
}
