module CKEditor4

open System.Text.RegularExpressions
open Fable.React
open Fable.Core
open Fable.Core.JsInterop
open Fable.React.Props

/// Bindings for the CKEditor 4 react component to be consumed from F#/Fable
/// See: https://ckeditor.com/docs/ckeditor4/latest/guide/dev_react.html#ckeditor-4-react-integration-demo
/// Bindings created by following this guide: https://github.com/fable-compiler/fable-react/blob/master/docs/using-third-party-react-components.md#using-third-party-react-components 

[<StringEnum>]
type EditorType =
    | Inline
    | Classic

[<StringEnum>]
type DefaultContentType =
    | Html
    | Text

[<StringEnum>]
type CopyFormattingContext =
    | Text
    | List
    | Table

[<StringEnum>]
type ScaytStoreOptions =
    | Options
    | [<CompiledName("ignore-all-caps-words")>] IgnoreAllCapsWords
    | [<CompiledName("ignore-domain-names")>] IgnoreDomainNames
    | [<CompiledName("ignore-words-with-mixed-cases")>] IgnoreWordsWithMixedCases
    | [<CompiledName("ignore-words-with-numbers")>] IgnoreWordsWithNumbers
    | Lang
    | All

[<StringEnum>]
type StartupFocus = Start | End

type AllowedContentRules = string
type AllowedContent =
    | Rules of AllowedContentRules
    | NoFilter

type AutoEmbedWidget =
    | WidgetNames of string list
    | Callback of (string -> string)

type CopyFormattingAllowedContexts =
    | Contexts of CopyFormattingContext list
    | All

type ProcessNumericalOption =
    | Force
    | Remaining
    | Nothing

type PixelsOrPercentage =
    | Pixels of int
    | Percentage of double
let unboxPixelsOrPercentage propertyName value =
    match value with
    | Pixels pxs -> unbox (propertyName, pxs)
    | Percentage percent -> unbox (propertyName, string percent + "%") // TODO check double gets converted to a string like this `50,6` and not something like `50.6`

type ForcePasteAsPlainTextOption =
    | PlainText
    | PreserveFormatting
    | AllowWord

// https://ckeditor.com/docs/ckeditor4/latest/api/CKEDITOR_plugins_mentions_configDefinition.html
type MentionsConfigDefinition =
    | Cache of bool
    | CaseSensitive of bool
    | Feed of string list // TODO improvement: expand with function option as well
    | ItemTemplate of string
    | ItemsLimit of int
    | Marker of string
    | MinChars of int
    | OutputTemplate of string
    | Pattern of Regex
    | Throttle of int
    
type StyleSetOption =
    | NoStyles
    | ByRegisteredName of string
    | ByNameWithUrl of name:string * url:string
    | ByDefinitions of {| name: string; element: string |} list // TODO might want to strongly type elements (instead of by string)

type ToolbarDefinition = string list
type TooBarOption =
    | ByName of string
    | ByDefinitions of ToolbarDefinition list

