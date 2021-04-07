module ReactFlow

open Fable.Core.JsInterop
open Fable.Core
open Feliz

let reactFlow : obj = importDefault "react-flow-renderer" // import the top-level ReactFlow element
let removeElements : obj = import "removeElements" "react-flow-renderer" // a child of specific modules - not needed for this demo, but as an example
let addEdge : obj = import "addEdge" "react-flow-renderer"

[<Erase>]
/// This interface allows us to stop adding random props to the react flow.
type IReactFlow = interface end
type EdgeType = Bezier | Straight | Step | SmoothStep
type ArrowHead = Arrow | ArrowClosed

type Edge =
    {
        id : string
        _type: EdgeType
        source : string
        target : string
        animated : bool
        label : string
        arrowHeadType : ArrowHead
    }

type NodeType = Input | Output | Default

type Node =
    {
        id : string
        _type : NodeType
        data : {| label : string |}
        position : {| x : int; y : int |}
    }

type FlowElement =
    | Node of Node
    | Edge of Edge
    static member node (id, ?_type, ?label, ?pos) =
        let x,y = pos |> Option.defaultValue (0,0)
        Node { id = id; _type = defaultArg _type Input; data = {| label = defaultArg label null |}; position = {| x = x; y = y |} }
    static member edge (src, target, ?_type, ?label, ?arrowHead, ?animated) =
        Edge { id = $"e-{src}-{target}"; source = src; target = target; _type = defaultArg _type Bezier; animated = defaultArg animated false; arrowHeadType = defaultArg arrowHead Arrow; label = defaultArg label null }
    member this.id = match this with Node n -> n.id | Edge e -> e.id

let funcToTuple handler = System.Func<_,_,_>(fun a b -> handler(a,b))

// The !! below is used to "unsafely" expose a prop into an IReactFlowProp.
[<Erase>]
type ReactFlow =
    /// Creates a new ReactFlow component.
    static member inline create (props:IReactFlow seq) = Interop.reactApi.createElement (reactFlow, createObj !!props)

    /// Provides the child elements in a flow.
    static member inline elements (elements:FlowElement seq) : IReactFlow =
        let elements = [|
            for element in elements do
                match element with
                | Node n -> box {| n with ``type`` = n._type.ToString().ToLower() |}
                | Edge e -> box {| e with ``type`` = e._type.ToString().ToLower(); arrowHeadType = e.arrowHeadType.ToString().ToLower() |}
        |]
        !!("elements" ==> Seq.toArray elements)
    static member inline onElementClick (handler:(obj*obj) -> unit) : IReactFlow = !!("onElementClick" ==> funcToTuple handler)
    static member inline onNodeDragStop (handler:(obj*obj) -> unit) : IReactFlow = !!("onNodeDragStop" ==> funcToTuple handler)
    static member inline onConnect (handler:{| source : string; target : string |} -> unit) : IReactFlow = !!("onConnect" ==> handler)

