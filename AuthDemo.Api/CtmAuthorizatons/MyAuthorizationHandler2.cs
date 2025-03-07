using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Api.CtmAuthorizatons
{
    public class MyAuthorizationHandler2 : AuthorizationHandler<MyAuthorizationHandler2>, IAuthorizationRequirement
    {
        private readonly string userName;

        public MyAuthorizationHandler2(string userName)
        {
            this.userName = userName;
        }
        /// <summary>
        /// 重写 AuthorizationHandler 父类中的 HandleRequirementAsync 方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MyAuthorizationHandler2 requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Name) &&
                context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Name)).Value == userName)
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