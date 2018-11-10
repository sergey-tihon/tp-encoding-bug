namespace MyNamespace

open System

// // Put any utilities here
[<AutoOpen>]
module Utilities = 
    let toYaml o =
        let serializer = SharpYaml.Serialization.Serializer()
        serializer.Serialize(o)

// Put any runtime constructs here
type DataSource(filename:string) = 
    member this.FileName = filename


// Put the TypeProviderAssemblyAttribute in the runtime DLL, pointing to the design-time DLL
[<assembly:CompilerServices.TypeProviderAssembly("LemonadeEncodingProvider.DesignTime.dll")>]
do ()
