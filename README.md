# unity-gamejam-utilities
Tiny set of utilities the help get started with gamejams.

This is what you should expect from it.
 - It should be easy to use from an empty Unity project
 - The code is not especially efficient
 - Ok in the short term, but risky in the long term (very few safety checks, usage of MonoBehaviours and Updates, etc)

The project also serves as a test of how to bundle a Unity package to be imported via the Package Manager.

## Importing via GIt URL

Available starting from Unity 2018.3.

Just add this line to the `Packages/manifest.json` file of your Unity Project:

```json
"dependencies": {
    "lumley.unitygamejamutilities": "https://github.com/lumley/unity-gamejam-utilities.git",
}
```

If you want to use a specific [release](https://github.com/lumley/unity-gamejam-utilities/releases) in your code, just add `#release` at the end, like so:
```json
"dependencies": {
    "lumley.unitygamejamutilities": "https://github.com/lumley/unity-gamejam-utilities.git#0.1.3",
}
```