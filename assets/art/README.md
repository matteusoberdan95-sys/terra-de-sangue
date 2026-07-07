# Arte - Terra Sangrada

Este README define o padrao visual da Aldeia em Cinzas. Siga isto ao trocar entre Codex, Cursor e editor Godot.

## Padrao que funcionou

O cenario ficou bom com poucos assets porque usamos a pintura como ela foi gerada:

1. **`aldeia_mid.png`** = imagem-mestra opaca da fase inteira.
2. **`aldeia_fg.png`** = primeiro plano com transparencia no centro.
3. **`AldeiaBackground.tscn`** = onde posicionar e escalar os sprites.
4. **`AldeiaParallaxBackground.cs`** = apenas parallax horizontal e vida ambiente leve.
5. **`AldeiaEmCinzasArena.cs`** = gameplay: camera, faixa de andar, spawn do player.

Essa combinacao evita emendas, evita personagem andando no ceu e evita o fundo virar uma colagem quebrada.

## Ajuste visual no Godot

1. Abra **`scenes/levels/AldeiaBackground.tscn`** no editor.
2. Rode o jogo com F5 em `scenes/Main.tscn` e compare com a cena aberta.
3. Mova **`AldeiaBackdrop`** ate os pes do Arandu coincidirem com a faixa de chao.
4. Use **`WalkBandGuide`** como guia dos pes no editor.
5. Use **`HorizonGuide`** como guia da base das cabanas.
6. Ajuste **`AldeiaForeground`** se precisar.
7. Salve a cena.

As guias sao para edicao. Elas nao devem aparecer no jogo rodando.

## Como a vida ambiente foi feita

Arquivo: `src/Prototype/AldeiaParallaxBackground.cs`

O script cria overlays pequenos por cima do PNG mestre:

- chamas em janelas/ruinas: `assets/art/vfx/aldeia_fire_sheet.png`;
- fumaca transparente: `assets/art/vfx/aldeia_smoke_sheet.png`;
- brasas subindo: `assets/art/vfx/aldeia_embers_sheet.png`;
- oscilacao sutil do foreground;
- parallax horizontal suave.

Se esses spritesheets existirem, o jogo usa os assets. Se nao existirem, o script cai no fallback procedural simples. Isso da movimento sem destruir a composicao da arte. Nao transformar isso em fundo procedural completo.

Importante: os VFX ficam dentro de `AmbientLife`, que acompanha o mesmo parallax horizontal do `AldeiaBackdrop`. Nao ancorar fogo/fumaca em coordenada fixa do mundo, senao a camera anda, o fundo desliza e o VFX parece solto em lugar aleatorio.

## O que NAO fazer

**Nao empilhar no codigo varios PNGs opacos da mesma cena** (`sky` + `mid` + `ground`).

Motivo: cada PNG opaco cobre o anterior ou exige recortes. Recorte gera linha, emenda, chao errado e personagem flutuando.

Tambem nao fazer:

- `RegionRect` automatico para montar cenario;
- fundo procedural C# substituindo a arte;
- constantes magicas de posicao no chat;
- `BackDepthLimit` / `FrontDepthLimit` visiveis quando PNG estiver ativo;
- usar `aldeia_ground.png` atual como chao lateral, porque ele esta em vista de cima.

## Opcao A - Recomendada agora

Use somente:

```text
assets/art/aldeia_mid.png
assets/art/aldeia_fg.png
```

O resultado deve parecer uma cena unica, pintada, com movimento leve por cima.

## Opcao B - Parallax real depois

So funciona se cada arquivo tiver transparencia real onde nao existe pixel daquele plano:

| Arquivo | Conteudo | Resto do PNG |
|---------|----------|--------------|
| `aldeia_sky.png` | Ceu, nuvens, fumaca distante | Transparente |
| `aldeia_mid.png` | Cabanas, arvores, fogo | Transparente fora do plano |
| `aldeia_ground.png` | Chao lateral da faixa de andar | Transparente |
| `aldeia_fg.png` | Primeiro plano nas bordas | Centro transparente |

Regra simples: se o arquivo e opaco, ele e um fundo inteiro, nao uma camada.

## Onde colocar arquivos

```text
assets/art/
  aldeia_mid.png      # imagem-mestra obrigatoria
  aldeia_fg.png       # foreground transparente opcional
  aldeia_sky.png      # reservado para parallax real futuro
  aldeia_ground.png   # reservado para parallax real futuro
  vfx/
    aldeia_fire_sheet.png
    aldeia_embers_sheet.png
    aldeia_smoke_sheet.png
  concept/
  sprites/player/
  sprites/enemies/
```

## Exportacao

- Formato: PNG.
- Personagens: fundo transparente.
- Fundo mestre: pode ser opaco.
- Camadas parallax: transparencia obrigatoria entre planos.
- Largura: 960px+; ideal perto de 2172px para scroll.
- Filtro no Godot: Linear para pintura suave.

Ver `docs/11_guia_krita_beat_em_up.md`.
