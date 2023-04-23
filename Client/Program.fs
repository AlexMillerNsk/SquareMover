module Client 

open Elmish
open Feliz
open Fable.Core
open Elmish.React
open Fable.Import
open Browser

type Model = {
    isDragging: bool
    x: float
    y: float}

type Msg = 
    |HandleMouseDown of MouseEvent
    |HandleMouseMove of MouseEvent
    |HandleMouseUp of MouseEvent

let init() = { isDragging = false; x = 0; y = 0 }, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | HandleMouseDown be -> {model with isDragging = true; x = be.clientX; y = be.clientY}, Cmd.none                               
    | HandleMouseUp   be -> {model with isDragging = false}, Cmd.none
    | HandleMouseMove be ->     if model.isDragging then
                                    let deltaX = be.clientX - model.x
                                    let deltaY = be.clientY - model.y
                                    let square = document.getElementById("square")
                                    let prevX = float (square.style.left.Replace("px", ""))
                                    let prevY = float (square.style.top.Replace("px", ""))
                                    square.style.left <- string (prevX + deltaX) + "px"
                                    square.style.top <- string (prevY + deltaY) + "px"
                                    {model with x = be.clientX; y = be.clientY}, Cmd.none
                                else model, Cmd.none



let div (classes: string list) (children: ReactElement list) =
    Html.div [
        prop.classes classes
        prop.children children
    ]

let render model (dispatch: Msg -> unit) =
    div [
        prop.onMouseDown (fun _ -> dispatch HandleMouseDown)
        prop.onMouseMove (fun _ -> dispatch HandleMouseDown )
        prop.onMouseUp (fun _ -> dispatch HandleMouseUp ) 
     ] 
   
        div [
            prop.style [
                "position", "absolute"
                "width", "100px"
                "height", "100px"
                "background-color", "blue"
                "left", "0px"
                "top", "0px"
            ]
            prop.id "square"
        ] []


Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
