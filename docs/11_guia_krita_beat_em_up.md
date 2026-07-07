# Guia Krita — Arte 2D Beat 'em Up

## Resposta curta

**Sim** — a ideia certa e desenhar no **Krita** (ou similar), exportar **PNG** com fundo transparente para personagens e **camadas separadas** para cenario, e montar tudo no Godot.

O que voce viu ate agora (poligonos coloridos e pixels gerados em codigo C#) e **placeholder de programador**, nao arte final. Serveu para provar combate e fases. **Nao e o visual do jogo.**

## O que e arte final vs placeholder

| Placeholder (hoje) | Arte final (Krita) |
|--------------------|--------------------|
| Poligonos na cena | Fundos pintados em camadas |
| `AranduSpriteArt.cs` gerando pixels | Sprite sheet desenhado frame a frame |
| Rapido para programar | Mais lento, mas e o que o jogador ve |

Os sprites pixel em C# (Arandu, mercenario) sao um meio-termo: melhor que massinha, mas ainda **provisorios** ate voce exportar do Krita.

## Pipeline recomendado (Sprint 25 revisada)

1. **Conceito** — 1 imagem da Aldeia em Cinzas (composicao, lua, cabanas).
2. **Fundos em camadas** — exportar PNG separados:
   - `aldeia_sky.png` — ceu + lua (largo, ~960px ou mais para parallax)
   - `aldeia_mid.png` — cabanas, arvores distantes
   - `aldeia_ground.png` — chao da faixa jogavel
   - `aldeia_fg.png` — troncos / primeiro plano (opcional)
3. **Personagem** — sprite sheet do Arandu:
   - idle, walk (4 frames), attack light, attack heavy, hit
   - Altura alvo: **48–64 px** na resolucao 480x270 (com zoom 2x na camera)
   - Fundo **transparente**
4. **Import no Godot** — colocar em `assets/art/` (ver README la).
5. **Integracao** — trocar `SpriteFrames` gerados em codigo por `SpriteFrames` do editor apontando para os PNGs.

## Configuracao Krita (sugestao)

- Documento: **480 x 270** para teste de composicao OU **960 x 270** para fundo parallax.
- DPI: 72 (pixel art / pintura digital para tela).
- Vista: **lateral** (beat 'em up), nunca isometrico.
- Paleta: usar `docs/01_biblia_de_arte.md` (preto quente, barro, sangue, ouro queimado, verde mata).

## Prompt de direcao (para voce ou referencia)

```text
dark brazilian mythic 2D beat em up, 1990s arcade side view,
painterly pixel finish, burned village at night, blood moon,
muddy ground, ember glow, high silhouette readability,
no text in image
```

## Ordem de producao sugerida

1. **Uma tela estatica** da Aldeia (proof of look) — validar com voce antes de animar tudo.
2. **Camadas de fundo** exportadas.
3. **Arandu idle + walk** (6–8 frames total).
4. **Mercenario** (mesmo padrao).
5. Integrar no Godot e jogar a fase 1.

## O jogo vai ficar feio?

**Nao**, se seguirmos o pipeline acima. Hoje parece massinha porque:

- 24 sprints focaram em **gameplay**, nao em arte Krita.
- A tentativa de fundo procedural na Sprint 25 foi um desvio — **revertida**.

O combate ja esta bom; falta **vestir** com arte desenhada. Esse e o momento certo para o Krita.

## Proximo passo pratico

1. Ajustar **`scenes/levels/AldeiaBackground.tscn`** no Godot (ver `assets/art/README.md`).
2. Proximo ganho visual: **sprite sheet do Arandu** (idle/walk).
3. Se quiser parallax 4 camadas: regerar PNGs com **transparencia** (Opcao B no README).

## Cenario no Godot (obrigatorio)

Posicao e escala dos fundos ficam na cena **`AldeiaBackground.tscn`**, nao em constantes C#.
O script `AldeiaParallaxBackground.cs` so move as camadas no eixo X (parallax).

## Referencias

- `docs/01_biblia_de_arte.md`
- `docs/05_pipeline_de_assets.md`
- `assets/art/README.md`
