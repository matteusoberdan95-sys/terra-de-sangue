# Kit de Movimento Soulslike — Plano de Design

Ultima atualizacao: 2026-07-07.

## Objetivo

Tornar o combate da Fase 1 **prazeroso de repetir**: ritmo, risco, recompensa e expressao do jogador — sem virar um simulador de inputs.

Inspiracao: peso e leitura de **Soulslike** + fluidez de **beat 'em up 2.5D** (eixo Y = profundidade).

---

## Pilares do que sentir bom

1. **Toda acao tem custo e consequencia** — stamina, recovery, posicao.
2. **Esquiva bem feita e a melhor mecanica** — salva, mas nao spamma.
3. **Combos sao opt-in, nao obrigatorios** — o jogador casual sobrevive com `J` e esquiva; quem quer estilo combina corrida, dash e strings.
4. **Inimigos punem panic** — telegraph ja existe; dodge e posicao Y sao a resposta.
5. **Sensacao antes de animacao bonita** — placeholders e pixel art bastam se o timing for certo.

---

## Controles propostos

| Input | Acao | Notas |
|-------|------|-------|
| `WASD` | Movimento na faixa 2.5D | Mantido |
| `J` | Ataque leve / combo | Expandir para 3 golpes |
| `K` | Ataque pesado | Commit forte, mais stamina |
| `E` | Execucao | Mantido |
| `Space` | **Pulo** | Hop curto na faixa 2.5D |
| `Shift` | **Impulso frontal** | Gap closer ofensivo |
| `A` ou `D` 2x rapido | **Corrida** | Sprint 17 |
| `Ctrl` | **Esquiva** | I-frames, custo de stamina |
| `W` + `A`/`D` + `Space` | **Pulo frontal** | Salto com avanco (Sprint 18) |

> Decisao 2026-07-07: `Space` = pulo. `Shift` = impulso frontal. `Ctrl` = esquiva.

---

## Stamina (barra nova no HUD)

| Parametro | Valor inicial |
|-----------|----------------|
| Maximo | 100 |
| Regeneracao (fora de acao) | 18/s |
| Regeneracao (em combate) | 10/s |
| Delay apos gasto | 0.35s |

| Acao | Custo |
|------|-------|
| Esquiva | 22 |
| Corrida (por ativacao) | 12 |
| Impulso frontal | 18 |
| Pulo | 14 |
| Pulo frontal | 20 |
| Ataque pesado `K` | 8 (opcional sprint 17) |

Sem stamina = acao bloqueada + feedback visual (barra pisca vermelho).

---

## Esquiva (`Space`)

- Duracao: **0.38s** total
- I-frames: **0.22s** no meio do rolamento
- Distancia: ~56px na direcao do `facing` (+ componente Y se `W`/`S` pressionados levemente)
- **Cancela** recovery de ataque leve (nao cancela pesado no prototipo)
- **Punicao**: recovery de 0.15s no fim se stamina zerou (whiff pesado)
- Som: sweep curto + passo seco
- Camera: shake minimo

Soulslike: esquiva e **posicionamento**, nao teleporte. Hitbox do player desliga durante i-frames.

---

## Corrida (double-tap `A` / `D`)

- Janela entre toques: **0.22s**
- Velocidade: **1.65x** do walk (~198 px/s)
- Duracao: **1.2s** ou ate soltar direcao oposta
- Ataque durante corrida: **golpe de entrada** (1 hit, mais knockback, recovery curto)
- Visual: sprite walk acelerado + po de cinza leve

Nao e sprint infinito — e ferramenta de engage e kite na faixa Y.

---

## Pulo e pulo frontal

### Pulo (`W` + `Space`)
- Altura visual: ~24px arco
- Duracao: 0.42s
- **Sem i-frames** completos (só 4 frames de invuln no apex — opcional)
- Uso: reposicionar eixo Y, flair, setup para ataque aereo futuro
- Beat em up: nao vira plataforma; limites da arena mantidos

### Pulo frontal (`W` + `A`/`D` + `Space`)
- Arco com avanco ~72px em X
- Recovery ao aterrissar: 0.12s
- Variante ofensiva: `J` no ar = **ataque descendente** (sprint 17)

---

## Impulso frontal (`Shift`)

- Dash rapido **sem** arco (diferente do pulo)
- Distancia: ~48px, 0.18s
- I-frames leves: 0.08s (menos que esquiva)
- Pode **passar pelo inimigo** no eixo Y se alinhado (dash through — estilo stylish)
- Custo stamina maior que corrida — ferramenta de combo, nao locomocao

---

## Combos expandidos (prazerosos, nao obrigatorios)

### String basica (acessivel)
```
J → J → J     (3 golpes leves, terceiro empurra)
```

### Rotas de estilo (recompensa skill)
```
J → J → K           Finisher pesado
Corrida → J         Golpe de abertura
Esquiva → J         Contra-ataque rapido (janela 0.4s)
Impulso → J         Dash attack
J (no ar)           Slam / martelo (Sprint 19)
```

### Janelas
- Combo window entre `J`: **0.30s** (subir de 0.28)
- Contra-ataque pos-esquiva: **0.40s**
- Cancel leve → esquiva: permitido no recovery

### Regra de ouro
Nenhum combo exige mais de **3 inputs** no prototipo da Fase 1.

---

## Interacao com inimigos (justo e legivel)

- Inimigos **nao homing** durante dodge do player
- Ataque em recovery de esquiva do player = fair
- Bruto: esquiva **obrigatoria** no smash (telegraph longo)
- Mercenario: punir com impulso frontal ou corrida → `J`

---

## HUD

- Barra de **stamina** abaixo da vida (amarelo queimado `#e0b75d`, vazio escuro)
- Flash ao gastar; vermelho ao tentar sem stamina

---

## Implementacao em sprints

### Sprint 16 — Esquiva + Stamina
- Barra stamina + `CombatHud`
- Esquiva com i-frames
- Contra-ataque `Esquiva → J`
- Docs + validacao

### Sprint 17 — Corrida + Impulso
- Double-tap `A`/`D`
- `Shift` impulso frontal
- Golpe de corrida e dash attack
- SFX dedicados

### Sprint 18 — Pulo + Combos 3 hits
- Pulo / pulo frontal
- String `J J J` e `J J K`
- Ajuste balance Fase 1

### Sprint 19 — Golpes aéreos
- Slam, martelo, corte no ápice, investida
- Ver `docs/14_golpes_aereos.md`

### Sprint 20 — Arsenal + Arquearia
- Aljava, flechas, bleed

### Sprint 21 (opcional) — Polish movimento
- Poeira, trilha de dash, sprites de roll/jump
- Bruto/sargento em pixel sprite

---

## Criterio de sucesso (Goku)

O jogador deve querer **repetir o primeiro encontro** so para:
- esquivar no ultimo frame,
- correr → bater,
- encadear `J J K` apos esquiva.

Se isso nao for divertido com pixel art, nao avancamos para Fase 2.

---

## Fora de escopo (por agora)

- Parry / bloqueio
- Stamina de inimigos
- Combos aereos longos
- Multiplayer
- Plataforma vertical real

---

## Proximo passo imediato

Validar **Sprint 19** (golpes aéreos), depois **Sprint 20** (Arsenal).
