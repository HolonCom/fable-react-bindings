module ComponentsPage

// General dependencies
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

// Bindings
let displayComponent name reactComponent =
    div [ Style [ Margin 25 ] ] [
        div [ Style [ MarginBottom 10; FontSize 25 ] ] [ str name ]
        reactComponent
    ]

let page =
    div [] [
        div [] [
            h1 [ ][str "ckeditor4-react"]
            CKEditor4.CKEditor [] |> displayComponent "CKEditor 4"
        ]
        div [] [
            h1 [ ][str "react-hamburger"]
            p[][str "More info: "
                a[Href "https://github.com/oscarferrandiz/react-hamburgers"; Target "_blank"][str "https://github.com/oscarferrandiz/react-hamburgers"]
            ]
            react_hamburgers.HamburgerExample true ()
        ]
    ]