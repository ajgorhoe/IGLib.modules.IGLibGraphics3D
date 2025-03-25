﻿
// This file is necessary to enable properties' init accessors when .NET 4.8 is targeted
// This is a fix for the following compiler error in .NET 4.8 targets:
// "Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported"

// Compiler directives not necessary:   if NET48   ... endif
namespace System.Runtime.CompilerServices
{
    public class IsExternalInit {}
}
