namespace Data

module DataAccess =

    open System
    open System.Data
    open System.Data.Linq
    open Microsoft.FSharp.Data.TypeProviders
    open Microsoft.FSharp.Linq

    [<Literal>]
    let ConnectionString = "Data Source=(localdb)\V11.0;Initial Catalog=TestDb;Integrated Security=SSPI;"
    type private DbSchema = SqlDataConnection<ConnectionString>

    let private db (connectionString:string) = DbSchema.GetDataContext(connectionString)

    let getNameById connectionstring id =
        let db = db ConnectionString
        let opt = db.Table1 |> Seq.tryFind (fun x -> x.Id = id)
        opt |> Option.map (fun x -> x.Name)