# Sprint 19 - Golpes Aereos

## Objetivo

Ataques durante o pulo: corte no apice, slam na descida, martelo aereo e investida apos pulo frontal.

Plano completo: `docs/14_golpes_aereos.md`.

---

## Escopo MVP

### Golpes

- [x] Corte no apice — `J` na subida (0–50% do pulo).
- [x] Slam descendente — `J` na descida (50–95%).
- [x] Martelo aereo — `K` na descida (1x por pulo).
- [x] Investida aerea — slam na descida apos pulo frontal (raio maior).

### Sistema

- [x] Max 1 ataque leve OU 1 pesado por pulo.
- [x] Hitbox circular no pouso do slam/martelo.
- [x] Recovery extra no pouso (slam 0.14s, martelo 0.22s).
- [x] `CombatFeel.ApplyAirSlamImpact` + SFX de pouso.

### Fora desta sprint

- Queda rapida `S+J`.
- Anti-aereo inimigo.
- Bleed no slam.
- Sprites dedicados de pulo.

---

## Controles

| Input | Acao |
|-------|------|
| `Space` + `J` (subida) | Corte no apice |
| `Space` + `J` (descida) | Slam |
| `Space` + `K` (descida) | Martelo aereo |
| Pulo frontal + `J` (descida) | Investida aerea |

---

## Validacao

- Build C# sem erros.
- Validacao interativa: aprovada 2026-07-07.

## Registro de validacao visual

Status atual: aprovado pelo usuario em 2026-07-07.
