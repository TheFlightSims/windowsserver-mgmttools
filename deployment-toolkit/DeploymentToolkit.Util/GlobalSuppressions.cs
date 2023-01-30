// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Style",
    "IDE0049:Simplify Names",
    Justification = "Native methods should clearify type",
    Scope = "namespaceanddescendants",
    Target = "~N:DeploymentToolkit.Util"
)]
[assembly: SuppressMessage(
    "Style",
    "IDE0044:Add readonly modifier",
    Justification = "Do not add readonly modifier to native structs",
    Scope = "namespaceanddescendants",
    Target = "~N:DeploymentToolkit.Util"
)] 