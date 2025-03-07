using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthDemo.Api.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace AuthDemo.Api.CtmAuthentication
{
    public class SessionAuthenicationHandler : IAuthenticationHandler
    {
        private HttpContext _context;
        private AuthenticationScheme _scheme;
        private string? _sessionId;
        private readonly IMySession _session;
        private readonly IMemoryCache _cache;
        public SessionAuthenicationHandler(IMemoryCache cache, IMySession session)
        {
            this._cache = cache;
            this._session = session;

        }
        public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
            _scheme = scheme;
            if (!TryGetSession(out string id) || !_cache.TryGetValue(_sessionId, out Dictionary<string, string> value))
            {
                var sessionId = Guid.NewGuid().ToString();
                _sessionId = sessionId;
                _context.Response.Cookies.Append(".AspNetCore.Sessions", sessionId);
                _cache.Set<Dictionary<string, string>>(sessionId, new Dictionary<string, string>(), TimeSpan.FromMinutes(20));
            }
        }
        private bool TryGetSession(out string sessionId)
        {
            var hasSession = _context.Request.Cookies.TryGetValue(".AspNetCore.Sessions", out sessionId);
            _sessionId = sessionId;
            return hasSession;
        }
        public Task<AuthenticateResult> AuthenticateAsync()
        {
            if (_cache.TryGetValue(_sessionId, out Dictionary<string, string> value))
            {
                _cache.Set<Dictionary<string, string>>(_sessionId, value, TimeSpan.FromMinutes(20));
                _session.InitSession(_sessionId, _cache);

                ClaimsIdentity claimsIdentity = new("Ctm");
                claimsIdentity.AddClaims(new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier,_sessionId)
                });
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), null, _scheme.Name)));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Session已过期"));
            }
        }

        public async Task ChallengeAsync(AuthenticationProperties? properties)
        {
            _context.Response.Redirect("/Login/NoLogin");
        }

        public async Task ForbidAsync(AuthenticationProperties? properties)
        {
            _context.Response.StatusCode = 403;
        }


    }
}