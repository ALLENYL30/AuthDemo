using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Api.CtmAuthorizatons
{
    public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationHandler>, IAuthorizationRequirement
    {
        private readonly string userId;

        public MyAuthorizationHandler(string userId)
        {
            this.userId = userId;
        }
        /// <summary>
        /// 重写 AuthorizationHandler 父类中的 HandleRequirementAsync 方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MyAuthorizationHandler requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) &&
                context.User.Claims.First(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value == userId)
            {
                // ...
                context.Succeed(requirement);
            }
            else
            {

                context.Fail();
            }
        }
    }
}