# Sprint 19 - Golpes Aereos

## Objetivo

Ataques durante o pulo: corte no apice, slam na descida, martelo aereo e investida apos pulo frontal.

Plano completo: `docs/14_golpes_aereos.md`.

---

## Escopo MVP

### Golpes

- [ ] Corte no apice — `J` na subida (0–50% do pulo).
- [ ] Slam descendente — `J` na descida (50–95%).
- [ ] Martelo aereo — `K` na descida (1x por pulo).
- [ ] Investida aerea — `J` na descida apos pulo frontal.

### Sistema

- [ ] Flag `_airAttackUsed` — max 1 ataque pesado ou leve+slam por pulo.
- [ ] Hitbox circular no pouso do slam.
- [ ] Recovery extra no pouso (slam 0.14s, martelo 0.22s).
- [ ] `CombatFeel` + decal de impacto no slam.
- [ ] SFX: whoosh aereo + crack de pouso.

### Fora desta sprint

- Queda rapida `S+J`.
- Anti-aereo inimigo.
- Bleed no slam (Sprint 21 com tacape tier).
- Sprites dedicados de pulo.

---

## Controles

| Input | Acao |
|-------|------|
| `Space` + `J` (subida) | Corte no apice |
| `Space` + `J` (descida) | Slam |
| `Space` + `K` (descida) | Martelo aereo |
| Pulo frontal + `J` | Investida aerea |

---

## Testes

- [ ] Slam acerta inimigo na faixa Y correta.
- [ ] Nao da para martelo + slam no mesmo pulo.
- [ ] Investida frontal alcanca mais longe que pulo parado.
- [ ] Recovery punivel — player parado apos slam.
- [ ] Combos chao `J-J-J` inalterados.

---

## Dependencias

- Sprint 18 validada.

---

## Registro de validacao visual

Status atual: nao iniciada.
