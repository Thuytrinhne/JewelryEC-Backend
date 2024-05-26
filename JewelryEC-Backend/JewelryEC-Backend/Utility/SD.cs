using System.Reflection.Metadata;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace JewelryEC_Backend.Utility
{
    public class SD
    {
        public enum RoleUser
        {
            USER,
            ADMIN,
            STAFF
        }
        public const int OTPValidTime_Mins = 30;
        public const int ResetPassValidTime_Mins = 30;

        public const int AccessTokenValidTime_Days = 7;

    }
    
}
