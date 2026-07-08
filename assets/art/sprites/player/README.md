# Sprites do Arandu

## Atual

| Arquivo | Uso | Frames |
|---------|-----|--------|
| `arandu_idle_sheet.png` | idle vivo | 8 frames, 256x256 |
| `arandu_walk_sheet.png` | caminhada | 8 frames, 256x256 |
| `arandu_run_sheet.png` | corrida | 8 frames, 256x256 |
| `arandu_attack_light_sheet.png` | ataque leve | 8 frames, 256x256 |
| `arandu_attack_heavy_sheet.png` | ataque forte | 8 frames, 256x256 |
| `arandu_bow_sheet.png` | arco (`R`) | 8 frames, 256x256 |
| `arandu_hit_sheet.png` | dano | 8 frames, 256x256 |
| `arandu_death_sheet.png` | morte | 8 frames, 256x256 |

Sheets em `incoming/` nao entram no jogo automaticamente.

## Regra da corrida

O PNG gerado como faixa horizontal, com o personagem andando pelo canvas, nao funciona direto no beat em up.
Ele precisa virar um sheet in-place:

- 8 celulas fixas de 256x256;
- personagem dentro de cada celula;
- pes no mesmo baseline;
- corpo com altura consistente;
- bordas esquerda/direita da celula com 8-10px transparentes;
- nada de frame vazio, cortado pela divisao de 256px ou menor que os demais.

O `arandu_run_sheet.png` atual foi corrigido manualmente a partir do arquivo recebido pelo usuario:

- o original tinha 2048x256, mas so 7 poses completas;
- varios desenhos atravessavam a divisao das celulas;
- o ultimo frame tinha apenas um pedaco do personagem;
- a versao integrada recorta por silhueta, fixa baseline e duplica uma pose intermediaria para fechar 8 frames sem sumir.
- a versao final remove componentes soltos e limpa gutters transparentes para evitar ponta fantasma do tacape com filtro linear.

## Regras

- Manter fundo transparente.
- Manter 8 frames em linha horizontal para ciclos base.
- Usar estes sheets aprovados como referencia de paleta, proporcao, cabelo, arma, tecido e postura.
- Nao gerar run/ataque/hit/death com outro corpo ou outra paleta.
- Se o tamanho do frame mudar, atualizar `AranduSpriteArt.cs`.
- Sheets recebidos com canvas irregular devem ser normalizados para 8 frames de 256x256 antes da integracao.
- Nao usar crop fixo por largura quando o canvas vier irregular; recortar por silhueta e revisar visualmente para nao cortar tacape, bracos ou pes.
- Apos integrar sheet novo, seguir pipeline em `docs/sprints/sprint_26_arandu_sprites_combate.md`.
- O scale externo do Arandu fica em `ExternalSpriteSheetArt.DefaultExternalScale` (0.30); corrida ganha +5% em `PlayerController`.

## Integrados no codigo

- `AranduSpriteArt.cs` carrega todos os sheets externos quando existem.
- `SpriteCharacterAnimator.cs` toca `run` quando o player esta em estado de corrida.
- Se `arandu_run_sheet.png` nao existir, o jogo cai para walk acelerado em vez de quebrar.

## Prompt base

```text
2D side-view beat em up game sprite sheet, dark brazilian mythic setting,
1990s arcade proportions, modern painterly finish (NOT chibi, NOT 3D),
transparent PNG background, 8 animation frames in one horizontal row,
each frame exactly 256 pixels wide and 256 pixels tall,
total image size 2048x256 pixels,
character must match the scale and camera angle of the attached reference warrior,
high silhouette readability at small size, dirty worn look, no text, no watermark
```

Anexe `arandu_walk_sheet.png` como referencia de escala em todas as geracoes.

## Prompt - arandu_run_sheet

```text
[Prompt base acima]

CHARACTER: Arandu, indigenous warrior guardian of the flooded forest,
same exact design as the attached walk sheet - identical face, hair, body paint,
loincloth, tacape axe, proportions and palette.

ANIMATION: run cycle 8 frames, faster aggressive sprint, body leaning forward,
tacape held ready, feet kicking mud, cloth trailing.

CRITICAL: in-place animation, character centered inside each 256x256 frame,
same foot baseline in every frame, no walking across the canvas, no changing scale,
full tacape visible in every frame with at least 22px transparent margin on all sides.

Side view facing RIGHT. Same scale as reference walk sheet.
```
