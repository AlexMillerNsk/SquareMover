import { Union, Record } from "./fable_modules/fable-library.4.0.4/Types.js";
import { union_type, class_type, record_type, float64_type, bool_type } from "./fable_modules/fable-library.4.0.4/Reflection.js";
import { Cmd_none } from "./fable_modules/Fable.Elmish.4.0.0/cmd.fs.js";
import { createElement } from "react";
import { equals, createObj } from "./fable_modules/fable-library.4.0.4/Util.js";
import { join } from "./fable_modules/fable-library.4.0.4/String.js";
import { singleton, ofArray } from "./fable_modules/fable-library.4.0.4/List.js";
import { Interop_reactApi } from "./fable_modules/Feliz.2.6.0/./Interop.fs.js";
import { ProgramModule_mkProgram, ProgramModule_run } from "./fable_modules/Fable.Elmish.4.0.0/program.fs.js";
import { Program_withReactSynchronous } from "./fable_modules/Fable.Elmish.React.4.0.0/react.fs.js";

export class Model extends Record {
    constructor(isDragging, x, y) {
        super();
        this.isDragging = isDragging;
        this.x = x;
        this.y = y;
    }
}

export function Model$reflection() {
    return record_type("Client.Model", [], Model, () => [["isDragging", bool_type], ["x", float64_type], ["y", float64_type]]);
}

export class Msg extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["HandleMouseDown", "HandleMouseMove", "HandleMouseUp"];
    }
}

export function Msg$reflection() {
    return union_type("Client.Msg", [], Msg, () => [[["Item", class_type("Browser.Types.MouseEvent", void 0, MouseEvent)]], [["Item", class_type("Browser.Types.MouseEvent", void 0, MouseEvent)]], [["Item", class_type("Browser.Types.MouseEvent", void 0, MouseEvent)]]]);
}

export function init() {
    return [new Model(false, 0, 0), Cmd_none()];
}

export function update(msg, model) {
    switch (msg.tag) {
        case 2: {
            const be_1 = msg.fields[0];
            return [new Model(false, model.x, model.y), Cmd_none()];
        }
        case 1: {
            const be_2 = msg.fields[0];
            if (model.isDragging) {
                const deltaX = be_2.clientX - model.x;
                const deltaY = be_2.clientY - model.y;
                const square = document.getElementById("square");
                const prevX = square.offsetLeft;
                const prevY = square.offsetTop;
                square.offsetLeft = (prevX + deltaX);
                square.offsetTop = (prevY + deltaY);
                return [new Model(model.isDragging, be_2.clientX, be_2.clientY), Cmd_none()];
            }
            else {
                return [model, Cmd_none()];
            }
        }
        default: {
            const be = msg.fields[0];
            return [new Model(true, be.clientX, be.clientY), Cmd_none()];
        }
    }
}

export function inputField(model, dispatch) {
    let elems_3, elems, value_3, elems_2, elems_1;
    return createElement("div", createObj(ofArray([["className", join(" ", ["field", "has-addons"])], (elems_3 = [createElement("div", createObj(ofArray([["className", join(" ", ["control", "is-expanded"])], (elems = [createElement("input", createObj(ofArray([["className", join(" ", ["input", "is-medium"])], (value_3 = model.x, ["ref", (e) => {
        if (!(e == null) && !equals(e.value, value_3)) {
            e.value = value_3;
        }
    }])])))], ["children", Interop_reactApi.Children.toArray(Array.from(elems))])]))), createElement("div", createObj(singleton((elems_2 = [createElement("button", createObj(ofArray([["className", join(" ", ["button", "is-primary", "is-medium"])], (elems_1 = [createElement("i", {
        className: join(" ", ["fa", "fa-plus"]),
    })], ["children", Interop_reactApi.Children.toArray(Array.from(elems_1))])])))], ["children", Interop_reactApi.Children.toArray(Array.from(elems_2))]))))], ["children", Interop_reactApi.Children.toArray(Array.from(elems_3))])])));
}

export function inputField2(model, dispatch) {
    let elems;
    return createElement("div", createObj(ofArray([["id", "square"], (elems = [createElement("canvas", {
        className: join(" ", ["button", "is-primary"]),
        width: 200,
        height: 100,
        onMouseDown: (ev) => {
            dispatch(new Msg(0, [ev]));
        },
        onMouseMove: (ev_1) => {
            dispatch(new Msg(1, [ev_1]));
        },
        onMouseUp: (ev_2) => {
            dispatch(new Msg(2, [ev_2]));
        },
    })], ["children", Interop_reactApi.Children.toArray(Array.from(elems))])])));
}

export const appTitle = createElement("p", {
    className: "title",
    children: "Elmish To-Do List",
});

export function render(model, dispatch) {
    let elems;
    return createElement("div", createObj(ofArray([["style", {
        padding: 20,
    }], (elems = [appTitle, inputField(model, dispatch), inputField2(model, dispatch)], ["children", Interop_reactApi.Children.toArray(Array.from(elems))])])));
}

ProgramModule_run(Program_withReactSynchronous("elmish-app", ProgramModule_mkProgram(init, update, render)));

