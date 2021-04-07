module Index

open Elmish
open Fable.React
open ReactFlow
open Browser.Dom
open Fable.Core.JsInterop

type Msg =
    | Connect of {| source : string; target : string |}
    | Clicked of string

type Model =
    {
        Elements : FlowElement list
        ClickCount : int
    }

let init () =
    let elements = [
        FlowElement.node ("1", Input, "An Input Element", (250, 25))
        FlowElement.node ("2", Default, "A Default Node", (100, 125))
        FlowElement.node ("3", Output, "An Output Element", (250, 250))
    ]

    { Elements = elements; ClickCount = 0 }, Cmd.none

module List =
    let modifyInList predicate mapper = List.map(fun r -> if predicate r then mapper r else r)

let update msg model =
    match msg with
    | Connect connection ->
        { model with Elements = FlowElement.edge (connection.source, connection.target) :: model.Elements }, Cmd.none
    | Clicked clicked ->
        let clickedElement = model.Elements |> List.find(fun r -> r.id = clicked)
        match clickedElement with
        | Edge e ->
            { model with
                Elements = model.Elements |> List.modifyInList ((=) clickedElement) (fun _ -> Edge { e with animated = not e.animated }) }, Cmd.none
        | Node ({ id = "2" } as n) ->
            { model with
                ClickCount = model.ClickCount + 1
                Elements =
                    model.Elements
                    |> List.modifyInList
                        ((=) clickedElement)
                        (fun _ -> FlowElement.Node { n with data = {| label = $"Clicks: {model.ClickCount}" |} })}, Cmd.none
        | Node _ ->
            model, Cmd.none


let view (model: Model) (dispatch: Msg -> unit) =
    div [ Props.Style [ Props.CSSProp.Height 1000 ] ] [
        ReactFlow.create [
            ReactFlow.elements model.Elements
            ReactFlow.onElementClick (fun (x,y) -> dispatch (Clicked y?id))
            ReactFlow.onConnect (Connect >> dispatch)
        ]
    ]
