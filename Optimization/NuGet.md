# NuGet Packaging

NuGet package configuration is in `Optimization.nuspec`.

## Release Preparation

Edit `Optimization.nuspec` for the new version number and issue

```cmd
nuget pack -Symbols
```

or use the version number `x.y.z` as a parameter

```cmd
nuget pack -Symbols -Version x.y.z
```

Two files will be created: `widemeadows.Optimization.VERSION.nupkg` and `widemeadows.Optimization.VERSION.symbol.nupkg`.

## Pushing to NuGet and SymbolSource

```cmd
nuget push widemeadows.Optimization.VERSION.nupkg
```

Using the above command, the regular package file will be pushed to [nuget.org](https://www.nuget.org), while the symbol file will be pushed to [symbolsource.org](https://www.symbolsource.org).