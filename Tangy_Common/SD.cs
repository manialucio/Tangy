using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangy_Common
{
    public static class SD
    {
        public const string ShoppingCart = "ShoppingCart";

        public static class OrderStatus
        {
            public const string Pending = "Pending";
            public const string Confirmed = "Confirmed";
            public const string Shipped = "Shipped";
            public const string Refunded = "Refunded";
            public const string Cancelled = "Cancelled";

        }
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Customer = "Customer";
        }

        public static class Auth
        {
            public const string AuthenticationScheme = "Bearer";
            public const string AuthenticationType = "jwtAuthType";
        }

        public static class LocalStorage
        {
            public const string JwtToken = "JWT Token";
            public const string UserDetails = "UserDetails";
        }
    }
}
