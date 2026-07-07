# Golpes Aéreos — Plano de Design

Ultima atualizacao: 2026-07-07.

## Objetivo

Dar **expressao ofensiva ao pulo** sem virar combate aéreo longo. O ar e uma **janela curta** para abrir inimigos, punir recuo na faixa Y ou fechar distancia com risco (recovery na queda).

Complementa Sprint 18 (pulo / pulo frontal) e antecede ou convive com Sprint 20 (arsenal).

---

## Pilares

1. **Poucos golpes, leitura clara** — no maximo 2 acoes por salto.
2. **Risco na queda** — recovery ao aterrissar; inimigo pode punir.
3. **Faixa 2.5D** — golpes ajustam eixo Y, nao plataforma livre.
4. **Gore escalonado** — corte leve no ar; slam esmaga e mancha o chao.
5. **Opcional, nao obrigatorio** — jogador casual usa pulo so para reposicionar.

---

## Fases do pulo (para timing)

```
Subida (0–35%)  →  Ápice (35–55%)  →  Descida (55–85%)  →  Pouso (85–100%)
```

Cada golpe aereo so funciona em **uma** fase — evita mash no ar.

---

## Golpes planejados

### 1. Corte no ápice — `J` (subida / ápice)

| Parametro | Valor |
|-----------|-------|
| Janela | 0–50% do pulo |
| Dano | 1 (leve) |
| Hitbox | Arco curto à frente, leve offset Y |
| Stamina | 0 |
| Uso | Poke aéreo, cancel para slam |

Visual: tacape varre horizontal; pixel 1 frame de `attack_light` estendido.

---

### 2. Slam descendente — `J` (descida)

| Parametro | Valor |
|-----------|-------|
| Janela | 50–95% do pulo |
| Dano | 2 |
| Hitbox | Círculo no pouso (raio ~28px) |
| Stamina | 6 |
| Knockback | Forte para baixo + empurra lateral |
| Recovery | 0.14s após tocar o chão |

Gore: decal de impacto + crack no chao; inimigo em sangramento recebe Sangramento I.

**Assinatura do kit** — o golpe que vende o pulo como ferramenta ofensiva.

---

### 3. Martelo aéreo — `K` (descida, commit)

| Parametro | Valor |
|-----------|-------|
| Janela | 45–90% do pulo |
| Dano | 3 |
| Stamina | 12 |
| Restricao | **1 por salto** |
| Recovery | 0.22s no pouso |
| Stagger | Longo em brutos |

Gore: hitstop maior, shake de camera; futuro tacape tier 2 aplica bleed.

Nao combina com slam `J` no mesmo pulo.

---

### 4. Investida aérea — `J` após **pulo frontal**

| Parametro | Valor |
|-----------|-------|
| Input | `W`+`A`/`D`+`Space`, depois `J` na descida |
| Dano | 2 (slam) + 0.5 bleed bonus |
| Alcance | Avanço do pulo + raio de slam |
| Fantasia | Arandu desce com tacape na diagonal |

Variante **estilo** mais recompensadora que pulo parado + slam.

---

### 5. Queda rápida — `S` + `J` (opcional Sprint 19B)

| Parametro | Valor |
|-----------|-------|
| Efeito | Acelera descida; hitbox em linha na faixa Y |
| Dano | 1 |
| Uso | Ajuste fino de profundidade + poke |

Fora do MVP se atrasar — nice to have.

---

## Regras de sistema

| Regra | Detalhe |
|-------|---------|
| Ações por pulo | Max **1 leve** OU **1 pesada** (slam/martelo) |
| Combo no ar | Sem `J-J-J` aéreo |
| Mira | Sempre na direção do `_facingX` |
| Inimigos | Mercenario pode jab anti-aéreo (futuro); bruto smash pune pouso longo |
| Stamina | Slam e martelo gastam; corte no ápice nao |
| Estado | `PlayerState.AirAttack` ou sub-flag `_airAttackUsed` |

---

## Integração técnica

```
PlayerController
  TryAirLightAttack()   — fase subida/ápice
  TryAirSlam()          — fase descida, J
  TryAirHeavySlam()     — fase descida, K
  ResolveAirHitbox()    — hitbox circular no pouso

CombatFeel
  ApplyAirSlamImpact()  — shake + hitstop medio

ImpactFeedback
  Gore decal radial no GlobalPosition do pouso
```

Animacao: reusar `attack_light` / `attack_heavy` com offset Y no `VisualRig` ate sprite dedicado (Sprint 22 polish).

---

## Controles resumidos

| Input | Ação |
|-------|------|
| `Space` + `J` (subida) | Corte no ápice |
| `Space` + `J` (descida) | Slam descendente |
| `Space` + `K` (descida) | Martelo aéreo |
| Pulo frontal + `J` | Investida aérea |
| `S` + `J` (ar) | Queda rápida *(opcional)* |

---

## HUD / feedback

- Flash curto no sprite durante janela de slam válida (descida).
- Som distinto: `whoosh` subida, `crack` no pouso.
- Partícula de poeira no aterrisseme do slam.

---

## Fora de escopo

- Combos aéreos longos (>2 inputs).
- Double jump.
- Grab aéreo.
- Ataque aéreo com arco (flecha no ar = Sprint 20+).

---

## Criterio de sucesso

O jogador deve querer:

- Pular frontal → **slam** no mercenario que recua na faixa Y.
- Usar corte no ápice para **medir distância** antes do slam.
- Sentir que **errar o pouso** deixa vulnerável — nao spammar pulo.

---

## Roadmap

| Sprint | Entrega |
|--------|---------|
| **19** | Slam `J`, martelo `K`, corte no ápice, investida pós-pulo frontal |
| **19B** *(opcional)* | Queda `S+J`, anti-aéreo mercenario |
| **22** *(polish)* | Sprites `jump_attack` / `slam_land` |

Detalhes de implementacao: `docs/sprints/sprint_19_golpes_aereos.md`.

## Ordem no backlog

```
Sprint 18 ✅ Pulo + combos chão
Sprint 19   Golpes aéreos
Sprint 20   Arsenal / arquearia / bleed
Sprint 21   Tacape tier + hemorragia
```
