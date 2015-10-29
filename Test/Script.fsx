#r @"../build/DataAccess.dll"

open Data.DataAccess

let getNameById = getNameById @"Data Source=(localdb)\V11.0;Initial Catalog=TestDb;Integrated Security=SSPI;"

let name = getNameById 1

printfn "%A" name