module Client 

open Elmish
open Feliz
open Fable.Core
open Elmish.React
open Browser.Types
open Browser
open Browser.Dom
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
                                    let prevX = square.offsetLeft
                                    let prevY = square.offsetTop
                                    square.offsetLeft <-  (prevX + deltaX) 
                                    square.offsetTop <-  (prevY + deltaY) 
                                    {model with x = be.clientX; y = be.clientY}, Cmd.none
                                else model, Cmd.none


                                    
let inputField (model:Model) (dispatch: Msg -> unit) =
  Html.div [
    prop.classes [ "field"; "has-addons" ]
    prop.children [
      Html.div [
        prop.classes [ "control"; "is-expanded"]
        prop.children [
          Html.input [
            prop.classes [ "input"; "is-medium" ]
            prop.valueOrDefault model.x
          ]
        ]
      ]
      Html.div [
        prop.children [
          Html.button [
            prop.classes [ "button"; "is-primary"; "is-medium" ]
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-plus" ] ]
            ]
          ]
        ]
      ]
    ]
  ]

let inputField2 (model:Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.id "square"
        prop.children [
          Html.canvas [
            prop.classes [ "button"; "is-primary" ]
            prop.width 200
            prop.height 100
            prop.onMouseDown (fun ev -> dispatch (HandleMouseDown ev))
            prop.onMouseMove (fun ev -> dispatch (HandleMouseMove ev))
            prop.onMouseUp (fun ev -> dispatch (HandleMouseUp ev))
          ]
        ]
    ]
let appTitle =
  Html.p [
    prop.className "title"
    prop.text "Elmish To-Do List"
  ]

let render (model:Model) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      inputField model dispatch
      inputField2 model dispatch
    ]
  ]


Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
