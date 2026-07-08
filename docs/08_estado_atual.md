# Estado Atual

Ultima atualizacao: 2026-07-07 (Sprint 26 — sheets Arandu integrados, combos J/K, arco, pipeline de arte).

## Sprint atual

**Sprint 26 - Arandu Sprites e Combate** (entregue no codigo; validacao visual pendente)

- Nova leva de sheets do Arandu em `assets/art/sprites/player/` (walk, idle, run, atk L/H, bow, hit, death).
- Pipeline Python em `tools/` para normalizar PNGs irregulares da IA.
- Combos: `J J J` (leve x3), `K K` (pesado x2), finisher `J J K`.
- Animacao de arco (`R`) com sheet dedicado; mira `W`/`S`.
- Removida seta vermelha placeholder de ataque.
- Escala externa `0.30` + boost +5% na corrida.

Ver `docs/sprints/sprint_26_arandu_sprites_combate.md` e `assets/art/sprites/player/README.md`.

## Sprint anterior (parcial)

**Sprint 25 - Aldeia Visual**

- Cenario em `scenes/levels/AldeiaBackground.tscn`
- Mercenario integrado (`mercenary_*.png`)
- Colisao beat em up via hurtboxes

## Arco jogavel (Fase 1)

1. **Aldeia em Cinzas** - 3 encontros + Sargento + memoria (Tacape T1)
2. **Capitao do Ferro** - chefe com sprite proprio + flechas
3. **Mata Fechada** - 3 encontros + memoria final + tela de conclusao (`R` rejoga)

## Controles

- `J` / `J J J` — combo leve (3 golpes)
- `K` / `K K` — combo pesado (2 golpes)
- `J J K` — finisher pesado do combo leve
- `R` — arco (segurar + `W`/`S` mira + soltar dispara)
- Double-tap `A`/`D` + corrida, `Shift` dash, etc. (sprints 20-23)

## Dor recente (arte)

- PNGs da IA chegam em tamanhos errados (ex. 2048x683) e com tacape cortado na borda do frame.
- **Nunca** integrar direto — usar `tools/normalize_sprite_sheet.py` + `tools/upscale_sheet_to_walk.py`.
- Frames 3 e 7 do attack light original tinham tacape quebrado; mapeamento no codigo pula esses indices.

## Proximo passo (ordem sugerida)

1. Validar visual in-game (F5) apos pull
2. Regerar `arandu_attack_light_sheet.png` se tacape ainda falhar
3. Sheets Bruto / Sargento / Capitao (`assets/art/sprites/enemies/README.md`)
4. Polir gore no combate
