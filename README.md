<p align="center">
<img src="https://github.com/DCFApixels/Unity-Notes/assets/99481254/d6e83712-be0b-4d74-9fce-b78872f32434" >
</p>

<p align="center">
<img alt="Version" src="https://img.shields.io/github/package-json/v/DCFApixels/Unity-Notes?color=%23FFC200&style=for-the-badge">
<img alt="License" src="https://img.shields.io/github/license/DCFApixels/Unity-Notes?color=FFC200&style=for-the-badge">
</p>

# Notes for Unity Editor

<table>
  <tr></tr>
  <tr>
    <td colspan="3">Readme Languages:</td>
  </tr>
  <tr></tr>
  <tr>
    <td nowrap width="100">
      <a href="https://github.com/DCFApixels/Unity-Notes/blob/main/README-RU.md">
        <img src="https://github.com/user-attachments/assets/7bc29394-46d6-44a3-bace-0a3bae65d755"></br>
        <span>Русский</span>
      </a>  
    </td>
    <td nowrap width="100">
      <a href="https://github.com/DCFApixels/Unity-Notes">
        <img src="https://github.com/user-attachments/assets/3c699094-f8e6-471d-a7c1-6d2e9530e721"></br>
        <span>English</span>
      </a>  
    </td>
  </tr>
</table>


<p align="center">
<img src="https://github.com/DCFApixels/Unity-Notes/assets/99481254/e8e3e6a9-9d35-48db-b786-45554fa3e08e" width="600" >
<br>
Notes on Scene View for designers.
</p>

## Installation
Versioning semantics - [Open](https://gist.github.com/DCFApixels/e53281d4628b19fe5278f3e77a7da9e8#file-dcfapixels_versioning_ru-md)
* ### Unity-module
Add the git-URL to the project [using PackageManager](https://docs.unity3d.com/2023.2/Documentation/Manual/upm-ui-giturl.html) or directly to the `Packages/manifest.json` file. Copy this git-URL: 
```
https://github.com/DCFApixels/Unity-Notes.git
```
* ### Using source code
Can install by copying the sources into the project.

<br>

## How to use
Just add the `Note` or `LazyNote` component to any GameObject. Or use RMB + "GameObject/Notes/Create Note"(or LazyNote) to automatically create a note object.

To add Author or Note Type presets, click on the ![gear](https://github.com/DCFApixels/Unity-Notes/assets/99481254/0d0efe29-6f54-44d1-a8a6-90f895e101ee) icon in the Note component.

To display text in the Scene View window, use the separator ">-<" all text before it will be displayed

Intended for use in the editor only, all note data will be removed in the Release build. 
