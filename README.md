<p align="center">
<img src="https://github.com/DCFApixels/Notes-Unity/assets/99481254/d6e83712-be0b-4d74-9fce-b78872f32434" >
</p>

<p align="center">
<img alt="Version" src="https://img.shields.io/github/package-json/v/DCFApixels/Notes-Unity?color=%23FFC200&style=for-the-badge">
<img alt="License" src="https://img.shields.io/github/license/DCFApixels/Notes-Unity?color=FFC200&style=for-the-badge">
</p>

# Notes for Unity Editor
Notes on Scene View for designers.
<p align="center">
<img src="https://github.com/DCFApixels/Notes-Unity/assets/99481254/e8e3e6a9-9d35-48db-b786-45554fa3e08e" width="600" >
</p>

## Installation

* ### Unity-module
Add the git-URL to the project [using PackageManager](https://docs.unity3d.com/2023.2/Documentation/Manual/upm-ui-giturl.html) or directly to the `Packages/manifest.json` file. Copy this git-URL: 
```
https://github.com/DCFApixels/Notes-Unity.git
```
* ### Using source code
Can install by copying the sources into the project.

### Versioning
Versioning semantics: [Open](https://gist.github.com/DCFApixels/c3b178a308b411f530361d1d56f1f929#versioning)


## How to use
Just add the "Note" or "LazyNote" component to any object. Or use RMB + "GameObject/Notes/Create Note"(or LazyNote) to automatically create a note object.

To add Author or Note Type presets, click on the ![gear](https://github.com/DCFApixels/Notes-Unity/assets/99481254/0d0efe29-6f54-44d1-a8a6-90f895e101ee) icon in the Note component.

To display text in the Scene View window, use the separator ">-<" all text before it will be displayed

Intended for use in the editor only, all note data will be removed in the Release build. 
