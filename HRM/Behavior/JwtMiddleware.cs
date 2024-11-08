using System.IdentityModel.Tokens.Jwt;

namespace HRM.Data.Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            var token = context.Request.Cookies["X-Access-Token"];

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    // Lấy ra id của người dùng từ claim
                    var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

                    //Lấy ra role của người dùng từ claim
                    var userRole = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;

                    // Lưu thông tin id, role của người dùng vào context để các middleware khác có thể sử dụng
                    context.Items["UserId"] = userId;
                    context.Items["UserRole"] = userRole;
                }

            }

            await _next(context);
        }
    }
}
