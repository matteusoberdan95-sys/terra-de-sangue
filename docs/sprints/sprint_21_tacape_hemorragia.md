# Sprint 21 - Tacape Tier 1 e Hemorragia

## Objetivo

Upgrade permanente do tacape via memoria, hemorragia como nivel de bleed e segundo artefato consumivel.

Plano: `docs/13_sistema_de_arsenal_e_arquearia.md`.

---

## Escopo desta sprint (MVP jogavel)

### Tacape Tier 1

- [x] `WeaponProgression` — tier permanente via memoria `mascara_quebrada`.
- [x] **Tacape de Osso**: +1 dano em golpes leves, janela de combo +0.02s.
- [x] Pesado `K` com tier 1+ aplica Sangramento II.
- [x] HUD mostra tier do tacape.

### Hemorragia

- [x] `BleedLevel.Hemorrhage` — 1.5 DPS, 6s, -10% velocidade inimigo.
- [x] Stack: Sangramento I + II → Hemorragia.
- [x] Rastro de sangue ao mover inimigo hemorragiando.
- [x] Visual: ferida vermelha intensa.

### Artefato: Clava Quebrada

- [x] 1 uso; `U` equipa, `K` = smash com Hemorragia instantanea.
- [x] Spawn no 3o encontro da Aldeia.

### Fora desta sprint

- Tacape tier 2 (bleed em K sem tier 1).
- Gancho de corrente, tipos de flecha avancados.
- Hemorragia II.

---

## Balance inicial

| Parametro | Valor |
|-----------|-------|
| Tier 1 bonus leve | +1 dano |
| Combo window bonus | +0.02s |
| Hemorragia DPS | 1.5 / 6s |
| Slow hemorragia | 10% |
| Clava usos | 1 |

---

## Testes de validacao

- [x] Coletar memoria apos mini-chefe → HUD mostra Tacape de Osso (T1).
- [x] `J` leve remove mais vida com tier 1.
- [x] `K` com tier 1 aplica sangramento pesado.
- [x] Clava no 3o encontro → `U`+`K` → hemorragia + rastro.
- [x] Build C# sem erros; validacao visual Godot.

---

## Registro de validacao visual

Status atual: **validado** em 2026-07-07 pelo usuario.
