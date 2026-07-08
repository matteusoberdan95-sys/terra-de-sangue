# Sprint 26 - Arandu Sprites, Combos e Arco

Ultima atualizacao: 2026-07-07

## Objetivo

Integrar nova leva de sprite sheets do Arandu (IA), corrigir escala/tacape cortado, combos de combate e animacao de arco.

## Entregas

### Arte integrada (`assets/art/sprites/player/`)

| Arquivo | Uso |
|---------|-----|
| `arandu_walk_sheet.png` | Referencia de escala + caminhada |
| `arandu_idle_sheet.png` | Idle |
| `arandu_run_sheet.png` | Corrida |
| `arandu_attack_light_sheet.png` | Combo leve `J` x3 |
| `arandu_attack_heavy_sheet.png` | Combo pesado `K` x2 |
| `arandu_bow_sheet.png` | Arco (`R`) |
| `arandu_hit_sheet.png` | Dano |
| `arandu_death_sheet.png` | Morte |

Originais recebidos em `assets/art/sprites/enemies/incoming/` (tamanhos irregulares). Nao commitar essa pasta como fonte final.

### Pipeline de arte (`tools/`)

1. `normalize_sprite_sheet.py` — remove fundo preto, divide em 8 frames, compoe 2048x256
2. `fit_sheet_safe_area.py` — margem segura por frame (evita corte nas bordas)
3. `upscale_sheet_to_walk.py` — alinha altura ao walk de referencia (corrige run pequeno)
4. `repack_sheet_to_walk_scale.py` — reempacota sheet irregular usando walk como escala

**Fluxo recomendado para sheet novo:**

```bash
# 1. Normalizar (se nao vier 2048x256)
py -3 tools/normalize_sprite_sheet.py entrada.png
mv entrada_normalized.png assets/art/sprites/player/nome_sheet.png

# 2. Alinhar escala ao walk (run, ataque, etc.)
py -3 tools/upscale_sheet_to_walk.py assets/art/sprites/player/arandu_walk_sheet.png \
  assets/art/sprites/player/nome_sheet.png assets/art/sprites/player/nome_sheet.png

# 3. Margem extra opcional
py -3 tools/fit_sheet_safe_area.py assets/art/sprites/player/nome_sheet.png
```

Para **ataque leve** com margem forcada:

```bash
py -3 tools/upscale_sheet_to_walk.py walk.png ataque_norm.png saida.png 4 1 0.92
# args: margem_esquerda_extra, enforce_margin=1, height_multiplier=0.92
```

### Combate (`PlayerController.cs`)

- **Combo leve `J J J`:** 3 golpes com janela ~0,42s
- **Combo pesado `K K`:** 2 golpes com janela ~0,38s
- **Finisher `J J K`:** mantido
- Removido placeholder visual `AttackSlash` (seta vermelha de ataque)

### Animacoes (`AranduSpriteArt.cs` + `SpriteCharacterAnimator.cs`)

| Animacao | Frames do sheet |
|----------|-----------------|
| `attack_light_1` | 0-1 |
| `attack_light_2` | 2, 4 (pula frame 3 — arte quebrada) |
| `attack_light_3` | 5, 6, 4 |
| `attack_heavy_1` | 0-3 |
| `attack_heavy_2` | 4-7 |
| `bow_draw` | 0-1 |
| `bow_aim_level` | 2 |
| `bow_aim_up` | 3 (`R` + `W`) |
| `bow_aim_down` | 4 (`R` + `S`) |
| `bow_release` | 5 |
| `bow_recovery` | 6-7 |

### Escala e corrida

- `ExternalSpriteSheetArt.DefaultExternalScale = 0.30f`
- Boost **+5%** no sprite durante `PlayerState.Run` (corrida ficava visualmente menor que walk apos normalizacao)
- Seta amarela de mira (`AimIndicator`) desligada quando `arandu_bow_sheet.png` existe

## Problemas conhecidos

1. **Frames 3 e 7 do attack light original** — tacape desgrudado/flutuando na arte IA; contornado no mapeamento de frames, ideal regerar sheet
2. **Sheets IA fora de 2048x256** — sempre passar pelo pipeline; nunca jogar direto em `player/`
3. **Tacape cortado** — personagem grande demais na celula 256px; prompts devem exigir margem 22px+ e personagem ~70% da celula

## Validacao pendente (Codex / jogador)

- [ ] F5: corrida mesmo tamanho percebido que walk
- [ ] `J J J` — 3 poses distintas, tacape visivel
- [ ] `K K` — 2 pesados distintos
- [ ] `R` — draw, mira W/S, disparo
- [ ] Reimportar PNGs no Godot apos pull

## Proximo passo sugerido

1. Regerar `arandu_attack_light_sheet.png` (frames 3 e 7 limpos)
2. Validar visual in-game e ajustar `DefaultExternalScale` se necessario
3. Sheets Bruto / Sargento / Capitao (mesmo pipeline)
