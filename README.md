WARNING: this repository is NOT maintained any longer.  Various automated pull request regarding security issues are ignored.


# Fable React Bindings
*Writing Fable React Bindings together*


This repo wants to be a collaborative workshop where we write Fable react Bindings together.

It provides a minimal Fable app showcasing a selection of (react) components with their fable bindings. 
Its purpose is to test and develop bindings for existing react-components that don't have F# type bindings available yet.

Here you will find:

<!-- TOC -->
- how to write fable react bindings
- fable react binding for
	- CkEditor 4
	- React Hamburgers
    - more to come ...

<!-- /TOC -->


Credit where credit is due: A lot of the information found here is taken from [fable2-samples](https://github.com/fable-compiler/fable2-samples) and [Using third party React components](https://github.com/fable-compiler/fable-react/blob/master/docs/using-third-party-react-components.md)
This repo is based on [fable2-samples/minimal](https://github.com/fable-compiler/fable2-samples/tree/master/minimal).


## Table of contents

<!-- TOC -->

- [Before you get started: Requirements](#before-you-get-started-requirements)
- [Getting started: Building and running the app](#getting-started-building-and-running-the-app)
- [Project structure](#project-structure)
    - [npm](#npm)
    - [Webpack](#webpack)
    - [F#](#f)
    - [Web assets](#web-assets)
- [Adding a new binding](#adding-a-new-binding)
    - [What should I know before I get started?](#what-should-i-know-before-i-get-started)
    - [How to create a binding](#how-to-create-a-binding)
        - [1. Install the react component](#1-install-the-react-component)
        - [2. Create a new file for the bindings](#2-create-a-new-file-for-the-bindings)
        - [3. Define the React component creation function](#3-define-the-react-component-creation-function)
        - [4. Define the props](#4-define-the-props)
        - [5. Advanced typing methods: Accepting `IHTMLProp`](#5-advanced-typing-methods-accepting-ihtmlprop)
        - [6. Advanced typing methods: Nested list of props](#6-advanced-typing-methods-nested-list-of-props)
        - [7. Advanced typing methods: Arbitrary restrictions](#7-advanced-typing-methods-arbitrary-restrictions)
    - [Showcase and test the component](#showcase-and-test-the-component)

<!-- /TOC -->

## Before you get started: Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.1 or higher
* [node.js](https://nodejs.org) with [npm](https://www.npmjs.com/)
* An F# editor like Visual Studio, Visual Studio Code with [Ionide](http://ionide.io/) or [JetBrains Rider](https://www.jetbrains.com/rider/).

## Getting started: Building and running the app

* Install JS dependencies: `npm install`
* Start Webpack dev server: `npx webpack-dev-server` or `npm start`
* After the first compilation is finished, in your browser open: http://localhost:8080/

Any modification you do to the F# code will be reflected in the web page after saving.

## Project structure

### npm

JS dependencies are declared in `package.json`, while `package-lock.json` is a lock file automatically generated.

### Webpack

[Webpack](https://webpack.js.org) is a JS bundler with extensions, like a static dev server that enables hot reloading on code changes. Fable interacts with Webpack through the `fable-loader`. Configuration for Webpack is defined in the `webpack.config.js` file. Note this sample only includes basic Webpack configuration for development mode, if you want to see a more comprehensive configuration check the [Fable webpack-config-template](https://github.com/fable-compiler/webpack-config-template/blob/master/webpack.config.js).

### F#

The sample only contains two F# files: the project (.fsproj) and a source file (.fs) in the `src` folder.

### Web assets

The `index.html` file and other assets like an icon can be found in the `public` folder.

## Adding a new binding

### What should I know before I get started?

You'll have to know what [React](https://reactjs.org/docs/hello-world.html) is, how a react component is used and what 'props' are. It's not necessary to know how to write React components, since you won't be writing implementations.

More important is to have some basic knowledge of [F#](https://docs.microsoft.com/en-us/dotnet/fsharp/what-is-fsharp). What a Module is, what Records and Discriminated Unions are and how to call functions.

You should have an idea of what [Fable](https://fable.io/docs/) and [Fable.React](https://github.com/fable-compiler/fable-react) are and be familiar with the basic syntax of calling React components in Fable.

### How to create a binding

#### 1. Install the react component

Using npm, install the React component you want to provide type bindings for.

For example, if you want to provide a type binding for [CKEditor4-react](https://www.npmjs.com/package/ckeditor4-react) you'll have to run this command inside the project's root folder:

`npm install ckeditor4-react`

#### 2. Create a new file for the bindings

Open the project in vs code with the ionide plugin activated. Create a new .fs file in the folder `fable-react-bindings\src\component-bindings`.

For example, for the CKEditor4-react component we'll create a new file `fable-react-bindings\src\component-bindings\CKEditor4.fs`. In this file we'll create all the necessary type bindings and the create function to make the component available for use.

#### 3. Define the React component creation function

A React component written in Javascript can be exported in multiple ways: `default import`, `member import` and `namespace import`. We'll assume here that the component was exported with the `default` keyword. If one of the other export methods was used, see [Using third party React components](https://github.com/fable-compiler/fable-react/blob/master/docs/using-third-party-react-components.md) section 3 for more info.

In javascript an import statement of a default export would look like this:

`import CKEditor4 from 'ckeditor4-react';`

Making the component available for F#/Fable consumption would look like this:

```
let inline CKEditor (props: IHTMLProp seq) : ReactElement =
    ofImport "default" "ckeditor4-react" (keyValueList CaseRules.LowerFirst props) []
```

Note that `CKEditor4Props` won't compile since we are yet to define the props.

#### 4. Define the props

To define the props we'll have to look up the react component's documentation. Definitely don't forget to take the documentation's url and paste it in a comment block on top of the binding's file.

Props are defined by providing a _Discriminated Union_ of all the options. Each case in the Discriminated Union can reference a _primitive_ (like string or int), a _Record_ or another _Discriminated Union_.

For example:

```
type CKEditorProps =
    | Data of string
    | EditorUrl of string
    | Type of EditorType
    | ReadOnly of bool
```

Where `EditorType` is given by:

```
type EditorType =
    | Inline
    | Classic
```

If one of the props is treated as a string enum in Javascript then the `[<StringEnum>]` ([docs](https://tpetricek.github.io/Fable/docs/interacting.html#StringEnum-attribute)) attribute can be useful for defining helper types.

For example:

```
[<StringEnum>]
type EditorType =
    | Inline
    | Classic
```

Try to make illegal states invalid. You'll need to have some knowledge about modeling with F# to get the optimal solutions sometimes, especially for more complex components.

If you don't know how to represent something you can always represent an option by `obj` and change it later.

For example:

```
type CKEditorProps =
    | Data of string
    | EditorUrl of string
    | Type of EditorType
    | ReadOnly of bool
    | OnChange of (obj -> unit) // CKEditor passes a huge event with all sorts of data and methods // TODO provide a type definition for the event?
```

_See also: [Using third party React components](https://github.com/fable-compiler/fable-react/blob/master/docs/using-third-party-react-components.md)_

#### 5. Advanced typing methods: Accepting `IHTMLProp`

You can enable your component to accept any html prop by tagging your props with the `IHTMLProp` interface.

For example:

```
type CKEditorProps =
    | Data of string
    | EditorUrl of string
    | Type of EditorType
    | ReadOnly of bool
    | OnChange of (obj -> unit) // CKEditor passes a huge event with all sorts of data and methods // TODO provide a type definition for the event?
    interface IHTMLProp
```

#### 6. Advanced typing methods: Nested list of props

When one of the props is another list of props, you might need to use a `static member` on the _Discriminated Union_, using `unbox` and `keyValueList`.

For example:

```
type CKEditorProps =
    | Data of string
    | EditorUrl of string
    | Type of EditorType
    | ReadOnly of bool
    | OnChange of (obj -> unit) // CKEditor passes a huge event with all sorts of data and methods // TODO provide a type definition for the event?
    static member Config (editorConfig: EditorConfig list) : CKEditorProps = unbox ("config", keyValueList CaseRules.LowerFirst editorConfig)
    interface IHTMLProp
```

#### 7. Advanced typing methods: Arbitrary restrictions

By using a `static member` we can enforce arbitrary restrictions.

For example, the CKEditor4 has an option `ForcePasteAsPlainText` which accepts either a `boolean` value (true/false) or a `string` which must contain the value "allow-word". Instead of accepting any `string`, `boolean` or `obj` we can restrict the options to only the allowable states. This we way know at compile-time that we past a correct value:

```
static member ForcePasteAsPlainText (option: ForcePasteAsPlainTextOption) =
    match option with
    | PlainText -> unbox ("forcePasteAsPlainText", true)
    | PreserveFormatting -> unbox ("forcePasteAsPlainText", false)
    | AllowWord -> unbox ("forcePasteAsPlainText", "allow-word")
```

where ForcePastAsPlainTextOption is given by:

```
type ForcePasteAsPlainTextOption =
    | PlainText
    | PreserveFormatting
    | AllowWord
```

### Showcase and test the component

To showcase and test a component at runtime it has to be added to `ComponentsPage.fs`. The `page` element consists of a div containing all the showcased components. Adding a line there referencing the component's create function will make the component show up in the list.

For example, if the div looks like this:

```
let page =
    div [] [
        Component1.ComponentCreate [] |> displayComponent "First Component"
        Component2.ComponentCreate [] |> displayComponent "Second Component"
    ]
```

We can update it to this:

```
let page =
    div [] [
        Component1.ComponentCreate [] |> displayComponent "First Component"
        Component2.ComponentCreate [] |> displayComponent "Second Component"
        CKEditor4.CKEditor [] |> displayComponent "CKEditor 4"
    ]
```
