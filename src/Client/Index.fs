module Index

open Elmish
open Fable.React
open ReactFlow
open Browser.Dom
type Model = obj
type Msg = obj

let init () = obj(), Cmd.none

let update msg model =
    model, Cmd.none

// Create data using anonymous record syntax here, but you can also use standard records
let elements = [|
    box {| id = "1"; ``type`` = Input.Value; data = {| label = "An Input Element" |}; position = {| x = 250; y = 5 |} |}
    box {| id = "2"; ``type`` = Output.Value; data = {| label = "An Output Element" |}; position = {| x = 100; y = 100 |} |}
    box {| id = "e1-2"; source = "1"; target = "2"; animated = true; ``type`` = Bezier.Value; arrowHeadType = ArrowClosed.Value |}
|]

let view (model: Model) (dispatch: Msg -> unit) =
    div [ Props.Style [ Props.CSSProp.Height 1000 ] ] [
        ReactFlow.create [
            ReactFlow.elements elements
            ReactFlow.onElementClick (fun (x,y) -> console.log y; window.alert "You clicked me!")
            ReactFlow.onNodeDragStop (fun (x,y) -> console.log y; window.alert "You dragged me!")
        ]
    ]
