<p align="center">
<img src="https://github.com/DCFApixels/Unity-Notes/assets/99481254/d6e83712-be0b-4d74-9fce-b78872f32434" >
</p>

<p align="center">
<img alt="Version" src="https://img.shields.io/github/package-json/v/DCFApixels/Unity-Notes?color=%23FFC200&style=for-the-badge">
<img alt="License" src="https://img.shields.io/github/license/DCFApixels/Unity-Notes?color=FFC200&style=for-the-badge">
</p>

# Заметки для Редактора Unity

<table>
  <tr></tr>
  <tr>
    <td colspan="3">Readme Languages:</td>
  </tr>
  <tr></tr>
  <tr>
    <td nowrap width="100">
      <a href="https://github.com/DCFApixels/Unity-Notes/blob/main/README-RU.md">
        <img src="https://github.com/user-attachments/assets/3c699094-f8e6-471d-a7c1-6d2e9530e721"></br>
        <span>Русский</span>
      </a>  
    </td>
    <td nowrap width="100">
      <a href="https://github.com/DCFApixels/Unity-Notes">
        <img src="https://github.com/user-attachments/assets/30528cb5-f38e-49f0-b23e-d001844ae930"></br>
        <span>English</span>
      </a>  
    </td>
  </tr>
</table>

<br>


<p align="center">
<img src="https://github.com/user-attachments/assets/e6a33020-e847-4cca-b1ca-14ebcaa03156" width="600" >
<br>
Заметки в Scene View для дизайнеров. 
</p>

## Установка
Семантика версионирования - [Открыть](https://gist.github.com/DCFApixels/e53281d4628b19fe5278f3e77a7da9e8#file-dcfapixels_versioning_ru-md)

* ### Unity-модуль
Добавьте git-URL в [PackageManager](https://docs.unity3d.com/2023.2/Documentation/Manual/upm-ui-giturl.html) или вручную в `Packages/manifest.json` файл. Используйте этот git-URL: 
```
https://github.com/DCFApixels/Unity-Notes.git
```
* ### В виде исходников
Можно установит просто скопировав исходники в папку проекта

<br>

## Как использовать
Просто добавьте компонент `Note` или `LazyNote` на любой GameObject. Либо используйте ПКМ + "GameObject/Notes/Create Note"(или LazyNote) для автоматического создания объекта с заметкой.

Чтобы добавить пресеты Авторов или Типов Заметок нажмите на иконку ![gear](https://github.com/DCFApixels/Unity-Notes/assets/99481254/0d0efe29-6f54-44d1-a8a6-90f895e101ee) в компоненте заметки.

Чтобы отобразить текст заметки в Scene View, используйте разделитель ">-<", весь текст перед разделителем будет отображен.

Заметки предназначены для использования только в редакторе, все данные из заметок будут удалены в Release сборке. 
