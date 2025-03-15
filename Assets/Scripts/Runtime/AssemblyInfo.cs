using System.Runtime.CompilerServices;
using Runtime;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS)]

namespace Runtime {
    static class AssemblyInfo {
        public const string NAMESPACE_RUNTIME = "Runtime";
        public const string NAMESPACE_EDITOR = "Editor";
        public const string NAMESPACE_TESTS = "Tests";
    }
}