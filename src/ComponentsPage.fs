module ComponentsPage

// General dependencies
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

// Bindings
open CKEditor4

let displayComponent name reactComponent =
    div [ Style [ Margin 25 ] ] [
        div [ Style [ MarginBottom 10; FontSize 25 ] ] [ str name ]
        reactComponent
    ]

let page =
    div [] [
        CKEditor4.CKEditor [] |> displayComponent "CKEditor 4"
    ]