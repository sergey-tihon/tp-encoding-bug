module LemonadeEncodingProviderTests


open MyNamespace
open NUnit.Framework

type Generative2 = LemonadeEncodingProvider.GenerativeProvider<2>

[<Test>]
let ``Can access properties of generative provider 2`` () =
    let obj = Generative2()
    let cnt = obj.GetContent("123")
    Assert.NotNull(cnt)
