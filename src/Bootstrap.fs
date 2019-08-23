module Bootstrap

open Elmish
open Elmish.React

// App
Program.mkSimple App.init App.update App.view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run