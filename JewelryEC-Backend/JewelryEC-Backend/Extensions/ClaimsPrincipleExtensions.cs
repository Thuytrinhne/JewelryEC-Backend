using System.Security.Claims;

namespace JewelryEC_Backend.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserId (this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
