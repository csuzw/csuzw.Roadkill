# csuzw.Roadkill

My [Roadkill wiki](http://www.roadkillwiki.net/) plugins

## GithubExtensions

GitHub flavored Markdown extensions for Roadkill

Adds Code Blocks (uses built in Syntax Highlighting plugin for styling), Tables and Strikethrough syntax from GitHub flavored Markdown

## TagTreeMenu

Creates simple menu structures based on provided tag tree.  Usage: `{menu=Tag1~A*(TAG2&tag4(Tag1?),Tag4(Tag5|Tag6))}`

The output for this would look like (where the links are matching pages):

* [A Page Link]()
	* **TAG2, tag4**
	* [Another Page Link]()
		* **Tag1?**
		* [Yet Another Page Link]()
	* **Tag4**
	* [Guess what? A Link!]()
		* **Tag5, Tag6**
		* [Have another link]()
		* [And another]()
		* [One more...]()

Valid symbols are `(` (open sub group), `)` (close sub group), `&` (logical AND), `|` (logical OR), `~` (null: ignored for all cases but contributes to full match), `,` (new branch) and tag names which can contain the characters `a-zA-Z0-9_?*`.  White space is ignored.  When matching tags case is ignored and `?` (single character) and `*` (any number of characters) are wildcards. 

Only pages whose tags match exactly will be displayed.  For example if a page has the following tags: `Alpha`, `Beta`, `Gamma` then the page would be displayed with the following definitions:
* `*`: Wildcard will match everything
* `alpha&BETA&GaMmA`: Case is ignored
* `Alpha|Beta|Gamma|Delta`
* `Alpha&Beta&Gamma`
* `Alpha(Beta|Delta(Gamma))`: Only the final `Gamma` would match the page
* `Alpha~Beta~Gamma~Delta`: No check is done on `Beta`, `Gamma`, or `Delta` but they are included when checking if all tags are matched.

The following would not match the page:
* `Alpha|Delta`: `Beta` and `Gamma` aren't matched
* `Alpha,Beta(Gamma)`: `Alpha` is in a different branch to `Beta` and `Gamma` so there is no complete match.

## PageReferences

Generates links for all pages that reference current page.  Usage: `{PageReference=Example-Page}`

Note that this currently only searches pages for links in the form `[Any Description](Example-Page)`.  One of the consequences of this is that pages that use plugins that generate links like TagTreeMenu won't be found.

I have no idea how performant this is on larger wiki's either!

## PluginTester

Simple Windows forms application for testing plugins.
