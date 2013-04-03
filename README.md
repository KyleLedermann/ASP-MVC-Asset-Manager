# Asset Manager

This asset manager package is to be integrated with your ASP.NET MVC Applications. This asset manager is a derived work of [https://github.com/speier/mvcassetshelper](https://github.com/speier/mvcassetshelper).

This package is a working demonstration and can be used as such.

## Usage

### AssetConfig

Remember when integrating the AssetConfig with your MVC Application to rename the namespace within the `~/App_Start/AssetConfig.cs` class file to match yours.

### Initializing the Asset Instance

Within the Global.asax's Application_Start() method, add after the registration of bundles (which can be commented out, if you so choose to use this as a replacement) by adding:

`AssetConfig.RegisterAssets(AssetHelper.Instance);`

### In Views

New assets can added within the views so that view-specific assets can be rendered at final response. This can be achieved by using: `@{ Html.Assets() ... }`.