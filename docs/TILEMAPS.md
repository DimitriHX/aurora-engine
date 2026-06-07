# Tilemaps de Tiled

Aurora carga mapas exportados por Tiled en formato JSON mediante
`TileMapManager`.

## Requisitos actuales

- Mapa ortogonal y finito.
- Tiles cuadrados.
- Capas de tiles sin compresion.
- Cada capa debe tener el mismo ancho y alto que el mapa.
- La imagen de cada tileset debe estar compilada por MonoGame.
- Los mapas infinitos, isometricos y hexagonales aun no estan soportados.

## Agregar un mapa

1. Exportar el mapa desde Tiled como JSON.
2. Agregar el JSON a `Content/Maps`.
3. Agregar las imagenes del tileset a `Content/Textures` y a
   `Content.mgcb`.
4. Registrar el mapa y asociar cada `source` de Tiled con su asset:

```csharp
TileMapResource world = tileMapManager.Add(
    "world",
    new TileMapDefinition(
        "Content/Maps/world.json",
        new Dictionary<string, string>
        {
            ["../tileset.tsx"] = "Textures/tileset"
        }
    )
    {
        CollisionLayer = "Collision"
    }
);
```

`TileMapManager` conserva el mapa en cache. Se puede recuperar con
`Get("world")`, eliminar con `Remove("world")` o limpiar todos los mapas con
`Clear()`.

La capa configurada como `CollisionLayer` no se renderiza. Cualquier celda no
vacia de esa capa se registra como solida.
