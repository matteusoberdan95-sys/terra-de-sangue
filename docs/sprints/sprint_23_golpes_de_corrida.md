# Sprint 23 - Golpes de Corrida (Final Fight)

## Objetivo

Golpes exclusivos enquanto o personagem **corre** (`A`/`D` 2x), no estilo **Final Fight** / **Cadillac Dinosaurs** — leitura clara de “estou em movimento ofensivo”.

## Contexto

Hoje existe `StartRunAttack()` apenas no `J` (golpe de entrada generico). Falta diferenciacao forte, animacao propria e variante pesada no `K`.

## Escopo proposto

### Corrida + `J` — golpe de entrada

- [x] Animacao/sprite dedicada (`run_attack_light`).
- [x] Mantem momentum parcial na direcao da corrida.
- [x] Knockback alto, dano leve+, recovery curto.
- [x] Nao consome combo chain normal.

### Corrida + `K` — golpe pesado de corrida

- [x] Variante pesada (`run_attack_heavy`).
- [x] Custo de stamina extra (6).
- [x] Recovery medio; stagger extra em inimigos.
- [x] 2 de dano no impacto.

### Game feel

- [x] SFX distintos (whoosh + impacto).
- [x] Hitstop levemente maior que golpe parado.
- [x] Cancel: corrida → golpe sem voltar a idle.

### HUD / UX

- [x] Flash sutil no sprite ao correr.
- [x] Documentar no `08_estado_atual.md`.

## Referencia de design

| Jogo | Referencia |
|------|------------|
| Final Fight | Dash punch / running attack |
| Cadillac Dinosaurs | Golpes mantendo deslocamento |

## Fora desta sprint

- Combos encadeando apos golpe de corrida.
- Tacape tier afetando animacao.

## Registro de validacao visual

Status atual: **validado** em 2026-07-07 pelo usuario.
