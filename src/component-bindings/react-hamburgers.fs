module react_hamburgers

open Fable.React
open Fable.Core
open Fable.Core.JsInterop
open Fable.React.Props

type HamburgerProp = 
    | Active of bool
    | ``Type`` of string
    interface IHTMLProp

let inline Hamburger (props : IHTMLProp seq) : ReactElement =
    ofImport "default" "react-hamburgers" (keyValueList CaseRules.LowerFirst props) []

Fable.Core.JsInterop.importAll "hamburgers/dist/hamburgers.css"

/// Example
let HamburgerExample (initialState:bool) = FunctionComponent.Of(fun () ->
    let toggleState = Fable.React.HookBindings.Hooks.useState initialState

    let toggle _ = 
        if toggleState.current = true then toggleState.update false 
        else toggleState.update true
        
    Hamburger [Active toggleState.current; ``Type`` "elastic"; Props.OnClick toggle]
)