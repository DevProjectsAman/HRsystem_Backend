using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace HRsystem.Api.Services.Auth;

public class CustomAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policies = new();

    public CustomAuthorizationPolicyProvider(
        IOptions<AuthorizationOptions> options,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        // Parse policy name: "Role:Permission", "Role:", ":Permission"
        string? role = null;
        string? permission = null;

        if (policyName.Contains(':'))
        {
            var parts = policyName.Split(':', 2, StringSplitOptions.TrimEntries);

            // Empty string means null
            role = string.IsNullOrWhiteSpace(parts[0]) ? null : parts[0];
            permission = parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : null;
        }
        else
        {
            // Enforce colon-based format only
            throw new InvalidOperationException("Invalid policy format. Use 'Role:Permission', 'Role:', or ':Permission'");
        }

        // ✅ Check cache after parsing
        if (_policies.TryGetValue(policyName, out var cachedPolicy))
            return Task.FromResult(cachedPolicy);

        // Build and cache new policy
        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(role, permission))
            .Build();

        _policies[policyName] = policy;
        return Task.FromResult(policy);
    }
}
