module App

(**
 The famous Increment/Decrement ported from Elm.
 You can find more info about Elmish architecture and samples at https://elmish.github.io/
*)

// General dependencies
open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

// MODEL

type Model = int

type Msg =
| Increment
| Decrement

let init() : Model = 0

// UPDATE

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1

// VIEW (rendered with React)

let view (model:Model) dispatch =

  div []
    [
        div [] [ ComponentsPage.page ]
        // TODO use elmish to create a dropdown of components to showcase
        (* Keep counter code in comment as example
        div [] [
            button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
            div [] [ str (string model) ]
            button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ]
        ]
        *)
    ]