// https://ckeditor.com/docs/ckeditor4/latest/api/CKEDITOR_config.html
type EditorConfig =
    | AutoGrow_bottomSpace of int
    | AutoGrow_maxHeight of int
    | AutoGrow_minHeight of int
    | AutoGrow_onStartUp of bool
    | AutoUpdateElement of bool
    | Autocomplete_commitKeystrokes of int list
    | Autolink_commitKeystrokes of int list
    | Autolink_emailRegex of Regex
    | Autolink_urlRegex of Regex
    | BaseFloatZIndex of int
    | BaseHref of string
    | BasicEntities of bool
    | BlockedKeystrokes of string list // TODO should be a list of KeyCodes
    | BodyClass of string
    | BodyId of string
    | BrowserContextMenuOnCtrl of bool
    | Clipboard_defaultContentType of DefaultContentType
    | Clipboard_notificationDuration of int
    | CloudServices_tokenUrl of string
    | CloudServices_uploadUrl of string
    | CodeSnippetGeshi_url of string
    | CodeSnippet_codeClass of string
    | CodeSnippet_languages of obj // TODO possibly improve
    | CodeSnippet_theme of string
    | ColorButton_backStyle of obj // TODO possibly improve
    | ColorButton_colors of string
    | ColorButton_colorsPerRow of int
    | ColorButton_enableAutomatic of bool
    | ColorButton_enableMore of bool
    | ColorButton_foreStyle of obj // TODO possibly improve
    | ColorButton_normalizeBackground of bool
    | ContentsCss of string list
    | ContentsLangDirection of string
    | ContentsLanguage of string
    | Contextmenu_contentsCss of string list
    | CopyFormatting_disallowRules of string
    | CopyFormatting_keystrokeCopy of int
    | CopyFormatting_keystrokePaste of int
    | CopyFormatting_outerCursor of bool
    | CoreStyles_bold of obj // TODO possibly improve
    | CoreStyles_italic of obj // TODO possibly improve
    | CoreStyles_strike of obj // TODO possibly improve
    | CoreStyles_subscript of obj // TODO possibly improve
    | CoreStyles_superscript of obj // TODO possibly improve
    | CoreStyles_underline of obj // TODO possibly improve
    | CustomConfig of string
    | DataIndentationChars of string
    | DefaultLanguage of string
    | Devtools_styles of string
    | Devtools_textCallback of (obj * obj * obj * obj -> unit) // TODO possibly improve
    | Dialog_backgroundCoverColor of string
    | Dialog_backgroundCoverOpacity of int
    | Dialog_buttonsOrder of string
    | Dialog_magnetDistance of int
    | Dialog_noConfirmCancel of bool
    | Dialog_startupFocusTab of bool
    | DisableNativeSpellChecker of bool
    | DisableNativeTableHandles of bool
    | DisableObjectResizing of bool
    | DisableReadonlyStyling of bool
    | DisallowedContent of string
    | Div_wrapTable of bool
    | DocType of string
    | Easyimage_toolbar of string list
    | EmailProtection of string
    | Embed_provider of string
    | Emoji_emojiListUrl of string
    | Emoji_minChars of int
    | EnableContextMenu of bool
    | EnableTabKeyTools of bool
    | EnterMode of int
    | Entities of bool
    | Entities_additional of string
    | Entities_greek of bool
    | Entities_latin of bool
    | ExtraAllowedContent of obj
    | ExtraPlugins of string list
    | FileTools_defaultFileName of string
    | FileTools_requestHeaders of obj
    | FilebrowserBrowseUrl of string
    | FilebrowserFlashBrowseUrl of string
    | FilebrowserFlashUploadUrl of string
    | FilebrowserImageBrowseLinkUrl of string
    | FilebrowserImageBrowseUrl of string
    | FilebrowserImageUploadUrl of string
    | FilebrowserUploadMethod of string
    | FilebrowserUploadUrl of string
    | FilebrowserWindowFeatures of string
    | FillEmptyBlocks of obj // TODO possibly improve
    | Find_highlight of obj // TODO possibly improve
    | FlashAddEmbedTag of bool
    | FlashConvertOnEdit of bool
    | FlashEmbedTagOnly of bool
    | FloatSpaceDockedOffsetX of int
    | FloatSpaceDockedOffsetY of int
    | FloatSpacePinnedOffsetX of int
    | FloatSpacePinnedOffsetY of int
    | FloatSpacePreferRight of bool
    | FontSize_defaultLabel of string
    | FontSize_sizes of string
    | FontSize_style of obj
    | Font_defaultLabel of string
    | Font_names of string
    | Font_style of obj
    | ForceEnterMode of bool
    | ForceSimpleAmpersand of bool
    | Format_address of obj
    | Format_div of obj
    | Format_h1 of obj
    | Format_h2 of obj
    | Format_h3 of obj
    | Format_h4 of obj
    | Format_h5 of obj
    | Format_h6 of obj
    | Format_p of obj
    | Format_pre of obj
    | Format_tags of string
    | FullPage of bool
    | Grayt_autoStartup of bool
    | HtmlEncodeOutput of bool
    | IgnoreEmptyParagraph of bool
    | Image2_alignClasses of string list
    | Image2_altRequired of bool
    | Image2_captionedClass of string
    | Image2_disableResizer of bool
    | Image2_maxSize of obj // TODO!
    | Image2_prefillDimensions of bool
    | ImageUploadUrl of string
    | Image_prefillDimensions of bool
    | Image_previewText of string
    | Image_removeLinkByEmptyURL of bool
    | IndentClasses of string list
    | IndentOffset of int
    | IndentUnit of string
    | JqueryOverrideVal of bool
    | JustifyClasses of string list
    | Keystrokes of string list // TODO should be typed as list of Key strokes
    | Language of string
    | Language_list of string list
    | LinkJavaScriptLinksAllowed of bool
    | LinkPhoneMsg of string
    | LinkPhoneRegExp of Regex
    | LinkShowAdvancedTab of bool
    | LinkShowTargetTab of bool
    | Magicline_color of string
    | Magicline_everywhere of bool
    | Magicline_holdDistance of int
    | Magicline_keystrokeNext of int
    | Magicline_keystrokePrevious of int
    | Magicline_tabuList of int
    | Magicline_triggerOffset of int
    | MathJaxClass of string
    | MathJaxLib of string
    | Menu_groups of string
    | Menu_subMenuDelay of int
    | Newpage_html of string
    | Notification_duration of int
    | On of obj
    | PasteFilter of string
    | PasteFromWordCleanupFile of string
    | PasteFromWordNumberedHeadingToList of bool
    | PasteFromWordPromptCleanup of bool
    | PasteFromWordRemoveStyles of bool
    | PasteFromWord_heuristicsEdgeList of bool
    | PasteFromWord_inlineImages of bool
    | PasteFromWord_keepZeroMargins of bool
    | Plugins of string list
    | ProtectedSource of Regex list
    | ReadOnly of bool
    | RemoveButtons of string
    | RemoveDialogTabs of string
    | RemoveFormatAttributes of string
    | RemoveFormatTags of string
    | RemovePlugins of string list
    | Resize_dir of string
    | Resize_enabled of bool
    | Resize_maxHeight of int
    | Resize_maxWidth of int
    | Resize_minHeight of int
    | Resize_minWidth of int
    | Scayt_autoStartup of bool
    | Scayt_cacheSize of int
    | Scayt_contextCommands of string
    | Scayt_contextMenuItemsOrder of string
    | Scayt_customDictionaryIds of string
    | Scayt_customPunctuation of string
    | Scayt_customerId of string
    | Scayt_disableCache of bool
    | Scayt_disableOptionsStorage of ScaytStoreOptions list
    | Scayt_elementsToIgnore of string
    | Scayt_handleCheckDirty of string
    | Scayt_handleUndoRedo of string
    | Scayt_ignoreAllCapsWords of bool
    | Scayt_ignoreDomainNames of bool
    | Scayt_ignoreWordsWithMixedCases of bool
    | Scayt_ignoreWordsWithNumbers of bool
    | Scayt_inlineModeImmediateMarkup of bool
    | Scayt_maxSuggestions of int
    | Scayt_minWordLength of int
    | Scayt_moreSuggestions of string
    | Scayt_multiLanguageMode of bool
    | Scayt_multiLanguageStyles of obj // TODO improve
    | Scayt_sLang of string
    | Scayt_serviceHost of string
    | Scayt_servicePath of string
    | Scayt_servicePort of string
    | Scayt_serviceProtocol of string
    | Scayt_srcUrl of string
    | Scayt_uiTabs of string
    | Scayt_userDictionaryName of string
    | SharedSpaces of obj // TODO improve
    | ShiftEnterMode of int
    | Skin of string
    | Smiley_columns of int
    | Smiley_descriptions of string list
    | Smiley_images of string list
    | Smiley_path of string
    | SourceAreaTabSize of int
    | SpecialChars of string list
    | StartupFocus of StartupFocus
    | StartupMode of string
    | StartupOutlineBlocks of bool
    | StartupShowBorders of bool
    | StylesheetParser_skipSelectors of Regex
    | StylesheetParser_validSelectors of Regex
    | TabIndex of int
    | TabSpaces of int
    | Templates of string
    | Templates_files of string list
    | Templates_replaceContent of bool
    | Title of string
    | ToolbarCanCollapse of bool
    | ToolbarGroupCycling of bool
    | ToolbarGroups of obj list // TODO improve
    | ToolbarLocation of string
    | ToolbarStartupExpanded of bool
    | UiColor of string
    | UndoStackSize of int
    | UploadUrl of string
    | UseComputedState of bool
    | Wsc_cmd of string
    | Wsc_customDictionaryIds of string
    | Wsc_customLoaderScript of string
    | Wsc_customerId of string
    | Wsc_height of string
    | Wsc_lang of string
    | Wsc_left of string
    | Wsc_top of string
    | Wsc_userDictionaryName of string
    | Wsc_width of string
    | Easyimage_styles of obj // TODO improve

    static member AllowedContent (allowedContent: AllowedContent) =
        match allowedContent with
        | Rules rules -> unbox ("allowedContent", rules)
        | NoFilter -> unbox ("allowedContent", true)

    static member AutoEmbed_widget (autoEmbedWidget: AutoEmbedWidget) =
        match autoEmbedWidget with
        | WidgetNames names -> unbox ("autoEmbed_widget", String.concat "," names)
        | Callback f -> unbox ("autoEmbed_widget", f)

    static member CopyFormatting_allowedContexts (copyFormattingAllowedContexts: CopyFormattingAllowedContexts) =
        match copyFormattingAllowedContexts with
        | Contexts contexts -> unbox ("copyFormatting_allowedContexts", contexts)
        | All -> unbox ("copyFormatting_allowedContexts", true)

    static member Easyimage_class (easyImageClass: string option) =
        match easyImageClass with
        | Some str -> unbox ("easyimage_class", true)
        | None -> unbox ("easyimage_class", null)

    static member Easyimage_defaultStyle (style: string option) =
        match style with
        | Some str -> unbox ("easyimage_defaultStyle", true)
        | None -> unbox ("easyimage_defaultStyle", null)

    static member Entities_processNumerical (processNumericalOption: ProcessNumericalOption) =
        match processNumericalOption with
        | Force -> unbox ("entities_processNumerical", "force")
        | Remaining -> unbox ("entities_processNumerical", "true")
        | Nothing -> unbox ("entities_processNumerical", "false")

    static member FilebrowserWindowHeight height = unboxPixelsOrPercentage "filebrowserWindowHeight" height

    static member FilebrowserWindowWidth width = unboxPixelsOrPercentage "filebrowserWindowWidth" width

    static member ForcePasteAsPlainText (option: ForcePasteAsPlainTextOption) =
        match option with
        | PlainText -> unbox ("forcePasteAsPlainText", true)
        | PreserveFormatting -> unbox ("forcePasteAsPlainText", false)
        | AllowWord -> unbox ("forcePasteAsPlainText", "allow-word")

    static member Height height = unboxPixelsOrPercentage "height" height

    static member Width width = unboxPixelsOrPercentage "width" width

    static member Mentions (mentions: MentionsConfigDefinition list) = unbox ("mentions", keyValueList CaseRules.LowerFirst mentions)

    static member StylesSet (option: StyleSetOption) =
        match option with
        | NoStyles -> unbox ("stylesSet", false)
        | ByRegisteredName name -> unbox ("stylesSet", name)
        | ByNameWithUrl (name, url) -> unbox ("stylesSet", name + ":" + url)
        | StyleSetOption.ByDefinitions definitions -> unbox ("stylesSet", definitions)

    static member Toolbar (option: TooBarOption) =
        match option with
        | ByName name -> unbox ("toolbar", name)
        | ByDefinitions definitions -> unbox ("toolbar", definitions)

type ChangeEvent = {
    Data: obj
    Editor: obj // TODO type editor: https://ckeditor.com/docs/ckeditor4/latest/api/CKEDITOR_editor.html
    ListenerData: obj
    Name: string
    Sender: obj
}

type CKEditorProps =
    | Data of string
    | EditorUrl of string
    | Type of EditorType
    | ReadOnly of bool
    | OnChange of (obj -> unit) // CKEditor passes a huge event with all sorts of data and methods // TODO provide a type definition for the event?
    static member Config (editorConfig: EditorConfig list) : CKEditorProps = unbox ("config", keyValueList CaseRules.LowerFirst editorConfig)
    interface IHTMLProp

let inline CKEditor (props: IHTMLProp seq) : ReactElement =
    ofImport "default" "ckeditor4-react" (keyValueList CaseRules.LowerFirst props) []
