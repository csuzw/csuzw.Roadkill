# csuzw.Roadkill

My [Roadkill wiki](http://www.roadkillwiki.net/) plugins

## GithubExtensions

GitHub flavored Markdown extensions for Roadkill

Adds Code Blocks (uses built in Syntax Highlighting plugin for styling), Tables and Strikethrough syntax from GitHub flavored Markdown

## TagTreeMenu

Creates simple menu pages based on provided tag tree.  Usage: `[[[ttm=Tag1!Tag7(Tag2&Tag4(Tag3),Tag4(Tag5|Tag6))]]]`

Valid symbols are `(` (open sub group), `)` (close sub group), `&` (logical AND), `|` (logical OR), `!` (null: ignored for all cases but contributes to full match), `,` (new branch) and tag names.  White space is ignored.  

Only pages whose tags match exactly will be displayed.  For example if a page has the following tags: `Alpha`, `Beta`, `Gamma` then the page would be displayed with the following definitions:
* `Alpha|Beta|Gamma|Delta`
* `Alpha&Beta&Gamma`
* `Alpha(Beta|Delta(Gamma))`: Only the final `Gamma` would match the page
* `Alpha!Beta!Gamma!Delta`: No check is done on `Beta`, `Gamma`, or `Delta` but they are included when checking if all tags are matched.

The following would not match the page:
* `Alpha|Delta`: `Beta` and `Gamma` aren't matched
* `Alpha,Beta(Gamma)`: `Alpha` is in a different branch to `Beta` and `Gamma` so there is no complete match

## PluginTester

Simple Windows forms application for testing plugins.
