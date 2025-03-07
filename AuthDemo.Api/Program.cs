using System.Security.Claims;
using AuthDemo.Api.Conts;
using AuthDemo.Api.Core;
using AuthDemo.Api.CtmAuthentication;
using AuthDemo.Api.CtmAuthorizatons;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IMySession, MySession>();
//注册鉴权架构
#region cookie
// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
// builder.Services.AddAuthentication("Cookies")
// .AddCookie(o=>{
//     o.LoginPath = "/Login/NoLogin";
// });
#endregion
#region 自定义token验证
builder.Services.AddAuthentication(op =>
{
    // 把自定义的鉴权方案添加到鉴权架构中
    op.AddScheme<SessionAuthenicationHandler>("session", "ctmSession");
    op.DefaultAuthenticateScheme = "session";
    op.DefaultChallengeScheme = "session";
    op.DefaultForbidScheme = "session";
});

#endregion

// 自定义授权
#region 授权
// builder.Services.AddAuthorization(op =>
// {
//     // op.AddPolicy(AuthorizationConts.MYPOLICY, p => 
//     //     p.RequireClaim(ClaimTypes.NameIdentifier, "6"));
//     op.AddPolicy(AuthorizationConts.MYPOLICY, p => 
//         p.RequireAssertion(
//             ass=>ass.User.HasClaim(c=>c.Type == ClaimTypes.NameIdentifier)
//             && ass.User.Claims.First(c=>c.Type.Equals(ClaimTypes.NameIdentifier)).Value == "6"));
// });
// builder.Services.AddAuthorization(op =>
// {
//     op.AddPolicy(AuthorizationConts.MYPOLICY,
//         p=>p.AddAuthenticationSchemes("token").Requirements.Add(new MyAuthorizationHandler("6")));
//     op.AddPolicy(AuthorizationConts.MYPOLICY2,
//         p=>p.AddAuthenticationSchemes("Cookies").Requirements.Add(new MyAuthorizationHandler2("Ace")));
// });
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
