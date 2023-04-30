module Client 

open Elmish
open Feliz
open Fable.Core
open Elmish.React
open Browser.Types
open Browser
open Browser.Dom
open Browser
open Fable.Core.JsInterop

type Model = {
    isDragging: bool
    x: float
    y: float}

type Msg = 
    |OnDragStart of MouseEvent
    |OnDrag of MouseEvent
    |OnDragEnd of MouseEvent

let init() = { isDragging = false; x = 0; y = 0 }, Cmd.none


let update (msg:Msg) (model:Model) =
    match msg with
    | OnDragStart ev -> {model with isDragging = true; x = ev.clientX; y = ev.clientY}, Cmd.none                               

    | OnDrag ev -> if model.isDragging then
                                let deltaX = ev.clientX - model.x
                                let deltaY = ev.clientY - model.y
                                let textfordrop = document.getElementById("textfordrop")
                                let prevX = textfordrop?style?left
                                let prevY = textfordrop?style?top
                                textfordrop?style?left <-  (prevX + deltaX) 
                                textfordrop?style?top <-  (prevY + deltaY) 
                                {model with x = ev.clientX; y = ev.clientY}, Cmd.none
                            else model, Cmd.none

    | OnDragEnd ev   -> let textfordrop = document.getElementById("textfordrop")
                        textfordrop?style?left <- (string ev.clientX  + "px")
                        console.log textfordrop?style?left
                        textfordrop?style?top  <- (string ev.clientY  + "px") 
                        console.log textfordrop?style?top
                        {model with isDragging = false}, Cmd.none
                                    
let modelStats (model:Model) (dispatch: Msg -> unit) =
  Html.div [
    prop.classes [ "field"; "has-addons" ]
    prop.children [
      Html.div [
        prop.classes [ "control"; "is-expanded"]
        prop.children [
          Html.input [
            prop.classes [ "input"; "is-medium" ]
            prop.valueOrDefault $"x value = {model.x}, y value = {model.y}"
          ]
        ]
      ]
      Html.div [
        prop.children [
          Html.button [
            prop.classes [ "button"; "is-primary"; "is-medium" ]
            prop.draggable true
            prop.children [
              Html.i [ prop.classes [ "fa"; "fa-plus" ] ]
            ]
          ]
        ]
      ]
    ]
  ]

let draggableSmthn (model:Model) (dispatch: Msg -> unit) =
  Html.p [
    prop.id "textfordrop"
    prop.style [style.position.initial]
    prop.text "tryin drop2"
    prop.onDragStart (fun ev -> dispatch (OnDragStart ev))
    prop.onDrag (fun ev -> dispatch (OnDrag ev))
    prop.onDragEnd (fun ev -> dispatch (OnDragEnd ev))  
    prop.draggable true
  ]


let dropTarget =
    Html.div [
        prop.style [style.width 450; style.height 300; style.left 360; style.top 260 ]
        prop.id "droptarget"
        prop.classes []
        prop.draggable true
        prop.text "23x"                
        ]

let appTitle =
  Html.p [
    prop.className "title"
    prop.text "tryin smthn new111"
  ]

let render (model:Model) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [ style.padding 30 ]
    prop.children [
      appTitle
      modelStats model dispatch
      dropTarget
      draggableSmthn model dispatch
    ]
  ]

Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
