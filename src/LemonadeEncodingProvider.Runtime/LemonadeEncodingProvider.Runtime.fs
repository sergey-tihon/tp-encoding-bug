namespace MyNamespace

open System

// Put any utilities here
[<AutoOpen>]
module Utilities = 
    open System.Net.Http

    let toContentString s =
        new StringContent(s, System.Text.Encoding.UTF8, "application/json")

// Put any runtime constructs here
type DataSource(filename:string) = 
    member this.FileName = filename


// Put the TypeProviderAssemblyAttribute in the runtime DLL, pointing to the design-time DLL
[<assembly:CompilerServices.TypeProviderAssembly("LemonadeEncodingProvider.DesignTime.dll")>]
do ()
