module ReactFlow

open Fable.Core.JsInterop
open Fable.Core
open Feliz

let reactFlow : obj = importDefault "react-flow-renderer" // import the top-level ReactFlow element
let removeElements : obj = import "removeElements" "react-flow-renderer" // a child of specific modules - not needed for this demo, but as an example
let addEdge : obj = import "addEdge" "react-flow-renderer"

[<Erase>]
/// This interface allows us to stop adding random props to the react flow.
type IReactFlowProp = interface end

//TODO: Create a Node type (https://reactflow.dev/docs/api/nodes/)
//TODO: Create an Edge type (https://reactflow.dev/docs/api/edges/)

// Some sample types you can use for setting properties on elements.
type EdgeType = Bezier | Straight | Step | SmoothStep member this.Value = this.ToString().ToLower()
type ArrowHead = Arrow | ArrowClosed member this.Value = this.ToString().ToLower()
type NodeType = Input | Output | Default member this.Value = this.ToString().ToLower()

// The !! below is used to "unsafely" expose a prop into an IReactFlowProp.

[<Erase>]
type ReactFlow =
    /// Creates a new ReactFlow component.
    static member inline create (props:IReactFlowProp seq) = Interop.reactApi.createElement (reactFlow, createObj !!props)

    /// Provides the child elements in a flow.
    static member inline elements (elements:_ array) : IReactFlowProp = !!("elements" ==> elements)
    static member inline onElementClick  (handler:obj -> unit) : IReactFlowProp = !!("onElementClick" ==> handler)
    static member inline onNodeDragStop  (handler:obj -> unit) : IReactFlowProp = !!("onNodeDragStop" ==> handler)

