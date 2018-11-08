module LemonadeEncodingProviderImplementation

open System
open System.Collections.Generic
open System.IO
open System.Reflection
open FSharp.Quotations
open FSharp.Core.CompilerServices
open MyNamespace
open ProviderImplementation
open ProviderImplementation.ProvidedTypes
open System.Net.Http

// Put any utility helpers here
[<AutoOpen>]
module internal Helpers =
    let x = 1

[<TypeProvider>]
type BasicGenerativeProvider (config : TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces (config, assemblyReplacementMap=[("LemonadeEncodingProvider.DesignTime", "LemonadeEncodingProvider.Runtime")])

    let ns = "LemonadeEncodingProvider"
    let asm = Assembly.GetExecutingAssembly()

    // check we contain a copy of runtime files, and are not referencing the runtime DLL
    do assert (typeof<DataSource>.Assembly.GetName().Name = asm.GetName().Name)  

    let createType typeName (count:int) =
        let asm = ProvidedAssembly()
        let myType = ProvidedTypeDefinition(asm, ns, typeName, Some typeof<obj>, isErased=false)

        let ctor = ProvidedConstructor([], invokeCode = fun args -> <@@ "My internal state" :> obj @@>)
        myType.AddMember(ctor)

        let meth = ProvidedMethod("GetContent", [ProvidedParameter("s", typeof<string>)], 
                    typeof<StringContent>, isStatic=false, 
                    invokeCode = (fun args -> 
                        <@@
                           let s = (%%args.[1] : string)
                           new StringContent(s, System.Text.Encoding.UTF8, "application/json")
                           //Utilities.toContentString s
                        @@>))
        myType.AddMember(meth)
        asm.AddTypes [ myType ]

        myType

    let myParamType = 
        let t = ProvidedTypeDefinition(asm, ns, "GenerativeProvider", Some typeof<obj>, isErased=false)
        t.DefineStaticParameters( [ProvidedStaticParameter("Count", typeof<int>)], fun typeName args -> createType typeName (unbox<int> args.[0]))
        t
    do
        this.AddNamespace(ns, [myParamType])


[<TypeProviderAssembly>]
do ()
