using Newtonsoft.Json.Serialization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompetitionGame.Controllers
{
    public static class JsonContractResolvers
    {
        // Newtonsoft recommends caching and reusing contract resolvers for best performance:
        // https://www.newtonsoft.com/json/help/html/Performance.htm#ReuseContractResolver
        // But be sure not to modify IgnoreIsSpecifiedMembers after the contract resolver is first used to generate a contract.

        public static readonly DefaultContractResolver IgnoreIsSpecifiedMembersResolver =
            new DefaultContractResolver { IgnoreIsSpecifiedMembers = true };
    }
}
